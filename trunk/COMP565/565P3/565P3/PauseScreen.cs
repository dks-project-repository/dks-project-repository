using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game465P3
{
    public class PauseScreen : DrawableGameComponent
    {
        protected InputHandler input;
        protected GameComponent parent;
        
        protected SpriteBatch spriteBatch;
        protected SpriteFont font;
        protected Texture2D pixel;
        protected Vector2 textCenter;
        protected byte alpha;

        public PauseScreen(DrawableGameComponent parent, InputHandler input)
            : base(parent.Game)
        {
            this.input = input;
            this.parent = parent;
            Enabled = false;
            Visible = false;
            DrawOrder = parent.DrawOrder + 10;
            Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            EnabledChanged += PauseScreen_EnabledChanged;
        }

        void PauseScreen_EnabledChanged(object sender, EventArgs e)
        {
            if (Enabled)
            {
                alpha = 0;
                parent.Enabled = false;
            }
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
            font = Game.Content.Load<SpriteFont>("Lindsey");
            textCenter = font.MeasureString("PAUSED") / 2;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Visible = true;

            if (input.IsKeyPressed(Settings.pause))
            {
                Visible = false;
                Enabled = false;
                parent.Enabled = true;
                input.update(); // otherwise it just pauses itself again
            }

            if (alpha < Settings.pauseBackgroundColor.A)
                alpha += 2;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Color bgColor;
            if (alpha < Settings.pauseBackgroundColor.A)
                bgColor = new Color(Settings.pauseBackgroundColor.R, Settings.pauseBackgroundColor.G, Settings.pauseBackgroundColor.B, alpha);
            else
                bgColor = Settings.pauseBackgroundColor;

            spriteBatch.Begin();
            Viewport v = GraphicsDevice.Viewport;
            spriteBatch.Draw(pixel, new Rectangle(0, 0, v.Width, v.Height), null, bgColor);
            spriteBatch.DrawString(font, "PAUSED", new Vector2(v.Width / 2 - textCenter.X, v.Height / 2 - textCenter.Y), Settings.pauseForegroundColor);
            spriteBatch.End();
        }
    }
}
