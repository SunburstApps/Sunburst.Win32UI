using System;
using System.Collections.Generic;
using System.Linq;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a GDI region (<c>HRGN</c>).
    public class Region : IDisposable
    {
        internal static int TranslateCombinationMode(RegionCombinationMode mode, string parameterName = null)
        {
            switch (mode)
            {
                case RegionCombinationMode.Intersect: return 1;
                case RegionCombinationMode.Union: return 2;
                case RegionCombinationMode.UnionExceptOverlap: return 3;
                default: throw new ArgumentException("Unrecognized regionCombinationMode", parameterName ?? nameof(mode));
            }
        }

        private static int TranslateFillMode(PolygonFillMode fillMode)
        {
            int nativeFillMode = 0;
            switch (fillMode)
            {
                case PolygonFillMode.Alternate: nativeFillMode = 1; break;
                case PolygonFillMode.NonZeroWinding: nativeFillMode = 2; break;
            }
            if (nativeFillMode == 0) throw new ArgumentException("Unrecognized PolygonFillMode", nameof(fillMode));
            else return nativeFillMode;
        }

        public static Region CreateRectangle(Rect frame)
        {
            return new Region(NativeMethods.CreateRectRgnIndirect(ref frame));
        }

        public static Region CreateEllipse(Rect frame)
        {
            return new Region(NativeMethods.CreateEllipticRgnIndirect(ref frame));
        }

        public static Region CreatePolygon(IList<Point> points, PolygonFillMode fillMode)
        {
            Point[] pointArray = new Point[points.Count];
            points.CopyTo(pointArray, 0);
            return new Region(NativeMethods.CreatePolygonRgn(pointArray, points.Count, TranslateFillMode(fillMode)));
        }

        public static Region CreateMultiplePolygon(IList<IList<Point>> polygons, PolygonFillMode fillMode)
        {
            var maxPointCount = (from shape in polygons select shape.Count).Sum();
            var points = new Point[maxPointCount];
            var endpoints = new List<int>(polygons.Count);

            int offset = 0;
            foreach (var shape in polygons)
            {
                shape.CopyTo(points, offset);
                offset += shape.Count;
                endpoints.Add(shape.Count);
            }

            var endpointArray = new int[endpoints.Count];
            endpoints.CopyTo(endpointArray, 0);
            return new Region(NativeMethods.CreatePolyPolygonRgn(points, endpointArray, maxPointCount, TranslateFillMode(fillMode)));
        }

        public static Region CreateRoundedRect(Rect frame, int cornerRadius)
        {
            return CreateRoundedRect(frame, cornerRadius, cornerRadius);
        }

        public static Region CreateRoundedRect(Rect frame, int widthRadius, int heightRadius)
        {
            return new Region(NativeMethods.CreateRoundRectRgn(frame.left, frame.top, frame.right, frame.bottom, widthRadius, heightRadius));
        }

        public static Region CreateFromGraphicsContextPath(NonOwnedGraphicsContext context)
        {
            return new Region(NativeMethods.PathToRegion(context.Handle));
        }

        public Region(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) throw new ArgumentException("Cannot use a NULL region handle", nameof(ptr));

            Handle = ptr;
        }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }

        public IntPtr Handle { get; protected internal set; }

        public void Combine(Region other, RegionCombinationMode mode)
        {
            NativeMethods.CombineRgn(Handle, Handle, other.Handle, TranslateCombinationMode(mode, nameof(mode)));
        }

        public void Offset(Point pt)
        {
            NativeMethods.OffsetRgn(Handle, Convert.ToInt32(pt.x), Convert.ToInt32(pt.y));
        }

        public bool ContainsPoint(Point pt)
        {
            return NativeMethods.PtInRegion(Handle, Convert.ToInt32(pt.x), Convert.ToInt32(pt.y));
        }

        public bool ContainsRect(Rect rect)
        {
            return NativeMethods.RectInRegion(Handle, ref rect);
        }

        public Rect BoundingBox
        {
            get
            {
                Rect retval = new Rect();
                NativeMethods.GetRgnBox(Handle, ref retval);
                return retval;
            }
        }

        public static bool EqualRegions(Region lhs, Region rhs)
        {
            // NonOwnedRegion does not implement IEquatable because I cannot specify the
            // invariant "if two objects are Equal(), they have the same GetHashCode()".
            // This is because the equality is implemented in unmanaged code, which does
            // not offer a hash function.

            return NativeMethods.EqualRgn(lhs.Handle, rhs.Handle);
        }
    }
}
