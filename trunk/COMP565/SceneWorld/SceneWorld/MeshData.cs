using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{
    public class MeshData
    {
        protected Mesh mesh;
        protected Material[] mat;
        protected float radius;
        protected Texture tex;

        public Mesh Mesh
        {
            get
            {
                return mesh;
            }
        }

        public Material[] Materials
        {
            get
            {
                return mat;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
        }

        public Texture Texture
        {
            get
            {
                return tex;
            }
        }

        private void initializeMesh(Device display, string meshFile)
        {
            ExtendedMaterial[] mtrl;
            mesh = Mesh.FromFile("..\\..\\MeshTextures\\" + meshFile, MeshFlags.Managed, display, out mtrl);
            mat = new Material[mtrl.Length];
            for (int i = 0; i < mtrl.Length; i++)
            {
                mat[i] = mtrl[i].Material3D;
                mat[i].Ambient = mat[i].Diffuse;
            }
            // compute min,center, and max from mesh's bounding sphere and box
            using (VertexBuffer vb = mesh.VertexBuffer)
            {
                Vector3 center;
                GraphicsStream vertexData = vb.Lock(0, 0, LockFlags.None);
                radius = Geometry.ComputeBoundingSphere(vertexData,
                   mesh.NumberVertices, mesh.VertexFormat, out center);
                vb.Unlock();
            }
        }

        public MeshData(Device display, string meshFile)
        {
            tex = null;
            initializeMesh(display, meshFile);
        }

        public MeshData(Device display, string meshFile, string textureFile)
        {
            initializeMesh(display, meshFile);
            tex = TextureLoader.FromFile(display, "..\\..\\MeshTextures\\" + textureFile);
        }
    }
}
