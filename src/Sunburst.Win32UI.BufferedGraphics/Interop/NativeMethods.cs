using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    internal static class NativeMethods
    {
        [DllImport("uxtheme.dll")]
        public static extern bool BufferedPaintInit();

        [DllImport("uxtheme.dll")]
        public static extern bool BufferedPaintUnInit();

        [DllImport("uxtheme.dll")]
        public static extern IntPtr BeginBufferedPaint(IntPtr hDCTarget, ref Rect targetRect, int bufferFormat, ref BP_PAINTPARAMS paintParams, ref IntPtr hDCPaint);

        [DllImport("uxtheme.dll")]
        public static extern int EndBufferedPaint(IntPtr hPaintBuffer, bool update);

        [DllImport("uxtheme.dll")]
        public static extern int GetBufferedPaintTargetRect(IntPtr hPaintBuffer, ref Rect rect);

        [DllImport("uxtheme.dll")]
        public static extern IntPtr GetBufferedPaintTargetDC(IntPtr hPaintBuffer);

        [DllImport("uxtheme.dll")]
        public static extern IntPtr GetBufferedPaintDC(IntPtr hPaintBuffer);

        [DllImport("uxtheme.dll")]
        public static extern int BufferedPaintClear(IntPtr hPaintBuffer, ref Rect rect);

        [DllImport("uxtheme.dll")]
        public static extern int BufferedPaintClear(IntPtr hPaintBuffer, IntPtr rectPtr);

        [DllImport("uxtheme.dll")]
        public static extern int BufferedPaintSetAlpha(IntPtr hPaintBuffer, IntPtr rectPtr, byte alpha);

        [DllImport("uxtheme.dll")]
        public static extern IntPtr BeginBufferedAnimation(IntPtr hWnd, IntPtr hDCTarget, ref Rect pRectTarget,
            BufferingFormat format, ref BP_PAINTPARAMS paintParams, ref BP_ANIMATIONPARAMS animParams,
            ref IntPtr hDCFrom, ref IntPtr hDCTo);

        [DllImport("uxtheme.dll")]
        public static extern int EndBufferedAnimation(IntPtr hAnimBuffer, bool bUpdate);

        [DllImport("uxtheme.dll")]
        public static extern bool BufferedPaintRenderAnimation(IntPtr hWnd, IntPtr hDC);

        [DllImport("uxtheme.dll")]
        public static extern int BufferedPaintStopAllAnimations(IntPtr hWnd);
    }
}
