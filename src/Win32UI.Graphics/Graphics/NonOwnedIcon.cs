using System;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Windows icon.
    /// </summary>
    public class NonOwnedIcon
    {
        /// <summary>
        /// Creates a new instance of Icon.
        /// </summary>
        /// <param name="ptr">
        /// The handle to the native icon data.
        /// </param>
        public NonOwnedIcon(IntPtr ptr)
        {
            Handle = ptr;
        }
        
        /// <summary>
        /// The handle to the native icon data.
        /// </summary>
        public IntPtr Handle
        { get; private set; }
    }
}
