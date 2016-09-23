using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public static class ResourceLoaderExtensions
    {
        public static NonOwnedIcon LoadIcon(this ResourceLoader module, string resourceName)
        {
            using (HGlobal buffer = HGlobal.WithStringUni(resourceName))
            {
                return new NonOwnedIcon(NativeMethods.LoadIcon(module.Handle, buffer.Handle));
            }
        }

        public static NonOwnedIcon LoadIcon(this ResourceLoader module, ushort resourceId)
        {
            return new NonOwnedIcon(NativeMethods.LoadIcon(module.Handle, (IntPtr)resourceId));
        }

        public static NonOwnedBitmap LoadBitmap(this ResourceLoader module, string resourceName)
        {
            using (HGlobal buffer = HGlobal.WithStringUni(resourceName))
            {
                return new NonOwnedBitmap(NativeMethods.LoadBitmap(module.Handle, buffer.Handle));
            }
        }

        public static NonOwnedBitmap LoadBitmap(this ResourceLoader module, ushort resourceId)
        {
            return new NonOwnedBitmap(NativeMethods.LoadBitmap(module.Handle, (IntPtr)resourceId));
        }

        public static Cursor LoadCursor(this ResourceLoader module, string resourceName)
        {
            using (HGlobal buffer = HGlobal.WithStringUni(resourceName))
            {
                return new Cursor(NativeMethods.LoadCursor(module.Handle, buffer.Handle));
            }
        }

        public static Cursor LoadCursor(this ResourceLoader module, ushort resourceId)
        {
            return new Cursor(NativeMethods.LoadCursor(module.Handle, (IntPtr)resourceId));
        }
    }
}
