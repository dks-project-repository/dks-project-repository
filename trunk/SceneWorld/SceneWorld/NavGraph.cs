/*Daniel Frankel
 * Kevin Yedlin
 * Comp 565 - Project 1
 */

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SceneWorld
{
    public class NavGraph
    {

        private NavNode[,] graph;
        private static float XSpace;
        private static float ZSpace;
        private static int XCount;
        private static  int ZCount;
        private SceneWorld sw;
        private MovableMesh3D avatar;

        public NavGraph(SceneWorld sw, MovableMesh3D a)
        {
            this.sw = sw;
            avatar = a;
            XSpace = 10;
            ZSpace = 10;
            XCount = 401;
            ZCount = 401;
            graph = new NavNode[401, 401];
            for (int i = 0; i < XCount; i++)
            {
                for (int j = 0; j < ZCount; j++)
                {
                    graph[i, j] = new NavNode();
                }

            }
        }

        //public NavGraph(SceneWorld sw,float Length, float Width)
        //{
        //    this.sw = sw;
        //    ZSpace = Length / 10;
        //    XSpace = Width / 10;
        //    ZCount = (int)(Length / ZSpace);
        //    XCount = (int)(Width / XSpace);
        //    graph = new NavNode[XCount, ZCount];
        //    foreach (NavNode N in graph)
        //        N.Navigable = true;
        //}

        public Vector3 nextMove(Vector3 loc, Vector3 at)
        {
            if (!check(loc + at))
                if (check(loc + new Vector3(at.X, at.Y, 0)))
                    return new Vector3(at.X, at.Y, 0);
                else if (check(loc + new Vector3(0, at.Y, at.Z)))
                    return new Vector3(0, at.Y, at.Z);
                else
                    return new Vector3(0, at.Y, 0);
            return at;
        }

        public bool check(Vector3 V)
        {
            int X = (int)(XSpace * (XCount - 1) / 2);
            int Z = (int)(ZSpace * (ZCount - 1) / 2);
            if (V.X > -X && V.X < X && V.Z > -Z && V.Z < Z)
                return graph[(int)(V.X / XSpace + XCount / 2), (int)(V.Z / ZSpace + ZCount / 2)].Navigable;
            return false;
        }

        public static Vector3 arrayCoord(Vector3 V)
        {
            V.Scale(0.1f);
            V.Add(new Vector3(((XCount / 2) * XSpace), 0, ((ZCount / 2) * ZSpace)));
            return V;
        }

        public Vector3 mapCoord(Vector3 V)
        {
            V.Subtract(new Vector3(XCount / 2, 0, ZCount / 2));
            V.Scale(10f);
            return V;
        }

        private int toArrayIndex(float i)
        {
            return (int)((i + XCount * XSpace / 2) / XSpace);
        }

        public void cullBuildings(List<IDrawable> list)
        {

            float minX, minZ, maxX, maxZ;
            Vector3 location = new Vector3();
            foreach (IDrawable I in list)
            {
                if (((Object3D)I).Name.CompareTo("ground") != 0 && !(I is MovableMesh3D))
                {
                    minX = I.Location.X - I.Radius - 10;
                    minZ = I.Location.Z - I.Radius - 10;
                    maxX = I.Location.X + I.Radius + 10;
                    maxZ = I.Location.Z + I.Radius + 10;

                    int x1 = toArrayIndex(minX);
                    int x2 = toArrayIndex(maxX);
                    int z1 = toArrayIndex(minZ);
                    int z2 = toArrayIndex(maxZ);

                    for (int i = x1; i < x2; i++)//for X = min to max && not out of bounds draw nodes
                        for (int j = z1; j < z2; j++)
                        {
                            location.X = i;
                            location.Z = j;

                            location = mapCoord(location);

                            location.Subtract(I.Location);

                            if (location.Length() <= I.Radius + 6)
                            {
                                graph[i, j].Navigable = false;
                            }
                        }
                    
                        
                }
            }
        }

        public virtual void draw()
        {
            int drawDist = 95;

            Matrix I;
            Microsoft.DirectX.Vector3 len = avatar.Location;
            //System.Drawing.Font myfont = new System.Drawing.Font("Arial", 1);
            using (Mesh potatoes = Mesh.Polygon(sw.Display, 10, 4))
            {
                Material m = new Material();

                sw.Display.Material = m;
                //Mesh potatoes = Mesh.TextFromFont(sw.Display, myfont, "BLAH", 0, 0.1f);
                Matrix temp = sw.Display.Transform.World;  // save Transform state
                //display.Transform.World = orientation;
                //if (Textured) display.SetTexture(0, Texture);
            

                float minX = avatar.Location.X - drawDist, maxX = avatar.Location.X + drawDist; // X-axis edge of drawing area for nodes
                float minZ = avatar.Location.Z - drawDist, maxZ = avatar.Location.Z + drawDist; // Z-axis edge of drawing area for nodes



                for (int i = toArrayIndex(minX); i < toArrayIndex(maxX); i++)//for X = min to max && not out of bounds draw nodes
                {
                    for (int j = toArrayIndex(minZ); j < toArrayIndex(maxZ); j++) //for Z = min to max && not out of bounds draw nodes
                    {
                        if (i >= 0 && i < XCount && j >= 0 && j < ZCount)
                        {
                            I = Matrix.Identity;

                            I *= Matrix.RotationZ((float)Math.PI / 4f);

                            I *= Matrix.RotationX((float)-Math.PI / 2f);

                            I *= Matrix.Translation(mapCoord(new Vector3(i, 0, j)));


                            sw.Display.Transform.World = I;

                            len.Subtract(mapCoord(new Vector3(i, 0, j)));

                            if (!graph[i, j].Navigable)
                            {
                                m.Diffuse = Color.Black;
                                m.Emissive = Color.DarkGray;
                                sw.Display.Material = m;
                            }
                            else
                            {
                                if ((i + j) % 2 == 0)
                                {
                                    m.Emissive = Color.Red;
                                    m.Diffuse = Color.Red;
                                }
                                else
                                {
                                    m.Emissive = Color.Black;
                                    m.Diffuse = Color.Black;
                                }
                                sw.Display.Material = m;
                            }
                            potatoes.DrawSubset(0);
                        }
                    }
                }
                sw.Display.Transform.World = temp; // restore Transform state
            }
        }

    }
    class NavNode
    {
        private bool navigable;

        public NavNode()
        {
            navigable = true;
        }

        public bool Navigable
        {
            get
            {
                return navigable;
            }
            set
            {
                navigable = value;
            }
        }
    }


}
