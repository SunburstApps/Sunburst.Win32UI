using System;

namespace Sunburst.Win32UI.Graphics
{
    public struct Point : IEquatable<Point>
    {
        public static readonly Point Zero = new Point(0, 0);

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public int x;
        public int y;

        public override bool Equals(object other)
        {
            if (other == null || !GetType().Equals(other.GetType())) return false;
            return Equals((Point)other);
        }

        public bool Equals(Point other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
