using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class NavGraph
    {
        private Node[,] nodes;
        private static Mesh sq;
        private static Material m1, m2, m3;
        private static Matrix matrix;
        private Device device;
        private static int[] yonDividers = { 2, 4, 8, 16, 32 };
        private int yonDivider = -1;

        public NavGraph(SceneWorld sw, List<IDrawable> drawables)
        {
            this.device = sw.Display;

            sq = Mesh.Cylinder(device, 4f, 0f, 4f, 4, 1);
            m1 = new Material();
            m2 = new Material();
            m3 = new Material();
            m1.Diffuse = Color.Gray;
            m2.Diffuse = Color.Gray;
            m3.Diffuse = Color.Gray;
            m1.Emissive = Color.Green;
            m2.Emissive = Color.DarkRed;
            m3.Emissive = Color.Yellow;
            matrix = Matrix.RotationX((float)-Math.PI / 2f) * Matrix.Translation(-2000, 0, -2000);

            // Set up the data structure
            nodes = new Node[401, 401];
            for (int i = 0; i < 401; i++)
                for (int j = 0; j < 401; j++)
                    nodes[i, j] = new Node();

            // Remove nodes inside bounding spheres
            foreach (IDrawable drawable in drawables)
            {
                // Skip movables
                if (drawable is MovableMesh3D)
                    continue;

                Vector3 pos = drawable.Location;
                float radius = drawable.Radius + 6;

                // Er... don't delete nodes because of the ground.
                if (drawable is Object3D && ((Object3D)drawable).Name == "ground")
                    continue;

                // x & z below refer to index units, not SceneWorld units
                float x = pos.X / 10f + 200;
                float z = pos.Z / 10f + 200;
                int minX = (int)Math.Floor((pos.X - radius) / 10f) + 200;
                int maxX = (int)Math.Ceiling((pos.X + radius) / 10f) + 200;
                int minZ = (int)Math.Floor((pos.Z - radius) / 10f) + 200;
                int maxZ = (int)Math.Ceiling((pos.Z + radius) / 10f) + 200;
                if (minX < 0) minX = 0;
                if (maxX > 400) maxX = 400;
                if (minZ < 0) minZ = 0;
                if (maxZ > 400) maxZ = 400;

                for (int i = minX; i < maxX; i++)
                    for (int j = minZ; j < maxZ; j++)
                        if (Math.Sqrt((x - i) * (x - i) + (z - j) * (z - j)) < radius / 10f)
                            nodes[i, j] = null;
            }

            int count = 0;
            for (int i = 0; i < 401; i++)
            {
                for (int j = 0; j < 401; j++)
                {
                    if (nodes[i, j] != null)
                        count++;
                }
            }
            sw.Trace = string.Format("\n{0:##.#}% of nodes traversable", count * 100 / (float)(401 * 401));
        }

        public void draw(Object3D cam, float yon)
        {
            if (yonDivider < 0)
                return;

            Matrix temp = device.Transform.World;  // save Transform state

            yon /= yonDividers[yonDivider];

            float x = cam.Location.X;
            float z = cam.Location.Z;
            // x & z below refer to index units, not SceneWorld units
            int minX = (int)Math.Floor((x - yon) / 10f) + 200;
            int maxX = (int)Math.Ceiling((x + yon) / 10f) + 200;
            int minZ = (int)Math.Floor((z - yon) / 10f) + 200;
            int maxZ = (int)Math.Ceiling((z + yon) / 10f) + 200;
            if (minX < 0) minX = 0;
            if (maxX > 400) maxX = 400;
            if (minZ < 0) minZ = 0;
            if (maxZ > 400) maxZ = 400;

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minZ; j < maxZ; j++)
                {
                    if (nodes[i, j] == null)
                        device.Material = m2;
                    else
                        device.Material = m1;
                    device.Transform.World = matrix * Matrix.Translation(i * 10, 0, j * 10);
                    sq.DrawSubset(0);
                }
            }

            device.Transform.World = temp; // restore Transform state
        }

        public void drawPath(List<Vector3> list)
        {
            Matrix temp = device.Transform.World;
            lock (list)
            {
                foreach (Vector3 v in list)
                {
                    device.Material = m3;
                    device.Transform.World = Matrix.RotationX((float)-Math.PI / 2f) * Matrix.Translation(v);
                    sq.DrawSubset(0);
                }
            }
            device.Transform.World = temp;
        }

        public void nextYonDivider()
        {
            yonDivider++;
            if (yonDivider == yonDividers.Length)
                yonDivider = -1;
        }

        public bool isTraversable(IndexPair ip)
        {
            return nodes[ip.x, ip.z] != null;
        }

        public bool isTraversable(Vector3 p)
        {
            return nodeAt(p) != null;
        }

        public static void locationFromIndex(out float x, out float z, IndexPair ind)
        {
            x = ind.x * 10 - 2000;
            z = ind.z * 10 - 2000;
        }

        public static Vector3 locationFromIndex(IndexPair ind)
        {
            Vector3 loc = new Vector3();
            locationFromIndex(out loc.X, out loc.Z, ind);
            return loc;
        }

        public static IndexPair indexFromLocation(float x, float z)
        {
            return new IndexPair((int)Math.Round(x / 10f) + 200,
                (int)Math.Round(z / 10f) + 200);
        }

        public static IndexPair indexFromLocation(Vector3 loc)
        {
            return indexFromLocation(loc.X, loc.Z);
        }

        // Calculates arguments used in Vector3.Hermite method
        public static void calcHermiteArgs(IndexPair last, IndexPair from, IndexPair to, out Vector3 pos1, out Vector3 tangent1, out Vector3 pos2, out Vector3 tangent2)
        {
            Vector3 pos0 = locationFromIndex(last);
            pos1 = locationFromIndex(from);
            pos2 = locationFromIndex(to);

            tangent1 = Vector3.Normalize(pos1 - pos0);
            tangent2 = Vector3.Normalize(pos2 - pos1);
        }

        // Takes angle in radians (CCW from +z), converts it to int 0..7 (0 is on +z axis)
        public static int directionFromAngle(float angle)
        {
            angle *= (4f / (float)Math.PI);
            int dir = ((int)Math.Round(angle));
            if (dir < 0)
            {
                dir = 7 + ((dir + 1) % 8);
            }
            else
            {
                dir %= 8;
            }
            return dir;
        }

        public static int directionFromVector(Vector3 At)
        {
            float angle = angleFromVector(At);
            return (int)Math.Round(angle * 4 / Math.PI);
        }

        public static float angleFromVector(Vector3 At)
        {
            float angle = (float)polarFromVector(At);
            angle -= (float)Math.PI / 2f;
            if (angle < 0)
                angle += 2 * (float)Math.PI;
            return angle;
        }

        public static int checkDir(int x)
        {
            if (x < 0)
                return x + 8;
            if (x > 7)
                return x - 8;
            return x;
        }


        private static double polarFromVector(Vector3 At)
        {
            if (At.X > 0 && At.Z >= 0)
                return Math.Atan(At.Z / At.X);
            if (At.X > 0 && At.Z < 0)
                return Math.Atan(At.Z / At.X) + 2 * Math.PI;
            if (At.X < 0)
                return Math.Atan(At.Z / At.X) + Math.PI;
            if (At.X == 0 && At.Z > 0)
                return Math.PI / 2;
            if (At.X == 0 && At.Z < 0)
                return 3 * Math.PI / 2;
            throw new ArgumentException();
        }

        public Node nodeAt(Vector3 position)
        {
            IndexPair ip = indexFromLocation(position);
            return nodes[ip.x, ip.z];
        }

        public Vector3 nextPosition(Vector3 oldPos, Vector3 newPos)
        {
            IndexPair ipOld = indexFromLocation(oldPos), ipNew = indexFromLocation(newPos);
            if (nodes[ipOld.x, ipNew.z] != null)
                return new Vector3(oldPos.X, newPos.Y, newPos.Z);
            if (nodes[ipNew.x, ipOld.z] != null)
                return new Vector3(newPos.X, newPos.Y, oldPos.Z);
            return oldPos;
        }

        public static IndexPair indexAt(IndexPair curr, int dir)
        {
            IndexPair ip;
            if (dir == 0)
                ip = new IndexPair(curr.x, curr.z + 1);
            else if (dir == 1)
                ip = new IndexPair(curr.x - 1, curr.z + 1);
            else if (dir == 2)
                ip = new IndexPair(curr.x - 1, curr.z);
            else if (dir == 3)
                ip = new IndexPair(curr.x - 1, curr.z - 1);
            else if (dir == 4)
                ip = new IndexPair(curr.x, curr.z - 1);
            else if (dir == 5)
                ip = new IndexPair(curr.x + 1, curr.z - 1);
            else if (dir == 6)
                ip = new IndexPair(curr.x + 1, curr.z);
            else if (dir == 7)
                ip = new IndexPair(curr.x + 1, curr.z + 1);
            else throw new ArgumentOutOfRangeException();

            ip.pathPredecessor = curr;
            ip.dir = dir;
            return ip;
        }

        public IndexPair nextIndex(IndexPair curr, int dir)
        {
            IndexPair ip = indexAt(curr, dir);

            if (nodes[ip.x, ip.z] == null)
            {
                return curr;
            }
            return ip;
        }

        public IndexPair nearestNavigable(IndexPair ip)
        {
            
            for (int i = 0; i < 200; i++)
            {
                ip.x += i;
                if (this.isTraversable(ip))
                    return ip;
                ip.x -= 2 * i;
                if (this.isTraversable(ip))
                    return ip;
                ip.x += i;
                ip.z += i;
                if (this.isTraversable(ip))
                    return ip;
                ip.z -= 2 * i;
                if (this.isTraversable(ip))
                    return ip;
                ip.z += i;

            }
            throw new Exception("Couldn't find nearest navigable node.");
        }

        public Vector3 nearestNavigable(Vector3 v)
        {
            return locationFromIndex(nearestNavigable(indexFromLocation(v)));
        }

        public class Node
        {

        }
    }

    public class IndexPair : IComparable<IndexPair>, IEquatable<IndexPair>
    {
        public int x, z, dir;
        public float cost, sourceCost;
        public IndexPair pathPredecessor;

        public IndexPair(int i, int j)
        {
            x = i;
            z = j;
            if (x < 0) x = 0;
            if (x > 400) x = 400;
            if (z < 0) z = 0;
            if (z > 400) z = 400;
        }

        public bool Equals(IndexPair ip)
        {
            return x == ip.x && z == ip.z;
        }

        /*public static IndexPair operator -(IndexPair o1, IndexPair o2)
        {
            return new IndexPair(o1.x - o2.x, o1.z - o2.z);
        }*/

        // Used for testing to see if two nodes are at the same location
        public int CompareTo(IndexPair ip)
        {
            if (x - ip.x != 0)
                return x - ip.x;
            return z - ip.z;
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(x * x + z * z);
            }
        }

        public static float dist(IndexPair one, IndexPair two)
        {
            float x = one.x - two.x;
            float z = one.z - two.z;
            return (float)Math.Sqrt(x * x + z * z);
        }

        public void backtrack(List<Vector3> path)
        {
            if (pathPredecessor != null)
                pathPredecessor.backtrack(path);
            path.Add(NavGraph.locationFromIndex(this));
        }

        public override string ToString()
        {
            return x + ", " + z + " (" + cost + ")";
        }

        public class Comparer : IComparer<IndexPair>
        {
            // Used for sorting the A* lists
            public int Compare(IndexPair ip1, IndexPair ip2)
            {
                if (ip1 == null && ip2 == null)
                    return 0;
                if (ip1 == null)
                    return -1;
                if (ip2 == null)
                    return 1;
                if (ip1.cost < ip2.cost)
                    return -1;
                if (ip1.cost > ip2.cost)
                    return 1;
                return 1000 * (ip1.x - ip2.x) + (ip1.z - ip2.z);
            }
        }
    }
}
