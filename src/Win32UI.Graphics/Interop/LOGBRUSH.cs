using System;

namespace Microsoft.Win32.UserInterface.Interop
{
    public struct LOGBRUSH
    {
        public uint lbStyle;
        public int lbColor;
        public IntPtr lbHatch;
    }
}
