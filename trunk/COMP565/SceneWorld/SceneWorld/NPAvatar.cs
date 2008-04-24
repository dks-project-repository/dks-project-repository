using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class NPAvatar : Avatar
    {
        private int remoteX, remoteY, remoteTurns = 0;
        private Vector3 oldPos;
        public Vector3 rightFeeler, leftFeeler;
        Mesh tp;
        Material tpm;
        Matrix mr, ml;

        // Constructor

        public NPAvatar(SceneWorld sw, string label, Vector3 pos, Vector3 orientAxis,
           float radians, string meshFile)
            : base(sw, label, pos, orientAxis, radians, meshFile)
        {  // change names for on-screen display of current camera
            firstPerson.Name = "npFirst ";
            follow.Name = "npFollow";
            top.Name = "npTop";
            tp = Mesh.Teapot(sw.Display);
            tpm = new Material();
            tpm.Emissive = Color.SeaShell;
            mr = Matrix.RotationY(-45);
            ml = Matrix.RotationY(45);
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
            oldPos = Location;

            if (distance.Length() < 500 && distance.LengthSq() != 0)
            {
                path.Clear();
                if (!collisionTurn())
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
                updateCameras();
            }
            if (path.Count == 0)
            {
                remoteTurns++;
                //if (posChange.Length() < .9f)

                if (!collisionTurn())
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

        private bool collisionTurn()
        {
            //TODO: use quaternions
            rightFeeler = Vector3.TransformNormal(At, mr) * 10;
            leftFeeler = Vector3.TransformNormal(At, ml) * 10;
            bool left = scene.NavGraph.isTraversable(NavGraph.indexFromLocation(Location + rightFeeler));
            bool right = scene.NavGraph.isTraversable(NavGraph.indexFromLocation(Location + leftFeeler));
            bool fwd = scene.NavGraph.isTraversable(NavGraph.indexFromLocation(Location + At * 10));
            if (left && !right)
                yaw = 1;
            if (right && !left)
                yaw = -1;
            if (right && left)
            {
                if (fwd)
                    yaw = 0;
                else
                    yaw = -1;
            }
            return !right || !left;
        }

        public override void draw()
        {
            base.draw();
            Matrix temp = display.Transform.World;  // save Transform state
            display.Transform.World = orientation * Matrix.Translation(rightFeeler);
            display.SetTexture(0, null);
            display.Material = tpm;
            tp.DrawSubset(0);
            display.Transform.World = orientation * Matrix.Translation(leftFeeler);
            tp.DrawSubset(0);
            display.Transform.World = temp; // restore Transform state
        }
    }
}
