using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WNDCLASSEXW
    {
        public uint cbSize;
        public uint style;
        public IntPtr lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public IntPtr lpszMenuName; // string
        public IntPtr lpszClassName; // string
        public IntPtr hIconSm;
    }
}
