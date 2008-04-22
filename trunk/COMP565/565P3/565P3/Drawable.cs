using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public class Drawable : Object3D
    {
        protected Model model;
        protected Matrix[] transforms;
        protected World game;
        public bool IsDrawn = true;

        public Drawable(World g, Vector3 location, Model m)
            : base(location)
        {
            game = g;

            model = m;
            transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
        }

        public void draw()
        {
            if (IsDrawn)
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = transforms[mesh.ParentBone.Index] * transform;
                        effect.View = game.currentCamera.transform;
                        effect.Projection = game.projection;
                    }
                    mesh.Draw();
                }
        }
    }
}
