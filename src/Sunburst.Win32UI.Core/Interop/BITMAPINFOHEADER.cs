#pragma warning disable CS0649
namespace Sunburst.Win32UI.Interop
{
    internal struct BITMAPINFOHEADER
    {
        public int biSize;
        public long biWidth;
        public long biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public long biXPelsPerMeter;
        public long biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
    }
}
