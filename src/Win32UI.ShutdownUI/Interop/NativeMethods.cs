using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
            throw new InvalidOperationException();
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, string reason);

        [DllImport("user32.dll")]
        public static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);
    }
}
