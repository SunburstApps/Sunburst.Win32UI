using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern uint SetTimer(IntPtr hWnd, uint nIDEvent, uint uElapse, IntPtr timerProc);

        [DllImport("user32.dll")]
        public static extern bool KillTimer(IntPtr hWnd, uint nIDEvent);
    }
}
