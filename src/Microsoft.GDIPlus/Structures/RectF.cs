using System;

namespace Microsoft.GDIPlus
{
    public struct RectF : IEquatable<RectF>
    {
        public static RectF Empty = new RectF();
        public bool IsEmpty => Equals(Empty);

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectF(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public RectF(PointF location, SizeF size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public SizeF Size => new SizeF(Width, Height);
        public PointF Location => new PointF(X, Y);
        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;
        public bool IsEmptyArea => Width <= float.Epsilon || Height <= float.Epsilon;

        public bool Contains(float x, float y) => x >= Left && x < Right && y >= Top && y < Bottom;
        public bool Contains(PointF point) => Contains(point.X, point.Y);

        public void Inflate(PointF point) => Inflate(point.X, point.Y);
        public void Inflate(float dx, float dy)
        {
            X -= dx;
            Y -= dy;
            Width += dx * 2;
            Height += dy * 2;
        }

        public void Offset(PointF point) => Offset(point.X, point.Y);
        public void Offset(float dx,float dy)
        {
            X += dx;
            Y += dy;
        }

        public static bool Union(RectF a, RectF b, out RectF result)
        {
            float right = Math.Max(a.Right, b.Right);
            float bottom = Math.Max(a.Bottom, b.Bottom);
            float left = Math.Min(a.Left, b.Left);
            float top = Math.Min(a.Top, b.Top);

            result = new RectF(left, top, right - left, bottom - top);
            return !result.IsEmpty;
        }

        public static bool Intersect(RectF a, RectF b, out RectF result)
        {
            float right = Math.Min(a.Right, b.Right);
            float bottom = Math.Min(a.Bottom, b.Bottom);
            float left = Math.Max(a.Left, b.Left);
            float top = Math.Max(a.Top, b.Top);

            result = new RectF(left, top, right - left, bottom - top);
            return !result.IsEmpty;
        }
        
        public bool Equals(RectF other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (!GetType().Equals(obj.GetType())) return false;
            return Equals((RectF)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }
    }
}
