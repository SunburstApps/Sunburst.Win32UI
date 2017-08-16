using System.ComponentModel;

namespace Microsoft.Win32.UserInterface.Events
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
