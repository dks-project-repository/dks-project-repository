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
            game.add(this);
        }

        public override void handleCollision(Vector3 velocBefore, Vector3 velocAfter)
        {
            Console.WriteLine("Projectile " + this.GetHashCode() + " removed.");
            game.remove(this);
        }

        public override void update()
        {
            base.update();

            int x, z;
            game.terrain.edgeTest(transform.Translation, out x, out z);
            if (x != 0 || z != 0 || transform.Translation.Y > Settings.ceiling)
            {
                Console.WriteLine("Projectile " + this.GetHashCode() + " removed.");
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
        public LobProjectile(World game, Model model, Avatar owner)
            : base(game, model, owner)
        {
            velocity = owner.actualAt * Settings.velocityStraightProjectile + owner.velocity;
            acceleration = new Vector3(0, Settings.gravity, 0);
        }
    }
}
