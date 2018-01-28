using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Windows icon.
    /// </summary>
    public class Icon : IDisposable
    {
        public static Icon Load(ResourceLoader loader, string resourceName)
        {
            using (HGlobal buffer = HGlobal.WithStringUni(resourceName))
            {
                return new Icon(NativeMethods.LoadIcon(loader.ModuleHandle, buffer.Handle));
            }
        }

        public static Icon Load(ResourceLoader loader, ushort resourceId)
        {
            return new Icon(NativeMethods.LoadIcon(loader.ModuleHandle, (IntPtr)resourceId));
        }

        /// <summary>
        /// Creates a new instance of Icon.
        /// </summary>
        /// <param name="ptr">
        /// The handle to the native icon data.
        /// </param>
        public Icon(IntPtr ptr)
        {
            Handle = ptr;
        }
        
        /// <summary>
        /// The handle to the native icon data.
        /// </summary>
        public IntPtr Handle { get; private set; }

        public void Dispose()
        {
            NativeMethods.DestroyIcon(Handle);
        }
    }
}
