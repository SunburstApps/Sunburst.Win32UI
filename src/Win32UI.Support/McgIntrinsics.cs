using System;

namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class McgIntrinsicsAttribute : Attribute { }

    [McgIntrinsics]
    public static class McgIntrinsics
    {
        public static IntPtr AddrOf<T>(T thing)
        {
            // This method is implemented in the CoreRT toolchain.
            throw new PlatformNotSupportedException();
        }
    }
}
