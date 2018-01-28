using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a GDI pen (<c>HPEN</c>).
    /// </summary>
    public class Pen : IDisposable
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

        public Pen(IntPtr ptr)
        {
            Handle = ptr;
        }

        public IntPtr Handle { get; protected set; }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }

        private LOGPEN Data
        {
            get
            {
                using (StructureBuffer<LOGPEN> ptr = new StructureBuffer<LOGPEN>())
                {
                    int retval = NativeMethods.GetObject(Handle, ptr.Size, ptr.Handle);
                    if (retval != ptr.Size) throw new System.ComponentModel.Win32Exception();
                    return ptr.Value;
                }
            }
        }

        public PenDashStyle DashStyle
        {
            get
            {
                uint styleFlags = Data.lopnStyle;

                if ((styleFlags & GDIConstants.PS_DOT) == GDIConstants.PS_DOT) return PenDashStyle.Dot;
                else if ((styleFlags & GDIConstants.PS_DASH) == GDIConstants.PS_DASH) return PenDashStyle.Dash;
                else if ((styleFlags & GDIConstants.PS_DASHDOT) == GDIConstants.PS_DASHDOT) return PenDashStyle.DashDot;
                else if ((styleFlags & GDIConstants.PS_DASHDOTDOT) == GDIConstants.PS_DASHDOTDOT) return PenDashStyle.DashDotDot;
                else return PenDashStyle.Solid;
            }
        }

        public PenEndCapStyle EndCapStyle
        {
            get
            {
                uint styleFlags = Data.lopnStyle;

                if ((styleFlags & GDIConstants.PS_ENDCAP_FLAT) == GDIConstants.PS_ENDCAP_FLAT) return PenEndCapStyle.Flat;
                else if ((styleFlags & GDIConstants.PS_ENDCAP_SQUARE) == GDIConstants.PS_ENDCAP_SQUARE) return PenEndCapStyle.Square;
                else return PenEndCapStyle.Round;
            }
        }

        public PenJoinCapStyle JoinCapStyle
        {
            get
            {
                uint styleFlags = Data.lopnStyle;

                if ((styleFlags & GDIConstants.PS_JOIN_BEVEL) == GDIConstants.PS_JOIN_BEVEL) return PenJoinCapStyle.Bevel;
                else if ((styleFlags & GDIConstants.PS_JOIN_MITER) == GDIConstants.PS_JOIN_MITER) return PenJoinCapStyle.Miter;
                else return PenJoinCapStyle.Round;
            }
        }

        public Color Color
        {
            get
            {
                return Color.FromWin32Color(Data.lopnColor);
            }
        }

        public long Width
        {
            get
            {
                return Data.lopnWidth.x;
            }
        }
    }
}
