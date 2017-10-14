#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    public struct NMLVDISPINFO
    {
        public const uint LVN_GETDISPINFO = 0xFFFFFF4E;

        public NMHDR header;
        public LVITEM item;
    }

    public struct NMLVFINDITEM
    {
        public const uint LVN_ODFINDITEM = 0xFFFFFF4C;

        public NMHDR header;
        public int startIndex;
        public LVFINDINFO findInfo;
    }
}
