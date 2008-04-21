/* Daniel Frankel
 * Kevin Yedlin
 * Comp 565 - Project 1
 * 
 * 
 * SceneWorld.cs
 * A starter kit for Comp 565 projects.
 * Uses an "on-idle" rendering cycle for fastest FPS.
 * Commented code for using fixed interval rendering w/ renderTimer.
 * 
 * Keyboard commands:  
 *    's'   start or continue rendering
 *    'p'   pause rendering -- no user events handled
 *    'Esc' reset avatar view / movement state to do nothing
 *    'q'   exit application
 * 
 * Mike Barnes 2/11/2008
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
        private bool fog = false;
        private bool flashlight = false;
        private bool lightToggle = true;

        // Camera, view and infomation/trace display
        // fov, hither, and yon are used for perspective viewing.
        private float fov = (float)Math.PI / 4;
        private float hither = 5.0f, yon = 500.0f;
        private int cameraIndex;
        public static TracePanel info;
        private bool navDraw;
        private NavGraph nav;

        // Constructor
        public SceneWorld()
        {
            //InitializeComponent();  // if used w/ Designer ToolBox
            // set Properties of SceneWorld
            Text = "Scene World";  // Form's title 
            Width = 800;    // size of Form
            Height = 600;
            fontColor = Color.Black;  // for on-screen display -- see ShowText()
            InitializeGraphics();
            info = new TracePanel(this);  // create Trace display
            //info.Show();
            buildScene();  // like XNA'a ContentLoader()
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
            navDraw = false;
            // show windows.
            info.Show();   // display the trace window
            Show();        // display the Scene World form
            // renderTimer.Enabled = true;
            //Trace = "To start game, press 'enter' in Scene World window";

            //Create the Lighting
            display.RenderState.Ambient = Color.LightGray;

            display.Lights[0].Type = LightType.Spot;
            display.Lights[0].Range = 200;
            display.Lights[0].InnerConeAngle = 1f;
            display.Lights[0].OuterConeAngle = 2f;
            display.Lights[0].Falloff = 0.75f;
            display.Lights[0].Attenuation0 = 0.1f;
            display.Lights[0].Diffuse = Color.Gray;
            display.Lights[0].Specular = Color.White;

            display.Lights[1].Type = LightType.Directional;
            display.Lights[1].Position = new Vector3(-2000, 0, 2000);
            display.Lights[1].Direction = new Vector3(1, -1, -1);
            display.Lights[1].Diffuse = Color.Gray;

            display.Lights[2].Type = LightType.Directional;
            display.Lights[2].Position = new Vector3(2000, 0, -2000);
            display.Lights[2].Direction = new Vector3(-1, -1, 1);
            display.Lights[2].Diffuse = Color.Gray;

            display.RenderState.Lighting = lightToggle;
            display.Lights[1].Enabled = true;
            display.Lights[2].Enabled = true;

        }


        // Properties

        public Device Display { get { return display; } }        // device to draw on

        public string Trace
        {
            get { return info.Trace; }
            set { info.Trace = value + '\n'; }
        }

        public static string Trace1
        {
            get { return info.Trace; }
            set { info.Trace = value + '\n'; }
        }      // update info's trace field.

        public NavGraph Nav
        {
            get { return nav; }
            set { nav = value; }
        }

        // Methods

        public void InitializeGraphics()
        {
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;
            pp.AutoDepthStencilFormat = DepthFormat.D16;
            pp.EnableAutoDepthStencil = true;
            pp.PresentationInterval = PresentInterval.Immediate;
            display = new Device(0, DeviceType.Hardware, this,
               CreateFlags.HardwareVertexProcessing, pp);
            //   CreateFlags.SoftwareVertexProcessing, pp);
            font = new Microsoft.DirectX.Direct3D.Font(display,
               new System.Drawing.Font("Courier New", 12.0f, FontStyle.Bold));
            // RenderStateManager renderState = display.RenderState;
            // renderState.FillMode = FillMode.WireFrame;
            SetupVertexFog(Color.LightSkyBlue, FogMode.Linear, false, 0.08f);
            display.SamplerState[0].MipFilter = TextureFilter.Linear;


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
            defaultCamera = new Camera(this, "origin  ", new Vector3(0.0f, yon - 10, 0.0f),
               new Vector3(1.0f, 0.0f, 0.0f), 1.57f);
            currentCamera = defaultCamera;
            camera.Add(defaultCamera);
            cameraIndex++;  // add defaultCamera before any Avatars.
            // load stationary mesh objects:  provide a name, *.x mesh file, and *.bmp texture file
            // place modeled meshes at origin without any rotation.

            ExtendedMaterial[] mtrl;
            drawable.Add(new ModeledMesh3D(this, "ground", "groundSW.x", "ground.jpg"));

            Mesh castle = ModeledMesh3D.openMeshFile("CASTLE.x", display, out mtrl);

            // place modeled mesh at position, provide rotation axis and rotation radians.

            display.Material = new Material();
            drawable.Add(new ModeledMesh3D(this, "Castle", new Vector3(1250, 0, 1250),
               new Vector3(0, 1, 0), 0.785f, castle, mtrl));
            drawable.Add(new ModeledMesh3D(this, "Castle", new Vector3(1250, 0, -1250),
               new Vector3(0, 1, 0), 2.356f, castle, mtrl));
            drawable.Add(new ModeledMesh3D(this, "Castle", new Vector3(-1250, 0, 1250),
               new Vector3(0, 1, 0), -0.785f, castle, mtrl));
            drawable.Add(new ModeledMesh3D(this, "Castle", new Vector3(-1250, 0, -1250),
               new Vector3(0, 1, 0), -2.356f, castle, mtrl));

            Mesh house = ModeledMesh3D.openMeshFile("House.x", display, out mtrl);

            //****************************************************************************
            //                               Right Group
            //****************************************************************************
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(800, 0, 0),
              new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(800, 0, 120),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(870, 0, 240),
               new Vector3(0, 1, 0), -.785f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(800, 0, -120),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(870, 0, -240),
               new Vector3(0, 1, 0), -2.355f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1005, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1125, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1245, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1365, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1005, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1125, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1245, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(1365, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57

            //****************************************************************************
            //                                Left Group
            //****************************************************************************
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-800, 0, 0),
              new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-800, 0, 120),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-870, 0, 240),
               new Vector3(0, 1, 0), .785f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-800, 0, -120),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-870, 0, -240),
               new Vector3(0, 1, 0), 2.355f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1005, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1125, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1245, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1365, 0, 290),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1005, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1125, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1245, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-1365, 0, -290),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57

            //****************************************************************************
            //                                Top Group
            //****************************************************************************
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(0, 0, 1365),
              new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(120, 0, 1365),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(240, 0, 1295),
               new Vector3(0, 1, 0), .785f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-120, 0, 1365),
               new Vector3(0, 1, 0), 0f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-240, 0, 1295),
               new Vector3(0, 1, 0), -.785f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, 1160),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, 1040),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, 920),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, 800),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, 1160),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, 1040),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, 920),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, 800),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57

            //****************************************************************************
            //                               Bottom Group
            //****************************************************************************
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(0, 0, -1365),
              new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(120, 0, -1365),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(240, 0, -1295),
               new Vector3(0, 1, 0), 2.355f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-120, 0, -1365),
               new Vector3(0, 1, 0), 3.14f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-240, 0, -1295),
               new Vector3(0, 1, 0), -2.355f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, -1160),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, -1040),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, -920),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(290, 0, -800),
               new Vector3(0, 1, 0), 1.57f, house, mtrl));//1.57

            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, -1160),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, -1040),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, -920),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57
            drawable.Add(new ModeledMesh3D(this, "House", new Vector3(-290, 0, -800),
               new Vector3(0, 1, 0), -1.57f, house, mtrl));//1.57


            // load any movable meshes than are not players.  Other animations ?
            // load avatar mesh objects
            // Avatar meshes do not have textures
            avatar = new Avatar(this, "Chaser", new Vector3(0, 0, 200),
                new Vector3(), 0.0f, "rook.x");//"chess_piece.x");
            drawable.Add(avatar);
            movable.Add(avatar);
            camera.Add(avatar.FirstPerson);
            camera.Add(avatar.FollowCamera);
            npAvatar = new NPAvatar(this, "Evader", new Vector3(0, 0, -200),
               new Vector3(), 0.0f, "pawn.x");//"chess_piece.x");
            drawable.Add(npAvatar);
            movable.Add(npAvatar);
            camera.Add(npAvatar.FirstPerson);
            camera.Add(npAvatar.FollowCamera);
            nav = new NavGraph(this, avatar);
            nav.cullBuildings(drawable);
            Trace = Vector3.CatmullRom(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 2), new Vector3(0, 0, 3), 12) + "";
        }

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
        }

        /// <summary>
        /// Draw the current scene
        /// Render is written to be used with a renderTimer tick if you want.
        /// To use w/o renderTimer call Render(null, null) as in Start().
        /// </summary>
        /// 
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (running)
            {
                display.RenderState.Ambient = Color.White;
                display.RenderState.Lighting = lightToggle;
                display.Lights[1].Enabled = true;
                display.Lights[2].Enabled = true;
                display.Lights[0].Enabled = flashlight;
            }

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Start();
        }

        public void Render(object sender, EventArgs e)
        {
            display.Clear(ClearFlags.Target | ClearFlags.ZBuffer,
               Color.LightSkyBlue, 1.0f, 0);
            display.RenderState.Ambient = Color.LightGray;
            display.BeginScene();

            // set perspective view and camera
            display.Transform.Projection = Matrix.PerspectiveFovLH(
              fov, (float)ClientSize.Width / (float)ClientSize.Height,
              hither, yon);
            //display.Transform.Projection = Matrix.OrthoLH(ClientSize.Width, ClientSize.Height, hither, yon);
            display.Transform.View = currentCamera.ViewMatrix;
            // move and draw objects into scene
            
            foreach (MovableMesh3D mObj in movable) mObj.move();
            foreach (IDrawable obj in drawable) obj.draw();

            

            if (navDraw)
                nav.draw();

            ShowText();

            display.EndScene();
            display.Present();
            frameCount++;

            

            display.Lights[0].Position = avatar.Location;
            display.Lights[0].Direction = avatar.At;
            display.Lights[0].Enabled = flashlight;
        }


        // Change avatar's direction and movement based on keystrokes
        // Sets "forces" working on avatar.
        // Forces continue to change direction and movement until modified   
        protected override void OnKeyUp(KeyEventArgs kea)
        {
            if (avatar == null) return;
            // Keyboard command interaction
            if (kea.KeyCode == Keys.R)
                avatar.reset();// re-create initial avatar at initial position and orientation
            else if (kea.KeyCode == Keys.Enter)
                Start();  // start or continue rendering
            else if (kea.KeyCode == Keys.P)
            {
                running = false;              // pause rendering
                fpsTimer.Enabled = false;
            }    // stop timer
            else if (kea.KeyCode == Keys.Escape || kea.KeyCode == Keys.Q)
            {
                running = false;
                Application.ExitThread();
            }   // stop application
            else if (kea.KeyCode == Keys.F)
            {
                fog = !fog;   // toggle fog state
                display.RenderState.FogEnable = fog;
            }   // set fog renderState
            else if (kea.KeyCode == Keys.L)
            {
                flashlight = !flashlight;
                display.Lights[0].Enabled = flashlight;
            }   // turn flashlight on and off
            else if (kea.KeyCode == Keys.T)
            {
                lightToggle = !lightToggle;
                display.RenderState.Lighting = lightToggle;
            }   // turn lights on and off
            // Keyboard camera selection
            else if (kea.KeyCode == Keys.F12 && avatar != null)
            {
                // F12 cycles through all cameras in cameraObject list.   
                cameraIndex = ++cameraIndex % camera.Count;
                currentCamera = camera[cameraIndex];
                if (currentCamera != defaultCamera)
                    avatar.AvatarCamera = currentCamera;
            }
            else if (kea.KeyCode == Keys.N)
                navDraw = !navDraw;
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
                    case Keys.Up:
                        avatar.Steps++;
                        break;
                    case Keys.Down:
                        avatar.Steps--;
                        break;
                    case Keys.Left:
                        avatar.Yaw++;
                        break;
                    case Keys.Right:
                        avatar.Yaw--;
                        break;
                    case Keys.W:
                        avatar.Steps = 0;
                        break;
                    case Keys.S:
                        avatar.Steps = 0;
                        break;
                    case Keys.A:
                        avatar.Yaw = 0;
                        break;
                    case Keys.D:
                        avatar.Yaw = 0;
                        break;
                }
            base.OnKeyUp(kea);
        }

        protected override void OnKeyDown(KeyEventArgs kea)
        {
            switch (kea.KeyCode)
            {
                case Keys.W:
                    avatar.Steps = 1;
                    break;
                case Keys.S:
                    avatar.Steps = -1;
                    break;
                case Keys.A:
                    avatar.Yaw = 1;
                    break;
                case Keys.D:
                    avatar.Yaw = -1;
                    break;
            }
            base.OnKeyDown(kea);
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
