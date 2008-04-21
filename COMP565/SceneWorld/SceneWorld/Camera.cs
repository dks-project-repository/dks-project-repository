using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{

    public class Camera : Object3D
    {

        private Matrix viewMatrix;

        // Constructor

        public Camera(SceneWorld sc, string label, Vector3 pos, Vector3 orient,
           float radians)
            : base(sc, label, pos, orient, radians)
        {
            viewMatrix = new Matrix();
            setViewMatrix();
        }

        // Properties

        public Matrix ViewMatrix
        {
            get
            {
                setViewMatrix();
                return viewMatrix;
            }
            set { viewMatrix = value; }
        }

        // Methods

        public void setViewMatrix()
        {
            viewMatrix.M11 = Right.X; viewMatrix.M12 = Up.X;
            viewMatrix.M13 = At.X; viewMatrix.M14 = 0.0f;
            viewMatrix.M21 = Right.Y; viewMatrix.M22 = Up.Y;
            viewMatrix.M23 = At.Y; viewMatrix.M24 = 0.0f;
            viewMatrix.M31 = Right.Z; viewMatrix.M32 = Up.Z;
            viewMatrix.M33 = At.Z; viewMatrix.M34 = 0.0f;
            viewMatrix.M41 = -1.0f * Vector3.Dot(Location, Right);   // location.X; 
            viewMatrix.M42 = -1.0f * Vector3.Dot(Location, Up);      // location.Y; 
            viewMatrix.M43 = -1.0f * Vector3.Dot(Location, At);      // location.Z; 
            viewMatrix.M44 = 1.0f;
        }
    }


}
