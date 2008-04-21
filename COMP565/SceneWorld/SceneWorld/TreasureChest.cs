using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class TreasureChest
    {
        private List<IndexPair> treasures;
        private static Mesh mesh;
        private static Material mat;
        private static Matrix matrix;
        private Device device;

        public TreasureChest(SceneWorld scene, int numTreasures)
        {
            device = scene.Display;
            matrix = Matrix.RotationX((float)-Math.PI / 2f) * Matrix.Translation(-2000, 10, -2000);

            mesh = Mesh.Sphere(device, 10, 8, 8);
            mat = new Material();
            mat.Emissive = System.Drawing.Color.White;

            treasures = new List<IndexPair>();
            Random r = new Random();
            for (int i = 0; i < numTreasures; i++)
            {
                IndexPair ip = null;
                bool allowed = false;
                while (!allowed)
                {
                    ip = new IndexPair(r.Next(401), r.Next(401));
                    if (scene.NavGraph.isTraversable(ip))
                    {
                        allowed = true;

                        foreach (IndexPair existing in treasures)
                        {
                            if ((existing - ip).Magnitude <= 30)
                            {
                                allowed = false;
                                break;
                            }
                        }
                    }
                }
                treasures.Add(ip);
            }
        }

        public IndexPair treasureWithin(Vector3 v, float dist)
        {
            IndexPair ip = NavGraph.indexFromLocation(v);
            foreach (IndexPair t in treasures)
            {
                if ((t - ip).Magnitude < dist / 10)
                    return t;
            }
            return null;
        }

        public void draw()
        {
            Matrix temp = device.Transform.World;  // save Transform state
            device.Material = mat;
            foreach (IndexPair ip in treasures)
            {
                device.Transform.World = matrix * Matrix.Translation(ip.x * 10, 0, ip.z * 10);
                mesh.DrawSubset(0);
            }
            device.Transform.World = temp; // restore Transform state
        }
    }
}
