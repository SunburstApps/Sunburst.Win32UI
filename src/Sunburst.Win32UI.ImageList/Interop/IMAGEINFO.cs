using System;
using Sunburst.Win32UI.Graphics;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    internal struct IMAGEINFO
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public Rect rcImage;
    }
}
