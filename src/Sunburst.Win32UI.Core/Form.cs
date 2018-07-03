using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    public class Form : Control
    {
        private FormState m_state = FormState.Normal;
        private FormBorderStyle m_borderStyle = FormBorderStyle.Resizable;

        private void InvalidateFrame()
        {
            NativeMethods.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0,
                                       MoveWindowFlags.IgnoreMove | MoveWindowFlags.IgnoreSize |
                                       MoveWindowFlags.IgnoreZOrder | MoveWindowFlags.FrameChanged);
        }

        public Form()
        {
            CreateHandle();
        }

        public Form(IntPtr hWnd, bool owns) : base(hWnd, owns) { }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WindowStyles.WS_CHILD;
                cp.Style |= WindowStyles.WS_OVERLAPPEDWINDOW;
                cp.ExtendedStyle |= WindowStyles.WS_EX_CONTROLPARENT;
                return cp;
            }
        }

        public bool HasSystemMenu
        {
            get
            {
                return (NativeWindow.Style & WindowStyles.WS_SYSMENU) == WindowStyles.WS_SYSMENU;
            }

            set
            {
                int style = NativeWindow.Style;
                if (value) style |= WindowStyles.WS_SYSMENU;
                else style &= ~WindowStyles.WS_SYSMENU;

                NativeWindow.Style = style;
                Invalidate();
                InvalidateFrame();
            }
        }

        public bool MaximizeBox
        {
            get
            {
                return (NativeWindow.Style & WindowStyles.WS_MAXIMIZEBOX) == WindowStyles.WS_MAXIMIZEBOX;
            }

            set
            {
                int style = NativeWindow.Style;
                if (value) style |= WindowStyles.WS_MAXIMIZEBOX;
                else style &= ~WindowStyles.WS_MAXIMIZEBOX;

                NativeWindow.Style = style;
                Invalidate();
                InvalidateFrame();
            }
        }

        public bool MinimizeBox
        {
            get
            {
                return (NativeWindow.Style & WindowStyles.WS_MINIMIZEBOX) == WindowStyles.WS_MINIMIZEBOX;
            }

            set
            {
                int style = NativeWindow.Style;
                if (value) style |= WindowStyles.WS_MINIMIZEBOX;
                else style &= ~WindowStyles.WS_MINIMIZEBOX;

                NativeWindow.Style = style;
                Invalidate();
                InvalidateFrame();
            }
        }

        public Icon Icon
        {
            get
            {
                return new Icon(NativeWindow.SendMessage(WindowMessages.WM_GETICON, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                NativeWindow.SendMessage(WindowMessages.WM_SETICON, IntPtr.Zero, value.Handle);
            }
        }

        public FormState State
        {
            get
            {
                return m_state;
            }

            set
            {
                m_state = value;
                if (HandleValid)
                {
                    if (value == FormState.Normal && NativeWindow.IsMaximized) NativeWindow.RestoreFromMaximize();
                    else if (value == FormState.Maximized) NativeWindow.Maximize();
                    else if (value == FormState.Minimized) NativeWindow.Minimize();
                    else NativeWindow.Show();
                }
            }
        }

        public Size MinimumSize { get; set; }
        public Size MaximumSize { get; set; }

        public event EventHandler<CancelEventArgs> FormClosing;
        protected virtual void OnFormClosing(CancelEventArgs e) => FormClosing?.Invoke(this, e);

        public event EventHandler FormClosed;
        protected virtual void OnFormClosed(EventArgs e) => FormClosed?.Invoke(this, e);

        public void Close()
        {
            CancelEventArgs args = new CancelEventArgs();
            OnFormClosing(args);
            if (args.Cancel) return;

            NativeWindow.DestroyWindow();
        }

        protected override void WndProc(ref Message m)
        {
            bool handled = false;

            if (m.MessageId == WindowMessages.WM_CLOSE)
            {
                Close();
            }
            else if (m.MessageId == WindowMessages.WM_DESTROY)
            {
                OnFormClosed(EventArgs.Empty);
            }
            else if (m.MessageId == WindowMessages.WM_GETMINMAXINFO)
            {
                unsafe
                {
                    MINMAXINFO* sizeInfo = (MINMAXINFO*)m.LParam;
                    sizeInfo->ptMinTrackSize = new Point(MinimumSize.width, MinimumSize.height);
                    sizeInfo->ptMaxTrackSize = new Point(MaximumSize.width, MaximumSize.height);
                }

                handled = true;
            }

            if (!handled) base.WndProc(ref m);
        }

        private readonly List<Control> m_ChildControls = new List<Control>();

        public void AddChild(Control child)
        {
            if (!HandleValid) CreateHandle();

            m_ChildControls.Add(child);
            NativeMethods.SetParent(child.Handle, Handle);
            child.IsVisible = true;
        }

        public void RemoveChild(Control child)
        {
            if (!HandleValid) CreateHandle();

            if (m_ChildControls.Remove(child))
            {
                child.IsVisible = false;
                NativeMethods.SetParent(child.Handle, IntPtr.Zero);
            }
        }

        public IEnumerable<Control> ChildControls => m_ChildControls;

        public void Show() => IsVisible = true;
        public void Hide() => IsVisible = false;

        #region Menus

        public Menu Menu
        {
            get => new Menu(NativeMethods.GetMenu(Handle));
            set => NativeMethods.SetMenu(Handle, value.Handle);
        }

        public Menu SystemMenu => new Menu(NativeMethods.GetSystemMenu(Handle, false));
        public void ResetSystemMenu() => NativeMethods.GetSystemMenu(Handle, true);

        #endregion

        public FormBorderStyle BorderStyle
        {
            get => m_borderStyle;
            set
            {
                m_borderStyle = value;
                if (HandleValid)
                {
                    int style = NativeWindow.Style;

                    if (m_borderStyle == FormBorderStyle.None)
                    {
                        style &= ~WindowStyles.WS_OVERLAPPEDWINDOW;
                    }
                    else if (m_borderStyle == FormBorderStyle.FixedSize)
                    {
                        style |= WindowStyles.WS_OVERLAPPEDWINDOW;
                        style &= ~WindowStyles.WS_SIZEBOX;
                    }
                    else if (m_borderStyle == FormBorderStyle.Resizable)
                    {
                        style |= WindowStyles.WS_OVERLAPPEDWINDOW;
                    }

                    NativeWindow.Style = style;
                    Invalidate();
                    InvalidateFrame();
                }
            }
        }
    }

    public enum FormState
    {
        Normal,
        Maximized,
        Minimized
    }

    /// <summary>
    /// Specifies the types of borders and title bar a <see cref="Form"/> can have.
    /// </summary>
    public enum FormBorderStyle
    {
        /// <summary>
        /// The <see cref="Form"/> has no title bar or resize borders.
        /// </summary>
        None,

        /// <summary>
        /// The <see cref="Form"/> has a title bar, but cannot be resized by the user.
        /// </summary>
        FixedSize,

        /// <summary>
        /// The <see cref="Form"/> has a title bar and can be resized by dragging the window edges.
        /// </summary>
        Resizable
    }
}
