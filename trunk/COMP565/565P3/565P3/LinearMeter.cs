using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public class LinearMeter : DrawableGameComponent
    {
        public delegate float MeterValue();
        protected MeterValue value;
        protected Rectangle rectangle;
        protected SpriteBatch spriteBatch;
        protected Color color;
        protected Color bgColor;
        protected Texture2D pixel;
        protected int border;

        public LinearMeter(DrawableGameComponent parent, Rectangle rectangle, Color color, Color bgColor, int border, MeterValue value)
            : base(parent.Game)
        {
            this.value = value;
            this.border = border;
            this.bgColor = bgColor;
            this.color = color;
            this.rectangle = rectangle;

            DrawOrder = parent.DrawOrder + 3;
            Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
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

            pixel = Game.Content.Load<Texture2D>("pixel");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            int width = (int) (value() * (rectangle.Width - border * 2));
            Rectangle r = new Rectangle(rectangle.X + border, rectangle.Y + border, width, rectangle.Height - border * 2);

            spriteBatch.Draw(pixel, rectangle, bgColor);
            spriteBatch.Draw(pixel, r, color);

            spriteBatch.End();
        }
    }
}
