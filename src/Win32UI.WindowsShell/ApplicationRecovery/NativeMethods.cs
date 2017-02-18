using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.ApplicationRecovery
{
    internal static class NativeMethods
    {
        internal delegate uint NativeRecoveryCallback(IntPtr state);
        internal static readonly NativeRecoveryCallback NativeRecoveryMethod = NativeRecoveryHandler;
        private static uint NativeRecoveryHandler(IntPtr parameter)
        {
            ApplicationRecoveryInProgress(out bool cancelled);

            GCHandle handle = GCHandle.FromIntPtr(parameter);
            RecoverySettings data = (RecoverySettings)handle.Target;
            data.InvokeCallback();
            handle.Free();

            return 0;
        }

        [DllImportAttribute("kernel32.dll")]
        internal static extern void ApplicationRecoveryFinished([MarshalAsAttribute(UnmanagedType.Bool)] bool success);
        [DllImportAttribute("kernel32.dll"), PreserveSigAttribute]
        internal static extern int ApplicationRecoveryInProgress([Out, MarshalAsAttribute(UnmanagedType.Bool)] out bool cancelled);
        [DllImportAttribute("kernel32.dll"), PreserveSigAttribute]
        internal static extern int RegisterApplicationRecoveryCallback(NativeRecoveryCallback callback, IntPtr param, uint pingInterval, uint unusedFlags);
        [DllImportAttribute("kernel32.dll"), PreserveSigAttribute]
        internal static extern int UnregisterApplicationRecoveryCallback();

        [DllImportAttribute("kernel32.dll"), PreserveSigAttribute]
        internal static extern int RegisterApplicationRestart([MarshalAsAttribute(UnmanagedType.BStr)] string commandLineArgs, RestartRestrictions flags);
        [DllImportAttribute("kernel32.dll"), PreserveSigAttribute]
        internal static extern int UnregisterApplicationRestart();
    }
}
