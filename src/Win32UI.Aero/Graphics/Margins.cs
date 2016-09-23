using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        public int Left, Right, Top, Bottom;

        public Margins(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Margins(int all)
        {
            Left = Top = Right = Bottom = all;
        }
    }
}
