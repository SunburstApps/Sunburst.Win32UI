﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Theming;

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

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int GetThemeTextExtent(IntPtr hTheme, IntPtr hDC, int partId, int stateId, string text, int textLength, int flags, [In] ref Rect boundingRect, out Rect extentRect);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeBackgroundRegion(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect rect, out IntPtr hRegion);

        [DllImport("uxtheme.dll")]
        public static extern int DrawThemeIcon(IntPtr hTheme, IntPtr hDC, int partId, int stateId, [In] ref Rect rect, IntPtr hImageList, int imageIndex);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeColor(IntPtr hTheme, int partId, int stateId, int propId, out int color_ref);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeMetric(IntPtr hTheme, IntPtr hDC, int partId, int stateId, int propId, out int value);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemePosition(IntPtr hTheme, int partId, int stateId, int propId, out Point value);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeFont(IntPtr hTheme, IntPtr hDC, int partId, int stateId, int propId, out LOGFONT value);

        [DllImport("uxtheme.dll")]
        public static extern int GetThemeRect(IntPtr hTheme, IntPtr hDC, int partId, int stateId, int propId, out Rect value);

        [DllImport("uxtheme.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsThemePartDefined(IntPtr hTheme, int partId, int stateId);
    }
}