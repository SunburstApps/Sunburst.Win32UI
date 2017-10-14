using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class ImageList : NonOwnedImageList, IDisposable
    {
        public ImageList() : base() { }

        public ImageList(Size imageSize, int initialCount, int maximumCount)
        {
            const uint ILC_COLOR32 = 0x20;
            Handle = NativeMethods.ImageList_Create(imageSize.width, imageSize.height, ILC_COLOR32, initialCount, maximumCount);
        }

        public void Dispose()
        {
            NativeMethods.ImageList_Destroy(Handle);
            Handle = IntPtr.Zero;
        }
    }
}
