using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Win32 <c>HRGN</c> that was obtained from a system method.
    /// This class cannot be used to create pens; use the <see cref="Region"/> class instead.
    public class NonOwnedRegion
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

        public NonOwnedRegion(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) throw new ArgumentException("Cannot use a NULL region handle", nameof(ptr));

            Handle = ptr;
        }

        protected NonOwnedRegion()
        {
            Handle = IntPtr.Zero;
        }

        public IntPtr Handle { get; protected internal set; }

        public void Combine(NonOwnedRegion other, RegionCombinationMode mode)
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

        public static bool EqualRegions(NonOwnedRegion lhs, NonOwnedRegion rhs)
        {
            // NonOwnedRegion does not implement IEquatable because I cannot specify the
            // invariant "if two objects are Equal(), they have the same GetHashCode()".
            // This is because the equality is implemented in unmanaged code, which does
            // not offer a hash function.

            return NativeMethods.EqualRgn(lhs.Handle, rhs.Handle);
        }
    }
}
