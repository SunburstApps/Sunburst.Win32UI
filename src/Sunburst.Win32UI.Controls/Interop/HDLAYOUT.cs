using System;
using System.Runtime.InteropServices;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    internal struct HDLAYOUT
    {
        public IntPtr prc;
        public IntPtr pwpos;
    }

    internal struct WINDOWPOS
    {
        public IntPtr hWnd;
        public IntPtr hWndInsertAfter;
        int x;
        int y;
        int cx;
        int cy;
        uint flags;

        public void Apply()
        {
            SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, flags);
        }

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);
    }
}
