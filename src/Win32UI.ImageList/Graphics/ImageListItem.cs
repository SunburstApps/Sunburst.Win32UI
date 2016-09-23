using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public sealed class ImageListItem
    {
        internal ImageListItem(IMAGEINFO nativeInfo)
        {
            Bitmap = new NonOwnedBitmap(nativeInfo.hbmImage);
            MaskBitmap = new NonOwnedBitmap(nativeInfo.hbmMask);
            Frame = nativeInfo.rcImage;
        }

        public NonOwnedBitmap Bitmap { get; private set; }
        public NonOwnedBitmap MaskBitmap { get; private set; }
        public Rect Frame { get; private set; }
    }
}
