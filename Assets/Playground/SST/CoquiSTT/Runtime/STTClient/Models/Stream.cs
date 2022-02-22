﻿using System;

namespace STTClient.Models
{
    /// <summary>
    /// Wrapper of the pointer used for the decoding stream.
    /// </summary>
    public class Stream : IDisposable
    {
        private unsafe IntPtr** _streamingStatePp;

        /// <summary>
        /// Initializes a new instance of <see cref="Stream"/>.
        /// </summary>
        /// <param name="streamingStatePP">Native pointer of the native stream.</param>
        public unsafe Stream(IntPtr** streamingStatePP)
        {
            _streamingStatePp = streamingStatePP;
        }

        /// <summary>
        /// Gets the native pointer.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the stream has been disposed or not yet initialized.</exception>
        /// <returns>Native pointer of the stream.</returns>
        internal unsafe IntPtr** GetNativePointer()
        {
            if (_streamingStatePp == null)
                throw new InvalidOperationException("Cannot use a disposed or uninitialized stream.");
            return _streamingStatePp;
        }

        public unsafe void Dispose() => _streamingStatePp = null;
    }
}
