﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SmoothGL.Graphics
{
    /// <summary>
    /// Exception which is thrown when an invalid operation is performed on uniform blocks.
    /// </summary>
    public class UniformBlockException : Exception
    {
        private string _uniformBlockName;

        /// <summary>
        /// Creates a new UniformBlockException.
        /// </summary>
        /// <param name="message">Message specifying the reason for the exception.</param>
        /// <param name="uniformBlockName">Name of the uniform block which caused the exception.</param>
        public UniformBlockException(string message, string uniformBlockName)
            : base(message)
        {
            _uniformBlockName = uniformBlockName;
        }

        /// <summary>
        /// Gets the name of the uniform block which caused the exception.
        /// </summary>
        public string UniformBlockName
        {
            get
            {
                return _uniformBlockName;
            }
        }
    }
}
