using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SceneWorld
{

    public interface IDrawable
    {
        float Radius { get; }
        Vector3 Location { get; }
        void draw();
    }
}

