using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateDialogIndirectParamW(IntPtr hInstance, IntPtr lpTemplate, IntPtr hWndParent, IntPtr dlgProc, IntPtr createParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr DialogBoxIndirectParamW(IntPtr hInstance, IntPtr lpTemplate, IntPtr hWndParent, IntPtr dlgProc, IntPtr createParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetPropW(IntPtr hWnd, string name);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern void SetPropW(IntPtr hWnd, string name, IntPtr value);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern void RemovePropW(IntPtr hWnd, string name);

        [DllImport("user32.dll")]
        public static extern IntPtr DialogBoxParamW(IntPtr hInstance, IntPtr templateName, IntPtr hWndParent,
            IntPtr dialogProcedure, IntPtr initParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateDialogParamW(IntPtr hInstance, IntPtr templateName, IntPtr hWndParent, IntPtr dialogProcedure, IntPtr initParam);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadAcceleratorsW(IntPtr hInstance, IntPtr resourceName);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginDeferWindowPos(int controlCount);

        [DllImport("user32.dll")]
        public static extern IntPtr DeferWindowPos(IntPtr hDefer, IntPtr hWnd, IntPtr hWndInsertAfter,
            int left, int top, int width, int height, int flags);

        [DllImport("user32.dll")]
        public static extern void EndDeferWindowPos(IntPtr hDefer);

        [DllImport("user32.dll")]
        public static extern void ScreenToClient(IntPtr hWnd, ref Point pt);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int itemNumber);

        [DllImport("user32.dll")]
        public static extern void EndDialog(IntPtr hDlg, IntPtr result);

        [DllImport("user32.dll")]
        public static extern bool MapDialogRect(IntPtr hWnd, ref Rect rect);

        [DllImport("kernel32.dll")]
        public static extern int MulDiv(int input, int numer, int denom);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int metricId);
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate IntPtr DLGPROC(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}
