using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class Flock
    {
        List<Boid> boids;
        private float cohesionWeight = 1;
        private float directionWeight = 1;
        private const float force = 10;
        private float blindspot;
        private float cohesion;
        private float repulsion;
        private Avatar avatar;
        //private Vector3 location;



        public Flock(SceneWorld sw, string label, string meshFile, float blindSpot, float cohesionRadius, float repulsionRadius)
        {
            avatar = sw.avatar;
            blindspot = blindSpot;
            cohesion = cohesionRadius;
            repulsion = repulsionRadius;
            Random rand = new Random();
            boids = new List<Boid>();
            for (int i = 0; i < 5; i++)
            {
                Vector3 offset = new Vector3((float)rand.Next(), 0, (float)rand.Next());
                Console.WriteLine(offset);
                boids.Add(new Boid(sw, label + i, avatar.Location + offset, avatar.At, 0.0f, meshFile, this));
            }
        }


        public void doFlock()
        {
            foreach (Boid b in boids)
            {
                b.At = cohesionWeight * (avatar.Location - b.Location) + directionWeight * b.At + b.getRepulsion() + b.getCohesion();
                b.At = Vector3.Normalize(b.At);
                b.Right = Vector3.Cross(b.Up, b.At);
            }
        }

        private Vector3 getAverageDirection(Boid a)
        {
            Vector3 temp = new Vector3();
            foreach (Boid b in boids)
                if (a.isVisible(b))
                    temp += b.At;
            return Vector3.Scale(temp, 1f / boids.Count);
        }

        public float CohesionWeight
        {
            get { return cohesionWeight; }
            set { cohesionWeight = value; }
        }
        public float DirectionWeight
        {
            get { return directionWeight; }
            set { directionWeight = value; }
        }
        public float Blindspot
        {
            get { return blindspot; }
            set { blindspot = value; }
        }
        public float Cohesion
        {
            get { return cohesion; }
            set { cohesion = value; }
        }
        public float Repulsion
        {
            get { return repulsion; }
            set { repulsion = value; }
        }

        public Vector3 Location
        {
            get { return avatar.Location; }
        }
        public void draw()
        {
            //if (autoMove && !(this is NPAvatar))
            foreach (Boid b in boids)
                b.draw();
        }


    }

    class Boid : Avatar
    {
        Flock flock;

        public Boid(SceneWorld sw, string label, Vector3 pos, Vector3 orientAxis,
      float radians, string meshFile, Flock f)
            : base(sw, label, pos, orientAxis, radians, meshFile)
        {
            flock = f;
        }


        public bool isVisible(Boid b)
        {
            //? is it necessary for both vectors to be normalized?
            if (Vector3.Dot(At, Vector3.Normalize(b.Location - Location)) >= Math.Cos((double)180 - (flock.Blindspot / 2))) return true;
            return false;
        }

        public Vector3 getCohesion()
        {
            Vector3 temp = flock.Location - Location;
            if (Vector3.Length(temp) <= flock.Cohesion)
                return temp;
            return new Vector3(0, 0, 0);
        }

        public Vector3 getRepulsion()
        {
            Vector3 temp = Location - flock.Location;
            if (Vector3.Length(temp) <= flock.Repulsion)
                return temp;
            return new Vector3(0, 0, 0);
        }
    }
}
