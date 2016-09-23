using System;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public struct Size : IEquatable<Size>
    {
        public static readonly Size Zero = new Size(0, 0);

        public Size(int w, int h) {
            this.width = w;
            this.height = h;
        }
        
        public int width;
        public int height;

        public override bool Equals(object other)
        {
            if (other == null || !GetType().Equals(other.GetType())) return false;
            return Equals((Size)other);
        }

        public bool Equals(Size other)
        {
            return this.width == other.width && this.height == other.height;
        }

        public override int GetHashCode()
        {
            return this.width.GetHashCode() ^ this.height.GetHashCode();
        }

        public override string ToString()
        {
            return $"<Size>(w={width}, h={height})";
        }
    }
}
