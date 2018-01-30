using System;
using System.ComponentModel;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Core
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

        public bool MaximizeBox
        {
            get
            {
                if (HandleValid) return (NativeWindow.Style & WindowStyles.WS_MAXIMIZEBOX) == WindowStyles.WS_MAXIMIZEBOX;
                else return (m_style & WindowStyles.WS_MAXIMIZEBOX) == WindowStyles.WS_MAXIMIZEBOX;
            }

            set
            {
                if (HandleValid)
                {
                    int style = NativeWindow.Style;
                    if (value) style |= WindowStyles.WS_MAXIMIZEBOX;
                    else style &= ~WindowStyles.WS_MAXIMIZEBOX;
                    NativeWindow.Style = style;

                    NativeWindow.Invalidate();
                }
                else
                {
                    if (value) m_style |= WindowStyles.WS_MAXIMIZEBOX;
                    else m_style &= ~WindowStyles.WS_MAXIMIZEBOX;
                }
            }
        }

        public bool MinimizeBox
        {
            get
            {
                if (HandleValid) return (NativeWindow.Style & WindowStyles.WS_MINIMIZEBOX) == WindowStyles.WS_MINIMIZEBOX;
                else return (m_style & WindowStyles.WS_MINIMIZEBOX) == WindowStyles.WS_MINIMIZEBOX;
            }

            set
            {
                if (HandleValid)
                {
                    int style = NativeWindow.Style;
                    if (value) style |= WindowStyles.WS_MINIMIZEBOX;
                    else style &= ~WindowStyles.WS_MINIMIZEBOX;
                    NativeWindow.Style = style;

                    NativeWindow.Invalidate();
                }
                else
                {
                    if (value) m_style |= WindowStyles.WS_MINIMIZEBOX;
                    else m_style &= ~WindowStyles.WS_MINIMIZEBOX;
                }
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
                if (HandleValid)
                {
                    if (NativeWindow.IsMaximized) return FormState.Maximized;
                    else if (NativeWindow.IsIconic) return FormState.Minimized;
                    else return FormState.Normal;
                }
                else
                {
                    return m_state;
                }
            }

            set
            {
                m_state = value;
                if (HandleValid)
                {
                    if (value == FormState.Normal) NativeWindow.RestoreFromMaximize();
                    else if (value == FormState.Maximized) NativeWindow.Maximize();
                    else if (value == FormState.Minimized) NativeWindow.Minimize();

                    NativeWindow.Show();
                }
            }
        }

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

            if (!handled) base.WndProc(ref m);
        }
    }

    public enum FormState
    {
        Normal,
        Maximized,
        Minimized
    }
}
