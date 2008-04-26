using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game465P3
{
    public class LinkedCamera : Camera, IDisposable
    {
        public Object3D target;

        public float zoom;
        public float targetZoom;

        public float FOV = Settings.FOVdegrees;
        public float prevFOV = Settings.FOVdegrees;
        public float targetFOV = Settings.FOVdegrees;
        public int FOVindex = 0;

        public LinkedCamera(World g, Object3D t)
            : base(g)
        {
            target = t;
            position = target.transform.Translation;
            zoom = targetZoom = Settings.minZoom;
        }

        public override void update()
        {
            // Allow freelook for non-movables
            if (!(target is Avatar))
            {
                Vector3 newAt = game.input.handleRotation(ref yaw, ref pitch);
                transform = target.transform * Matrix.CreateLookAt(Vector3.Zero, newAt, Vector3.Up);
            }
            else
            {
                // First person zoom
                if (game.input.IsKeyPressed(Settings.changeFOV))
                {
                    FOVindex++;
                    if (FOVindex >= Settings.FOVdividers.Length)
                        FOVindex = 0;
                    //prevFOV = targetFOV;
                }
                if (game.input.IsKeyDown(Settings.FOVzoom) && targetZoom <= Settings.minZoom)
                {
                    targetFOV = Settings.FOVdegrees / Settings.FOVdividers[FOVindex];
                }
                else
                {
                    targetFOV = Settings.FOVdegrees;
                }
                if (FOV == targetFOV)
                    prevFOV = targetFOV;
                else if (FOV < targetFOV && FOV < prevFOV || FOV > targetFOV && FOV > prevFOV)
                    prevFOV = FOV;
                if (FOV != targetFOV)
                {
                    int dir = Math.Sign(targetFOV - FOV);
                    FOV += (targetFOV - prevFOV) / Settings.FOVchangeFrames;
                    if (dir > 0 && FOV > targetFOV || dir < 0 && FOV < targetFOV)
                        FOV = targetFOV;
                    game.createProjection(FOV);
                }


                // Third person zoom
#if XBOX360
                if (game.input.IsButtonPressed(Settings.zoomIn))
                    targetZoom /= Settings.zoomMultiplier;
                if (game.input.IsButtonPressed(Settings.zoomOut))
                    targetZoom *= Settings.zoomMultiplier;
#else
                if (game.input.Scroll > 0 || game.input.IsButtonPressed(Settings.zoomIn))
                    targetZoom /= Settings.zoomMultiplier;
                if (game.input.Scroll < 0 || game.input.IsButtonPressed(Settings.zoomOut))
                    targetZoom *= Settings.zoomMultiplier;
#endif
                targetZoom = MathHelper.Clamp(targetZoom, Settings.minZoom, Settings.maxZoom);
                if (zoom != targetZoom)
                {
                    if (targetZoom <= Settings.minZoom + .01f)
                        zoom = targetZoom - .01f;
                    else if (Math.Abs(zoom - targetZoom) < .05f)
                        zoom = targetZoom;
                    else
                        zoom += (targetZoom - zoom) / 8f;
                }

                Avatar a = (Avatar)target;
                a.update();
                Vector3 aAt = a.actualAt, loc = a.transform.Translation;
                loc.Y += Settings.cameraHeight;

                if (zoom <= Settings.minZoom)
                {
                    a.IsDrawn = false;
                    transform = Matrix.CreateLookAt(loc, loc + aAt, Vector3.Up);
                }
                else
                {
                    a.IsDrawn = true;
                    position = loc - zoom * aAt;
                    if (game.terrain.IsOnHeightMap(position))
                    {
                        float terrainHeight = game.terrain.GetHeight(position) + Settings.cameraHeight;
                        if (position.Y < terrainHeight)
                            position.Y = terrainHeight;
                    }
                    else if (position.Y < loc.Y)
                    {
                        position.Y = loc.Y;
                    }
                    transform = Matrix.CreateLookAt(position, loc, Vector3.Up);
                }
            }
        }

        public void Dispose()
        {
            if (target is Drawable)
                ((Drawable)target).IsDrawn = true;

            targetFOV = FOV = Settings.FOVdegrees;
            game.createProjection(FOV);
        }
    }
}
