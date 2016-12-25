using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class NativeMethods
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenThemeData(IntPtr hWnd, string themeClassList);

        [DllImport("uxtheme.dll")]
        public static extern int CloseThemeData(IntPtr hTheme);

        [DllImport("uxtheme.dll")]
        public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hDC, int partId, int startId, [In] ref Rect rect, IntPtr clipRectPtr);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int DrawThemeText(IntPtr hTheme, IntPtr hDC, int partId, int stateId, string pchText, int textLength, int flags, int reservedFlags, ref Rect rect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeBackgroundContentRect(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect boundingRect, out Rect contentRect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeBackgroundExtent(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect contentRect, out Rect boundingRect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemePartSize(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect rect, PartSizingMode sizeMode, out Size size);
    }
}
