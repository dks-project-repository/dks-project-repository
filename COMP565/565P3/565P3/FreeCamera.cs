using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game465P3
{
    public class FreeCamera : Camera
    {
        public FreeCamera(World game, Matrix m)
            : base(game)
        {
            transform = m;
        }

        public FreeCamera(World game, Vector3 cameraPos, Vector3 cameraTarget)
            : base(game)
        {
            lookAt(cameraPos, cameraTarget);
        }

        public override void update()
        {
            Vector3 newAt = game.input.handleRotation(ref yaw, ref pitch);
            if (game.input.IsKeyDown(Settings.freeCameraSpeedUp) || game.input.IsButtonDown(Settings.freeCameraSpeedUpButton))
                position += game.input.handleTranslation(newAt) * Settings.freeCameraSpeedFast;
            else
                position += game.input.handleTranslation(newAt) * Settings.freeCameraSpeed;

            transform = Matrix.CreateLookAt(position, position + newAt, Vector3.Up);
        }

        // yaw-euler conversion stolen from my 465 project
        public void lookAt(Vector3 position, Vector3 target)
        {
            transform = Matrix.CreateLookAt(position, target, Vector3.Up);
            this.position = position;
            yaw = (float)polarFromVector(position - target);
            pitch = MathHelper.PiOver2 - (float)Math.Asin(transform.M23);
        }

        private static double polarFromVector(Vector3 At)
        {
            if (At.X > 0 && At.Z >= 0)
                return Math.Atan(At.Z / At.X);
            if (At.X > 0 && At.Z < 0)
                return Math.Atan(At.Z / At.X) + 2 * Math.PI;
            if (At.X < 0)
                return Math.Atan(At.Z / At.X) + Math.PI;
            if (At.X == 0 && At.Z > 0)
                return Math.PI / 2;
            if (At.X == 0 && At.Z < 0)
                return 3 * Math.PI / 2;
            throw new ArgumentException();
        }
    }
}
