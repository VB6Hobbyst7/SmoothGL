﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace SmoothGL.Graphics
{
    /// <summary>
    /// Defines an abstract buffer persistent in graphics memory.
    /// </summary>
    public abstract class Buffer : GraphicsResource, IUnsafeBuffer
    {
        private static int _currentBufferId = 0;

        /// <summary>
        /// Invalidates the binding cache used to speed up buffer binding operations.
        /// This method is not required to be called by client code.
        /// </summary>
        public static void InvalidateBindingCache()
        {
            _currentBufferId = 0;
        }
        
        private int _bufferId;
        private BufferTarget _target;
        private BufferUsage _usage;
        
        /// <summary>
        /// Creates a new buffer.
        /// </summary>
        /// <param name="size">Allocated buffer size in bytes.</param>
        /// <param name="target">Specifies the type of the buffer.</param>
        /// <param name="usage">Hint for the driver concerning the frequency the data in this buffer is expected to change.</param>
        protected Buffer(int size, BufferTarget target, BufferUsage usage)
        {
            _target = target;
            _usage = usage;

            GL.GenBuffers(1, out _bufferId);
            Resize(size);
        }

        /// <summary>
        /// Binds this buffer to the graphics device. This method is not required to be called by client code.
        /// </summary>
        public void Bind()
        {
            CheckDisposed();
            if (_currentBufferId != _bufferId)
            {
                GL.BindBuffer(_target, _bufferId);
                _currentBufferId = _bufferId;
            }
        }

        /// <summary>
        /// Binds this buffer to the graphics device at a certain location. This method is not required to be called by client code.
        /// </summary>
        /// <param name="location">The location this buffer is bound to.</param>
        public void Bind(int location)
        {
            CheckDisposed();
            GL.BindBufferBase((BufferRangeTarget)_target, location, _bufferId);
        }

        /// <summary>
        /// Resizes this buffer to a new size. All data in this buffer will be discarded.
        /// </summary>
        /// <param name="size">Allocated buffer size in bytes.</param>
        protected void Resize(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size", "Buffer size must be non-negative.");
            
            Bind();
            GL.BufferData(_target, new IntPtr(size), IntPtr.Zero, (BufferUsageHint)_usage);
        }

        /// <summary>
        /// Replaces a single data object in the buffer.
        /// </summary>
        /// <param name="data">Data object to upload. Only instances of value types (structs) are allowed as this parameter.</param>
        /// <param name="offset">The offset in bytes from the beginning of the buffer at which the data object should be stored.</param>
        protected void SetData(object data, int offset)
        {
            Bind();
            int size = Marshal.SizeOf(data);
            IntPtr buffer = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(data, buffer, false);
                GL.BufferSubData(_target, new IntPtr(offset), new IntPtr(size), buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// Replaces a single data object in the buffer.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="data">Data object to upload.</param>
        /// <param name="offset">The offset in bytes from the beginning of the buffer at which the data object should be stored.</param>
        /// <param name="size">Size of the data object in bytes.</param>
        protected void SetData<T>(T data, int offset, int size) where T : struct
        {
            Bind();
            GL.BufferSubData(_target, new IntPtr(offset), new IntPtr(size), ref data);
        }

        /// <summary>
        /// Replaces multiple data objects in the buffer.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="data">Array of sequential data objects to upload.</param>
        /// <param name="offset">The offset in bytes from the beginning of the buffer at which the data objects should be stored.</param>
        /// <param name="size">Size of the data objects in bytes.</param>
        protected void SetData<T>(T[] data, int offset, int size) where T : struct
        {
            Bind();
            GL.BufferSubData(_target, new IntPtr(offset), new IntPtr(size), data);
        }

        void IUnsafeBuffer.SetData(object data, int offset)
        {
            SetData(data, offset);
        }

        void IUnsafeBuffer.SetData<T>(T data, int offset, int size)
        {
            SetData(data, offset, size);
        }

        void IUnsafeBuffer.SetData<T>(T[] data, int offset, int size)
        {
            SetData(data, offset, size);
        }

        protected sealed override void FreeResources()
        {
            if (_currentBufferId == _bufferId)
                _currentBufferId = 0;

            GL.DeleteBuffers(1, ref _bufferId);
        }
        
        protected int Id
        {
            get
            {
                return _bufferId;
            }
        }
        
        protected override string ResourceName
        {
            get
            {
                return "Buffer";
            }
        }

        /// <summary>
        /// Gets the hint for the driver concerning the frequency the data in this buffer is expected to change.
        /// </summary>
        public BufferUsage Usage
        {
            get
            {
                return _usage;
            }
        }
    }
}
