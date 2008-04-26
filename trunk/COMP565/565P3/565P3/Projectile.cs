using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public abstract class Projectile : Movable
    {
        Avatar owner;

        public Projectile(World game, Model model, Avatar owner)
            : base(game, Vector3.Zero, model)
        {
            this.owner = owner;
            transform = Matrix.CreateWorld(Vector3.Zero, owner.actualAt, Vector3.Up);
            Vector3 v = owner.transform.Translation + owner.actualAt * (owner.modelBounds.Max - owner.modelBounds.Min).Length() * 1.25f;
            v.Y += Settings.cameraHeight / 2;
            if (game.terrain.onGround(v))
                v.Y = game.terrain.GetHeight(v) + Settings.collisionHeightAboveTerrain;
            transform.Translation = v;
            bounds = transformBounds();
            game.oct.Add(this); //TODO: figure out if i want this line
            skiing = true;
            game.add(this);
        }

        public override bool handleCollision(Vector3 normal)
        {
            BoundingBox b = BoundingBox.CreateFromSphere(new BoundingSphere(transform.Translation, Settings.explosionRadius));
            foreach (Avatar a in game.oct.getAllWithin<Avatar>(b))
            {
                Vector3 aDiff = a.transform.Translation - transform.Translation;
                float aDist = aDiff.Length();
                if (aDist < Settings.explosionRadius)
                {
                    float damage = (Settings.explosionRadius - aDist) / Settings.explosionRadius * Settings.explosionDamage;
                    if (aDist != 0)
                        a.velocity += Vector3.Normalize(aDiff) * damage * Settings.explosionAccel;
                    a.Health -= damage;
                }
            }

            game.remove(this);
            return true;
        }

        public override void update()
        {
            Vector3 oldPos = transform.Translation;
            
            base.update();

            Avatar a = game.oct.intersection<Avatar>(oldPos, transform.Translation);
            if (a != null)
            {
                transform.Translation = a.transform.Translation;
                handleCollision(Vector3.Zero);
                return;
            }

            int x, z;
            game.terrain.edgeTest(transform.Translation, out x, out z);
            if (x != 0 || z != 0 || transform.Translation.Y > Settings.ceiling)
            {
                game.remove(this);
            }
        }
    }

    public class StraightProjectile : Projectile
    {
        public StraightProjectile(World game, Model model, Avatar owner)
            : base(game, model, owner)
        {
            velocity = owner.actualAt * Settings.velocityStraightProjectile + owner.velocity;
        }
    }

    public class LobProjectile : Projectile
    {
        private int bounces;

        public LobProjectile(World game, Model model, Avatar owner)
            : base(game, model, owner)
        {
            velocity = owner.actualAt * Settings.velocityLobProjectile + owner.velocity;
            acceleration = new Vector3(0, Settings.gravity, 0);
            bounces = Settings.lobBounces;
        }

        public override bool handleCollision(Vector3 normal)
        {
            if (bounces > 0 && normal != Vector3.Zero)
            {
                bounces--;
                velocity = Object3D.rotate(normal, -velocity, MathHelper.Pi);
                velocity *= 1 - Settings.frictionKinetic;
                return false;
            }

            return base.handleCollision(normal);
        }
    }
}
