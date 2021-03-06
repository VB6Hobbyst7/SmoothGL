﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;

using SmoothGL.Graphics;


namespace SmoothGL.Content
{
    /// <summary>
    /// Reader class which reads a vertex array from a stream.
    /// </summary>
    public class VertexArrayReader : IContentReader<VertexArray>
    {
        /// <summary>
        /// Reads a vertex array from a stream.
        /// </summary>
        /// <param name="stream">Stream to read data from.</param>
        /// <param name="requestedType">The concrete type requested. Should be the specified type or subtypes.</param>
        /// <param name="contentManager">Content manager used to load additional data.</param>
        /// <returns>The read object.</returns>
        public VertexArray Read(Stream stream, Type requestedType, ContentManager contentManager)
        {
            MeshData meshData = contentManager.Load<MeshData>(stream);
            VertexBuffer vertexBuffer = meshData.ToVertexBuffer();
            ElementBuffer elementBuffer = meshData.ToElementBuffer();

            contentManager.Add(vertexBuffer);
            contentManager.Add(elementBuffer);
            
            return new VertexArray(vertexBuffer, elementBuffer);
        }

        /// <summary>
        /// Indicates whether this class can also read subtypes of the specified type.
        /// </summary>
        public bool CanReadSubtypes
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the name of this reader.
        /// </summary>
        public string ReaderName
        {
            get
            {
                return "VertexArrayReader";
            }
        }
    }
}
