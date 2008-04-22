using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public class Projectile : Movable
    {
        Avatar owner;

        public Projectile(World game, Vector3 location, Model model)
            : base(game, location, model)
        { }

        public override void handleCollision(Vector3 velocBefore, Vector3 velocAfter)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
