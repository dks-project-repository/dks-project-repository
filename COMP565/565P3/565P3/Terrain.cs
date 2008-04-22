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

            collider = new BoxCollider.CollisionMesh(model, 4);
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

        // TODO: delete everything below this line

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

        // Figure out where a line intersects the heightmap
        public Vector3? intersection(Vector3 start, Vector3 end)
        {
            Ray ray = new Ray(start, Vector3.Normalize(end - start));

            foreach (Vector3[] triangle in trianglesBelowSegment(start, end))
            {
                // See if given triangle intersects with ray
                Vector3? intersection = triangleRayIntersection(triangle, ray);
                if (intersection != null)
                {
                    // intersection! yay.
                    return intersection;
                }
            }

            return null;
        }

        private static bool isBetween(float number, float endpoint1, float endpoint2)
        {
            if (endpoint1 < endpoint2)
                return number >= endpoint1 && number <= endpoint2;
            return number >= endpoint2 && number <= endpoint1;
        }

        // Enumerable list of sets of 3 vertices that form triangles on the heightmap that share x/z with the given line
        public IEnumerable<Vector3[]> trianglesBelowSegment(Vector3 start, Vector3 end)
        {
            // Convert to map origin
            start -= map.Position;
            end -= map.Position;

            // First get the quad we're currently in
            float x0 = start.X - (start.X % map.TerrainScale);
            float x1 = x0 + map.TerrainScale;
            float z0 = start.Z - (start.Z % map.TerrainScale);
            float z1 = z0 + map.TerrainScale;

            Vector3 direction = end - start;
            bool xIncreasing = direction.X > 0, zIncreasing = direction.Z > 0;

            float scale = map.TerrainScale;
            Vector3[] triangle1 = new Vector3[3], triangle2 = new Vector3[3];
            bool firstQuad = true;
            bool triangle1First = true;

            while (isBetween(x0, start.X, end.X) && isBetween(z0, start.Z, end.Z) &&
                   isBetween(x1, start.X, end.X) && isBetween(z1, start.Z, end.Z))
            {
                // Send the two triangles of the associated quad. This code matches up with how triangles are made in TerrainProcessor.
                // Coordinates are converted back to world-origin coordinates, and Y value is retrieved.
                triangle1[0].X = x0;
                triangle1[0].Z = z0;
                triangle1[0] += map.Position;
                triangle1[0].Y = GetHeight(triangle1[0]);
                triangle1[1].X = x1;
                triangle1[1].Z = z0;
                triangle1[1] += map.Position;
                triangle1[1].Y = GetHeight(triangle1[1]);
                triangle1[2].X = x1;
                triangle1[2].Z = z1;
                triangle1[2] += map.Position;
                triangle1[2].Y = GetHeight(triangle1[2]);

                triangle2[0].X = triangle1[0].X;
                triangle2[0].Z = triangle1[0].Z;
                triangle1[0].Y = triangle1[0].Y;
                triangle2[1].X = x1;
                triangle2[1].Z = z1;
                triangle2[1] -= map.Position;
                triangle2[1].Y = GetHeight(triangle1[1]);
                triangle2[2].X = x0;
                triangle2[2].Z = z1;
                triangle2[2] -= map.Position;
                triangle2[2].Y = GetHeight(triangle1[2]);

                // Figure out which triangle in the quad is first along our ray
                if (xIncreasing && !zIncreasing)
                {
                    triangle1First = false;
                }
                else if (zIncreasing && !xIncreasing)
                {
                    triangle1First = true;
                }
                else
                {
                    if ((direction.X < direction.Z) ^ (xIncreasing /*&& zIncreasing*/))
                    {
                        triangle1First = false;
                    }
                    else
                    {
                        triangle1First = true;
                    }
                }

                // All the firstQuad stuff skips the first triangle we'd normally return if start is not in said triangle
                if (triangle1First)
                {
                    if (firstQuad && !inTriangle(triangle1, start))
                    {
                        yield return triangle2;
                    }
                    else
                    {
                        yield return triangle1;
                        yield return triangle2;
                    }
                }
                else
                {
                    //triangle2First
                    if (firstQuad && !inTriangle(triangle2, start))
                    {
                        yield return triangle1;
                    }
                    else
                    {
                        yield return triangle2;
                        yield return triangle1;
                    }

                }


                // Now get the next quad our line goes through

                Vector3 px1, px2, pz1, pz2;
                if (xIncreasing)
                {
                    px1 = new Vector3(x0, 0, z1);
                    px2 = new Vector3(x1, 0, z1);
                }
                else
                {
                    px1 = new Vector3(x0, 0, z0);
                    px2 = new Vector3(x1, 0, z0);
                }
                if (zIncreasing)
                {
                    pz1 = new Vector3(x1, 0, z0);
                    pz2 = new Vector3(x1, 0, z1);
                }
                else
                {
                    pz1 = new Vector3(x0, 0, z0);
                    pz2 = new Vector3(x0, 0, z1);
                }

                Vector3? xIntersection = segmentIntersection(start, end, px1, px2);
                Vector3? zIntersection = segmentIntersection(start, end, pz1, pz2);

                if (xIntersection == null && zIntersection == null)
                {
                    // move diagonally to next quad
                    x0 += xIncreasing ? scale : -scale;
                    z0 += zIncreasing ? scale : -scale;
                    x1 += xIncreasing ? scale : -scale;
                    z1 += zIncreasing ? scale : -scale;
                }
                else if (zIntersection == null)
                {
                    // x intersection! move along x-axis to next quad
                    z0 += zIncreasing ? scale : -scale;
                    z1 += zIncreasing ? scale : -scale;
                }
                else
                {
                    // z intersection! move along z-axis to next quad
                    x0 += xIncreasing ? scale : -scale;
                    x1 += xIncreasing ? scale : -scale;
                }
            }
        }

        // Gives point of triangle-line intersection, or null if they don't intersect
        public static Vector3? triangleRayIntersection(Vector3[] triangle, Ray ray)
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
        public static bool inTriangle(Vector3[] triangle, Vector3 point)
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

        /*
        // Enumerable list of all intersections of given line segment with heightmap quad edges
        public IEnumerable<Vector3> edgeLineIntersections(Vector3 start3, Vector3 end3)
        {
            // Convert to map origin
            start3 -= map.Position;
            end3 -= map.Position;

            // Just pretend all the Ys in the Vector2s are Zs.
            Vector2 start = new Vector2(start3.X, start3.Z);
            Vector2 end = new Vector2(end3.X, end3.Z);

            Vector2 direction = Vector2.Normalize(end - start);

            int xdir = direction.X > 0 ? 1 : -1;
            int zdir = direction.Y > 0 ? 1 : -1;

            // Find first edge that line crosses.

            float xbelow = start.X - (start.X % map.TerrainScale);
            float xabove = xbelow + map.TerrainScale;
            float zbelow = start.Y - (start.Y % map.TerrainScale);
            float zabove = zbelow + map.TerrainScale;

            // xstart/xend are parallel to the x-axis
            Vector2 xstart = zdir > 0 ? new Vector2(xbelow, zabove) : new Vector2(xbelow, zbelow);
            Vector2 xend = zdir > 0 ? new Vector2(xabove, zabove) : new Vector2(xabove, zbelow);

            Vector2 zstart = xdir > 0 ? new Vector2(xabove, zbelow) : new Vector2(xbelow, zbelow);
            Vector2 zend = xdir > 0 ? new Vector2(xabove, zabove) : new Vector2(xbelow, zabove);

            Vector3 intersection = segmentIntersection(start, end, xstart, xend);

            bool xIntersection;
            if (intersection.X != float.PositiveInfinity)
            {
                // The first intersection is parallel to the x-axis
                xIntersection = true;
            }
            else
            {
                // The first intersection is parallel to the z-axis
                xIntersection = false;
                intersection = segmentIntersection(start, end, zstart, zend);
            }

            // Now that we have the first edge intersection, convert it to indices, send it through the iterator, then get the next one.
            while (true)
            {
                // Convert intersection to the two index pairs that define the edge we're currently on
                //int[,] result = new int[2, 2];
                //result[0, 0] = (int)(intersection.X / map.TerrainScale);
                //result[0, 1] = (int)(intersection.Y / map.TerrainScale);
                //result[1, 0] = result[0, 0] + (xIntersection ? 1 : 0);
                //result[1, 1] = result[0, 1] + (xIntersection ? 0 : 1);
                yield return intersection;

                // TODO: get the next edge
            }

            // This iterator never ends. It must be broken by the caller.
        }
        */

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
    }
}
