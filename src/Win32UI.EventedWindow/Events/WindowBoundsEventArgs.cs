using System.ComponentModel;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Events
{
    public class WindowBoundsEventArgs : HandledEventArgs
    {
        public Size MinimumSize { get; set; } = new Size();
        public Size MaximumSize { get; set; } = new Size();
    }
}
