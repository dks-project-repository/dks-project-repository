#region File Description
//-----------------------------------------------------------------------------
// CollisionMesh.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoxCollider
{
    public class CollisionMesh
    {
        // mesh vertices
        Vector3[] vertices;
        // mesh faces
        CollisionFace[] faces;
        // tree with meshes faces
        CollisionTree tree;

        public CollisionMesh(Model model, uint subdivLevel)
        {
            int totalNumFaces = 0;
            int totalNumVertices = 0;
            foreach (ModelMesh mesh in model.Meshes)
            {
                int numberVertices, numberIndices;
                numberVertices =
                    mesh.VertexBuffer.SizeInBytes / mesh.MeshParts[0].VertexStride;
                if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
                    numberIndices = mesh.IndexBuffer.SizeInBytes / sizeof(short);
                else
                    numberIndices = mesh.IndexBuffer.SizeInBytes / sizeof(int);

                totalNumVertices += numberVertices;
                totalNumFaces += numberIndices / 3;
            }

            vertices = new Vector3[totalNumVertices];
            faces = new CollisionFace[totalNumFaces];

            int vertexCount = 0;
            int faceCount = 0;
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                int numberVertices =
                    mesh.VertexBuffer.SizeInBytes / mesh.MeshParts[0].VertexStride;
                Matrix meshTransform = modelTransforms[mesh.ParentBone.Index];

                if (mesh.MeshParts[0].VertexStride == 16)
                {
                    VertexPositionColor[] meshVertices =
                        new VertexPositionColor[numberVertices];
                    mesh.VertexBuffer.GetData<VertexPositionColor>(meshVertices);

                    for (int i = 0; i < numberVertices; i++)
                    {
                        vertices[i + vertexCount] =
                            Vector3.Transform(meshVertices[i].Position, meshTransform);
                    }
                }
                if (mesh.MeshParts[0].VertexStride == 20)
                {
                    VertexPositionTexture[] meshVertices =
                                              new VertexPositionTexture[numberVertices];
                    mesh.VertexBuffer.GetData<VertexPositionTexture>(meshVertices);

                    for (int i = 0; i < numberVertices; i++)
                    {
                        vertices[i + vertexCount] =
                            Vector3.Transform(meshVertices[i].Position, meshTransform);
                    }
                }
                else if (mesh.MeshParts[0].VertexStride == 24)
                {
                    VertexPositionColorTexture[] meshVertices =
                        new VertexPositionColorTexture[numberVertices];

                    mesh.VertexBuffer.GetData<VertexPositionColorTexture>(meshVertices);

                    for (int i = 0; i < numberVertices; i++)
                    {
                        vertices[i + vertexCount] =
                            Vector3.Transform(meshVertices[i].Position, meshTransform);
                    }
                }
                else if (mesh.MeshParts[0].VertexStride == 32)
                {
                    VertexPositionNormalTexture[] meshVertices =
                        new VertexPositionNormalTexture[numberVertices];
                    mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(meshVertices);

                    for (int i = 0; i < numberVertices; i++)
                    {
                        vertices[i + vertexCount] =
                            Vector3.Transform(meshVertices[i].Position, meshTransform);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false, "Unsupported vertex format");
                }

                int numberFaces = 0;

                if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
                {
                    short[] meshIndices =
                            new short[mesh.IndexBuffer.SizeInBytes / sizeof(short)];
                    mesh.IndexBuffer.GetData<short>(meshIndices);

                    int count = 0;
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        for (int i = 0; i < meshPart.PrimitiveCount; i++)
                        {
                            faces[numberFaces + faceCount] =
                                new CollisionFace(count, meshIndices,
                                vertexCount + meshPart.BaseVertex, vertices);
                            count += 3;
                            numberFaces++;
                        }
                    }
                }
                else
                {
                    int[] meshIndices =
                            new int[mesh.IndexBuffer.SizeInBytes / sizeof(int)];
                    mesh.IndexBuffer.GetData<int>(meshIndices);

                    int count = 0;
                    foreach (ModelMeshPart mesh_part in mesh.MeshParts)
                    {
                        for (int i = 0; i < mesh_part.PrimitiveCount; i++)
                        {
                            faces[numberFaces + faceCount] =
                                new CollisionFace(count, meshIndices,
                                vertexCount + mesh_part.BaseVertex, vertices);
                            count += 3;
                            numberFaces++;
                        }
                    }
                }

                vertexCount += numberVertices;
                faceCount += numberFaces;
            }

            CollisionBox box = new CollisionBox(float.MaxValue, -float.MaxValue);
            for (int i = 0; i < vertexCount; i++)
                box.AddPoint(vertices[i]);

            if (subdivLevel > 6)
                subdivLevel = 6; // max 8^6 nodes
            tree = new CollisionTree(box, subdivLevel);
            for (int i = 0; i < faceCount; i++)
                tree.AddElement(faces[i]);
        }

        public bool PointIntersect(
            Vector3 rayStart,
            Vector3 rayEnd,
            out float intersectDistance,
            out Vector3 intersectPosition,
            out Vector3 intersectNormal)
        {
            return tree.PointIntersect(rayStart, rayEnd, vertices,
                out intersectDistance, out intersectPosition, out intersectNormal);
        }

        public bool BoxIntersect(
            CollisionBox box,
            Vector3 rayStart,
            Vector3 rayEnd,
            out float intersectDistance,
            out Vector3 intersectPosition,
            out Vector3 intersectNormal)
        {
            return tree.BoxIntersect(box, rayStart, rayEnd, vertices,
                out intersectDistance, out intersectPosition, out intersectNormal);
        }

        public void PointMove(
            Vector3 pointStart,
            Vector3 pointEnd,
            float frictionFactor,
            float bumpFactor,
            uint recurseLevel,
            out Vector3 pointResult)
        {
            tree.PointMove(pointStart, pointEnd, vertices,
                frictionFactor, bumpFactor, recurseLevel,
                out pointResult);
        }

        public bool BoxMove(
            CollisionBox box, Vector3 pointStart, Vector3 pointEnd,
            float frictionFactor, float bumpFactor, uint recurseLevel,
            out Vector3 pointResult)
        {
            return tree.BoxMove(box, pointStart, pointEnd,
                vertices, frictionFactor, bumpFactor, recurseLevel,
                out pointResult);
        }

        public void GetElements(CollisionBox b, List<CollisionTreeElem> e)
        {
            tree.GetElements(b, e);
        }

        public void AddElement(CollisionTreeElem e)
        {
            tree.AddElement(e);
        }

        public void RemoveElement(CollisionTreeElemDynamic e)
        {
            tree.RemoveElement(e);
        }
    }
}
