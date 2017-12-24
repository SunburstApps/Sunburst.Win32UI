using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool redraw);

        [DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("user32.dll")]
        public static extern bool SetScrollRange(IntPtr hWnd, int nBar, int nMinPos, int nMaxPos, bool redraw);

        [DllImport("user32.dll")]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int nMinPos, out int nMaxPos);

        [DllImport("user32.dll")]
        public static extern bool ShowScrollBar(IntPtr hWnd, int nBar, bool show);

        [DllImport("user32.dll")]
        public static extern bool EnableScrollBar(IntPtr hWnd, int wSBFlags, int wArrows);

        [DllImport("user32.dll")]
        public static extern bool GetScrollInfo(IntPtr hWnd, int wSBFlags, ref SCROLLINFO info);

        [DllImport("user32.dll")]
        public static extern bool SetScrollInfo(IntPtr hWnd, int wSBFlags, ref SCROLLINFO info);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int SetWindowTheme(IntPtr hWnd, string appName, string idList);
    }
}
