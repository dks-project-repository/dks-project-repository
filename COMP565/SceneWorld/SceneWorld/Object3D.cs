using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{

    public abstract class Object3D
    {
        /// <summary>
        /// </summary>
        /// <remarks>Position in scene</remarks>
        protected string name;     // string identifier
        protected static int count = 0;  // count for unique object id
        protected int id;                // unique identifier
        protected Vector3 location;      // object's position
        protected Matrix orientation;    // object's orientation 
        protected SceneWorld scene;
        protected Vector3 orientationAxis;  // initial rotation axis on placement
        protected float orientationRadians; // initial rotation on orientationAxiz
        // constructors and initialize method

        protected void initializeObject3D(SceneWorld sw, Vector3 pos, Vector3 orientAxis, float radians)
        {
            scene = sw;
            id = count++;
            Trace = String.Format("Loaded  {0:D2}  {1}", id, Name);
            location = pos;
            orientationAxis = orientAxis;
            orientationRadians = radians;
            orientation = Matrix.Identity;

            orientation *= Matrix.RotationAxis(orientationAxis, orientationRadians);
            // update location value when object is oriented on loading
            //if (this is ModeledMesh3D) location.TransformCoordinate(orientation);
            orientation.M41 = location.X;
            orientation.M42 = location.Y;
            orientation.M43 = location.Z;
        }

        public Object3D(SceneWorld sw)
        {
            name = "unknown";
            initializeObject3D(sw, new Vector3(), new Vector3(), 0.0f);
        }

        public Object3D(SceneWorld sw, Vector3 pos, string label)
        {
            name = label;
            initializeObject3D(sw, pos, new Vector3(), 0.0f);
        }

        public Object3D(SceneWorld sw, Vector3 position, Vector3 orientAxis,
           float radians)
        {
            name = "unknown";
            initializeObject3D(sw, position, orientAxis, radians);
        }

        public Object3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians)
        {
            name = label;
            initializeObject3D(sw, position, orientAxis, radians);
        }

        // Properties

        public Matrix Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public Vector3 Location
        {
            get { return location; }
            set
            {
                location = value;
                orientation.M41 = location.X;  // update matrix position info also
                orientation.M42 = location.Y;
                orientation.M43 = location.Z;
            }
        }

        protected string Trace { set { scene.Trace = value; } }  // update scene.info's trace field.

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected int Id
        {
            get { return id; }
        }


        public Vector3 Right
        {
            get
            {
                Vector3 vec;
                vec.X = orientation.M11; //return right; }
                vec.Y = orientation.M12;
                vec.Z = orientation.M13;
                return vec;
            }
            set
            {
                orientation.M11 = value.X;
                orientation.M12 = value.Y;
                orientation.M13 = value.Z;
            }
        }

        public Vector3 Up
        {
            get
            {
                Vector3 vec;
                vec.X = orientation.M21;
                vec.Y = orientation.M22;
                vec.Z = orientation.M23;
                return vec;
            }
            set
            {
                orientation.M21 = value.X;
                orientation.M22 = value.Y;
                orientation.M23 = value.Z;
            }
        }

        public Vector3 At
        {
            get
            {
                Vector3 vec;
                vec.X = orientation.M31;
                vec.Y = orientation.M32;
                vec.Z = orientation.M33;
                return vec;
            }
            set
            {
                orientation.M31 = value.X;
                orientation.M32 = value.Y;
                orientation.M33 = value.Z;
            }
        }

    }
}
