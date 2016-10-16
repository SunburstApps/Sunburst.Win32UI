using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
    public class ResourceLoader : IDisposable
    {
        public static ResourceLoader GetEntryModule() => EntryModule.Value;
        private static readonly Lazy<ResourceLoader> EntryModule = new Lazy<ResourceLoader>(() =>
        {
            ResourceLoader loader = new ResourceLoader(NativeMethods.GetModuleHandleW(null));
            loader.ShouldDisposeHandle = false;
            return loader;
        });

        public ResourceLoader(IntPtr hModule)
        {
            ModuleHandle = hModule;
        }

        public static ResourceLoader LoadModule(string fullPath)
        {
            if (fullPath == null) throw new ArgumentNullException(nameof(fullPath));
 
            IntPtr handle = NativeMethods.LoadLibraryEx(fullPath, IntPtr.Zero,
                NativeMethods.LOAD_LIBRARY_AS_DATAFILE | NativeMethods.LOAD_LIBRARY_AS_IMAGE_RESOURCE |
                NativeMethods.LOAD_LIBRARY_SEARCH_APPLICATION_DIR);
            if (handle == IntPtr.Zero) throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            return new ResourceLoader(handle);
        }

        private bool ShouldDisposeHandle = true;
        public IntPtr ModuleHandle { get; private set; }

        private unsafe Stream LoadCustomResourceImpl(IntPtr hResource)
        {
            IntPtr hResData = NativeMethods.LoadResource(ModuleHandle, hResource);
            IntPtr data = NativeMethods.LockResource(hResData);
            int size = NativeMethods.SizeofResource(ModuleHandle, hResource);

            return new UnmanagedMemoryStream((byte *)data.ToPointer(), Convert.ToInt64(size));
        }

        public Stream LoadCustomResource(string resourceType, ushort resourceId)
        {
            IntPtr hResource = NativeMethods.FindResourceW(ModuleHandle, resourceType, (IntPtr)resourceId);
            return LoadCustomResourceImpl(hResource);
        }

        public Stream LoadCustomResource(string resourceType, string resourceName)
        {
            using (HGlobal nameBuffer = HGlobal.WithStringUni(resourceName))
            {
                IntPtr hResource = NativeMethods.FindResourceW(ModuleHandle, resourceType, nameBuffer.Handle);
                return LoadCustomResourceImpl(hResource);
            }
        }

        public void Dispose()
        {
            if (ShouldDisposeHandle)
            {
                NativeMethods.FreeLibrary(ModuleHandle);
                ModuleHandle = IntPtr.Zero;
            }
        }
    }
}
