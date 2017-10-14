using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct TOOLINFOW
    {
        public int cbSize;
        public int uFlags;
        public IntPtr hWnd;
        public IntPtr uId;
        public Rect rect;
        public IntPtr hinst;
        public string lpszText;
        public IntPtr lParam;
        private IntPtr lpReserved;
    }
}
