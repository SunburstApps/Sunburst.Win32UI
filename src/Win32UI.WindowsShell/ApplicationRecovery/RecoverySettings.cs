using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.ApplicationRecovery
{
    public sealed class RecoverySettings
    {
        public static bool RecoveryInProgress()
        {
            int hr = NativeMethods.ApplicationRecoveryInProgress(out bool cancelled);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return cancelled;
        }

        public static void RecoveryComplete(bool recoverySucceeded) => NativeMethods.ApplicationRecoveryFinished(recoverySucceeded);

        public RecoverySettings(Action<object> cb, object parameter, TimeSpan pingInterval)
        {
            Callback = cb;
            CallbackParameter = parameter;
            PingInterval = pingInterval;
        }

        public Action<object> Callback { get; set; }
        public object CallbackParameter { get; set; }
        public TimeSpan PingInterval { get; private set; }

        public bool Register()
        {
            GCHandle handle = GCHandle.Alloc(this);
            int hr = NativeMethods.RegisterApplicationRecoveryCallback(
                NativeMethods.NativeRecoveryMethod, GCHandle.ToIntPtr(handle),
                Convert.ToUInt32(PingInterval.TotalMilliseconds), 0
            );

            return hr == 0;
        }

        public bool Unregister()
        {
            int hr = NativeMethods.UnregisterApplicationRecoveryCallback();
            return hr == 0;
        }

        internal void InvokeCallback() => Callback?.Invoke(CallbackParameter);
    }
}
