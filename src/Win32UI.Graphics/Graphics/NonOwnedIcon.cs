using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Windows icon.
    /// </summary>
    public class NonOwnedIcon
    {
        public static NonOwnedIcon Load(ResourceLoader loader, string resourceName)
        {
            using (HGlobal buffer = HGlobal.WithStringUni(resourceName))
            {
                return new NonOwnedIcon(NativeMethods.LoadIcon(loader.Handle, buffer.Handle));
            }
        }

        public static NonOwnedIcon Load(ResourceLoader loader, ushort resourceId)
        {
            return new NonOwnedIcon(NativeMethods.LoadIcon(loader.Handle, (IntPtr)resourceId));
        }

        /// <summary>
        /// Creates a new instance of Icon.
        /// </summary>
        /// <param name="ptr">
        /// The handle to the native icon data.
        /// </param>
        public NonOwnedIcon(IntPtr ptr)
        {
            Handle = ptr;
        }
        
        /// <summary>
        /// The handle to the native icon data.
        /// </summary>
        public IntPtr Handle
        { get; private set; }
    }
}
