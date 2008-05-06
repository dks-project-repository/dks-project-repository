using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class TreasureChest
    {
        private Dictionary<IndexPair, ModeledMesh3D> treasuresList;
        private List<IndexPair> treasures;
        private List<IndexPair> collectedTreasures;
        private MeshData uncollectedTreasure;
        private MeshData collectedTreasure;
        private static Matrix matrix;
        private Device device;
        private SceneWorld scene;
        private int treasureCount;

        public TreasureChest(SceneWorld scene, int numTreasures)
        {
            treasureCount = numTreasures;
            this.scene = scene;
            device = scene.Display;
            matrix = Matrix.RotationX((float)-Math.PI / 2f) * Matrix.Translation(-2000, 10, -2000);
            uncollectedTreasure = new MeshData(scene.Display, "treasure.x");
            collectedTreasure = new MeshData(scene.Display, "treasurecollected.x");
            treasuresList = new Dictionary<IndexPair, ModeledMesh3D>();
            treasures = new List<IndexPair>();
            collectedTreasures = new List<IndexPair>();
            Random r = new Random();
            for (int i = 0; i < treasureCount; i++)
            {
                IndexPair ip = null;
                Vector3 loc;
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
                loc = NavGraph.locationFromIndex(ip);
                treasuresList.Add(ip, new ModeledMesh3D(scene, "uncollected treasure", loc, new Vector3(0, 1, 0), 0, uncollectedTreasure));
                treasures.Add(ip);
            }
        }

        public void collectTreasure(IndexPair ip)
        {  
            collectedTreasures.Add(ip);
            treasures.Remove(ip);
            treasuresList[ip] = new ModeledMesh3D(scene, "collected treasure", NavGraph.locationFromIndex(ip), new Vector3(0, 1, 0), 0, collectedTreasure);
        }

        public IndexPair treasureWithin(Vector3 v, float dist)
        {
            dist /= 10;
            IndexPair ip = NavGraph.indexFromLocation(v);
            foreach (IndexPair t in treasures)
                if (IndexPair.dist(t, ip) < dist)
                    return t;
            return null;
        }

        public void draw()
        {
            foreach (ModeledMesh3D m in treasuresList.Values)
                m.draw();
        }

        public int TreasureCount
        {
            get { return treasureCount; }
        }
    }
}
