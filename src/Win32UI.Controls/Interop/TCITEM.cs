using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct TCITEM
    {
        public uint mask;
        public uint dwState;
        public uint dwStateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public IntPtr lParam;
    }
}
