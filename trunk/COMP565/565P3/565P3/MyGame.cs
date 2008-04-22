using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Game465P3
{
    public class MyGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public InputHandler input;

        protected World world;

        protected int timeMultiplierIndex = Settings.timeMultiplierDefaultIndex;

        int fps, frames;
        double lastTime;

        int width, height;

        protected SpriteBatch spriteBatch;
        protected SpriteFont font;

        public MyGame(bool profile)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            input = new InputHandler(this);
            world = new World(this, input);
            Components.Add(world);

            this.IsMouseVisible = true;
            this.InactiveSleepTime = new TimeSpan(2500000);
            this.Window.AllowUserResizing = true;

            if (profile)
            {
                this.IsFixedTimeStep = !this.IsFixedTimeStep;
                graphics.SynchronizeWithVerticalRetrace = !graphics.SynchronizeWithVerticalRetrace;
            }
        }

        protected override void Initialize()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            pp.AutoDepthStencilFormat = DepthFormat.Depth16;
            pp.EnableAutoDepthStencil = true;

            graphics.PreferMultiSampling = true;
            GraphicsDevice.SamplerStates[0].MaxAnisotropy = GraphicsDevice.GraphicsDeviceCapabilities.MaxAnisotropy;
            GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Anisotropic;
            GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Anisotropic;
            GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Anisotropic;
#if !XBOX360
            graphics.PreferredBackBufferWidth = Settings.initWidth;
            graphics.PreferredBackBufferHeight = Settings.initHeight;
#endif
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Miramonte");
        }

        protected override void Dispose(bool disposing)
        {
            spriteBatch.Dispose();
            base.Dispose(disposing);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            input.update();

            // Allows the game to exit
            if (input.IsKeyDown(Settings.quit) || input.IsButtonPressed(Buttons.Back))
                this.Exit();

            // Control time rate
            if (input.IsKeyPressed(Settings.timeSlowDown))
            {
                timeMultiplierIndex--;
                if (timeMultiplierIndex < 0)
                    timeMultiplierIndex = 0;
                this.TargetElapsedTime = new TimeSpan(10000000 / (int)(60 * Settings.timeMultipliers[timeMultiplierIndex]));
            }
            if (input.IsKeyPressed(Settings.timeSpeedUp))
            {
                timeMultiplierIndex++;
                if (timeMultiplierIndex == Settings.timeMultipliers.Length)
                    timeMultiplierIndex--;
                this.TargetElapsedTime = new TimeSpan(10000000 / (int)(60 * Settings.timeMultipliers[timeMultiplierIndex]));
            }

            // Toggle max FPS
            if (input.IsKeyPressed(Settings.uncapFPS))
            {
                this.IsFixedTimeStep = !this.IsFixedTimeStep;
                graphics.SynchronizeWithVerticalRetrace = !graphics.SynchronizeWithVerticalRetrace;
                graphics.ApplyChanges();
            }

            // Toggle fullscreen
            if (input.IsKeyPressed(Keys.Enter) && (input.IsKeyDown(Keys.LeftAlt) || input.IsKeyDown(Keys.RightAlt)))
            {
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = width;
                    graphics.PreferredBackBufferHeight = height;
                }
                else
                {
                    width = Window.ClientBounds.Width;
                    height = Window.ClientBounds.Height;
                    graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                }
                graphics.ToggleFullScreen();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ShowText(gameTime);
        }

        protected void ShowText(GameTime gameTime)
        {
            frames++;
            double now = gameTime.TotalGameTime.TotalMilliseconds;
            if (now > lastTime + 1000)
            {
                fps = (int)Math.Round(frames * 1000 / (now - lastTime));
                lastTime = now;
                frames = 0;
            }

            spriteBatch.Begin();

            printString(spriteBatch, "" + fps, 0);

            if (timeMultiplierIndex != Settings.timeMultiplierDefaultIndex)
                printString(spriteBatch, Settings.timeMultipliers[timeMultiplierIndex] + "x", 1);

            if (world.currentCamera is LinkedCamera)
            {
                printString(spriteBatch, string.Format("V: {0:f0}", world.avatar.Velocity * 60), 3);
            }

            spriteBatch.End();
        }

        protected void printMatrix(SpriteBatch sb, Object3D o, int line)
        {
            Matrix m = o.transform;
            printString(sb, string.Format("R: {0:f3} {1:f3} {2:f3} {3:f3}", m.M11, m.M12, m.M13, m.M14), line);
            printString(sb, string.Format("U: {0:f3} {1:f3} {2:f3} {3:f3}", m.M21, m.M22, m.M23, m.M24), line + 1);
            printString(sb, string.Format("A: {0:f3} {1:f3} {2:f3} {3:f3}", m.M31, m.M32, m.M33, m.M34), line + 2);
            printString(sb, string.Format("L: {0:f3} {1:f3} {2:f3} {3:f3}", m.M41, m.M42, m.M43, m.M44), line + 3);
        }

        public static string formatVector(Vector3 v)
        {
            return string.Format("{0:f3} {1:f3} {2:f3}", v.X, v.Y, v.Z);
        }

        protected void printString(SpriteBatch sb, string s, int line)
        {
            sb.DrawString(font, s, new Vector2(3, line * 18), Color.White);
        }
    }
}
