using System;

namespace Microsoft.GDIPlus
{
    public struct Size : IEquatable<Size>
    {
        public static Size Empty = new Size();
        public bool IsEmpty => Width == 0 && Height == 0;

        public int Width;
        public int Height;

        public Size(Size other)
        {
            Width = other.Width;
            Height = other.Height;
        }

        public Size(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public Size Add(Size otherSize)
        {
            return new Size(Width + otherSize.Width, Height + otherSize.Height);
        }

        public Size Subtract(Size otherSize)
        {
            return new Size(Width - otherSize.Width, Height - otherSize.Height);
        }

        public bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (!GetType().Equals(obj.GetType())) return false;
            return Equals((Size)obj);
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }
    }
}
