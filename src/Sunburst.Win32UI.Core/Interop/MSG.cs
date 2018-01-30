using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    internal struct MSG
    {
        public IntPtr hWnd;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public Point pt;
    }
}
