/*Daniel Frankel
 * Kevin Yedlin
 * Comp 565 - Project 1
 */

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
        protected SceneWorld sw;

        // Constructors and initialize method

        public void initialize()
        {
            Right = new Vector3(1.0f, 0.0f, 0.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            At = new Vector3(0.0f, 0.0f, 1.0f);
            pitch = roll = yaw = 0;
            steps = vertical = 0;
        }

        public MovableMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, string meshFile)
            : base(sw, label, position, orientAxis, radians, meshFile)
        {
            this.sw = sw;
            initialize();
        }

        public MovableMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, string meshFile, string textureFile)
            : base(sw, label, position, orientAxis, radians, meshFile, textureFile)
        {
            this.sw = sw;
            initialize();
        }

        // Properties

        protected float OrientationRadians
        {
            get
            {
                return orientationRadians;
            }
            set
            {
                orientationRadians = value;
            }
        }

        public int Steps
        {
            get
            {
                return steps;
            }
            set
            {
                steps = value;
            }
        }

        public int Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
            }
        }

        public int Roll
        {
            get
            {
                return roll;
            }
            set
            {
                roll = value;
            }
        }

        public int Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = value;
            }
        }

        public int Vertical
        {
            get
            {
                return vertical;
            }
            set
            {
                vertical = value;
            }
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
            // need temporary vectors since Properties can't be passed as ref arguments
            Vector3 right = Right, up = Up, at = At, position = Location;

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
            { // if @ not a bad node.
                Vector3 move = sw.Nav.nextMove(position, at);
                position += move;   // forward 
                steps = 1;
            }
            else if (steps < 0)
            { //if opposite of @ not a bad node.
                Vector3 move = sw.Nav.nextMove(position, at * -1);
                position += move;   // backward
                steps = -1;
            }

            // update properties and cameras
            Right = right;
            Up = up;
            At = at;
            Location = position;
        }

    }
}
