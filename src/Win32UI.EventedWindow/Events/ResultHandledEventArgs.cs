﻿using System;

namespace Microsoft.Win32.UserInterface.Events
{
    public class ResultHandledEventArgs : HandledEventArgs
    {
        public IntPtr ResultPointer { get; set; } = IntPtr.Zero;
    }
}
