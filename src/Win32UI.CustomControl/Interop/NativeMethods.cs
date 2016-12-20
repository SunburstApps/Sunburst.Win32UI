using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr CreateWindowEx(int extendedStyle, string windowClass, string windowName,
            int style, int left, int top, int width, int height, IntPtr hWndParent, IntPtr hMenu,
            IntPtr hInstance, IntPtr createParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetProp(IntPtr hWnd, string name);

        [DllImport("user32.dll")]
        public static extern void SetProp(IntPtr hWnd, string name, IntPtr value);

        [DllImport("user32.dll")]
        public static extern void RemoveProp(IntPtr hWnd, string name);

        [DllImport("user32.dll")]
        public static extern IntPtr RegisterClassExW(ref WNDCLASSEXW windowClass);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateWindowEx(
            uint dwExStyle, string lpszClassName, string lpszWindowName,
            uint dwStyle, int left, int top, int width, int height,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpCreateParam
        );

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
