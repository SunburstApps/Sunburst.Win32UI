using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public static class FontExtensions
    {
        private static int GetPointSizeImpl(LOGFONT font_struct, IntPtr hDC)
        {
            const int LOGPIXELSY = 90;
            return NativeMethods.MulDiv(Convert.ToInt32(-font_struct.lfHeight),
                72, NativeMethods.GetDeviceCaps(hDC, LOGPIXELSY));
        }

        private static void SetPointSizeImpl(ref LOGFONT font_struct, IntPtr hDC, int pointSize)
        {
            const int LOGPIXELSY = 90;
            font_struct.lfHeight = -NativeMethods.MulDiv(pointSize,
                72, NativeMethods.GetDeviceCaps(hDC, LOGPIXELSY));
        }

        public static int GetPointSize(this LOGFONT font_struct, NonOwnedGraphicsContext ctx = null)
        {
            if (ctx == null)
            {
                IntPtr hDC = NativeMethods.GetDC(IntPtr.Zero);
                try
                {
                    return GetPointSizeImpl(font_struct, hDC);
                }
                finally
                {
                    NativeMethods.ReleaseDC(IntPtr.Zero, hDC);
                }
            }
            else
            {
                return GetPointSizeImpl(font_struct, ctx.Handle);
            }
        }

        public static void SetPointSize(this LOGFONT font_struct, int pointSize, NonOwnedGraphicsContext ctx = null)
        {
            if (ctx == null)
            {
                IntPtr hDC = NativeMethods.GetDC(IntPtr.Zero);
                try
                {
                    SetPointSizeImpl(ref font_struct, hDC, pointSize);
                }
                finally
                {
                    NativeMethods.ReleaseDC(IntPtr.Zero, hDC);
                }
            }
            else
            {
                SetPointSizeImpl(ref font_struct, ctx.Handle, pointSize);
            }
        }
    }
}
