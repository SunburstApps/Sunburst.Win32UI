using System;
using System.Collections.Generic;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{   
    public static class AeroGlassGraphics
    {
        public static void ExtendFrameIntoClientArea(this Dialog window, Margins margins)
        {
            NativeMethods.DwmExtendFrameIntoClientArea(window.Handle, ref margins);
        }

        public static void DrawGlassText(this NonOwnedGraphicsContext context, Window owner, Rect drawRect, string text, NonOwnedFont font, Color color, TextAlignment halign, VerticalTextAlignment valign, StringDrawingFlags flags)
        {
            IntPtr hTheme = NativeMethods.OpenThemeData(owner.Handle, "Button");

            try
            {
                context.Select(font, () =>
                {
                    DrawThemeTextOptions opts = new DrawThemeTextOptions(true);
                    opts.TextColor = color;
                    opts.GlowSize = 10;
                    opts.AntiAliasedAlpha = true;

                    int nativeFlags = 0;

                    const int DT_LEFT = 0x00000000;
                    const int DT_CENTER = 0x00000001;
                    const int DT_RIGHT = 0x00000002;
                    const int DT_VCENTER = 0x00000004;
                    const int DT_BOTTOM = 0x00000008;
                    const int DT_WORDBREAK = 0x00000010;
                    const int DT_EXPANDTABS = 0x00000040;
                    const int DT_NOPREFIX = 0x00000800;
                    const int DT_PATH_ELLIPSIS = 0x00004000;
                    const int DT_WORD_ELLIPSIS = 0x00040000;

                    switch (halign)
                    {
                        case TextAlignment.Left: nativeFlags |= DT_LEFT; break;
                        case TextAlignment.Center: nativeFlags |= DT_CENTER; break;
                        case TextAlignment.Right: nativeFlags |= DT_RIGHT; break;
                    }

                    switch (valign)
                    {
                        case VerticalTextAlignment.Top: break; // There is no DT_* flag for top alignment.
                        case VerticalTextAlignment.Center: nativeFlags |= DT_VCENTER; break;
                        case VerticalTextAlignment.Bottom: nativeFlags |= DT_BOTTOM; break;
                    }

                    if (flags.HasFlag(StringDrawingFlags.BreakOnWords)) nativeFlags |= DT_WORDBREAK;
                    if (flags.HasFlag(StringDrawingFlags.AddWordEllipsis)) nativeFlags |= DT_WORD_ELLIPSIS;
                    if (flags.HasFlag(StringDrawingFlags.AddPathEllipsis)) nativeFlags |= DT_PATH_ELLIPSIS;
                    if (flags.HasFlag(StringDrawingFlags.ExpandTabCharacters)) nativeFlags |= DT_EXPANDTABS;
                    if (flags.HasFlag(StringDrawingFlags.IgnoreAmpersands)) nativeFlags |= DT_NOPREFIX;

                    NativeMethods.DrawThemeTextEx(hTheme, context.Handle, 0, 0, text, text.Length, nativeFlags, ref drawRect, ref opts);
                });
            }
            finally
            {
                NativeMethods.CloseThemeData(hTheme);
            }
        }

        public static Color GlassColor
        {
            get
            {
                DwmColorizationParameters parameters = new DwmColorizationParameters();
                NativeMethods.DwmGetColorizationParameters(ref parameters);
                return Color.FromWin32Color((int)parameters.Color1);
            }
        }
    }
}
