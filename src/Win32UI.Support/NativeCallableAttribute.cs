namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class NativeCallableAttribute : Attribute
    {
        public string EntryPoint;
        public CallingConvention CallingConvention;
    }
}
