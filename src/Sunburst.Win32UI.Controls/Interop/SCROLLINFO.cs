#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    internal struct SCROLLINFO
    {
        public uint cbSize;
        public uint fMask;
        public int nMin;
        public int nMax;
        public uint nPage;
        public int nPos;
        public int nTrackPos;

        public const uint SIF_ALL = 0x17, SIF_DISABLENOSCROLL = 0x8;
    }
}
