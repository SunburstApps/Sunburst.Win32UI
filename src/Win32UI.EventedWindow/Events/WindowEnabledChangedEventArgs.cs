using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
