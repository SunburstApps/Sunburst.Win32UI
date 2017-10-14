using System;

namespace Sunburst.Win32UI.Handles
{
    /// <summary>
    /// An interface that defines a property to get the native equivalent of a CLR object.
    /// </summary>
    public interface INativeHandle
    {
        /// <summary>
        /// The handle to the native equivalent of the receiver.
        /// </summary>
        IntPtr Handle { get; }
    }
}
