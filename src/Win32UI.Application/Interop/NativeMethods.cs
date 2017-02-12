using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetMessageW(out MSG msg, IntPtr hWnd, int a, int b);

        [DllImport("user32.dll")]
        public static extern void TranslateMessage(ref MSG msg);

        [DllImport("user32.dll")]
        public static extern void DispatchMessageW(ref MSG msg);

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int exitCode);

        [DllImport("user32.dll")]
        public static extern bool IsDialogMessage(IntPtr hDialog, ref MSG msg);

        [DllImport("user32.dll")]
        public static extern int TranslateAcceleratorW(IntPtr hWnd, IntPtr hAccel, ref MSG msg);


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, string reason);

        [DllImport("user32.dll")]
        public static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);
    }
}
