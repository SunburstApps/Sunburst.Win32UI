using System.ComponentModel;

namespace Sunburst.Win32UI.Events
{
    public sealed class WindowEnabledChangedEventArgs : HandledEventArgs
    {
        public WindowEnabledChangedEventArgs(bool enabled)
        {
            IsEnabled = enabled;
        }

        public bool IsEnabled { get; private set; }
    }
}
