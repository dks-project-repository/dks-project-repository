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
        protected Matrix[] boneTransforms;
        protected World game;
        public bool IsDrawn = true;
        
        // Bounding box drawing stuff
        protected static BasicEffect basicEffect;
        protected static short[] boxIndices = { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };

        public Drawable(World g, Vector3 location, Model m)
            : base(location)
        {
            game = g;

            model = m;
            boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            if (model.Tag is BoundingBox)
            {
                modelBounds = (BoundingBox)model.Tag;
                bounds = transformBounds();
            }

            if (basicEffect == null)
                basicEffect = new BasicEffect(game.GraphicsDevice, null);
        }

        public void draw()
        {
            if (IsDrawn)
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = boneTransforms[mesh.ParentBone.Index] * transform;
                        effect.View = game.currentCamera.transform;
                        effect.Projection = game.projection;
                    }
                    mesh.Draw();
                }

            if (game.drawBoundingBoxes)
                drawBoundingBox();
        }

        protected void drawBoundingBox()
        {
            if (bounds.Min == bounds.Max)
                return;

            basicEffect.World = Matrix.Identity;
            basicEffect.View = game.currentCamera.transform;
            basicEffect.Projection = game.projection;

            Vector3[] corners = bounds.GetCorners();
            VertexPositionColor[] vertices = new VertexPositionColor[8];
            for (int i = 0; i < 8; i++)
            {
                vertices[i] = new VertexPositionColor(corners[i], Color.White);
            }

            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 8, boxIndices, 0, 12);
                pass.End();
            }
            basicEffect.End();
        }
    }
}
