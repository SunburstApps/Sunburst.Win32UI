using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    internal struct PAINTSTRUCT
    {
        public IntPtr hDC;
        public bool fErase;
        public Rect rcPaint;
        // Reserved fields:
        private bool fRestore;
        private bool fIncUpdated;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        private byte[] rgbReserved;
    }
}
