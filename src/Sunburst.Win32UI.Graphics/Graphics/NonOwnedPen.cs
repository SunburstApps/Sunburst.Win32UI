using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Win32 <c>HPEN</c> that was obtained from a system method.
    /// This class cannot be used to create pens; use the <see cref="Pen"/> class instead.
    /// </summary>
    public class NonOwnedPen
    {
        protected NonOwnedPen()
        {
            Handle = IntPtr.Zero;
        }

        public NonOwnedPen(IntPtr ptr)
        {
            Handle = ptr;
        }

        public IntPtr Handle { get; protected set; }

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
