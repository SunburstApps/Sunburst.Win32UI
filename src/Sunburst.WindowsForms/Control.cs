using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Sunburst.WindowsForms.Interop;

namespace Sunburst.WindowsForms
{
    public class Control : Component, IWin32Window
    {
        private ControlNativeWindow nativeWindow;

        public IntPtr Handle => nativeWindow.Handle;

        protected virtual void DefWndProc(ref Message m)
        {
            nativeWindow.DefaultProcessMessage(ref m);
        }

        protected internal virtual void WndProc(ref Message m)
        {
            nativeWindow.DefaultProcessMessage(ref m);
        }
    }
}
