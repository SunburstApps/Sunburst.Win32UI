using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate IntPtr WNDPROC(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    internal static class NativeMethods
    {
        public const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
            LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
            LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandleW(string moduleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string fileName, IntPtr hFile, int dwFlags);

        [DllImport("kernel32.dll")]
        public static extern void FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResourceW(IntPtr hModule, string lpType, IntPtr lpName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LockResource(IntPtr hResourceData);

        [DllImport("kernel32.dll")]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateWindowEx(int extendedStyle, string windowClass, string windowName,
            int style, int left, int top, int width, int height, IntPtr hWndParent, IntPtr hMenu,
            IntPtr hInstance, IntPtr createParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(IntPtr hWnd, IntPtr buffer, int bufferLength);

        [DllImport("user32.dll")]
        public static extern void SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendNotifyMessage(IntPtr hWnd, uint messageId, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void EnableWindow(IntPtr hWnd, bool enable);

        [DllImport("user32.dll")]
        public static extern void SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void MoveWindow(IntPtr hWnd, int left, int top, int width, int height, bool redraw);

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void GetWindowRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll")]
        public static extern void GetClientRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        public static extern void SetWindowLongPtr(IntPtr hWnd, int index, IntPtr value);

        [DllImport("user32.dll")]
        public static extern void InvalidateRect(IntPtr hWnd, IntPtr rectPtr, bool redrawBackground);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(WNDPROC wndProc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr wndProcRaw, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowProc(IntPtr hWnd, int index, WNDPROC wndProc);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowProcRaw(IntPtr hWnd, int index, IntPtr wndProc);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, string reason);

        [DllImport("user32.dll")]
        public static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int relationship);
        public const int GW_CHILD = 5, GW_HWNDNEXT = 2;

        [DllImport("comctl32.dll")]
        public static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX init_struct);

        public struct INITCOMMONCONTROLSEX
        {
            public int dwSize, dwICC;
        }
    }
}
