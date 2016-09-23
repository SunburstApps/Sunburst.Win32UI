using System;

namespace Microsoft.GDIPlus
{
    public struct SizeF : IEquatable<SizeF>
    {
        public static SizeF Empty = new SizeF();
        public bool IsEmpty => Width == 0 && Height == 0;

        public float Width;
        public float Height;

        public SizeF(SizeF other)
        {
            Width = other.Width;
            Height = other.Height;
        }

        public SizeF(float w, float h)
        {
            Width = w;
            Height = h;
        }

        public SizeF Add(SizeF otherSize)
        {
            return new SizeF(Width + otherSize.Width, Height + otherSize.Height);
        }

        public SizeF Subtract(SizeF otherSize)
        {
            return new SizeF(Width - otherSize.Width, Height - otherSize.Height);
        }

        public bool Equals(SizeF other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (!GetType().Equals(obj.GetType())) return false;
            return Equals((SizeF)obj);
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }
    }
}
