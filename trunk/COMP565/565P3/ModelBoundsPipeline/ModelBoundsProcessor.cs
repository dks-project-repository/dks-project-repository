using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace ModelBoundsPipeline
{
    [ContentProcessor(DisplayName = "Model + BoundingBox")]
    public class ModelBoundsProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ModelContent m = base.Process(input, context);
            m.Tag = calculateBoundingBox(input.Children);
            return m;
        }

        private BoundingBox calculateBoundingBox(NodeContentCollection ncc)
        {
            calculateMinMax(ncc);
            Vector3 min = new Vector3(minX, minY, minZ);
            Vector3 max = new Vector3(maxX, maxY, maxZ);
            return new BoundingBox(min, max);
        }

        // Adapted from http://andyq.no-ip.com/blog/?p=16
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float minZ = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float maxZ = float.MinValue;
        private void calculateMinMax(NodeContentCollection ncc)
        {
            foreach (NodeContent nc in ncc)
            {
                if (nc is MeshContent)
                {
                    MeshContent mc = (MeshContent)nc;
                    Matrix transform = mc.AbsoluteTransform;
                    foreach (Vector3 basev in mc.Positions)
                    {
                        Vector3 v = Vector3.Transform(basev, transform);
                        if (v.X < minX)
                            minX = v.X;

                        if (v.Y < minY)
                            minY = v.Y;

                        if (v.Z < minZ)
                            minZ = v.Z;

                        if (v.X > maxX)
                            maxX = v.X;

                        if (v.Y > maxY)
                            maxY = v.Y;

                        if (v.Z > maxZ)
                            maxZ = v.Z;
                    }
                }
                else
                    calculateMinMax(nc.Children);
            }
        }
    }
}