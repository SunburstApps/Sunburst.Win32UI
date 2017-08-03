using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Windows font that was obtained from a system method.
    /// This class cannot be used to create a font; use the <see cref="Font"/> class instead.
    /// </summary>
    public class NonOwnedFont
    {
        /// <summary>
        /// Creates a new instance of Font.
        /// </summary>
        /// <param name="ptr">
        /// The native handle to the font data.
        /// </param>
        public NonOwnedFont(IntPtr ptr)
        {
            Handle = ptr;
        }

        protected NonOwnedFont()
        {
            Handle = IntPtr.Zero;
        }

        public LOGFONT GetFontDescriptor()
        {
            using (StructureBuffer<LOGFONT> ptr = new StructureBuffer<LOGFONT>())
            {
                int returnedSize = NativeMethods.GetObject(Handle, ptr.Size, ptr.Handle);
                if (ptr.Size != returnedSize)
                    throw new InvalidOperationException($"GetObject() returned incorrect size (got {returnedSize} bytes, expected {ptr.Size} bytes)");
                return ptr.Value;
            }
        }

        /// <summary>
        /// The native handle to the font data.
        /// </summary>
        public IntPtr Handle { get; protected set; }
    }
}
