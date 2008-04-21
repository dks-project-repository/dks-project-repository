using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;


namespace SceneWorld
{
    class Path1
    {
        private List<Vector3> list;
        //private Avatar avatar;

        public List<Vector3> List
        {
            get { return list; }

            set { list = value; }
        }

        //public Path(Avatar a)
        //{
        //    avatar = a;
        //    path = new List<Vector3>();
        //}

        public Path1()
        {
            list = new List<Vector3>();
        }

        //public Vector3 nextMove()
        //{
        //    if (path.Count == 0) return avatar.At;
        //    if (path.Count >= 3)
        //        return Vector3.CatmullRom(avatar.Location, avatar.Location+avatar.At, path[0], path[1], 1 / 12) - avatar.Location;
        //    if (path.Count == 2)
        //        return Vector3.CatmullRom(avatar.Location, path[0], path[1], path[1], 1 / 12) - avatar.Location;
        //    else
        //        return path[0] - avatar.Location;
            
        //}

        


        //private int CheckAngle()// used to compare the avatar's current angle and 
        //    // ((float)Math.Acos(Vector3.Dot(avatar.At, new Vector3(1, 0, 0))))
        //{
        //    return getAngle(new Vector3(1, 0, 0)).CompareTo(getAngle(avatar.Location));
        //}

        //private int getAngle(Vector3 B)
        // //returns the angle an avatar must turn to face a given map coordinate
        // //EXPRESSION TRUNCATES ANGLE TO INT FOR COMPARISON
        //{
        //    Vector3 A = B - avatar.Location;
        //    A.Normalize();
        //    return (int)Math.Acos(Vector3.Dot(avatar.At, A));

        //}
    }
}
