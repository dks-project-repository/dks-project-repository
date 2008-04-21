/*Daniel Frankel
 * Kevin Yedlin
 * Comp 565 - Project 1
*/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{

    public class ModeledMesh3D : Object3D, IDrawable
    {
        protected Device display = null;
        protected Mesh mesh = null;
        protected Material[] material;
        protected Material[] meshMaterial;
        protected ExtendedMaterial[] mtrl;
        protected Texture texture = null;
        protected bool textured = false;
        // mesh bounding sphere radius -- center will be mesh's Location
        protected float radius;

        // Constructors and initialize method

        private void initializeMesh(string meshFile)
        {
            display = scene.Display;  // get display device from SceneWorld
            textured = false;
            mesh = Mesh.FromFile("..\\..\\MeshTextures\\" + meshFile, MeshFlags.Managed, display, out mtrl);
            meshMaterial = new Material[mtrl.Length];
            for (int i = 0; i < mtrl.Length; i++)
            {
                meshMaterial[i] = mtrl[i].Material3D;
                if (!meshFile.ToLower().Contains("avatar"))
                    meshMaterial[i].Ambient = meshMaterial[i].Diffuse;

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
            // display mesh's center Location and radius
            Trace = string.Format("center: ( {0:F0} {1:F0} {2:F0} )  radius:  {3:F0}\n",
               Location.X, Location.Y, Location.Z, Radius);
        }

        private void initializeMesh(Mesh mesh, ExtendedMaterial[] mtrl)
        {
            display = scene.Display;  // get display device from SceneWorld
            textured = false;
            meshMaterial = new Material[mtrl.Length];
            for (int i = 0; i < mtrl.Length; i++)
            {
                meshMaterial[i] = mtrl[i].Material3D;
                meshMaterial[i].Ambient = meshMaterial[i].Diffuse;

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
            // display mesh's center Location and radius
            Trace = string.Format("center: ( {0:F0} {1:F0} {2:F0} )  radius:  {3:F0}\n",
               Location.X, Location.Y, Location.Z, Radius);
        }

        private void initializeTexturedMesh(string meshFile, string textureFile)
        {
            initializeMesh(meshFile);
            Textured = true;
            Texture = TextureLoader.FromFile(display, "..\\..\\MeshTextures\\" + textureFile);

        }

        // mesh w/o position, orientation axis, radians, or texture
        public ModeledMesh3D(SceneWorld sw, string label, string meshFile)
            : base(sw, label, new Vector3(), new Vector3(), 0.0f)
        {
            initializeMesh(meshFile);
        }

        // mesh w/o position, orientation axis, or radians and texture
        public ModeledMesh3D(SceneWorld sw, string label, string meshFile, string textureFile)
            : base(sw, label, new Vector3(), new Vector3(), 0.0f)
        {
            initializeTexturedMesh(meshFile, textureFile);
        }

        // mesh w/o texture
        public ModeledMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, string meshFile)
            : base(sw, label, position, orientAxis, radians)
        {
            initializeMesh(meshFile);
        }

        public ModeledMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, Mesh mesh, ExtendedMaterial[] mtrl)
            : base(sw, label, position, orientAxis, radians)
        {
            this.mesh = mesh;
            initializeMesh(mesh, mtrl);
        }

        // mesh w/ all arguments
        public ModeledMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, string meshFile, string textureFile)
            : base(sw, label, position, orientAxis, radians)
        {
            initializeTexturedMesh(meshFile, textureFile);
        }

        public static Mesh openMeshFile(string meshFile, Device display, out ExtendedMaterial[] mtrl)
        {
            return Mesh.FromFile("..\\..\\MeshTextures\\" + meshFile, MeshFlags.Managed, display, out mtrl);
        }
        // Properties

        public bool Textured
        {
            get { return textured; }
            set { textured = value; }
        }

        public Texture Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public float Radius { get { return radius; } }

        // Methods 

        public virtual void draw()
        {
            Matrix temp = display.Transform.World;  // save Transform state
            display.Transform.World = orientation;
            //if (Textured)
                display.SetTexture(0, Texture);
            for (int i = 0; i < meshMaterial.Length; i++)
            {
                display.Material = meshMaterial[i];
                mesh.DrawSubset(i);
            }
            display.Transform.World = temp; // restore Transform state
        }
    }
}
