using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game465P3
{
    public abstract class Object3D
    {
        public Matrix transform;
        public BoundingBox modelBounds;
        public BoundingBox bounds;

        public Object3D()
        {
            transform = Matrix.Identity;
        }

        public Object3D(Vector3 location)
        {
            transform = Matrix.Identity;
            transform.Translation = location;
        }

        protected BoundingBox calculateBounds()
        {
            Vector3[] corners = modelBounds.GetCorners();
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = Vector3.Transform(corners[i], transform);
            }
            return BoundingBox.CreateFromPoints(corners);
        }

        //protected BoundingSphere calculateSphere()
        //{
        //    Vector3 center = (bounds.Min + bounds.Max) / 2 + transform.Translation;
        //    float radius = ((bounds.Max - bounds.Min) / 2).Length();
        //    return new BoundingSphere(center, radius);
        //}

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
