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

        protected virtual void WndProc(ref Message m)
        {
            DefWndProc(ref m);
        }

        internal void CallWndProc(ref Message m) => WndProc(ref m);
    }
}
