using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public class DamageScreen : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D pixel;
        protected byte alpha;

        public DamageScreen(DrawableGameComponent parent)
            : base(parent.Game)
        {
            DrawOrder = parent.DrawOrder + 5;
            Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            spriteBatch.Dispose();
            Game.Components.Remove(this);
            base.Dispose(disposing);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = Game.Content.Load<Texture2D>("pixel");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (alpha > 0)
                alpha--;
        }

        public void setDamage(float damage)
        {
            if (damage <= 0)
            {
                alpha = (byte)0;
                return;
            }

            int a = (int)(damage * 255 + 30);
            alpha = (byte)(a < 255 ? a : 255);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (alpha > 0)
            {
                Color bgColor = new Color(255, 0, 0, alpha);

                spriteBatch.Begin();
                Viewport v = GraphicsDevice.Viewport;
                spriteBatch.Draw(pixel, new Rectangle(0, 0, v.Width, v.Height), null, bgColor);
                spriteBatch.End();
            }
        }
    }
}