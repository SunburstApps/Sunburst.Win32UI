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
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] Point[] pt, int pointCount);

        [DllImport("user32.dll")]
        public static extern bool GetScrollInfo(IntPtr hWnd, int wSBFlags, ref SCROLLINFO info);

        [DllImport("user32.dll")]
        public static extern bool SetScrollInfo(IntPtr hWnd, int wSBFlags, ref SCROLLINFO info, bool redraw);

        [DllImport("user32.dll")]
        public static extern bool SetScrollPos(IntPtr hWnd, int nBar, int position, bool redraw);

        [DllImport("user32.dll")]
        public static extern int ScrollWindowEx(IntPtr hWnd, int dx, int dy, IntPtr scrollRectPtr, IntPtr clipRectPtr, IntPtr hRgnUpdate, IntPtr updateRectPtr, uint flags);

        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfoW(uint action, uint param, IntPtr value, uint winIniFlag);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        public static extern bool IsChild(IntPtr hWndParent, IntPtr hWndChild);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point pt);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point pt);

        [DllImport("user32.dll")]
        public static extern void GetCursorPos(out Point pt);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern int GetMessagePos();

        [DllImport("user32.dll")]
        public static extern IntPtr GetCapture();

        [DllImport("user32.dll")]
        public static extern void SetCapture(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void ReleaseCapture();
    }
}
