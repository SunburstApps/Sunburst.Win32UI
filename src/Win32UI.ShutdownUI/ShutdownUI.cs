using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public static class ShutdownUI
    {
        public static void CreateShutdownBlock(Window window, string reason)
        {
            bool success = NativeMethods.ShutdownBlockReasonCreate(window.Handle, reason);
            if (!success) throw new System.ComponentModel.Win32Exception();
        }

        public static void DestroyShutdownBlock(Window window)
        {
            bool success = NativeMethods.ShutdownBlockReasonDestroy(window.Handle);
            if (!success) throw new System.ComponentModel.Win32Exception();
        }

        public static void BlockShutdown(Window window, string reason, Action action)
        {
            // This is outside the try clause so that any exceptions thrown by it will
            // be ignored by the finally clause. This prevents a possible second exception
            // from being thrown if creating the shutdown block failed.
            CreateShutdownBlock(window, reason);

            try
            {
                action();
            }
            finally
            {
                DestroyShutdownBlock(window);
            }
        }
    }
}
