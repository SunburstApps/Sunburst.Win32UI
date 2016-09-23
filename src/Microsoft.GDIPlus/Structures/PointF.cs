using System;

namespace Microsoft.GDIPlus
{
    public struct PointF : IEquatable<PointF>
    {
        public static PointF Empty = new PointF();
        public bool IsEmpty => X == 0 && Y == 0;

        public float X;
        public float Y;

        public PointF(SizeF size)
        {
            X = size.Width;
            Y = size.Height;
        }

        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public PointF Add(PointF other)
        {
            return new PointF(X + other.X, Y + other.Y);
        }

        public PointF Subtract(PointF other)
        {
            return new PointF(X - other.X, Y - other.Y);
        }

        public bool Equals(PointF other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (!GetType().Equals(obj.GetType())) return false;
            return Equals((PointF)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
