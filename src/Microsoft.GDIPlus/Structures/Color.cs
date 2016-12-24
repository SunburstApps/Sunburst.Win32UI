using System;

namespace Microsoft.GDIPlus
{
    public struct Color : IEquatable<Color>
    {
        public static Color FromARGB(uint argb)
        {
            byte red = (byte)(argb & 0xFF);
            byte green = (byte)((argb >> 8) & 0xFF);
            byte blue = (byte)((argb >> 16) & 0xFF);
            byte alpha = (byte)((argb >> 24) & 0xFF);

            return new Color(red, green, blue, alpha);
        }

        public static uint ToARGB(Color color)
        {
            return (uint)(color.Red | color.Green << 8 | color.Blue << 16 | color.Alpha << 24);
        }

        public Color(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = 255;
        }

        public Color(byte r, byte g, byte b, byte a)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
        }

        public byte Red { get; set; }
        public byte Blue { get; set; }
        public byte Green { get; set; }
        public byte Alpha { get; set; }

        public bool Equals(Color other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue && Alpha == other.Alpha;
        }

        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(Color))) return false;
            return Equals((Color)obj);
        }

        public override int GetHashCode()
        {
            return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode() ^ Alpha.GetHashCode();
        }

        public override string ToString()
        {
            return $"#{Alpha.ToString("X2")}{Red.ToString("X2")}{Green.ToString("X2")}{Blue.ToString("X2")}";
        }
    }
}
