using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game465P3
{
    public class Terrain : Drawable
    {
        protected TanksOnAHeightmap.HeightMapInfo map;
        public BoxCollider.CollisionMesh collider;

        public Terrain(World game, Vector3 location, Model model)
            : base(game, location, model)
        {
            map = model.Tag as TanksOnAHeightmap.HeightMapInfo;
            if (map == null)
            {
                string message = "The terrain model did not have a HeightMapInfo " +
                    "object attached. Are you sure you are using the " +
                    "TerrainProcessor?";
                throw new InvalidOperationException(message);
            }

            collider = new BoxCollider.CollisionMesh(model, 5);

            Vector3 max = new Vector3(map.Width / 2, 0, map.Height / 2);
            Vector3 min = new Vector3(-map.Width / 2, -MaxTerrainHeight, -map.Height / 2);
            bounds = new BoundingBox(min, max);
        }

        public float GetHeight(Vector3 pos)
        {
            return map.GetHeight(pos);
        }

        public Vector3 GetNormal(Vector3 pos)
        {
            return map.GetQuadNormal(pos);
        }

        public bool IsOnHeightMap(Vector3 pos)
        {
            return map.IsOnHeightmap(pos);
        }

        public bool onGround(Vector3 location)
        {
            return location.Y <= game.terrain.GetHeight(location) + Settings.collisionHeightAboveTerrain;
        }

        public float Width { get { return map.Width; } }
        public float Height { get { return map.Height; } }
        public Vector3 Position { get { return map.Position; } }
        public float MaxTerrainHeight { get { return map.Bumpiness; } }

        public void drawTopDown()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateTranslation(0, -90000, 0);
                    effect.View = Matrix.CreateRotationX(MathHelper.PiOver2);
                    effect.Projection = Matrix.CreateOrthographic(map.Width, map.Height, 0, 100000);
                }
                mesh.Draw();
            }
        }

        // Clamps p.X and p.Z to be within the map, with padding of TerrainScale
        public void clamp(ref Vector3 p, out int x, out int z)
        {

            p -= map.Position;

            if (p.X <= map.TerrainScale)
            {
                p.X = map.TerrainScale;
                x = -1;
            }
            else if (p.X >= map.Width - map.TerrainScale)
            {
                p.X = map.Width - map.TerrainScale;
                x = 1;
            }
            else
                x = 0;

            if (p.Z <= map.TerrainScale)
            {
                p.Z = map.TerrainScale;
                z = -1;
            }
            else if (p.Z >= map.Height - map.TerrainScale)
            {
                p.Z = map.Height - map.TerrainScale;
                z = 1;
            }
            else
                z = 0;

            p += map.Position;
        }

        // Sets x/z to -1 if on min edge, 1 if on max edge, 0 otherwise
        public void edgeTest(Vector3 pos, out int x, out int z)
        {
            pos -= map.Position;

            float p = pos.X;
            if (p == map.TerrainScale)
                x = -1;
            else if (p == map.Width - map.TerrainScale)
                x = 1;
            else
                x = 0;

            p = pos.Z;
            if (p == map.TerrainScale)
                z = -1;
            else if (p == map.Height - map.TerrainScale)
                z = 1;
            else
                z = 0;
        }


        #region Useful stuff that I'm not using anymore

        // Gives point of triangle-line intersection, or null if they don't intersect
        static Vector3? triangleRayIntersection(Vector3[] triangle, Ray ray)
        {
            Plane p = new Plane(triangle[0], triangle[1], triangle[2]);
            float? result = ray.Intersects(p);
            if (result != null)
            {
                // We now have a ray-plane intersection. We want a ray-triangle intersection.
                Vector3 intersection = ray.Position + ray.Direction * (float)result;

                // Check to see if the intersection is in the triangle by converting to barycentric coordinates
                if (inTriangle(triangle, intersection))
                {
                    return intersection;
                }
            }
            return null;
        }

        // Algorithm from http://en.wikipedia.org/wiki/Barycentric_coordinates_%28mathematics%29
        static bool inTriangle(Vector3[] triangle, Vector3 point)
        {
            float A = triangle[0].X - triangle[2].X;
            float B = triangle[1].X - triangle[2].X;
            float C = triangle[2].X - point.X;
            float D = triangle[0].Y - triangle[2].Y;
            float E = triangle[1].Y - triangle[2].Y;
            float F = triangle[2].Y - point.Y;
            float G = triangle[0].Z - triangle[2].Z;
            float H = triangle[1].Z - triangle[2].Z;
            float I = triangle[2].Z - point.Z;

            if (A == 0 && B == 0)
            {
                // handle case where triangle is perpendicular to x-axis... see wikipedia article
                float temp = A;
                A = D;
                D = temp;
                temp = B;
                B = E;
                E = temp;
                temp = C;
                C = F;
                F = temp;
            }

            float b1 = (B * (F + I) - C * (E + H)) / (A * (E + H) - B * (D + G));
            float b2 = (A * (F + I) - C * (D + G)) / (B * (D + G) - A * (E - H));

            if (b1 >= 0 && b1 <= 1 && b2 >= 0 && b2 <= 1)
            {
                // Intersection is inside the triangle!
                return true;
            }

            return false;
        }

        // Code adapted from http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline2d/
        static Vector3? segmentIntersection(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4)
        {
            float denom = ((P4.Z - P3.Z) * (P2.X - P1.X)) -
                          ((P4.X - P3.X) * (P2.Z - P1.Z));

            float numeratorA = ((P4.X - P3.X) * (P1.Z - P3.Z)) -
                               ((P4.Z - P3.Z) * (P1.X - P3.X));

            float numeratorB = ((P2.X - P1.X) * (P1.Z - P3.Z)) -
                               ((P2.Z - P1.Z) * (P1.X - P3.X));

            if (denom == 0.0f)
            {
                //if (numeratorA == 0.0f && numeratorB == 0.0f)
                //{
                //    return IntersectResult.Coincident;
                //}
                //return IntersectResult.Parallel;
                return null;
            }

            float ua = numeratorA / denom;
            float ub = numeratorB / denom;

            // NOTE: these were formely <= instead of <
            if (ua >= 0.0f && ua < 1.0f && ub >= 0.0f && ub < 1.0f)
            {
                // Get the intersection point.
                Vector3 intersection = new Vector3();
                intersection.X = P1.X + ua * (P2.X - P1.X);
                intersection.Z = P1.Z + ua * (P2.Z - P1.Z);

                //return IntersectResult.Intersecting;
                return intersection;
            }

            //return IntersectResult.NotIntersecting;
            return null;
        }

        #endregion
    }
}
