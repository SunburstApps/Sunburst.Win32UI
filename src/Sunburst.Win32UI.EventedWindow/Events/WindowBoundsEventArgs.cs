using System.ComponentModel;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Events
{
    public class WindowBoundsEventArgs : HandledEventArgs
    {
        public Size MinimumSize { get; set; } = new Size();
        public Size MaximumSize { get; set; } = new Size();
    }
}
