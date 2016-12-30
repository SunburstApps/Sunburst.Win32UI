#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
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

        public const uint SIF_RANGE = 0x1, SIF_PAGE = 0x2, SIF_POS = 0x4, SIF_DISABLENOSCROLL = 0x8, SIF_ALL = 0x17;
        public const int SB_HORZ = 0, SB_VERT = 1;
    }
}
