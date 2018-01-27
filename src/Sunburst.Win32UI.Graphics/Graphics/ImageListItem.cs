using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public sealed class ImageListItem
    {
        internal ImageListItem(IMAGEINFO nativeInfo)
        {
            Bitmap = new Bitmap(nativeInfo.hbmImage);
            MaskBitmap = new Bitmap(nativeInfo.hbmMask);
            Frame = nativeInfo.rcImage;
        }

        public Bitmap Bitmap { get; private set; }
        public Bitmap MaskBitmap { get; private set; }
        public Rect Frame { get; private set; }
    }
}
