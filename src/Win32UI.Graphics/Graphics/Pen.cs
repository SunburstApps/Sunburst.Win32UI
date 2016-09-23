using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Win32 <c>HPEN</c>.
    /// </summary>
    public class Pen : NonOwnedPen, IDisposable
    {
        public Pen(Color color, int width)
        {
            Handle = NativeMethods.CreatePen(0, width, Color.ToWin32Color(color));
        }

        public Pen(Color color, PenDashStyle dashStyle)
        {
            int styleValue = 0;
            switch (dashStyle)
            {
                case PenDashStyle.Solid: styleValue |= GDIConstants.PS_SOLID; break;
                case PenDashStyle.Dot: styleValue |= GDIConstants.PS_DOT; break;
                case PenDashStyle.Dash: styleValue |= GDIConstants.PS_DASH; break;
                case PenDashStyle.DashDot: styleValue |= GDIConstants.PS_DASHDOT; break;
                case PenDashStyle.DashDotDot: styleValue |= GDIConstants.PS_DASHDOTDOT; break;
            }

            Handle = NativeMethods.CreatePen(styleValue, 1, Color.ToWin32Color(color));
        }

        public Pen(Color color, int width, PenEndCapStyle capStyle = PenEndCapStyle.Round, PenJoinCapStyle joinStyle = PenJoinCapStyle.Round)
        {
            int styleValue = 0;
            switch (capStyle)
            {
                case PenEndCapStyle.Round: styleValue |= GDIConstants.PS_ENDCAP_ROUND; break;
                case PenEndCapStyle.Flat: styleValue |= GDIConstants.PS_ENDCAP_FLAT; break;
                case PenEndCapStyle.Square: styleValue |= GDIConstants.PS_ENDCAP_SQUARE; break;
            }
            switch (joinStyle)
            {
                case PenJoinCapStyle.Round: styleValue |= GDIConstants.PS_JOIN_ROUND; break;
                case PenJoinCapStyle.Bevel: styleValue |= GDIConstants.PS_JOIN_BEVEL; break;
                case PenJoinCapStyle.Miter: styleValue |= GDIConstants.PS_JOIN_MITER; break;
            }

            LOGBRUSH brush = new LOGBRUSH();
            brush.lbColor = Color.ToWin32Color(color);
            Handle = NativeMethods.ExtCreatePen(styleValue, width, ref brush, 0, Array.Empty<int>());
        }

        public Pen(IntPtr ptr) : base(ptr) { }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }
    }
}
