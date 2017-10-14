using System;

namespace Sunburst.Win32UI.Graphics
{
    public struct Range : IEquatable<Range>
    {
        public static readonly Range Zero = new Range(0, 0);

        public Range(uint loc, uint len)
        {
            Location = loc;
            Length = len;
        }
        
        public uint Location;
        public uint Length;

        public uint MaxValue
        {
            get
            {
                return Location + Length;
            }
        }

        public override bool Equals(object other)
        {
            if (other == null || !GetType().Equals(other.GetType())) return false;
            return Equals((Range)other);
        }

        public bool Equals(Range other)
        {
            return this.Location == other.Location && this.Length == other.Length;
        }

        public override int GetHashCode()
        {
            return Location.GetHashCode() ^ Length.GetHashCode();
        }

        public override string ToString()
        {
            return $"<Range>(loc={Location}, len={Length})";
        }
    }
}
