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

        public const uint SIF_RANGE = 0x1, SIF_PAGE = 0x2, SIF_POS = 0x4, SIF_DISABLENOSCROLL = 0x8, SIF_TRACKPOS = 0x10, SIF_ALL = 0x17;
        public const int SB_HORZ = 0, SB_VERT = 1;

        public const int SB_LINEUP = 0;
        public const int SB_LINELEFT = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINERIGHT = 1;
        public const int SB_PAGEUP = 2;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGERIGHT = 3;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_LEFT = 6;
        public const int SB_BOTTOM = 7;
        public const int SB_RIGHT = 7;
        public const int SB_ENDSCROLL = 8;
    }
}
