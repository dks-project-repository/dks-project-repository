using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game465P3
{
    public class Octree
    {
        protected Node root;

        public Octree(float xDim, float yDim, float zDim, int numDivisions)
        {
            if (numDivisions < 0)
                numDivisions = 0;
            if (numDivisions > 6)
                numDivisions = 6;

            Vector3 max = new Vector3(xDim / 2, yDim / 2, zDim / 2);
            Vector3 min = -max;
            root = new Node(null, new BoundingBox(min, max), numDivisions);
        }

        public bool Add(Object3D o)
        {
            Node n = root.GetContainer(o.bounds);
            if (n == null)
                return false;

            n.AddHere(o);
            return true;
        }

        public bool Remove(Object3D o)
        {
            Node n = root.GetContainer(o.bounds);
            if (n == null)
                return false;

            return n.Remove(o);
        }

        public bool Move(Object3D o, BoundingBox newBounds)
        {
            if (Remove(o))
            {
                BoundingBox oldBox = o.bounds;
                o.bounds = newBounds;

                Node n = root.GetContainer(oldBox);
                if (n.Move(o))
                    return true;

                // If failure, undo changes
                o.bounds = oldBox;
                n.AddHere(o);
            }
            return false;
        }

        public T intersection<T>(Vector3 v1, Vector3 v2) where T : Object3D
        {
            Vector3 diff = v2 - v1;
            float len = diff.Length();

            if (len == 0)
                return null;

            Ray r = new Ray(v1, Vector3.Normalize(diff));
            return intersection<T>(r, len);
        }

        public T intersection<T>(Ray r, float maxDist) where T : Object3D
        {
            float dummy;
            return root.Intersection<T>(r, maxDist, out dummy);
        }

        public List<Object3D> getAllWithin(BoundingBox b)
        {
            List<Object3D> list = new List<Object3D>();
            root.GetAllWithin(b, list);
            return list;
        }

        public List<T> getAllWithin<T>(BoundingBox b) where T : Object3D
        {
            List<T> list = new List<T>();
            root.GetAllWithin<T>(b, list);
            return list;
        }

        protected class Node
        {
            protected BoundingBox box;
            protected List<Object3D> list;
            protected Node[] children;
            protected Node parent;

            public Node(Node parent, BoundingBox box, int subdivisions)
            {
                this.parent = parent;
                this.box = box;
                list = new List<Object3D>();
                if (subdivisions > 0)
                {
                    Vector3 center = (box.Min + box.Max) / 2;

                    // Bottom
                    Vector3 v000 = box.Min;
                    Vector3 vc00 = new Vector3(center.X, box.Min.Y, box.Min.Z);
                    Vector3 v00c = new Vector3(box.Min.X, box.Min.Y, center.Z);
                    Vector3 vc0c = new Vector3(center.X, box.Min.Y, center.Z);

                    //Middle
                    Vector3 v0c0 = new Vector3(box.Min.X, center.Y, box.Min.Z);
                    Vector3 vcc0 = new Vector3(center.X, center.Y, box.Min.Z);
                    Vector3 v0cc = new Vector3(box.Min.X, center.Y, center.Z);
                    Vector3 vccc = center;
                    Vector3 v1cc = new Vector3(box.Max.X, center.Y, center.Z);
                    Vector3 vcc1 = new Vector3(center.X, center.Y, box.Max.Z);
                    Vector3 v1c1 = new Vector3(box.Max.X, center.Y, box.Max.Z);

                    // Top
                    Vector3 vc1c = new Vector3(center.X, box.Max.Y, center.Z);
                    Vector3 v11c = new Vector3(box.Max.X, box.Max.Y, center.Z);
                    Vector3 vc11 = new Vector3(center.X, box.Max.Y, box.Max.Z);
                    Vector3 v111 = box.Max;

                    children = new Node[8];
                    subdivisions--;
                    children[0] = new Node(this, new BoundingBox(v000, vccc), subdivisions);
                    children[1] = new Node(this, new BoundingBox(vc00, v1cc), subdivisions);
                    children[2] = new Node(this, new BoundingBox(v00c, vcc1), subdivisions);
                    children[3] = new Node(this, new BoundingBox(vc0c, v1c1), subdivisions);
                    children[4] = new Node(this, new BoundingBox(v0c0, vc1c), subdivisions);
                    children[5] = new Node(this, new BoundingBox(vcc0, v11c), subdivisions);
                    children[6] = new Node(this, new BoundingBox(v0cc, vc11), subdivisions);
                    children[7] = new Node(this, new BoundingBox(vccc, v111), subdivisions);
                }
            }

            public void AddHere(Object3D o)
            {
                list.Add(o);
            }

            public bool Remove(Object3D o)
            {
                return list.Remove(o);
            }

            public bool Move(Object3D o)
            {
                // Note: removal from old node is done in Octree.Move

                if (box.Contains(o.bounds) != ContainmentType.Contains)
                {
                    if (parent == null)
                        return false;
                    return parent.Move(o);
                }
                GetContainer(o.bounds).AddHere(o);
                return true;
            }

            // Return smallest node in which the object fits
            public Node GetContainer(BoundingBox b)
            {
                if (box.Contains(b) != ContainmentType.Contains)
                    return null;

                if (children != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Node n = children[i].GetContainer(b);
                        if (n != null)
                            return n;
                    }
                }

                return this;
            }

            public void GetAllWithin(BoundingBox b, List<Object3D> result)
            {
                if (box.Contains(b) != ContainmentType.Disjoint)
                {
                    result.AddRange(list);

                    if (children != null)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            children[i].GetAllWithin(b, result);
                        }
                    }
                }
            }

            public void GetAllWithin<T>(BoundingBox b, List<T> result) where T : Object3D
            {
                if (box.Contains(b) != ContainmentType.Disjoint)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] is T)
                            result.Add((T)list[i]);
                    }

                    if (children != null)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            children[i].GetAllWithin(b, result);
                        }
                    }
                }
            }

            public T Intersection<T>(Ray r, float maxDist, out float minDist) where T : Object3D
            {
                minDist = maxDist;

                // Check if this box intersects the ray segment
                float? intersectTest = r.Intersects(box);
                if (intersectTest != null && (float)intersectTest <= maxDist)
                {
                    T minObject = null;

                    // Check objects in this node's List
                    int count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (list[i] as T != null)
                        {
                            float? dist = r.Intersects(list[i].bounds);
                            if (dist != null && (float)dist < minDist)
                            {
                                minDist = (float)dist;
                                minObject = (T)list[i];
                            }
                        }
                    }

                    // Check children nodes
                    if (children != null)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            float childDist;
                            T childObject = children[i].Intersection<T>(r, minDist, out childDist);
                            if (childObject != null)
                            {
                                minDist = childDist;
                                minObject = childObject;
                            }
                        }
                    }

                    return minObject;
                }

                return null;
            }
        }
    }
}
