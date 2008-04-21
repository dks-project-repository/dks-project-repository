using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class NPAvatar : Avatar
    {
        private int remoteX, remoteY, remoteTurns = 0;
        private Vector3 oldPos;

        // Constructor

        public NPAvatar(SceneWorld sw, string label, Vector3 pos, Vector3 orientAxis,
           float radians, string meshFile)
            : base(sw, label, pos, orientAxis, radians, meshFile)
        {  // change names for on-screen display of current camera
            firstPerson.Name = "npFirst ";
            follow.Name = "npFollow";
            top.Name = "npTop";
        }

        // Methods 

        /// <summary>
        /// Set Steps and Yaws randomly for remote players. 
        /// This is a "temporary defintion" for NPC phases
        /// </summary>
        public override void move()
        {
            Avatar player = scene.avatar;
            Vector3 distance = Location - player.Location;
            IndexPair treasure = null;
            Vector3 posChange = Location - oldPos;
            oldPos = Location;

            if (distance.Length() < 500 && distance.LengthSq() != 0)
            {
                path.Clear();

                if (posChange.Length() < .9f)
                    collisionTurn();
                else
                {
                    yaw = 0;
                    At = Vector3.Normalize(distance);
                    Right = Vector3.Cross(Up, At);
                }

                steps += 1;
                oldPos = Location;
                base.move();

            }
            else if (path.Count > 0 || (treasure = scene.Treasures.treasureWithin(Location, 500)) != null)
            {
                if (++currStep == 12)
                    currStep = 0;

                if (path.Count == 0)
                {
                    Console.Write("Finding path to " + treasure + " (distance: " +
                        (IndexPair.dist(treasure, NavGraph.indexFromLocation(location)) * 10) + ")...");
                    AStar(this, treasure, 550);
                    Console.WriteLine("Done. Path length = " + path.Count);
                    currStep = 0;
                }

                followPath();
            }
            if (path.Count == 0)
            {
                remoteTurns++;
                if (posChange.Length() < .9f)
                    collisionTurn();
                else
                {
                    if (remoteTurns > 25)
                    {
                        remoteTurns = 0;
                        remoteX = random.Next(2);       // 0..1 move forward only;
                        remoteY = -1 + random.Next(3);   // turn left or right ;
                    }
                    steps += remoteX;
                    yaw += remoteY;         // always turn
                }
                base.move();
            }// now use MovableMesh's move via Avatar's move();
        }

        private void collisionTurn()
        {
            steps = 1;
            int dir = NavGraph.directionFromVector(At);
            IndexPair loc = NavGraph.indexFromLocation(Location);
            for (int i = 1; i <= 3; i++)
            {
                if (scene.NavGraph.isTraversable(NavGraph.indexAt(loc, NavGraph.checkDir(dir - i))))
                {
                    yaw = -1;
                    break;
                }
                else if (scene.NavGraph.isTraversable(NavGraph.indexAt(loc, NavGraph.checkDir(dir + i))))
                {
                    yaw = 1;
                    break;
                }
            }
        }
    }
}
