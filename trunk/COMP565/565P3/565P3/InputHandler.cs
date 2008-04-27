using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game465P3
{
    public class InputHandler : IDisposable
    {
        Game game;

        protected KeyboardState lastKeyState, currKeyState;
        protected GamePadState lastgamePadState, currgamePadState;
        protected bool gamePadConnected;

#if !XBOX360
        protected int centerX, centerY;
        protected int dX, dY;

        protected MouseState lastMouse = new MouseState(), currMouse = new MouseState();
        protected bool mouselook = false;

        public bool Mouselook
        {
            get
            {
                return mouselook;
            }
            set
            {
                mouselook = value;
                if (mouselook)
                {
                    Window_ClientSizeChanged(null, null);
                    recenter();
                    currMouse = Mouse.GetState(); // avoids view jumping caused by turning on mouselook
                }
                game.IsMouseVisible = !mouselook;
            }
        }

        public int Scroll { get { return currMouse.ScrollWheelValue - lastMouse.ScrollWheelValue; } }
        public bool LeftMouseDown { get { return currMouse.LeftButton == ButtonState.Pressed; } }
        public bool RightMouseDown { get { return currMouse.RightButton == ButtonState.Pressed; } }
        public bool LeftMouseClick { get { return currMouse.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released; } }
        public bool RightMouseClick { get { return currMouse.RightButton == ButtonState.Pressed && lastMouse.RightButton == ButtonState.Released; } }
#endif

        public InputHandler(Game g)
        {
            game = g;
            lastKeyState = currKeyState = Keyboard.GetState();
            gamePadConnected = GamePad.GetCapabilities(PlayerIndex.One).IsConnected;

#if !XBOX360
            game.Window.ClientSizeChanged += Window_ClientSizeChanged;
#endif
        }

#if !XBOX360
        protected void Window_ClientSizeChanged(object o, EventArgs args)
        {
            centerX = game.GraphicsDevice.Viewport.Width / 2;
            centerY = game.GraphicsDevice.Viewport.Height / 2;
            if (mouselook)
                recenter();
        }

        protected void recenter()
        {
            Mouse.SetPosition(centerX, centerY);
        }
#endif

        public void update()
        {
            lastKeyState = currKeyState;
            currKeyState = Keyboard.GetState();

            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                lastgamePadState = currgamePadState;
                currgamePadState = GamePad.GetState(PlayerIndex.One);
                gamePadConnected = true;
            }
            else
                gamePadConnected = false;

#if !XBOX360
            lastMouse = currMouse;
            currMouse = Mouse.GetState();

            if (mouselook)
            {
                dX = currMouse.X - centerX;
                dY = currMouse.Y - centerY;
                recenter();
            }
#endif
        }

        public void Dispose()
        {
#if !XBOX360
            game.Window.ClientSizeChanged -= Window_ClientSizeChanged;
#endif
        }

        public bool IsKeyPressed(Keys key)
        {
            return currKeyState.IsKeyDown(key) && !lastKeyState.IsKeyDown(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return currKeyState.IsKeyDown(key);
        }

        public bool IsButtonPressed(Buttons button)
        {
            if (!gamePadConnected)
                return false;
            return currgamePadState.IsButtonDown(button) && !lastgamePadState.IsButtonDown(button);
        }

        public bool IsButtonDown(Buttons button)
        {
            if (!gamePadConnected)
                return false;
            return currgamePadState.IsButtonDown(button);
        }

        // Calculates at vector given yaw/pitch input
        public Vector3 handleRotation(ref float yaw, ref float pitch)
        {
            // Keyboard
            if (currKeyState.IsKeyDown(Settings.rotateLeft))
            {
                yaw -= Settings.rotateStep;
            }
            if (currKeyState.IsKeyDown(Settings.rotateRight))
            {
                yaw += Settings.rotateStep;
            }

            // Mouse
#if !XBOX360
            if (mouselook)
            {
                if (dX != 0)
                {
                    yaw += Settings.mouseSensitivity * dX;
                }
                if (dY != 0)
                {
                    pitch -= Settings.mouseSensitivity * dY;
                }
            }
#endif

            // Gamepad
            if (gamePadConnected)
            {
                Vector2 stick = currgamePadState.ThumbSticks.Right;
                if (stick.Y != 0)
                {
                    yaw -= Settings.thumbstickSensitivity * stick.Y;
                }
                if (stick.X != 0)
                {
                    pitch += Settings.thumbstickSensitivity * stick.X;
                }
            }

            pitch = MathHelper.Clamp(pitch, .01f, MathHelper.Pi - .01f);

            Vector3 at = Vector3.Zero;
            at.X = (float)Math.Sin(pitch) * (float)Math.Cos(yaw);
            at.Z = (float)Math.Sin(pitch) * (float)Math.Sin(yaw);
            at.Y = (float)-Math.Cos(pitch);
            return at;
        }

        public Vector3 handleTranslation(Vector3 at)
        {
            Vector3 right = Vector3.Normalize(Vector3.Cross(at, Vector3.Up));
            Vector3 movement = Vector3.Zero;

            // Keyboard
            if (IsKeyDown(Settings.moveForward1) || IsKeyDown(Settings.moveForward2))
            {
                movement += at;
            }
            if (IsKeyDown(Settings.moveBackward1) || IsKeyDown(Settings.moveBackward2))
            {
                movement -= at;
            }

            if (IsKeyDown(Settings.strafeLeft))
            {
                movement -= right;
            }
            if (IsKeyDown(Settings.strafeRight))
            {
                movement += right;
            }

            // Gamepad
            if (gamePadConnected)
            {
                Vector2 stick = currgamePadState.ThumbSticks.Left;
                if (stick.Y != 0)
                {
                    movement += stick.Y * at;
                }
                if (stick.X != 0)
                {
                    movement += stick.X * right;
                }
            }

            if (movement != Vector3.Zero)
                return Vector3.Normalize(movement);
            return Vector3.Zero;
        }
    }
}
