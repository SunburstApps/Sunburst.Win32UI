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
    }
}
