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
        //protected Material[] material;
        protected Material[] meshMaterial;
        protected ExtendedMaterial[] mtrl;
        protected Texture texture = null;
        protected bool textured = false;
        // mesh bounding sphere radius -- center will be mesh's Location
        protected float radius;
        public static bool alternateMoveMode = false;

        // Constructors and initialize method

        // Only use this if you're only making one copy of an object
        // Less memory and faster to use the same Mesh and Material[] multiple times
        private void initializeMesh(string meshFile)
        {
            MeshData data = new MeshData(scene.Display, meshFile);
            initializeMesh(data);
        }

        private void initializeMesh(MeshData data)
        {
            display = scene.Display;  // get display device from SceneWorld
            textured = false;
            mesh = data.Mesh;
            meshMaterial = data.Materials;
            radius = data.Radius;

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

        private void initializeTexturedMesh(MeshData data)
        {
            initializeMesh(data);
            if (data.Texture != null)
            {
                Textured = true;
                Texture = data.Texture;
            }
        }

        // mesh w/o position, orientation axis, radians, or texture
        public ModeledMesh3D(SceneWorld sw, string label, string meshFile)
            : base(sw, label, new Vector3(), new Vector3(), 0.0f)
        {
            initializeMesh(meshFile);
        }

        // mesh w/o position, orientation axis, radians, or texture - MeshData
        public ModeledMesh3D(SceneWorld sw, string label, MeshData mesh)
            : base(sw, label, new Vector3(), new Vector3(), 0.0f)
        {
            initializeMesh(mesh);
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

        // mesh w/ all arguments
        public ModeledMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, string meshFile, string textureFile)
            : base(sw, label, position, orientAxis, radians)
        {
            initializeTexturedMesh(meshFile, textureFile);
        }

        // mesh w/ all arguments (or w/o texture) - MeshData
        public ModeledMesh3D(SceneWorld sw, string label, Vector3 position,
           Vector3 orientAxis, float radians, MeshData mesh)
            : base(sw, label, position, orientAxis, radians)
        {
            initializeTexturedMesh(mesh);
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
            /*if (Textured)*/
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
