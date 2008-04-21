using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{

    /// <summary>
    /// Defines the move() method for moving a mesh.  
    /// Every call to move() will also call updateCameras() for MovableMeshes. 
    /// Movement properties are set from the user (via a World object).
    /// Animations (not implemented) could send updates to Movement properties.
    /// </summary>

    public class MovableMesh3D : ModeledMesh3D
    {
        protected Random random = new Random();
        protected int pitch = 0, roll = 0, yaw = 0;
        protected int steps = 0, vertical = 0;

        protected Vector3 verticalOffset = new Vector3(0.0f, 1.0f, 0.0f);
        protected float angle = .01f; // radians to rotate each frame  

        protected IndexPair lastNode, currNode, nextNode;
        protected int nodeDir, nextNodeDir, numStepsToNode, currStepToNode;
        protected Vector3 hermitePos1, hermiteTan1, hermitePos2, hermiteTan2;

        // Constructors and initialize method

        public void initialize()
        {
            Right = new Vector3(1.0f, 0.0f, 0.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            At = new Vector3(0.0f, 0.0f, 1.0f);
            pitch = roll = yaw = 0;
            steps = vertical = 0;
            initNodeTravel();
        }

        public void initNodeTravel()
        {
            currNode = NavGraph.indexFromLocation(Location);
            Location = NavGraph.locationFromIndex(currNode); // snap to grid
            nodeDir = NavGraph.directionFromVector(At);
            lastNode = NavGraph.indexAt(currNode, (nodeDir + 4) % 8);
            nextNode = currNode;
            currStepToNode = numStepsToNode = 0;
        }

        public MovableMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, string meshFile)
            : base(sw, label, position, orientAxis, radians, meshFile)
        {
            initialize();
        }

        public MovableMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, string meshFile, string textureFile)
            : base(sw, label, position, orientAxis, radians, meshFile, textureFile)
        {
            initialize();
        }

        // Properties

        protected float OrientationRadians
        {
            get { return orientationRadians; }
            set { orientationRadians = value; }
        }

        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        public int Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }

        public int Roll
        {
            get { return roll; }
            set { roll = value; }
        }

        public int Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }

        public int Vertical
        {
            get { return vertical; }
            set { vertical = value; }
        }

        // Methods

        public void reset()
        {
            initialize();
        }

        /// <summary>
        /// Used in moving to change values of orientation vectors
        /// </summary>
        /// <param name="axes"> Axis to rotate on</param>
        /// <param name="v1"> Axis to update</param>
        /// <param name="v2"> Second axis to update</param>
        /// <param name="radians"> rotation amount</param>
        public void rotate(Vector3 axes, ref Vector3 v1, ref Vector3 v2, float radians)
        {
            Matrix matrix = Matrix.Identity;  // temporary matrix
            matrix *= Matrix.RotationAxis(axes, radians);
            orientation *= matrix;
            v1.TransformCoordinate(matrix);
            v2.TransformCoordinate(matrix);
        }

        /// <summary>
        /// steps represents the speed of forward or backward movement.
        /// pitch, roll, and yaw are the number of radians to rotate.
        /// These values can be incremented / decremented by complimentary arrow key presses.
        /// </summary>      
        public virtual void move()
        {
            if (alternateMoveMode)
            {
                move2();
                return;
            }

            // need temporary vectors since Properties can't be passed as ref arguments
            Vector3 right = Right, up = Up, at = At, position = Location, positionOld = new Vector3();
            positionOld.Add(Location);

            if (pitch > 0)
            {
                rotate(right, ref up, ref at, angle);
                pitch = 1;
            }
            else if (pitch < 0)
            {
                rotate(right, ref up, ref at, -angle);
                pitch = -1;
            }

            if (yaw > 0)
            {
                rotate(up, ref right, ref at, -angle);
                yaw = 1;
            } // 1       
            else if (yaw < 0)
            {
                rotate(up, ref right, ref at, angle);
                yaw = -1;
            } // -1      

            if (roll > 0)
            {
                rotate(at, ref up, ref right, angle);
                roll = 1;
            }
            else if (roll < 0)
            {
                rotate(at, ref up, ref right, -angle);
                roll = -1;
            }

            if (vertical > 0)
            {
                position += verticalOffset;   // up 
                vertical = 1;
            }
            else if (vertical < 0)
            {
                position -= verticalOffset;   // down
                vertical = -1;
            }

            if (steps > 0)
            {
                position += at;   // forward 
                steps = 1;
            }
            else if (steps < 0)
            {
                position -= at;   // backward
                steps = -1;
            }

            // Stay on map
            position.X = Math.Min(Math.Max(position.X, -2000), 2000);
            position.Y = Math.Min(Math.Max(position.Y, -2000), 2000);
            position.Z = Math.Min(Math.Max(position.Z, -2000), 2000);

            if (scene.NavGraph.nodeAt(position) == null)
            {
                position = scene.NavGraph.nextPosition(positionOld, position);
            }

            // update properties and cameras
            //Right = right; Up = up; At = at;
            Location = position;
        }

        private void move2()
        {
            if (currStepToNode >= numStepsToNode)
            {
                // figure out next movement
                lastNode = currNode;
                currNode = nextNode;

                if (yaw > 0)
                {
                    yaw = 1;
                }
                else if (yaw < 0)
                {
                    yaw = -1;
                }

                // handle direction change
                nodeDir = nextNodeDir;
                nextNodeDir = (nodeDir + yaw + 8) % 8;

                // select next node
                if (steps > 0)
                {
                    steps = 1;
                    nextNode = scene.NavGraph.nextIndex(currNode, nextNodeDir);
                }
                else if (steps < 0)
                {
                    steps = -1;
                    nextNode = scene.NavGraph.nextIndex(currNode, (nextNodeDir + 4) % 8);
                }

                NavGraph.calcHermiteArgs(lastNode, currNode, nextNode, out hermitePos1, out hermiteTan1,
                    out hermitePos2, out hermiteTan2);

                // calculate number of steps to next node
                if (currNode != nextNode)
                {
                    if (nodeDir % 2 == 0)
                    {
                        numStepsToNode = 29;
                    }
                    else
                    {
                        numStepsToNode = 41;
                    }
                }
                else
                {
                    numStepsToNode = 0;
                }
                currStepToNode = 0;

                //if (!(this is NPAvatar))
                //Trace = string.Format("{0:f} {1:f} {2:f} : {3}", location.X, location.Y, location.Z, nodeDir);
            }

            // now do the actual movement for this tick

            float partComplete = currStepToNode / (float)numStepsToNode;

            // logit to cancel out ease-in ease-out of hermite
            partComplete = 9f / 11f * partComplete + 1f / 11f;
            partComplete = (float)Math.Log10(partComplete / (1 - partComplete)) / 2f + .5f;



            if (currNode != nextNode)
            {
                Vector3 pos = Vector3.Hermite(hermitePos1, hermiteTan1, hermitePos2, hermiteTan2, partComplete);

                Vector3 right = new Vector3(1, 0, 0), at = new Vector3(0, 0, 1);
                Vector3 newAt = pos - Location;
                if (newAt.Length() != 0)
                {
                    rotate(Up, ref right, ref at, -NavGraph.angleFromVector(newAt));
                }

                Right = right;
                At = at;
                Location = pos;
            }

            currStepToNode++;
        }
    }
}
