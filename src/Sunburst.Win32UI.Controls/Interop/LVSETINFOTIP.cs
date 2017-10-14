using System.Runtime.InteropServices;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct LVSETINFOTIP
    {
        public uint cbSize;
        public int dwFlags;
        public string pszText;
        public int iItem;
        public int iSubItem;
    }
}
