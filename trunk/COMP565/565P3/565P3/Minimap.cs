using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public class Minimap : DrawableGameComponent
    {
        protected Texture2D texture;
        protected Rectangle rectangleMinimap;
        protected Rectangle rectangleFullscreen;
        protected SpriteBatch spriteBatch;
        protected float mapWidth, mapHeight;
        protected List<MinimapDrawable> sprites;
        protected Color shading;
        protected RenderFunction renderer;
        protected InputHandler input;
        public bool Fullscreen;

        protected struct MinimapDrawable
        {
            public MinimapDrawable(Texture2D texture, Vector3 location, Vector3 at)
            {
                this.texture = texture;
                this.location = location;
                this.at = at;
            }

            public Texture2D texture;
            public Vector3 location;
            public Vector3 at;
        }

        //public Minimap(World g)
        //    : base(g.Game)
        //{
        //    game = g;
        //    sprites = new LinkedList<MinimapDrawable>();
        //    shading = new Color(new Vector4(1, 1, 1, Settings.minimapAlpha));
        //}

        public delegate void RenderFunction();

        public Minimap(Game g, int parentDrawOrder, InputHandler input, RenderFunction func, float mapWidth, float mapHeight)
            : base(g)
        {
            sprites = new List<MinimapDrawable>();
            shading = new Color(new Vector4(1, 1, 1, Settings.minimapAlpha));

            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.input = input;
            renderer = func;

            DrawOrder = parentDrawOrder + 1;

            Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            makeMinimap();

            Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            GraphicsDevice.DeviceReset += GraphicsDevice_DeviceReset;

            Window_ClientSizeChanged(null, null);
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void makeMinimap()
        {
            int w = (int)(mapWidth / 8);
            int h = (int)(mapHeight / 8);

            using (RenderTarget2D minimapRenderTarget = new RenderTarget2D(GraphicsDevice, w, h, 1, SurfaceFormat.Color))
            {
                RenderTarget2D oldTarget = (RenderTarget2D)GraphicsDevice.GetRenderTarget(0);
                GraphicsDevice.SetRenderTarget(0, minimapRenderTarget);

                DepthStencilBuffer oldBuffer = GraphicsDevice.DepthStencilBuffer;
                using (DepthStencilBuffer db = new DepthStencilBuffer(GraphicsDevice, w, h, DepthFormat.Depth16))
                {
                    GraphicsDevice.DepthStencilBuffer = db;
                    GraphicsDevice.Clear(Color.Lime);
                    renderer();
                    GraphicsDevice.DepthStencilBuffer = oldBuffer;
                }
                GraphicsDevice.SetRenderTarget(0, oldTarget);

                texture = new Texture2D(GraphicsDevice, w, h, 1, TextureUsage.Linear, SurfaceFormat.Color);
                texture = minimapRenderTarget.GetTexture();
            }
        }

        public void Draw(Texture2D texture, Vector3 location, Vector3 rotation)
        {
            sprites.Add(new MinimapDrawable(texture, location, rotation));
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle rectangle = Fullscreen ? rectangleFullscreen : rectangleMinimap;
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(texture, rectangle, shading);
            foreach (MinimapDrawable s in sprites)
            {
                int x = (int)((s.location.X / mapWidth + .5f) * rectangle.Width);
                int z = (int)((s.location.Z / mapHeight + .5f) * rectangle.Height);
                Rectangle r = new Rectangle(rectangle.X + x, z, s.texture.Width, s.texture.Height);
                spriteBatch.Draw(s.texture, r, null, shading, polarFromVector(s.at), new Vector2(s.texture.Width / 2, s.texture.Height / 2), SpriteEffects.None, 0);
            }
            sprites.Clear();
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Toggle minimap fullscreen
            if (input.IsKeyPressed(Settings.showMap))
            {
                Fullscreen = !Fullscreen;
            }
        }

        protected override void Dispose(bool disposing)
        {
            GraphicsDevice.DeviceReset -= GraphicsDevice_DeviceReset;
            Game.Window.ClientSizeChanged -= Window_ClientSizeChanged;
            texture.Dispose();

            Game.Components.Remove(this);
            base.Dispose(disposing);
        }

        protected void Window_ClientSizeChanged(object o, EventArgs args)
        {
            float mapAspectRatio = mapWidth / mapHeight;
            int vWidth = GraphicsDevice.Viewport.Width;
            int vHeight = GraphicsDevice.Viewport.Height;
            int height = (int)(vHeight * Settings.minimapSize);
            int width = (int)(height * mapAspectRatio);
            rectangleMinimap = new Rectangle(GraphicsDevice.Viewport.Width - height, 0, width, height);
            rectangleFullscreen = new Rectangle((int)(vWidth - vHeight * mapAspectRatio) / 2, 0, (int)(vHeight * mapAspectRatio), vHeight);
        }

        protected void GraphicsDevice_DeviceReset(object o, EventArgs args)
        {
            makeMinimap();
        }

        protected static float polarFromVector(Vector3 At)
        {
            double result;
            if (At.X > 0 && At.Z >= 0)
                result = Math.Atan(At.Z / At.X);
            else if (At.X > 0 && At.Z < 0)
                result = Math.Atan(At.Z / At.X) + 2 * Math.PI;
            else if (At.X < 0)
                result = Math.Atan(At.Z / At.X) + Math.PI;
            else if (At.X == 0 && At.Z > 0)
                result = Math.PI / 2;
            else if (At.X == 0 && At.Z < 0)
                result = 3 * Math.PI / 2;
            else throw new ArgumentException();
            return (float)result + MathHelper.PiOver2;
        }
    }
}
