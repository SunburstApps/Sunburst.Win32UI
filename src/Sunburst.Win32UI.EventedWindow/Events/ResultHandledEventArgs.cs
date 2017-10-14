using System;
using System.ComponentModel;

namespace Sunburst.Win32UI.Events
{
    public class ResultHandledEventArgs : HandledEventArgs
    {
        public IntPtr ResultPointer { get; set; } = IntPtr.Zero;
    }
}
