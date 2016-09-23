using System;

namespace Microsoft.GDIPlus
{
    public struct Point : IEquatable<Point>
    {
        public static Point Empty = new Point();
        public bool IsEmpty => X == 0 && Y == 0;

        public int X;
        public int Y;

        public Point(Size size)
        {
            X = size.Width;
            Y = size.Height;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point Add(Point other)
        {
            return new Point(X + other.X, Y + other.Y);
        }

        public Point Subtract(Point other)
        {
            return new Point(X - other.X, Y - other.Y);
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (!GetType().Equals(obj.GetType())) return false;
            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
