using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class NativeMethods
    {
        [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
        public static extern void DwmGetColorizationParameters(ref DwmColorizationParameters parameters);
        [DllImport("dwmapi.dll", EntryPoint = "#131", PreserveSig = false)]
        public static extern void DwmSetColorizationParameters(ref DwmColorizationParameters parameters, uint unknown = 1);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, ref DwmBlurBehind blurBehind);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins margins);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmIsCompositionEnabled(out int pfEnabled);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenThemeData(IntPtr hWnd, string classList);
        [DllImport("uxtheme.dll")]
        public static extern void CloseThemeData(IntPtr hTheme);
        [DllImport("uxtheme.dll")]
        public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref Rect pRect, ref Rect pClipRect);
        [DllImport("uxtheme.dll")]
        public static extern int DrawThemeIcon(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref Rect pRect, IntPtr himl, int iImageIndex);
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref Rect pRect, ref DrawThemeTextOptions pOptions);
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern Int32 GetThemeFont(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, out LOGFONT pFont);
        [DllImport("uxtheme.dll", ExactSpelling = true)]
        public static extern int GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, IntPtr prc, out Rect pMargins);
        [DllImport("uxtheme.dll", ExactSpelling = true, PreserveSig = false)]
        public static extern void SetWindowThemeAttribute(IntPtr hWnd, WindowThemeAttributeType wtype, ref WTA_OPTIONS attributes, int size);
    }
}
