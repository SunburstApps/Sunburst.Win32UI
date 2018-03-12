using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    public class Form : Control
    {
        private int m_style = WindowStyles.WS_OVERLAPPEDWINDOW, m_exStyle = 0;
        private FormState m_state = FormState.Normal;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WindowStyles.WS_CHILD;
                cp.Style |= m_style;
                cp.ExtendedStyle |= m_exStyle;
                return cp;
            }
        }

        private void UpdateControlStyles()
        {
            if (!HandleValid) return;

            int style = NativeWindow.Style;
            int exStyle = NativeWindow.ExtendedStyle;

            void ProcessStyle(int flag)
            {
                style &= ~flag;
                if ((m_style & flag) == flag) style |= flag;
            }

            ProcessStyle(WindowStyles.WS_MINIMIZEBOX);
            ProcessStyle(WindowStyles.WS_MAXIMIZEBOX);
            ProcessStyle(WindowStyles.WS_SYSMENU);

            NativeWindow.Style = style;
            NativeWindow.ExtendedStyle = exStyle;
            Invalidate();
        }

        public bool SystemMenu
        {
            get
            {
                return (m_style & WindowStyles.WS_SYSMENU) == WindowStyles.WS_SYSMENU;
            }

            set
            {
                if (value) m_style |= WindowStyles.WS_SYSMENU;
                else m_style &= ~WindowStyles.WS_SYSMENU;
                UpdateControlStyles();
            }
        }

        public bool MaximizeBox
        {
            get
            {
                return (m_style & WindowStyles.WS_MAXIMIZEBOX) == WindowStyles.WS_MAXIMIZEBOX;
            }

            set
            {
                if (value) m_style |= WindowStyles.WS_MAXIMIZEBOX;
                else m_style &= ~WindowStyles.WS_MAXIMIZEBOX;
                UpdateControlStyles();
            }
        }

        public bool MinimizeBox
        {
            get
            {
                return (m_style & WindowStyles.WS_MINIMIZEBOX) == WindowStyles.WS_MINIMIZEBOX;
            }

            set
            {
                if (value) m_style |= WindowStyles.WS_MINIMIZEBOX;
                else m_style &= ~WindowStyles.WS_MINIMIZEBOX;
                UpdateControlStyles();
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
            if (child.Visible) child.Show();
        }

        public void RemoveChild(Control child)
        {
            if (!HandleValid) CreateHandle();

            if (m_ChildControls.Remove(child))
            {
                child.Hide();
                NativeMethods.SetParent(child.Handle, IntPtr.Zero);
            }
        }

        public IEnumerable<Control> ChildControls => m_ChildControls;
    }

    public enum FormState
    {
        Normal,
        Maximized,
        Minimized
    }
}
