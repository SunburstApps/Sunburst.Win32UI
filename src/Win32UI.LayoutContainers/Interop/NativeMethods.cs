using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class NativeMethods
    {
        public static readonly IntPtr HWND_TOP = IntPtr.Zero;
        public static readonly IntPtr HWND_BOTTOM = (IntPtr)1;
        public static readonly IntPtr HWND_TOPMOST = (IntPtr)(-1);
        public static readonly IntPtr HWND_NOTOPMOST = (IntPtr)(-2);

        public const int GW_CHILD = 5, GW_HWNDNEXT = 2;
        
        [DllImport("user32.dll")]
        public static extern IntPtr BeginDeferWindowPos(int controlCount);

        [DllImport("user32.dll")]
        public static extern IntPtr DeferWindowPos(IntPtr hDefer, IntPtr hWnd, IntPtr hWndInsertAfter,
            int left, int top, int width, int height, DeferWindowPosFlags flags);

        [DllImport("user32.dll")]
        public static extern void EndDeferWindowPos(IntPtr hDefer);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int relationship);

        [DllImport("user32.dll")]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref Point pt, int pointCount);
    }
}
