using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    static class Settings
    {
        // Game
        public const float hither = 0.25f;
        public const float yon = 100000.0f;
        public const float FOVdegrees = 90;
        public const int initWidth = 1024;
        public const int initHeight = 576;
        public const int octreeDepth = 0;

        public static readonly float[] timeMultipliers = { 0.25f, 0.5f, 1, 2, 4, 8, 16, 32 };
        public const int timeMultiplierDefaultIndex = 2;

        // Physics
        public const float gravity = -.02f;
        public const float drag = 0.0001f;
        public const float jetPackAccel = 1.375f * -gravity;
        public const float jetPackLateralProportion = .2f;

        public const float jetExpend = 1 / 180f;
        public const float jetRefuel = 1 / 300f;
        public const float jetMinFuel = 1 / 16f;

        public const float skiStrafeProportion = .005f;
        public const float walkSpeed = .2f;
        public const float frictionSki = .001f;
        public const float frictionKinetic = .2f;
        public const float frictionStaticCutoff = -Settings.gravity * 2;
        public const float frictionSlopeCutoff = 0.8f;

        // I suggest not modifying these. They compensate for inaccuracies in ground collisions.
        public const float collisionHeightAboveTerrain = 0.4f; // This also controls jump height
        public const float collisionHeightDisplacement = 0.001f;

        // Controls
        public const Keys moveForward1 = Keys.W;
        public const Keys moveForward2 = Keys.Up;
        public const Keys moveBackward1 = Keys.S;
        public const Keys moveBackward2 = Keys.Down;
        public const Keys strafeLeft = Keys.A;
        public const Keys strafeRight = Keys.D;
        public const Keys rotateLeft = Keys.Left;
        public const Keys rotateRight = Keys.Right;
        public const Keys ski = Keys.Space;
        public const Keys freeCameraSpeedUp = Keys.LeftShift;
        public const Keys toggleMouselook = Keys.M;
        public const Keys killSelf = Keys.K;
        public const Keys switchWeapon = Keys.Q;

        public const Keys pause = Keys.P;
        public const Keys quit = Keys.Escape;
        public const Keys switchCamera = Keys.O;
        public const Keys uncapFPS = Keys.U;
        public const Keys timeSpeedUp = Keys.OemPlus;
        public const Keys timeSlowDown = Keys.OemMinus;
        public const Keys showMap = Keys.C;
        public const Keys drawBoundingBoxes = Keys.B;
        public const Keys changeFOV = Keys.Z;
        public const Keys FOVzoom = Keys.E;

        public const Buttons jet = Buttons.RightShoulder;
        public const Buttons skiButton = Buttons.LeftShoulder;
        public const Buttons freeCameraSpeedUpButton = Buttons.LeftStick;
        public const Buttons zoomIn = Buttons.DPadUp;
        public const Buttons zoomOut = Buttons.DPadDown;
        public const Buttons fireButton = Buttons.RightTrigger;
        public const Buttons switchWeaponButton = Buttons.Y;

        public const float thumbstickSensitivity = .0025f;
        public const float mouseSensitivity = .0025f;
        public const float rotateStep = .025f;

        // Camera
        public const float zoomMultiplier = 1.4f;
        public const float minZoom = 10;
        public const float maxZoom = 5976;

        public const int FOVchangeFrames = 15;
        public static readonly int[] FOVdividers = { 2, 5, 10, 20 };

        public const float cameraHeight = 2;
        public const float freeCameraSpeed = .5f;
        public const float freeCameraSpeedFast = 5;

        // GUI
        public const float minimapAlpha = 0.667f;
        public const float minimapSize = 1 / 4f;

        public static readonly Color reticleColor = new Color(Color.Lime.R, Color.Lime.G, Color.Lime.B, 127);

        public static readonly Color pauseBackgroundColor = new Color(31, 31, 31, 127);
        public static readonly Color pauseForegroundColor = Color.White;

        public static readonly Color healthMeterColor = Color.Crimson;
        public static readonly Color jetMeterColor = Color.RoyalBlue;
        public static readonly Color meterBackgroundColor = new Color(0, 0, 0, 127);

        // Avatar
        public const int deadMinTicks = 60;
        public const float damageMinSpeed = 90 / 60f;
        public const float damageSpeedMultiplier = .4f;

        // Projectiles
        public const float velocityStraightProjectile = 3.33f;
        public const float velocityLobProjectile = 2.5f;
        public const int lobBounces = 3;
        public const float explosionRadius = 5;
        public const float explosionDamage = .4f;
        public const float explosionAccel = 5f;
        public const float ceiling = 1000;
    }
}
