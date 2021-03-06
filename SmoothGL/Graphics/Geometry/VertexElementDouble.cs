﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace SmoothGL.Graphics
{
    /// <summary>
    /// Describes a vertex element as a vector of double precision floating point numbers.
    /// </summary>
    public class VertexElementDouble : IVertexElement
    {
        private int _location;
        private int _numberOfComponents;
        
        /// <summary>
        /// Creates a new vertex element description for a vector of doubles.
        /// </summary>
        /// <param name="location">Location at which this vertex element is accessible in the vertex shader.</param>
        /// <param name="numberOfComponents">Number of components of the described vector in the range between one and four.</param>
        public VertexElementDouble(int location, int numberOfComponents)
        {
            if (numberOfComponents < 1 || numberOfComponents > 4)
                throw new ArgumentOutOfRangeException("numberOfComponents", "The number of components must lie between one and four.");

            _location = location;
            _numberOfComponents = numberOfComponents;
        }

        public void ApplyDefinition(int strideSize, int offset, int divisor)
        {
            GL.EnableVertexAttribArray(_location);
            GL.VertexAttribLPointer(_location, _numberOfComponents, VertexAttribDoubleType.Double, strideSize, new IntPtr(offset));
            GL.VertexAttribDivisor(_location, divisor);
        }

        public int Size
        {
            get
            {
                return _numberOfComponents * 8;
            }
        }
    }
}
