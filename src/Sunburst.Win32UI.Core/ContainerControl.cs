using System;
using System.Collections.Generic;
using Sunburst.Win32UI.Layout;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    public class ContainerControl : Control
    {
        public ContainerControl() : base() { }
        public ContainerControl(IntPtr hWnd, bool owns) : base(hWnd, owns) { }

        private LayoutEngine m_LayoutEngine = null;
        private bool m_LayoutEngineInitialized = false;
        public LayoutEngine LayoutEngine
        {
            get
            {
                return m_LayoutEngine;
            }

            set
            {
                m_LayoutEngine = value;
                m_LayoutEngineInitialized = false;

                if (HandleValid)
                {
                    m_LayoutEngine.Initialize(this);
                    m_LayoutEngineInitialized = true;
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExtendedStyle |= WindowStyles.WS_EX_CONTROLPARENT;
                return cp;
            }
        }

        public void AddChild(Control child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (!HandleValid) CreateHandle();

            NativeMethods.SetParent(child.Handle, Handle);
        }

        public void RemoveChild(Control child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (!HandleValid) CreateHandle();

            if (NativeMethods.GetParent(child.Handle) == Handle)
            {
                child.IsVisible = false;
                NativeMethods.SetParent(child.Handle, IntPtr.Zero);
            }
            else
            {
                throw new ArgumentException("The given control is not a child of this window", nameof(child));
            }
        }

        public IEnumerable<Control> ChildControls
        {
            get
            {
                List<Control> controls = new List<Control>();
                for (IntPtr hWnd = NativeMethods.GetWindow(Handle, NativeMethods.GW_CHILD); hWnd != IntPtr.Zero; hWnd = NativeMethods.GetWindow(hWnd, NativeMethods.GW_HWNDNEXT))
                {
                    controls.Add(new Control(hWnd, false));
                }
                return controls;
            }
        }

        protected override void WndProc(ref Message m)
        {
            bool handled = false;

            if (m.MessageId == WindowMessages.WM_CREATE)
            {
                if (LayoutEngine == null) LayoutEngine = new DefaultLayoutEngine();
                if (!m_LayoutEngineInitialized)
                {
                    LayoutEngine.Initialize(this);
                    m_LayoutEngineInitialized = true;
                }
            }
            else if (m.MessageId == WindowMessages.WM_SIZE)
            {
                LayoutEngine.DoLayout(this, ChildControls);
            }

            if (!handled) base.WndProc(ref m);
        }
    }
}
