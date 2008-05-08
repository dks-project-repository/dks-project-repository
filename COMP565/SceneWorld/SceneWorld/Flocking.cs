using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class Flock
    {
        private List<Boid> boids;
        private float forceWeight;
        private float directionWeight;
        private float avatarWeight;
        private float crossoverRadius;
        private float blindspot;
        private Avatar avatar;

        public Flock(SceneWorld sw, string label, string meshFile, int numBoids, float blindspot, float forceWeight, float directionWeight, float crossoverRadius, float avatarWeight)
        {
            avatar = sw.avatar;
            this.blindspot = blindspot;
            this.forceWeight = forceWeight;
            this.directionWeight = directionWeight;
            this.avatarWeight = avatarWeight;
            this.crossoverRadius = crossoverRadius;
            Random rand = new Random();
            boids = new List<Boid>();
            for (int i = 0; i < numBoids; i++)
            {
                Vector3 blah = new Vector3((float)(rand.NextDouble() * 2 - 1), 0, (float)(rand.NextDouble() * 2 - 1));
                boids.Add(new Boid(sw, label + i, avatar.Location + blah * (crossoverRadius * 4), avatar.Up, (float)rand.NextDouble(), meshFile, this));
            }
        }


        public void doFlock()
        {
            foreach (Boid b in boids)
            {
                b.At = b.At + forceWeight * getForces(b) + directionWeight * getAverageDirection(b);
                b.At = Vector3.Normalize(b.At);
                b.Right = Vector3.Cross(b.Up, b.At);
                b.Steps = 1;
                b.move();
            }
        }

        public Vector3 getAverageDirection(Boid a)
        {
            Vector3 temp = new Vector3();
            foreach (Boid b in boids)
                if (a != b && a.isVisible(b))
                    temp += b.At * (1 / (a.Location - b.Location).Length());
            return Vector3.Normalize(temp);
        }

        public float Blindspot
        {
            get { return blindspot; }
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

        public Vector3 getForces(Boid a)
        {
            Vector3 temp = new Vector3();
            Vector3 diff;
            foreach (Boid b in boids)
            {
                if (a != b && a.isVisible(b))
                {
                    diff = b.Location - a.Location;
                    temp += Vector3.Normalize(diff) * (diff.Length() - crossoverRadius);
                }
            }
            diff = Location - a.Location;
            temp += Vector3.Normalize(diff) * (diff.Length() * avatarWeight - crossoverRadius);
            return Vector3.Normalize(temp);
        }
    }

    public class Boid : MovableMesh3D
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
    }
}
