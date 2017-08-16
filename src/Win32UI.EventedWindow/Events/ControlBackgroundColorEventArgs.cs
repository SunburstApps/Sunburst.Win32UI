using System.ComponentModel;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Events
{
    public sealed class ControlBackgroundColorEventArgs<TControl> : HandledEventArgs where TControl : Window
    {
        public ControlBackgroundColorEventArgs(NonOwnedGraphicsContext context, TControl control)
        {
            GraphicsContext = context;
            Control = control;
        }

        public NonOwnedBrush BackgroundBrush { get; set; } = null;
        public NonOwnedGraphicsContext GraphicsContext { get; private set; }
        public TControl Control { get; private set; }
    }
}
