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
            Vector3 v = owner.transform.Translation;
            v.Y += Settings.cameraHeight;
            transform.Translation = v;
            skiing = true;
            game.add(this);
        }

        public override bool handleCollision(Vector3 normal)
        {
            game.remove(this);
            return true;
        }

        public override void update()
        {
            base.update();

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
            if (bounces > 0)
            {
                bounces--;
                velocity = Object3D.rotate(normal, -velocity, MathHelper.Pi);
                velocity *= 1 - Settings.frictionKinetic;
                return false;
            }

            game.remove(this);
            return true;
        }
    }
}
