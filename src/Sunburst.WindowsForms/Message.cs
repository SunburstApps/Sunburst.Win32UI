using System;
using System.Drawing;

namespace Sunburst.WindowsForms
{
    public struct Message
    {
        public IntPtr hWnd { get; set; }
        public uint msg { get; set; }
        public IntPtr wParam { get; set; }
        public IntPtr lParam { get; set; }

        public IntPtr Result { get; set; }
    }
}
