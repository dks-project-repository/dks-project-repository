using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public abstract class Movable : Drawable, Updatable
    {
        public Vector3 velocity;
        public float Velocity { get { return velocity.Length(); } }
        protected Vector3 acceleration;
        protected Vector3 tractionForce;
        protected bool skiing;
        protected static readonly Vector3 gravity = new Vector3(0, -Settings.gravity, 0);

        public Movable(World game, Vector3 location, Model model)
            : base(game, location, model)
        {
        }

        public virtual void update()
        {
            // N.B. DO NOT modify transform.Translation of a movable outside of this method. It will break the bounding box.

            // Calculate velocity
            velocity += acceleration;
            float v = velocity.Length();
            if (v != 0)
                velocity -= Settings.drag * v * v * Vector3.Normalize(velocity);

            // Calculate position
            Vector3 location = transform.Translation;
            float intersectDist;
            Vector3 intersectPos;
            Vector3 intersectNormal;
            float travelAmountLeft = 1;
            while (travelAmountLeft > 0)
            {
                Vector3 newLocation = location + velocity * travelAmountLeft;

                // Traction & friction (on ground only)
                if (game.terrain.onGround(location))
                {
                    Vector3 normal = game.terrain.GetNormal(location);
                    float normalForce = Object3D.project(Vector3.Down, normal).Length();

                    // Traction
                    if (tractionForce.LengthSquared() != 0)
                    {
                        if (!skiing) // Normal movement
                            velocity += Object3D.orthogonalize(tractionForce * (Settings.walkSpeed * normalForce), normal);
                        else
                        {
                            // Allow slight lateral movement while skiing
                            // First get a vector normal to velocity
                            Vector3 strafe = Vector3.Normalize(Vector3.Cross(velocity, Vector3.Up));
                            // Then figure out magnitude of lateral movement (dependent on velocity & avatar orientation wrt velocity)
                            Vector3 project = Object3D.project(velocity, transform.Forward);
                            float projectMultiplier = project.Length() * (project.Z < 0 ? 1 : -1);
                            strafe *= (tractionForce.X > 0 ? 1 : -1) * Settings.skiStrafeProportion * projectMultiplier;
                            velocity += strafe;
                        }
                    }

                    // Friction
                    if (skiing)
                        velocity *= 1 - normalForce * Settings.frictionSki;
                    else
                    {
                        velocity -= gravity;
                        velocity *= 1 - normalForce * Settings.frictionKinetic;
                        v = velocity.Length();
                        velocity += gravity;

                        if (v < Settings.frictionStaticCutoff && normalForce > Settings.frictionSlopeCutoff)
                            velocity = Vector3.Zero;
                    }
                }

                if (game.terrain.collider.PointIntersect(location, newLocation, out intersectDist, out intersectPos, out intersectNormal))
                {
                    // Subtract amount we traveled from TAL
                    float travelAmount = intersectDist / v;
                    travelAmountLeft -= travelAmount;

                    // The offset is because we won't collide next tick if we're exactly on the terrain
                    newLocation = intersectPos + Settings.collisionHeightDisplacement * intersectNormal;

                    // avoid slipping from the intersectNormal adjustment
                    if ((location - newLocation).Length() > Settings.collisionHeightDisplacement * 2)
                    {
                        location = newLocation;
                    }

                    if (handleCollision(intersectNormal))
                        break;
                }
                else
                {
                    // No collision!
                    location = newLocation;

                    travelAmountLeft = 0;
                }
            }

            // Stay on the map
            int x = 0, z = 0;
            game.terrain.clamp(ref location, out x, out z);
            if (x != 0)
                velocity.X = 0;
            if (z != 0)
                velocity.Z = 0;

            // Don't fall through the ground
            float terrainY = game.terrain.GetHeight(location) + Settings.collisionHeightDisplacement;
            if ((x != 0 || z != 0) && game.terrain.onGround(location) || terrainY > location.Y)
                location.Y = terrainY;

            game.oct.Move(this, location - transform.Translation);
            transform.Translation = location;
        }

        public abstract bool handleCollision(Vector3 normal);
    }
}
