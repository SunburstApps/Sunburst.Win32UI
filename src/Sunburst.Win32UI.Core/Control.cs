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
    public class Control : Component, IWin32Window
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

        public Control(IntPtr hWnd, bool ownsHandle)
        {
            NativeWindow = new NativeWindow(hWnd, ownsHandle);
        }

        protected void SubclassWindow()
        {
            if (!HandleValid) throw new InvalidOperationException("You must have a valid handle before calling this method");
            if (NativeWindow is ControlNativeWindow) throw new InvalidOperationException("The NativeWindow has already been subclassed");

            NativeWindow = ControlNativeWindow.SubclassWindow(this, Handle, NativeWindow.OwnsHandle);
        }

        protected override void Dispose(bool disposing)
        {
            NativeWindow?.Dispose();
            base.Dispose(disposing);
        }

        public virtual void CreateHandle()
        {
            if (NativeWindow != null) throw new InvalidOperationException($"Cannot call {nameof(CreateHandle)}() on a {nameof(Control)} instance that already has one");

            CreateParams cp = CreateParams;
            cp.ParentHandle = IntPtr.Zero;
            NativeWindow = ControlNativeWindow.Create(this, cp);
        }

        protected virtual CreateParams CreateParams
        {
            get
            {
                CreateParams cp = new CreateParams();
                cp.Style = WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS | WindowStyles.WS_CHILD;
                cp.ClassStyle = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_DBLCLKS;
                return cp;
            }
        }

        /// <summary>
        /// The handle to the Win32 control or top-level window that this instance wraps.
        /// </summary>
        public IntPtr Handle => NativeWindow.Handle;
        public NativeWindow NativeWindow { get; private set; } = null;
        protected bool HandleValid => NativeWindow != null && NativeMethods.IsWindow(NativeWindow.Handle);

        protected void DefWndProc(ref Message m)
        {
            if (NativeWindow is ControlNativeWindow native)
            {
                native.DefWndProc(ref m);
            }
            else
            {
                throw new InvalidOperationException($"Cannot call {nameof(DefWndProc)} on a {nameof(Control)} that was created with an arbitrary HWND");
            }
        }

        internal void CallWndProc(ref Message m) => WndProc(ref m);

        public string Text
        {
            get => NativeWindow.Text;
            set => NativeWindow.Text = value;
        }

        public bool Enabled
        {
            get => NativeWindow.Enabled;
            set => NativeWindow.Enabled = value;
        }

        public Font Font
        {
            get => new Font(NativeWindow.SendMessage(WindowMessages.WM_GETFONT, IntPtr.Zero, IntPtr.Zero));
            set => NativeWindow.SendMessage(WindowMessages.WM_SETFONT, Font.Handle, (IntPtr)1);
        }

        public bool IsVisible
        {
            get
            {
                if (!HandleValid) throw new InvalidOperationException($"Cannot get {nameof(IsVisible)} until the handle has been created");
                return NativeWindow.IsVisible;
            }

            set
            {
                if (!HandleValid) throw new InvalidOperationException($"Cannot set {nameof(IsVisible)} until the handle has been created");

                if (value) NativeWindow.Show();
                else NativeWindow.Hide();
            }
        }

        public void Activate() => NativeWindow.Activate();
        public void Focus() => NativeWindow.Focus();
        public void Move(Rect location) => NativeMethods.MoveWindow(Handle, location.left, location.top, location.Width, location.Height, true);
        public void BringToTop() => NativeMethods.BringWindowToTop(Handle);

        public Rect WindowRect
        {
            get
            {
                if (!HandleValid) throw new InvalidOperationException($"Cannot get {nameof(WindowRect)} until the handle has been created");

                Rect rect = new Rect();
                NativeMethods.GetWindowRect(Handle, ref rect);
                NativeMethods.MapWindowPoints(IntPtr.Zero, NativeMethods.GetParent(Handle), ref rect);

                return rect;
            }

            set
            {
                if (!HandleValid) throw new InvalidOperationException($"Cannot set {nameof(WindowRect)} until the handle has been created");
                NativeMethods.MoveWindow(Handle, value.left, value.top, value.Width, value.Height, true);
            }
        }

        public void Update() => NativeMethods.UpdateWindow(Handle);

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

        public AutoScaleMode AutoScaleMode { get; set; } = AutoScaleMode.Font;
        private Font mOldScalingFont;

        public AutoSizeMode AutoSizeMode { get; set; } = AutoSizeMode.GrowOnly;
        public virtual Size GetPreferredSize()
        {
            Rect rect = WindowRect;
            return new Size(rect.Width, rect.Height);
        }

        protected virtual void WndProc(ref Message m)
        {
            bool handled = false;

            if (m.MessageId == WindowMessages.WM_SETFONT && AutoScaleMode == AutoScaleMode.Font)
            {
                Size GetAutoScaleDimensions(Font font)
                {
                    using (GraphicsContext ctx = GraphicsContext.CreateOffscreenContext())
                    {
                        ctx.CurrentFont = font;
                        TEXTMETRIC metric;
                        NativeMethods.GetTextMetrics(ctx.Handle, out metric);

                        Size scaleFactor = new Size();
                        scaleFactor.height = metric.tmHeight;
                        if ((metric.tmPitchAndFamily & 1) != 0)
                        {
                            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            int alphabetLength = alphabet.Length;

                            Size extents = Size.Zero;
                            NativeMethods.GetTextExtentPoint32(ctx.Handle, alphabet, alphabetLength, ref extents);
                            scaleFactor.width = (int)Math.Round((float)extents.width / alphabetLength);
                        }
                        else
                        {
                            scaleFactor.width = metric.tmAveCharWidth;
                        }

                        return scaleFactor;
                    }
                }

                Font newFont = new Font(m.WParam);
                if (mOldScalingFont != null)
                {
                    Size newScaleFactor = GetAutoScaleDimensions(newFont);
                    Size oldScaleFactor = GetAutoScaleDimensions(mOldScalingFont);

                    Rect myRect = WindowRect;
                    int top = myRect.top, left = myRect.left, width = myRect.Width, height = myRect.Height;
                    top = top * newScaleFactor.height / oldScaleFactor.height;
                    left = left * newScaleFactor.width / oldScaleFactor.width;
                    width = width * newScaleFactor.width / oldScaleFactor.width;
                    height = height * newScaleFactor.height / oldScaleFactor.height;

                    NativeMethods.SetWindowPos(Handle, IntPtr.Zero, left, top, width, height, MoveWindowFlags.IgnoreZOrder | MoveWindowFlags.DoNotActivate);
                }

                mOldScalingFont = newFont;
                handled = false;
            }

            if (!handled) MessageReflector.ReflectMessage(ref m, out handled);
            if (!handled) DefWndProc(ref m);
        }
    }

    public enum AutoScaleMode
    {
        Disabled = 0,
        Font
    }

    public enum AutoSizeMode
    {
        Disabled = 0,
        GrowOnly,
        GrowAndShrink
    }
}
