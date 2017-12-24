using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    public sealed class ExternalWindow : IWin32Window
    {
        public ExternalWindow(IntPtr hWnd)
        {
            Handle = hWnd;
        }

        public IntPtr Handle { get; }

        public string GetText()
        {
            int length = NativeMethods.GetWindowTextLength(Handle);
            using (HGlobal ptr = new HGlobal((length + 1) * Marshal.SystemDefaultCharSize))
            {
                NativeMethods.GetWindowText(Handle, ptr.Handle, length + 1);
                return Marshal.PtrToStringUni(ptr.Handle);
            }
        }

        public void SetText(string text) => NativeMethods.SetWindowText(Handle, text);

        public bool GetEnabled() => NativeMethods.IsWindowEnabled(Handle);
        public void SetEnabled(bool enable) => NativeMethods.EnableWindow(Handle, enable);
        public void MakeActive() => NativeMethods.SetActiveWindow(Handle);
        public void Focus() => NativeMethods.SetFocus(Handle);
        public bool IsVisible => NativeMethods.IsWindowVisible(Handle);
        public bool IsIconic => NativeMethods.IsWindowIconic(Handle);
        public bool IsMaximized => NativeMethods.IsZoomed(Handle);
        public void Move(Rect location) => NativeMethods.MoveWindow(Handle, location.left, location.top, location.Width, location.Height, true);
        public void BringToTop() => NativeMethods.BringWindowToTop(Handle);

        public Rect WindowRect
        {
            get
            {
                if (!NativeMethods.IsWindow(Handle)) throw new InvalidOperationException("not a valid HWND");

                Rect rect = new Rect();
                NativeMethods.GetWindowRect(Handle, ref rect);
                return rect;
            }
        }

        public Rect ClientRect
        {
            get
            {
                if (!NativeMethods.IsWindow(Handle)) throw new InvalidOperationException("not a valid HWND");

                Rect rect = new Rect();
                NativeMethods.GetClientRect(Handle, ref rect);
                return rect;
            }
        }

        public IntPtr GetWindowLongPtr(int index) => NativeMethods.GetWindowLongPtr(Handle, index);
        public void SetWindowLongPtr(int index, IntPtr value) => NativeMethods.SetWindowLongPtr(Handle, index, value);
        public int GetStyle() => (int)GetWindowLongPtr(-16);
        public void SetStyle(int style) => SetWindowLongPtr(-16, (IntPtr)style);
        public int GetExtendedStyle() => (int)GetWindowLongPtr(-20);
        public void SetExtendedStyle(int style) => SetWindowLongPtr(-20, (IntPtr)style);
        public void Show() => NativeMethods.ShowWindow(Handle, 1);
        public void Hide() => NativeMethods.ShowWindow(Handle, 0);
        public void Maximize() => NativeMethods.ShowWindow(Handle, 3);
        public void Minimize() => NativeMethods.ShowWindow(Handle, 6);
        public void RestoreFromMaximize() => NativeMethods.ShowWindow(Handle, 9);
        public void Update() => NativeMethods.UpdateWindow(Handle);

        public IntPtr SendMessage(uint messageId, IntPtr wParam, IntPtr lParam) => NativeMethods.SendMessage(Handle, messageId, wParam, lParam);
        public IntPtr PostMessage(uint messageId, IntPtr wParam, IntPtr lParam) => NativeMethods.PostMessage(Handle, messageId, wParam, lParam);
        public IntPtr SendNotifyMessage(uint messageId, IntPtr wParam, IntPtr lParam) => NativeMethods.SendNotifyMessage(Handle, messageId, wParam, lParam);
        public void Invalidate(bool redraw = true) => NativeMethods.InvalidateRect(Handle, IntPtr.Zero, redraw);
        public void Invalidate(Rect frame, bool redraw = true)
        {
            using (StructureBuffer<Rect> rectPtr = new StructureBuffer<Rect>())
            {
                rectPtr.Value = frame;
                NativeMethods.InvalidateRect(Handle, rectPtr.Handle, redraw);
            }
        }
    }
}
