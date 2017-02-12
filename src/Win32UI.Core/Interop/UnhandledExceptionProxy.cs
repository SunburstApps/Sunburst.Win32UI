using System;

namespace Microsoft.Win32.UserInterface.Interop
{
    public static class UnhandledExceptionProxy
    {
        public static event Action<Exception> UnhandledException;

        public static void OnUnhandledException(Exception ex)
        {
            UnhandledException?.Invoke(ex);

            // Don't ever return from this method, or else the program will most
            // likely crash in a far less dignified method.
            Environment.FailFast("Unhandled exception in .NET Core application", ex);
        }
    }
}
