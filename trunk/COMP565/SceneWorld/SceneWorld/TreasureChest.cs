using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class TreasureChest
    {
        private Dictionary<IndexPair, ModeledMesh3D> meshes;
        private List<IndexPair> treasures;
        private List<IndexPair> collectedTreasures;
        private MeshData mesh;
        private static Material mat;
        private static Matrix matrix;
        private Device device;

        public TreasureChest(SceneWorld scene, int numTreasures)
        {
            device = scene.Display;
            matrix = Matrix.RotationX((float)-Math.PI / 2f) * Matrix.Translation(-2000, 10, -2000);

            mesh = new MeshData(scene.Display, "treasure.x");
            meshes = new Dictionary<IndexPair, ModeledMesh3D>();

            mat = new Material();
            mat.Emissive = System.Drawing.Color.White;

            treasures = new List<IndexPair>();
            collectedTreasures = new List<IndexPair>();
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
                            if (IndexPair.dist(existing, ip) <= 30)
                            {
                                allowed = false;
                                break;
                            }
                        }
                    }
                }
                meshes.Add(ip, new ModeledMesh3D(scene, "treasure", mesh));
                treasures.Add(ip);
            }
        }

        public void collectTreasure(IndexPair ip)
        {
            treasures.Remove(ip);
            collectedTreasures.Add(ip);
        }

        public IndexPair treasureWithin(Vector3 v, float dist)
        {
            dist /= 10;
            IndexPair ip = NavGraph.indexFromLocation(v);
            foreach (IndexPair t in treasures)
            {
                if (IndexPair.dist(t, ip) < dist)
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
                meshes[ip].Location = NavGraph.locationFromIndex(ip);
                meshes[ip].draw();
            }
            device.Transform.World = temp; // restore Transform state
        }
    }
}
