using System;

namespace Microsoft.GDIPlus
{
    public struct Rect : IEquatable<Rect>
    {
        public static Rect Empty = new Rect();
        public bool IsEmpty => Equals(Empty);

        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Rect(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public Rect(Point location, Size size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public Size Size => new Size(Width, Height);
        public Point Location => new Point(X, Y);
        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;
        public bool IsEmptyArea => Width > 0 || Height > 0;

        public bool Contains(int x, int y) => x >= Left && x < Right && y >= Top && y < Bottom;
        public bool Contains(Point point) => Contains(point.X, point.Y);

        public void Inflate(Point point) => Inflate(point.X, point.Y);
        public void Inflate(int dx, int dy)
        {
            X -= dx;
            Y -= dy;
            Width += dx * 2;
            Height += dy * 2;
        }

        public void Offset(Point point) => Offset(point.X, point.Y);
        public void Offset(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public static bool Union(Rect a, Rect b, out Rect result)
        {
            int right = Math.Max(a.Right, b.Right);
            int bottom = Math.Max(a.Bottom, b.Bottom);
            int left = Math.Min(a.Left, b.Left);
            int top = Math.Min(a.Top, b.Top);

            result = new Rect(left, top, right - left, bottom - top);
            return !result.IsEmpty;
        }

        public static bool Intersect(Rect a, Rect b, out Rect result)
        {
            int right = Math.Min(a.Right, b.Right);
            int bottom = Math.Min(a.Bottom, b.Bottom);
            int left = Math.Max(a.Left, b.Left);
            int top = Math.Max(a.Top, b.Top);

            result = new Rect(left, top, right - left, bottom - top);
            return !result.IsEmpty;
        }

        public bool Equals(Rect other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (!GetType().Equals(obj.GetType())) return false;
            return Equals((Rect)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }
    }
}
