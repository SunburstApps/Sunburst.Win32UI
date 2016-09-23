using System;

namespace Microsoft.Win32.UserInterface.Events
{
    public sealed class UnhandledExceptionEventArgs : EventArgs
    {
        internal UnhandledExceptionEventArgs(Exception ex)
        {
            Exception = ex;
        }

        public Exception Exception { get; private set; }
    }
}
