using System;
using System.Collections.Generic;
using System.Linq;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Win32 <c>HRGN</c>.
    /// </summary>
    public class Region : NonOwnedRegion, IDisposable
    {
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

        public Region(IntPtr ptr) : base(ptr) { }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }
    }
}
