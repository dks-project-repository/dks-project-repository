using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game465P3
{
    public abstract class Camera : Object3D, Updatable
    {
        protected World game;
        protected float yaw, pitch = MathHelper.PiOver2;
        public Vector3 position; // world coordinates

        public Camera(World g)
        {
            game = g;
        }

        public virtual void update()
        {
            game.cameraFrustum = new BoundingFrustum(transform * game.projection);
        }
    }
}
