using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Handles;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    /// <summary>
    /// Represents a Windows control or top-level window (<c>HWND</c>).
    /// </summary>
    public class Window
    {
        /// <summary>
        /// Creates a new instance of <see cref="Window"/> that does not contain a valid handle.
        /// </summary>
        public Window()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Window"/> with the given handle.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to a Win32 control or top-level window.
        /// </param>
        public Window(IntPtr hWnd)
        {
            Handle = hWnd;
        }

        public static IntPtr CreateHandle(string windowClass, Rect frame, string text,
            int style = 0, int extendedStyle = 0, Window parent = null, IMenuHandle hMenu = null)
        {
            IntPtr hWndParent = parent?.Handle ?? IntPtr.Zero;
            return NativeMethods.CreateWindowEx(extendedStyle, windowClass, text,
                style, frame.left, frame.top, frame.Width, frame.Height, hWndParent, hMenu?.Handle ?? IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero);
        }

        public virtual void CreateHandle(Rect frame, string text, int style = 0, int extendedStyle = 0,
            Window parent = null, IMenuHandle hMenu = null)
        {
            if (WindowClassName == null) throw new InvalidOperationException($"{nameof(WindowClassName)} must be overridden");
            Handle = CreateHandle(WindowClassName, frame, text, style, extendedStyle, parent, hMenu);
        }

        public bool DestroyHandle()
        {
            if (Handle != IntPtr.Zero && NativeMethods.IsWindow(Handle))
            {
                NativeMethods.DestroyWindow(Handle);
                Handle = IntPtr.Zero;
                return true;
            }

            return false;
        }

        /// <summary>
        /// The handle to the Win32 control or top-level window that this instance wraps.
        /// </summary>
        public IntPtr Handle { get; set; }

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

        public Window Parent => new Window(NativeMethods.GetParent(Handle));
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

        public virtual string WindowClassName => null;
    }
}
