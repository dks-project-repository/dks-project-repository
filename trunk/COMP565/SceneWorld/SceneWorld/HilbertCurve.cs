using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    // Algorithm from Wikipedia: Hilbert curve
    public class HilbertCurve : IDisposable
    {
        private List<Vector3> curve;
        private List<Vector3> path;
        private VertexBuffer vb;
        private Vector3 curr;
        private Vector3 offset;
        private float dist;

        public List<Vector3> Path { get { return path; } }

        // Drawing stuff
        private Mesh mesh;
        private Material material;
        private Matrix matrix;
        private Device device;

        public HilbertCurve(Device device, float mapWidth, float viewRadius, NavGraph navgraph)
        {
            // Drawing stuff
            this.device = device;
            mesh = Mesh.Cylinder(device, 4f, 0f, 4f, 4, 1);
            material = new Material();
            material.Emissive = Color.Magenta;
            matrix = Matrix.RotationX((float)-Math.PI / 2f) * Matrix.Translation(offset * 2);

            curve = new List<Vector3>();
            offset = new Vector3(-mapWidth / 2, 1, -mapWidth / 2);


            float invSqrt2 = 0.707106781f;
            viewRadius /= invSqrt2;
            int level = 0;
            dist = mapWidth;
            while (dist > viewRadius)
            {
                dist /= 2;
                level++;
            }
            curr = new Vector3();
            updateAndAdd(dist / 2, dist / 2);

            // Create the path
            HilbertU(level);


            adjustToTraversables(navgraph);
            createPath(navgraph);

            vb = new VertexBuffer(typeof(CustomVertex.PositionOnly), curve.Count, device, Usage.None, VertexFormats.Position, Pool.Managed);

            //Fill an array of the appropriate type with the VB data using Lock()
            CustomVertex.PositionOnly[] vbData = (CustomVertex.PositionOnly[])vb.Lock(0, typeof(CustomVertex.PositionOnly), LockFlags.None, curve.Count);
            for (int i = 0; i < curve.Count; i++)
            {
                //set your vertices to something...
                vbData[i].Position = curve[i];
            }
            //Unlock the vb before you can use it elsewhere
            vb.Unlock();
        }

        private void adjustToTraversables(NavGraph n)
        {
            Vector3[] points = curve.ToArray();
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = n.nearestNavigable(points[i]);
            }
            curve.Clear();
            curve.AddRange(points);
        }

        public void Dispose()
        {
            vb.Dispose();
        }

        private void updateAndAdd(float dx, float dz)
        {
            curr.X += dx;
            curr.Z += dz;
            curve.Add(curr + offset);
        }

        // Make U-shaped curve at this scale:
        private void HilbertU(int level)
        {
            if (level > 0)
            {
                HilbertD(level - 1); updateAndAdd(0, dist);
                HilbertU(level - 1); updateAndAdd(dist, 0);
                HilbertU(level - 1); updateAndAdd(0, -dist);
                HilbertC(level - 1);
            }
        }

        // Make D-shaped (really "]" shaped) curve at this scale:
        private void HilbertD(int level)
        {
            if (level > 0)
            {
                HilbertU(level - 1); updateAndAdd(dist, 0);
                HilbertD(level - 1); updateAndAdd(0, dist);
                HilbertD(level - 1); updateAndAdd(-dist, 0);
                HilbertA(level - 1);
            }
        }

        // Make C-shaped (really "[" shaped) curve at this scale:
        private void HilbertC(int level)
        {
            if (level > 0)
            {
                HilbertA(level - 1); updateAndAdd(-dist, 0);
                HilbertC(level - 1); updateAndAdd(0, -dist);
                HilbertC(level - 1); updateAndAdd(dist, 0);
                HilbertU(level - 1);
            }
        }

        // Make A-shaped (really "?" shaped) curve at this scale:
        private void HilbertA(int level)
        {
            if (level > 0)
            {
                HilbertC(level - 1); updateAndAdd(0, -dist);
                HilbertA(level - 1); updateAndAdd(-dist, 0);
                HilbertA(level - 1); updateAndAdd(0, dist);
                HilbertD(level - 1);
            }
        }

        public void draw()
        {
            //This lock overload simply locks the entire VB -- setting ReadOnly can improve perf when reading a vertexbuffer
            CustomVertex.PositionOnly[] vbData = (CustomVertex.PositionOnly[])vb.Lock(0, LockFlags.ReadOnly);
            device.Material = material;
            device.SetStreamSource(0, vb, 0);
            device.DrawPrimitives(PrimitiveType.LineStrip, 0, curve.Count - 1);
            vb.Unlock();
        }

        public int closetIndexTo(Vector3 v)
        {
            float minDistSq = float.PositiveInfinity;
            int closestIndex = -1;
            for (int i = 0; i < path.Count; i++)
            {
                float distSq = (path[i] - v).LengthSq();
                if (distSq < minDistSq)
                {
                    closestIndex = i;
                    minDistSq = distSq;
                }
            }
            return closestIndex;
        }

        private void createPath(NavGraph n)
        {
            path = new List<Vector3>();
            IndexPair source, dest;
            dest = NavGraph.indexFromLocation(curve[0]);
            for (int i = 1; i < curve.Count; i++)
            {
                Console.WriteLine("AStar: Hilbert " + i + " / " + (curve.Count - 1));
                source = dest;
                dest = NavGraph.indexFromLocation(curve[i]);
                AStar(source, dest, n);
                path.RemoveAt(path.Count - 1);
            }
        }

        private IndexPair AStar(IndexPair curr, IndexPair dest, NavGraph navgraph)
        {            
            SortedList<IndexPair, bool> open = new SortedList<IndexPair, bool>(new IndexPair.Comparer());
            SortedList<IndexPair, bool> closed = new SortedList<IndexPair, bool>(new IndexPair.Comparer());
            List<Vector3> path = new List<Vector3>();

            IndexPair next;
            curr.pathPredecessor = null;
            curr.cost = 0;
            IndexPair source = curr;
            open.Add(curr, true);

            float maxDist = IndexPair.dist(source, dest) * 1.5f;

            while (open.Count > 0)
            {
                // get first indexpair in the list
                curr = open.Keys[0];
                if (curr.Equals(dest))
                {
                    // we now have a path

                    // stick it in a list
                    curr.backtrack(this.path);
                    return curr;
                }
                else
                {
                    // keeping looking
                    open.RemoveAt(0);
                    closed.Add(curr, true);
                    int nextDir = curr.dir - 1;
                    if (nextDir < 0)
                        nextDir += 8;
                    int nodesAdded = 0;
                    int maxIndex = 8;
                    for (int i = 0; i < maxIndex; i++)
                    {
                        next = NavGraph.indexAt(curr, nextDir);
                        if (navgraph.isTraversable(next))
                        {

                            // source cost
                            next.cost = next.sourceCost = curr.sourceCost +
                                    (nextDir % 2 == 0 ? 1 : 1.41421356237f);

                            // heuristic
                            float destCost = IndexPair.dist(dest, next);
                            if (destCost > maxDist)
                                continue;
                            next.cost += destCost;
                            if (!open.ContainsKey(next) && !closed.ContainsKey(next))
                            {
                                open.Add(next, true);
                                nodesAdded++;
                            }
                        }

                        nextDir++;
                        if (nextDir >= 8)
                            nextDir -= 8;

                        if (i == 2 && nodesAdded == 0)
                        {
                            // If stuck, allow to go other directions
                            maxIndex = 8;
                        }
                    }
                }
            }
            return null;
        }
    }
}
