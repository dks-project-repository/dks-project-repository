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
        private MeshData mesh;
        private static Matrix matrix;
        private Device device;
        private int chaserCount, evaderCount;
        private SceneWorld sw;

        public TreasureChest(SceneWorld scene, int numTreasures)
        {
            sw = scene;
            device = scene.Display;
            matrix = Matrix.RotationX((float)-Math.PI / 2f) * Matrix.Translation(-2000, 10, -2000);
            mesh = new MeshData(scene.Display, "treasure.x");
            treasuresList = new Dictionary<IndexPair, ModeledMesh3D>();
            treasures = new List<IndexPair>();
            collectedTreasures = new List<IndexPair>();
            Random r = new Random();
            for(int i = 0; i < numTreasures; i++)
            {
                IndexPair ip = null;
                Vector3 loc;
                bool allowed = false;
                while(!allowed)
                {
                    ip = new IndexPair(r.Next(401), r.Next(401));
                    if(scene.NavGraph.isTraversable(ip))
                    {
                        allowed = true;

                        foreach(IndexPair existing in treasures)
                        {
                            if(IndexPair.dist(existing, ip) <= 30)
                            {
                                allowed = false;
                                break;
                            }
                        }
                    }
                }
                loc = NavGraph.locationFromIndex(ip);
                treasuresList.Add(ip, new ModeledMesh3D(scene, "treasure", loc, new Vector3(0, 1, 0), 0, mesh));
                Console.WriteLine(loc.X + "," + loc.Z);
                treasures.Add(ip);
            }
        }

        public void collectTreasure(IndexPair ip, string name)
        {
            if(name.CompareTo("Chaser") == 0)
            {
                chaserCount++;
                treasures.Remove(ip);
                collectedTreasures.Add(ip);
                sw.Trace = "Chaser collects a treasure: " + chaserCount + " Collected out of 4";
            }
            if(name.CompareTo("Evader") == 0)
            {
                evaderCount++;
                collectedTreasures.Add(ip);
                treasures.Remove(ip);
                sw.Trace = "Evader collects a treasure: " + evaderCount + " Collected out of 4";
            }
            if(chaserCount > treasuresList.Count / 2)
                sw.Trace = "Chaser is victorious";
            if(evaderCount > treasuresList.Count / 2)
                sw.Trace = "Evader is victorious";
        }

        public IndexPair treasureWithin(Vector3 v, float dist, string name)
        {
            dist /= 10;
            IndexPair ip = NavGraph.indexFromLocation(v);
            foreach(IndexPair t in treasures)
            {
                if(IndexPair.dist(t, ip) < dist)
                {
                    collectTreasure(ip, name);
                    return t;
                }
            }
            return null;
        }

        public void draw()
        {
            foreach(ModeledMesh3D m in treasuresList.Values)
                m.draw();
        }
    }
}
