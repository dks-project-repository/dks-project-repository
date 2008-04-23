using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game465P3
{
    public abstract class Object3D
    {
        public Matrix transform;

        public Object3D()
        {
            transform = Matrix.Identity;
        }

        public Object3D(Vector3 location)
        {
            transform = Matrix.Identity;
            transform.Translation = location;
        }

        public static Vector3 rotate(Vector3 axes, Vector3 v1, float radians)
        {
            Quaternion q = Quaternion.CreateFromAxisAngle(axes, radians);
            return Vector3.Transform(v1, q);
        }

        public static Vector3 orthogonalize(Vector3 v, Vector3 normal)
        {
            // These two statements are equivalent... see http://www.euclideanspace.com/maths/geometry/elements/plane/lineOnPlane/
            return v - Vector3.Dot(v, normal) * normal;
            //return Vector3.Cross(normal, Vector3.Cross(v, normal));
        }

        public static Vector3 project(Vector3 v, Vector3 normal)
        {
            return Vector3.Dot(v, normal) * normal;
        }

        public static Vector3 orthonormalize(Vector3 v, Vector3 normal)
        {
            return Vector3.Normalize(orthogonalize(v, normal));
        }
    }
}