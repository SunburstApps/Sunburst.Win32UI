using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    /// <summary>
    /// Represents a Windows control or top-level window (<c>HWND</c>).
    /// </summary>
    public class Control : IWin32Window
    {
        static Control()
        {
            NativeMethods.INITCOMMONCONTROLSEX init_struct = new NativeMethods.INITCOMMONCONTROLSEX();
            init_struct.dwSize = Marshal.SizeOf<NativeMethods.INITCOMMONCONTROLSEX>();
            init_struct.dwICC = 0xFFFF;
            NativeMethods.InitCommonControlsEx(ref init_struct);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Control"/> that does not contain a valid handle.
        /// </summary>
        public Control()
        {
        }

        public Control(IntPtr hWnd)
        {
            m_NativeWindow = new NativeWindow(hWnd);
        }

        public virtual void CreateHandle()
        {
            if (m_NativeWindow != null) throw new InvalidOperationException($"Cannot call {nameof(CreateHandle)}() on a {nameof(Control)} instance that already has one");

            m_NativeWindow = new ControlNativeWindow(this);
            m_NativeWindow.CreateHandle(CreateParams);
        }

        protected virtual CreateParams CreateParams
        {
            get
            {
                CreateParams cp = new CreateParams();
                cp.Style = WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS | WindowStyles.WS_CHILD;
                return cp;
            }
        }

        public void DestroyHandle()
        {
            if (m_NativeWindow != null)
            {
                m_NativeWindow.DestroyHandle();
                m_NativeWindow = null;
            }
            else
            {
                throw new InvalidOperationException($"Cannot call {nameof(DestroyHandle)} on a {nameof(Control)} that doesn't have one");
            }
        }

        /// <summary>
        /// The handle to the Win32 control or top-level window that this instance wraps.
        /// </summary>
        public IntPtr Handle => m_NativeWindow.Handle;
        private NativeWindow m_NativeWindow = null;

        protected void DefWndProc(ref Message m)
        {
            if (m_NativeWindow is ControlNativeWindow native)
            {
                native.DefaultProcessMessage(ref m);
            }
            else
            {
                throw new InvalidOperationException($"Cannot call {nameof(DefWndProc)} on a {nameof(Control)} that was created with an arbitrary HWND");
            }
        }

        protected virtual void WndProc(ref Message m) => DefWndProc(ref m);
        internal void CallWndProc(ref Message m) => WndProc(ref m);

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

        public void SetTimer(uint timerId, TimeSpan interval)
        {
            if (timerId == 0) throw new ArgumentException("Timer ID cannot be zero", nameof(timerId));
            uint retval = NativeMethods.SetTimer(Handle, timerId, Convert.ToUInt32(Math.Round(interval.TotalMilliseconds)), IntPtr.Zero);
            if (retval == 0) throw new Win32Exception();
        }

        public void KillTimer(uint timerId)
        {
            if (timerId == 0) throw new ArgumentException("Timer ID cannot be zero", nameof(timerId));
            bool retval = NativeMethods.KillTimer(Handle, timerId);
            if (!retval) throw new Win32Exception();
        }
    }
}
