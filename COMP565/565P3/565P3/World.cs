using System;
using System.Collections.Generic;
using System.Text;
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
    public class World : DrawableGameComponent
    {
        public Camera currentCamera;
        public Matrix projection;

        protected List<Drawable> drawables;
        protected List<Updatable> updatables;
        protected List<Drawable> toRemove;

        public Terrain terrain;
        public Avatar avatar;

        protected SpriteBatch spriteBatch;
        protected Texture2D reticle;
        protected Rectangle reticleRectangle;

        protected Texture2D arrow;

        protected Minimap minimap;
        protected PauseScreen pause;
        protected DamageScreen damage;
        public void setDamage(float d) { damage.setDamage(d); }

        protected LinearMeter health;
        protected LinearMeter jet;

        public InputHandler input;

        public Octree oct;

        public Model disc, lob;

        public World(Game game, InputHandler input)
            : base(game)
        {
            this.input = input;

            drawables = new List<Drawable>();
            updatables = new List<Updatable>();
            toRemove = new List<Drawable>();

            Game.Window.ClientSizeChanged += Window_ClientSizeChanged;

            pause = new PauseScreen(this, input);
            damage = new DamageScreen(this);
        }

        protected override void Dispose(bool disposing)
        {
            Game.Window.ClientSizeChanged -= Window_ClientSizeChanged;

            minimap.Dispose();
            pause.Dispose();

            base.Dispose(disposing);
        }

        protected void Window_ClientSizeChanged(object o, EventArgs args)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Settings.FOVdegrees), GraphicsDevice.Viewport.AspectRatio, Settings.hither, Settings.yon);

            reticleRectangle = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 8, GraphicsDevice.Viewport.Height / 2 - 8, 16, 16);
        }

        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Window_ClientSizeChanged(null, null);
            currentCamera = avatar.camera;
#if !XBOX360
            input.Mouselook = true;
#endif
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            terrain = new Terrain(this, Vector3.Zero, Game.Content.Load<Model>("terrain"));
            drawables.Add(terrain);

            oct = new Octree(terrain.Width, Settings.ceiling * 2, terrain.Height, Settings.octreeDepth);

            avatar = new Avatar(this, Vector3.Zero, Game.Content.Load<Model>("turret"));
            drawables.Add(avatar);
            oct.Add(avatar);

            health = new LinearMeter(this, new Rectangle(40, 5, 200, 20),
                Settings.healthMeterColor, Settings.meterBackgroundColor, 1, avatar.getHealth);
            jet = new LinearMeter(this, new Rectangle(40, 25, 200, 20),
                Settings.jetMeterColor, Settings.meterBackgroundColor, 1, avatar.getJet);

            Vector3 v = Vector3.Zero;
            v.Y = terrain.GetHeight(v) + 10;
            drawables.Add(new Drawable(this, v, Game.Content.Load<Model>("axes")));

            reticle = Game.Content.Load<Texture2D>("reticle");
            arrow = Game.Content.Load<Texture2D>("arrow");

            minimap = new Minimap(Game, this.DrawOrder, input, terrain.drawTopDown, terrain.Width, terrain.Height);

            disc = Game.Content.Load<Model>("disc");
            lob = Game.Content.Load<Model>("lob");
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            base.OnVisibleChanged(sender, args);
            minimap.Visible = this.Visible;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Pause world simulation
            if (input.IsKeyPressed(Settings.pause))
            {
                pause.Enabled = true;
                return;
            }

            // Switch between cameras
            if (input.IsKeyPressed(Settings.switchCamera))
            {
                if (currentCamera is FreeCamera)
                    currentCamera = avatar.camera;
                else
                {
                    ((LinkedCamera)currentCamera).Dispose();
                    currentCamera = new FreeCamera(this, new Vector3(0.0f, 20.0f, -50.0f), Vector3.Zero);
                }
            }

            // mouselook toggle
#if !XBOX360
            if (input.IsKeyPressed(Settings.toggleMouselook))
            {
                input.Mouselook = !input.Mouselook;
            }
#endif

            foreach (Updatable u in updatables)
            {
                u.update();
            }

            foreach (Drawable d in toRemove)
            {
                drawables.Remove(d);
                if (d is Updatable)
                    updatables.Remove((Updatable)d);
            }
            toRemove.Clear();

            currentCamera.update();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.AlphaBlendEnable = false;
            GraphicsDevice.RenderState.AlphaTestEnable = false;
            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

            minimap.Draw(arrow, avatar.transform.Translation, avatar.transform.Forward);

            base.Draw(gameTime);

            foreach (Drawable d in drawables)
            {
                d.draw();
            }

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
#if XBOX360
            if (Enabled)
#else
            if (input.Mouselook && Enabled)
#endif
                spriteBatch.Draw(reticle, reticleRectangle, Settings.reticleColor);
            spriteBatch.End();
        }

        public void add(Drawable d)
        {
            //Console.WriteLine(d.GetType().Name + " " + d.GetHashCode() + " added.");
            drawables.Add(d);
            if (d is Updatable)
                updatables.Add((Updatable)d);
        }

        public void remove(Drawable d)
        {
            //Console.WriteLine(d.GetType().Name + " " + d.GetHashCode() + " removed.");
            toRemove.Add(d);
        }
    }
}
