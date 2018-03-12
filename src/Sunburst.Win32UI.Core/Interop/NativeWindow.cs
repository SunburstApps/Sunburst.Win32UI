using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    public class NativeWindow : IWin32Window, IDisposable
    {
        public NativeWindow() { }
        public NativeWindow(IntPtr hWnd, bool ownsHandle)
        {
            Handle = hWnd;
            OwnsHandle = ownsHandle;
        }

        public IntPtr Handle { get; protected set; } = IntPtr.Zero;
        private bool OwnsHandle { get; }

        public string Text
        {
            get
            {
                int length = NativeMethods.GetWindowTextLength(Handle);
                using (HGlobal ptr = new HGlobal((length + 1) * Marshal.SystemDefaultCharSize))
                {
                    NativeMethods.GetWindowText(Handle, ptr.Handle, length + 1);
                    return Marshal.PtrToStringUni(ptr.Handle);
                }
            }

            set => NativeMethods.SetWindowText(Handle, value);
        }

        public bool Enabled
        {
            get => NativeMethods.IsWindowEnabled(Handle);
            set => NativeMethods.EnableWindow(Handle, value);
        }

        public void Activate() => NativeMethods.SetActiveWindow(Handle);
        public void Focus() => NativeMethods.SetFocus(Handle);
        public bool IsVisible => NativeMethods.IsWindowVisible(Handle);
        public bool IsIconic => NativeMethods.IsWindowIconic(Handle);
        public bool IsMaximized => NativeMethods.IsWindowZoomed(Handle);
        public void Move(Rect location) => NativeMethods.MoveWindow(Handle, location.left, location.top, location.Width, location.Height, true);
        public void BringToTop() => NativeMethods.BringWindowToTop(Handle);
        public void DestroyWindow() => NativeMethods.DestroyWindow(Handle);

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

        public int Style
        {
            get => (int)GetWindowLongPtr(-16);
            set => SetWindowLongPtr(-16, (IntPtr)value);
        }

        public int ExtendedStyle
        {
            get => (int)GetWindowLongPtr(-20);
            set => SetWindowLongPtr(-20, (IntPtr)value);
        }

        public IntPtr GetWindowLongPtr(int index) => NativeMethods.GetWindowLongPtr(Handle, index);
        public void SetWindowLongPtr(int index, IntPtr value) => NativeMethods.SetWindowLongPtr(Handle, index, value);
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

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed objects here.
                }

                if (OwnsHandle) DestroyWindow();

                disposedValue = true;
            }
        }

        ~NativeWindow() {
          // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
          Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
