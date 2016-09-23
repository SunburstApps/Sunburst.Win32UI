using System.Runtime.InteropServices;

#pragma warning disable CS0649
namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        [MarshalAs(UnmanagedType.LPArray)]
        public RGBQUAD[] bmiColors;
    }
}
