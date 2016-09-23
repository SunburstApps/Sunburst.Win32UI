using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MINMAXINFO
    {
        public Point ptReserved;
        public Point ptMaxSize;
        public Point ptMaxPosition;
        public Point ptMinTrackSize;
        public Point ptMaxTrackSize;
    }
}
