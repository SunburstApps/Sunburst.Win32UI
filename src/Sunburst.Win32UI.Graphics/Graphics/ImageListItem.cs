using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
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
