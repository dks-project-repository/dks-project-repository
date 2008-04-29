/*
 * SceneWorld.cs
 * Based on a starter kit for Comp 565 projects.
 * Uses an "on-idle" rendering cycle for fastest FPS.
 * Commented code for using fixed interval rendering w/ renderTimer.
 * 
 * Keyboard commands:  
 *    'Enter' start or continue rendering
 *    'p'     pause rendering -- no user events handled
 *    'f'     toggle between vertex fog and no vertex fog
 *    'n'     toggle amount of navgraph drawn
 *    'm'     toggle avatar movement mode between free and node-to-node
 *    'l'     toggle lighting
 *    'k'     toggle spotlight
 *    'y'     change yon distance
 *    'Esc'   reset avatar view / movement state to do nothing
 *    'q'     exit application
 *    'w'     Move forward
 *    's'     Move backward
 *    'a'     Turn left
 *    'd'     Turn right
 *    Arrows  Auto-move/turn
 * 
 * Mike Barnes 2/11/2008
 * Steven Barnett 3/12/2008
 */

// Be sure to also add the referece "Microsoft.DirectX.Direct3DX" 
// to your Project's References.
// You do not need a "using" statement for that reference. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{

    public class SceneWorld : Form
    {
        // collections
        private List<IDrawable> drawable = null;
        private List<Camera> camera = null;
        private List<MovableMesh3D> movable = null;
        // objects
        private Camera defaultCamera = null, currentCamera = null;
        public Avatar avatar = null, npAvatar = null;
        // world attributes
        private Device display = null;  // device to draw on
        private Microsoft.DirectX.Direct3D.Font font;
        private Color fontColor;
        // Timer(s):  FPS computation, fixed interval rendering (if desired)
        Timer fpsTimer = null;
        // Timer renderTimer = null;  // if you want to render on fixed intervals
        private int frameCount, fpsValue;
        private bool running = false;  // is game running or not?
        private bool fog = false;        // fog initially off
        private bool lighting = true;
        private bool spotlight = true;
        private bool hilbertDraw = true;
        // Camera, view and infomation/trace display
        // fov, hither, and yon are used for perspective viewing.
        private float fov = (float)Math.PI / 4;
        private float hither = 5.0f, yon = 1000;
        private int cameraIndex;
        private TracePanel info;
        private NavGraph navgraph;
        public NavGraph NavGraph { get { return navgraph; } }
        private TreasureChest treasures;
        public TreasureChest Treasures { get { return treasures; } }
        public HilbertCurve hilbert;
        public Flock flock;

        // Constructor
        public SceneWorld()
        {
            //InitializeComponent();  // if used w/ Designer ToolBox
            // set Properties of SceneWorld
            Text = "Scene World";  // Form's title 
            Width = 800;    // size of Form
            Height = 600;
            fontColor = Color.White;  // for on-screen display -- see ShowText()
            InitializeGraphics();
            info = new TracePanel(this);  // create Trace display
            buildScene();  // like XNA'a ContentLoader()
            navgraph = new NavGraph(this, drawable);
            hilbert = new HilbertCurve(display, 4000, 500, navgraph);
            treasures = new TreasureChest(this, 4);
            // enable timer(s)
            // renderTimer = new Timer();  // for fixed interval rendering
            // renderTimer.Tick += new System.EventHandler(Render);
            // renderTimer.Interval = 40; // expected fps: 30 fps = 33, 40 fps = 25
            fpsTimer = new Timer();
            fpsTimer.Tick += new System.EventHandler(FpsTimerTick);
            fpsTimer.Interval = 1000;
            // initialize counts and indices
            frameCount = 0;
            cameraIndex = 0;
            // show windows.
            info.Show();   // display the trace window
            Show();        // display the Scene World form
            // renderTimer.Enabled = true;
            //Trace = "To start game, press 's' in Scene World window";

            display.RenderState.Ambient = Color.DarkGray;
            display.Lights[0].Type = LightType.Directional;
            display.Lights[0].Diffuse = Color.LightGray;
            display.Lights[0].Specular = Color.LightGray;
            //display.Lights[0].Direction = new Vector3(.4f, -.7f, .3f);
            display.Lights[0].Direction = new Vector3(.4f, 0, .3f);
            display.Lights[0].Enabled = true;
            display.Lights[1].Type = LightType.Spot;
            //display.Lights[1].Position = new Vector3(325, 0, -115);
            //display.Lights[1].Direction = new Vector3(.48f, .5f, -.88f);
            //display.Lights[1].Range = 400;
            display.Lights[1].Position = new Vector3(0, 349, 0);
            display.Lights[1].Direction = new Vector3(1f, -1f, 1f);
            display.Lights[1].Range = 1000;
            display.Lights[1].InnerConeAngle = .1f;
            display.Lights[1].OuterConeAngle = .3f;
            display.Lights[1].Falloff = .5f;
            display.Lights[1].Attenuation0 = 1f;
            display.Lights[1].Diffuse = Color.Lime;
            display.Lights[1].Enabled = spotlight;
        }

        // Properties

        public Device Display { get { return display; } }        // device to draw on

        public string Trace
        {
            get { return info.Trace; }
            set { info.Trace = value + '\n'; }
        }      // update info's trace field.

        // Methods

        public void InitializeGraphics()
        {
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;
            pp.AutoDepthStencilFormat = DepthFormat.D16;
            pp.EnableAutoDepthStencil = true;
            //pp.PresentationInterval = PresentInterval.Immediate;
            try
            {
                display = new Device(0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, pp);
            }
            catch (Exception)
            {
                display = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, pp);
            }
            font = new Microsoft.DirectX.Direct3D.Font(display,
               new System.Drawing.Font("Courier New", 12.0f, FontStyle.Bold));
            // RenderStateManager renderState = display.RenderState;
            // renderState.FillMode = FillMode.WireFrame;
            SetupVertexFog(Color.LightSkyBlue, FogMode.Linear, false, 0.08f);
            display.SamplerState[0].MipFilter = TextureFilter.Anisotropic;
        }

        // create meshes, avatars, cameras.
        public void buildScene()
        {
            //create collections
            drawable = new List<IDrawable>();
            movable = new List<MovableMesh3D>();
            camera = new List<Camera>();
            // create cameras and objects and add them to appropriate collections
            // Always have a defaultCamera and set avatar to the defaultCamera -- <ESC> reset action
            defaultCamera = new Camera(this, "origin  ", new Vector3(0.0f, yon - 5, 0.0f),
               new Vector3(1.0f, 0.0f, 0.0f), 1.57f);
            currentCamera = defaultCamera;
            camera.Add(defaultCamera);
            cameraIndex++;  // add defaultCamera before any Avatars.
            // load stationary mesh objects:  provide a name, *.x mesh file, and *.bmp texture file
            // place modeled meshes at origin without any rotation.
            drawable.Add(new ModeledMesh3D(this, "ground", "groundSW.x", "openfootageNETGrassDirt8.jpg"));
            // place modeled mesh at position, provide rotation axis and rotation radians.
            MeshData data = new MeshData(display, "ThemeBuilding.x");
            //drawable.Add(new ModeledMesh3D(this, "ThemeBuilding", new Vector3(349, 0, 349), new Vector3(0, 1, 0), 0, "ThemeBuilding.x"));
            //drawable.Add(new ModeledMesh3D(this, "ThemeBuilding", new Vector3(-349, 0, -349), new Vector3(0, 1, 0), (float)Math.PI / 4f, "ThemeBuilding.x"));
            data = new MeshData(display, "box.x", "brick.jpg");
            bool[,] maze = {{true, true, true, true, true, true},
                            {true, false, false, false, false, false},
                            {true, false, false, false, false, false},
                            {true, false, false, true, true, true},
                            {true, false, false, false, false, true},
                            {true, false, false, false, false, true},
                            {true, true, true, true, true, true}};
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (maze[i, j])
                        drawable.Add(new ModeledMesh3D(this, "maze",
                            new Vector3(i * 70 + 230, 0, j * 70 - 650),
                            new Vector3(0, 1, 0), 0, data));
                }

            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (maze[i, j])
                        drawable.Add(new ModeledMesh3D(this, "maze",
                            new Vector3(j * 70 - 650, 0, i * 70 + 230),
                            new Vector3(0, 1, 0), 0, data));
                }

            }
            data = new MeshData(display, "house.x", "rainbow.jpg");
            Random rand = new Random();
            for (int i = -6; i <= 6; i++)
            {
                for (int j = -6; j <= 6; j++)
                {
                    if (Math.Abs(i) < 3 && Math.Abs(j) < 3)
                        continue;
                    drawable.Add(new ModeledMesh3D(this, "building",
                        new Vector3(i * 328 + rand.Next(-41, 41) - Math.Sign(i) * 132,
                                    0,
                                    j * 328 + rand.Next(-41, 41) - Math.Sign(j) * 132),
                        new Vector3(0, 1, 0), (float)(rand.NextDouble() * Math.PI * 2), data));
                }
            }
            // load any movable meshes than are not players.  Other animations ?
            // load avatar mesh objects
            // Avatar meshes do not have textures
            avatar = new Avatar(this, "Chaser", new Vector3(0, 0, 100),
               new Vector3(), 0.0f, "turret.x");
            avatar.autoMove = false;
            drawable.Add(avatar);
            movable.Add(avatar);
            camera.Add(avatar.FirstPerson);
            camera.Add(avatar.FollowCamera);
            camera.Add(avatar.TopCamera);
            npAvatar = new NPAvatar(this, "Evader", new Vector3(100, 0, -200),
               new Vector3(), 0.0f, "thing.x");
            drawable.Add(npAvatar);
            movable.Add(npAvatar);
            camera.Add(npAvatar.FirstPerson);
            camera.Add(npAvatar.FollowCamera);
            camera.Add(npAvatar.TopCamera);
            flock = new Flock(this, "flock", "turret.x", 10, 15, .2f, .05f, 60, 5);

        }

        /// <summary>
        /// Set Vertex Fog parameters
        /// </summary>
        /// <param name="color"></param>
        /// <param name="mode"></param>
        /// <param name="bUseRange"></param>
        /// <param name="fDensity"></param>   
        void SetupVertexFog(Color color, FogMode mode, bool bUseRange, float fDensity)
        {
            float Start = yon / 3.0f; //0.5f;     // Linear fog distances.
            float End = yon;      //0.8f;

            // Enable fog blending.
            display.RenderState.FogEnable = fog;

            // Set the fog color.
            display.RenderState.FogColor = color;

            // Set fog parameters.
            if (mode == FogMode.Linear)
            {
                display.RenderState.FogVertexMode = mode;
                display.RenderState.FogStart = Start;
                display.RenderState.FogEnd = End;
            }
            else
            {
                display.RenderState.FogVertexMode = mode;
                display.RenderState.FogDensity = fDensity;
            }

            // Enable range-based fog if desired (only supported for
            // vertex fog). For this example, it is assumed that bUseRange
            // is set only if the driver exposes the 
            // RasterCaps.SupportsFogRange capability.
            // Note: This is slightly more performance-intensive
            // than non-range-based fog.

            if (bUseRange)
                display.RenderState.RangeFogEnable = true;
        }

        /// <summary>
        /// Set running to false to stop on-idle rendering
        /// when Form is closed.
        /// Not needed w/ fixed interval rendering
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            running = false;
            Trace = "Window Closing";
            base.OnFormClosed(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Start();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (running)
            {
                display.Lights[0].Enabled = true;
                display.Lights[1].Enabled = spotlight;
                display.RenderState.Ambient = Color.DarkGray;
            }
        }

        // Start game
        private void Start()
        {
            fpsTimer.Enabled = true;
            // remaining lines not needed w/ fixed interval rendering
            running = true;    // set false in OnWindowClosing()    
            while (running)
            {  // game loop -- quit with window close
                Render(null, null);
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Set the fps value and reset the count of frames drawn
        /// </summary>
        public void FpsTimerTick(object sender, EventArgs e)
        {
            fpsValue = frameCount;
            frameCount = 0;
        }

        /// <summary>
        /// Update the on-screen display of player and view information
        /// </summary>
        private void ShowText()
        {
            if (avatar == null) return;
            font.DrawText(null, string.Format("Avatar   Location {0:f0} {1:f0} {2:f0}",
               avatar.Location.X, avatar.Location.Y, avatar.Location.Z),
               new Rectangle(10, 10, 200, 20), DrawTextFormat.NoClip, fontColor);
            font.DrawText(null, string.Format("Look at  {0:f2} {1:f2} {2:f2}",
               avatar.At.X, avatar.At.Y, avatar.At.Z),
               new Rectangle(350, 10, 650, 20), DrawTextFormat.NoClip, fontColor);
            font.DrawText(null, string.Format("{0}", avatar.Name),
               new Rectangle(650, 10, 750, 20), DrawTextFormat.NoClip, fontColor);

            font.DrawText(null, string.Format("{0} Location {1:f0} {2:f0} {3:f0}",
               currentCamera.Name, currentCamera.Location.X, currentCamera.Location.Y,
               currentCamera.Location.Z), new Rectangle(10, 30, 200, 20),
               DrawTextFormat.NoClip, fontColor);
            font.DrawText(null, string.Format("Look at  {0:f2} {1:f2} {2:f2}",
               currentCamera.At.X, currentCamera.At.Y, currentCamera.At.Z),
               new Rectangle(350, 30, 650, 20), DrawTextFormat.NoClip, fontColor);
            // commented out fixed interval display
            //font.DrawText(null, string.Format("FPS {0:f0} :: {1:f0}", fpsValue,
            //   1000 / renderTimer.Interval),
            font.DrawText(null, string.Format("FPS {0:f0}", fpsValue),
             new Rectangle(650, 30, 750, 20), DrawTextFormat.NoClip, fontColor);

            font.DrawText(null, string.Format("FindingPath:{0:1} StopFindingPath:{1} {2}", avatar.findingPath ? 'T' : 'F', avatar.stopFindingPath ? 'T' : 'F', avatar.findingPathWaitCount),
             new Rectangle(10, 50, 200, 20), DrawTextFormat.NoClip, fontColor);
            checkwin();
        }

        /// <summary>
        /// Draw the current scene
        /// Render is written to be used with a renderTimer tick if you want.
        /// To use w/o renderTimer call Render(null, null) as in Start().
        /// </summary>
        public void Render(object sender, EventArgs e)
        {
            display.Clear(ClearFlags.Target | ClearFlags.ZBuffer,
               Color.LightSkyBlue, 1.0f, 0);
            display.BeginScene();

            // remove when you add your lighting
            //display.RenderState.Lighting = false;
            // set perspective view and camera
            display.Transform.Projection = Matrix.PerspectiveFovLH(
               fov, (float)ClientSize.Width / (float)ClientSize.Height,
               hither, yon);
            display.Transform.View = currentCamera.ViewMatrix;
            // move and draw objects into scene
            navgraph.draw(avatar, yon);
            foreach (IDrawable obj in drawable) obj.draw();
            foreach (MovableMesh3D mObj in movable) mObj.move();
            treasures.draw();
            if (hilbertDraw)
            {
                hilbert.draw();
                navgraph.drawPath(hilbert.Path);
            }
            flock.doFlock();
            flock.draw();

            ShowText();

            display.EndScene();
            display.Present();
            frameCount++;
        }


        // Change avatar's direction and movement based on keystrokes
        // Sets "forces" working on avatar.
        // Forces continue to change direction and movement until modified   
        protected override void OnKeyUp(KeyEventArgs kea)
        {
            if (avatar == null) return;
            // Keyboard command interaction
            if (kea.KeyCode == Keys.Escape)
                avatar.reset();// re-create initial avatar at initial position and orientation
            else if (kea.KeyCode == Keys.Enter) Start();  // start or continue rendering
            else if (kea.KeyCode == Keys.P)
            {
                running = false;              // pause rendering
                fpsTimer.Enabled = false;
            }    // stop timer
            else if (kea.KeyCode == Keys.Q)
            {
                running = false;
                Application.ExitThread();
            }   // stop application
            else if (kea.KeyCode == Keys.F)
            {
                fog = !fog;   // toggle fog state
                display.RenderState.FogEnable = fog;
            }   // set fog renderState
            else if (kea.KeyCode == Keys.N)
            {
                // change amount of navgraph drawn
                navgraph.nextYonDivider();
            }
            else if (kea.KeyCode == Keys.M)
            {
                // alternate avatar move mode
                ModeledMesh3D.alternateMoveMode = !ModeledMesh3D.alternateMoveMode;
                if (ModeledMesh3D.alternateMoveMode)
                {
                    foreach (MovableMesh3D m in movable)
                        m.initNodeTravel();
                }
            }
            else if (kea.KeyCode == Keys.L)
            {
                // toggle lighting
                lighting = !lighting;
                display.RenderState.Lighting = lighting;
            }
            else if (kea.KeyCode == Keys.K)
            {
                // toggle spotlight
                spotlight = !spotlight;
                display.Lights[1].Enabled = spotlight;
            }
            else if (kea.KeyCode == Keys.Y)
            {
                // change yon distance
                if (yon == 500)
                    yon = 1000;
                else if (yon == 1000)
                    yon = 2000;
                else if (yon == 2000)
                    yon = 4820;
                else
                    yon = 500;
                camera[0].Location = new Vector3(0, yon - 5, 0);
                display.RenderState.FogStart = yon / 3f;
                display.RenderState.FogEnd = yon;
            }
            // Keyboard camera selection
            else if (kea.KeyCode == Keys.F12 && avatar != null)
            {
                // F12 cycles through all cameras in cameraObject list.   
                cameraIndex = ++cameraIndex % camera.Count;
                currentCamera = camera[cameraIndex];
                if (currentCamera != defaultCamera) avatar.AvatarCamera = currentCamera;
            }
            else if (kea.KeyCode == Keys.Z)
            {
                avatar.autoMove = !avatar.autoMove;
                if (!avatar.autoMove)
                    avatar.clearPath();
            }
            else if (kea.KeyCode == Keys.H)
            {
                hilbertDraw = !hilbertDraw;
            }
            /*  
            // We aren't using these in our assignment ... but there are here for interest
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
               switch (kea.KeyCode) {
                  case Keys.Up: avatar.Pitch--; break;      // up arrow
                  case Keys.Down: avatar.Pitch++; break;    // down arrow
                  case Keys.Left: avatar.Roll++; break;     // left arrow
                  case Keys.Right: avatar.Roll--; break;    // right arrow
               }
            */
            // Keyboard view and move selection
            else  // move avatar direction in X Z plane   
                switch (kea.KeyCode)
                {
                    case Keys.W: if (avatar.Steps > 0) avatar.Steps = 0; break;
                    case Keys.S: if (avatar.Steps < 0) avatar.Steps = 0; break;
                    case Keys.A: if (avatar.Yaw > 0) avatar.Yaw = 0; break;
                    case Keys.D: if (avatar.Yaw < 0) avatar.Yaw = 0; break;
                }
            base.OnKeyUp(kea);
        }

        // Disable key repeats for OnKeyDown
        // Inspiration: http://blogs.msdn.com/jfoscoding/archive/2005/01/24/359334.aspx
        public override bool PreProcessMessage(ref Message msg)
        {
            int code = msg.LParam.ToInt32();
            //if ((code & 0x40000000) != 0 && (code & 0x80000000) == 0)
            if ((code & 0xC0000000 ^ 0x40000000) == 0)
                return true;
            return base.PreProcessMessage(ref msg);
        }

        // Change avatar's direction and movement based on keystrokes
        protected override void OnKeyDown(KeyEventArgs kea)
        {
            if (avatar == null) return;
            // Keyboard command interaction
            switch (kea.KeyCode)
            {
                case Keys.Up: avatar.Steps++; break;
                case Keys.Down: avatar.Steps--; break;
                case Keys.Left: avatar.Yaw++; break;
                case Keys.Right: avatar.Yaw--; break;
                case Keys.W: avatar.Steps = 1; break;
                case Keys.S: avatar.Steps = -1; break;
                case Keys.A: avatar.Yaw = 1; break;
                case Keys.D: avatar.Yaw = -1; break;
            }
            base.OnKeyDown(kea);
        }

        protected void checkwin()
        {
            if (avatar.Winner)
            {
                font.DrawText(null, "YOU ARE VICTORIOUS!!!!", new Rectangle(Size.Width / 2 - 100, Size.Height / 2 - 10, 200, 20), DrawTextFormat.NoClip, fontColor);
                running = false;
                fpsTimer.Enabled = false;
            }
            if (npAvatar.Winner)
            {
                font.DrawText(null, "EVADER IS VICTORIOUS!!!!", new Rectangle(Size.Width / 2 - 100, Size.Height / 2 - 10, 200, 20), DrawTextFormat.NoClip, fontColor);
                running = false;
                fpsTimer.Enabled = false;
            }


        }

        static void Main(string[] args)
        {
            using (SceneWorld scene = new SceneWorld())
            {
                //scene.Start();  // start the game  
                Application.Run(scene);  // use w/ fixed interval rendering   
            }
        }

    }

}
