using System;
using Microsoft.Win32.UserInterface.Graphics;

#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct IMAGEINFO
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int unused_1;
        public int unused_2;
        public Rect rcImage;
    }
}
