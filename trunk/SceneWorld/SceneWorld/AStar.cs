using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace SceneWorld
{
    class AStar
    {
        //private Object3D closest;

        //private void updateClosest(List<Object3D> Obj)//sets "closest" to the nearest object, given a list of followable objects, 
        //{

        //    foreach (Object3D O in Obj)
        //    {
        //        if (Vector3.Length(avatar.Location - O.Location) <= Vector3.Length(avatar.Location - closest.Location))
        //            closest = O;

        //    }
        //}

        static float sqrt2 = (float)Math.Sqrt(2);
        public List<Vector3> aStar(Vector3 start, Vector3 end)
        {
            int[,] adjacent = { { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };

            List<Vector3> path = new List<Vector3>();
            SortedList<IndexPair, int> open = new SortedList<IndexPair, int>();
            SortedList<IndexPair, int> closed = new SortedList<IndexPair, int>();
            IndexPair s = new IndexPair(NavGraph.arrayCoord(start));
            IndexPair e = new IndexPair(NavGraph.arrayCoord(end));
            IndexPair cur;

            open.Add(s, 0);

            while (open.Count != 0)
            {
                cur = open.Keys[0];

                if (cur.Equals(e))
                {
                    backtrack(cur, path);
                    return path;
                }
                closed.Add(cur, 0);
                open.Remove(cur);

                for (int i = 0; i < 8; i++)
                {
                    if (!(cur.x + adjacent[i, 0] < 0 || cur.x + adjacent[i, 0] > 400 || cur.z + adjacent[i, 1] < 0 || cur.z + adjacent[i, 1] > 400))
                    {
                        IndexPair n = new IndexPair(cur.x + adjacent[i, 0], cur.z + adjacent[0, 1]);
                        n.last = cur;

                        if (i % 2 == 0)
                            n.SourceCost = sqrt2 + cur.SourceCost;
                        else
                            n.SourceCost = 1 + cur.SourceCost;

                        n.HeuristicCost = (float)Math.Sqrt(Math.Pow(n.x + e.x, 2) + Math.Pow(n.z + e.z, 2));

                        if (!open.ContainsKey(n) && !closed.ContainsKey(n))
                            open.Add(n, 0);
                    }
                }
            }


            //OrderedCollection open, closed // sets ordered on cost
            //open = source node // 0 cost, null parent
            //while open ! empty
            //  cur = open's lowest cost node
            //  if cur == goal then path complete
            //  else
            //      closed += cur
            //      foreach of cur's valid adjacent nodes
            //          if (node !in open && !in closed)
            //              calculate cost
            //              open += node
            //path = traversal back on parent link


            return path;

        }

        private void backtrack(IndexPair cur, List<Vector3> path)
        {
            if (cur.last == null)
            {
                path.Add(cur);
                return;
            }
            backtrack(cur.last, path);
            path.Add(cur);
            
        }


        class IndexPair : IComparable<IndexPair>
        {
            public int x, z;
            public float SourceCost, HeuristicCost;
            public IndexPair last;

            public IndexPair(int x, int z)
            {
                this.x = x;
                this.z = z;
            }

            public IndexPair(Vector3 v)
            {
                x = (int)v.X;
                z = (int)v.Z;
            }

            public int CompareTo(IndexPair other)
            {
                float t = (SourceCost + HeuristicCost) - (other.SourceCost + other.HeuristicCost);

                if (t == 0)
                    return (1000 * x + z) - (other.x * 1000 + other.z);
                return (int)t;
            }

            public override bool Equals(object obj)
            {
                return x == ((IndexPair)obj).x && z == ((IndexPair)obj).z;
            }
        }

    }
}
