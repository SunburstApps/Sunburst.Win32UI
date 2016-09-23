using System;

#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct CREATESTRUCT
    {
        public IntPtr lpCreateParams;
        public IntPtr hInstance;
        public IntPtr hMenu;
        public IntPtr hWndParent;
        public int cy;
        public int cx;
        public int y;
        public int x;
        public int style;
        public string lpszName;
        public string lpszClass;
        public int dwExStyle;
    }
}
