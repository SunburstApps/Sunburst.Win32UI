using System;

namespace Sunburst.WindowsForms.Interop
{
    internal sealed class ControlNativeWindow : NativeWindow
    {
        public ControlNativeWindow(Control owner)
        {
            Owner = owner;
        }

        public Control Owner { get; }

        public void DefaultProcessMessage(ref Message m)
        {
            base.ProcessMessage(ref m);
        }

        protected override void ProcessMessage(ref Message m)
        {
            Owner.WndProc(ref m);
        }
    }
}
