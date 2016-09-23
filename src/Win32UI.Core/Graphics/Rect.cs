using System;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// An onscreen rectangle.
    /// </summary>
    public struct Rect : IEquatable<Rect>
    {
        /// <summary>
        /// A Rect with all values set to zero.
        /// </summary>
        public static readonly Rect Zero = new Rect();

        /// <summary>
        /// The left coordinate of the rectangle.
        /// </summary>
        public int left;

        /// <summary>
        /// The top coordinate of the rectangle.
        /// </summary>
        public int top;

        /// <summary>
        /// The right coordinate of the rectangle.
        /// </summary>
        public int right;

        /// <summary>
        /// The bottom coordinate of the rectangle.
        /// </summary>
        public int bottom;

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public int Width
        {
            get
            {
                return right - left;
            }
        }

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public int Height
        {
            get
            {
                return bottom - top;
            }
        }

        public override bool Equals(object other)
        {
            if (other == null || !GetType().Equals(other.GetType())) return false;
            return Equals((Rect)other);
        }

        public bool Equals(Rect other)
        {
            return this.left == other.left && this.top == other.top && this.right == other.right && this.bottom == other.bottom;
        }

        public override int GetHashCode()
        {
            return this.left.GetHashCode() ^ this.top.GetHashCode() ^ this.right.GetHashCode() ^ this.bottom.GetHashCode();
        }

        public override string ToString()
        {
            return $"<Rect>(top={top}, left={left}, bottom={bottom}, right={right})";
        }
    }
}
