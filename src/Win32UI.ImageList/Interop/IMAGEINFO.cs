using System;
using Microsoft.Win32.UserInterface.Graphics;

#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
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
