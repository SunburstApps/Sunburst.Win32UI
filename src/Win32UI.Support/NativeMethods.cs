using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
    internal static class NativeMethods
    {
        public const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
            LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
            LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandleW(string moduleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string fileName, IntPtr hFile, int dwFlags);

        [DllImport("kernel32.dll")]
        public static extern void FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResourceW(IntPtr hModule, string lpType, IntPtr lpName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LockResource(IntPtr hResourceData);

        [DllImport("kernel32.dll")]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);
    }
}
