using System;

namespace Sunburst.WindowsForms
{
    public struct Message
    {
        public IntPtr hWnd { get; set; }
        public int msg { get; set; }
        public IntPtr wParam { get; set; }
        public IntPtr lParam { get; set; }

        public IntPtr Result { get; set; }

        public Message(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            this.hWnd = hWnd;
            this.msg = msg;
            this.wParam = wParam;
            this.lParam = lParam;
            this.Result = IntPtr.Zero;
        }
    }
}