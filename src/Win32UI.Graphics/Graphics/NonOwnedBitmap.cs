using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Win32 <c>HBITMAP</c> that was obtained from a system method.
    /// This class cannot be used to create a bitmap; use the <see cref="Bitmap"/> class instead.
    /// </summary>
    public class NonOwnedBitmap
    {
        public NonOwnedBitmap(IntPtr ptr)
        {
            Handle = ptr;
        }

        public IntPtr Handle { get; protected set; }

        private BitmapHeader Header
        {
            get
            {
                using (StructureBuffer<BitmapHeader> ptr = new StructureBuffer<BitmapHeader>())
                {
                    int returnedSize = NativeMethods.GetObject(Handle, ptr.Size, ptr.Handle);
                    if (returnedSize != ptr.Size) throw new InvalidOperationException($"GetObject() returned unexpected size {returnedSize}");
                    return ptr.Value;
                }
            }
        }

        public Size Size
        {
            get
            {
                var header = Header;
                return new Size(header.bmWidth, header.bmHeight);
            }
        }

        public Bitmap Duplicate()
        {
            // Technique taken from here: http://stackoverflow.com/questions/5687263/copying-a-bitmap-from-another-hbitmap
            using (var sourceContext = GraphicsContext.CreateOffscreenContext())
            {
                using (var destContext = sourceContext.CreateCompatibleContext())
                {
                    const int SRCCOPY = 0x00CC0020;
                    var header = Header;

                    sourceContext.CurrentBitmap = this;
                    NativeMethods.BitBlt(sourceContext.Handle, 0, 0, header.bmWidth, header.bmHeight, destContext.Handle, 0, 0, SRCCOPY);
                    return destContext.CreateBitmap(header.bmWidth, header.bmHeight);
                }
            }
        }
    }
}
