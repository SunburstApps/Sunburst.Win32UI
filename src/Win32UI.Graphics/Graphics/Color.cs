using System;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public struct Color : IEquatable<Color>
    {
        public static Color FromWin32Color(int color_ref)
        {
            byte red = Convert.ToByte(color_ref & 0xFF);
            byte green = Convert.ToByte((color_ref >> 8) & 0xFF);
            byte blue = Convert.ToByte((color_ref >> 16) & 0xFF);
            return new Color(red, green, blue);
        }

        public static int ToWin32Color(Color color)
        {
            return color.Red | (color.Green << 8) | (color.Blue << 16);
        }

        public Color(byte r, byte b, byte g)
        {
            Red = r;
            Blue = b;
            Green = g;
        }

        public byte Red { get; set; }
        public byte Blue { get; set; }
        public byte Green { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType())) return false;
            return Equals((Color)obj);
        }

        public bool Equals(Color other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue;
        }

        public override int GetHashCode()
        {
            return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode();
        }

        public override string ToString()
        {
            return $"#{Red.ToString("X2")}{Green.ToString("X2")}{Blue.ToString("X2")}";
        }
    }
}
