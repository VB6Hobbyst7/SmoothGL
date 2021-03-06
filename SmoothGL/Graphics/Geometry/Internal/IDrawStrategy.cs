﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace SmoothGL.Graphics.Internal
{
    public interface IDrawStrategy
    {
        void Draw(Primitive primitiveType, int startElement, int numberOfElements);
        void DrawMultiple(Primitive primitiveType, int startElement, int numberOfElements, int numberOfInstances);
    }
}
