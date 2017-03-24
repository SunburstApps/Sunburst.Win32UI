using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Windows bitmap.
    /// </summary>
    public class Bitmap : NonOwnedBitmap, IDisposable
    {
        public Bitmap(IntPtr ptr) : base(ptr) { }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }

        private static Bitmap LoadImageFromBitmapSource(NativeMethods.IWICBitmapSource source)
        {
            int width, height;
            source.GetSize(out width, out height);

            BITMAPINFO info = new BITMAPINFO();
            info.biSize = Marshal.SizeOf<BITMAPINFOHEADER>();
            info.biWidth = width;
            info.biHeight = -height; // the negation is required here
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biCompression = 0; // == BL_RGB

            using (GraphicsContext context = GraphicsContext.CreateOffscreenContext())
            {
                IntPtr imageBits = IntPtr.Zero;
                IntPtr hBitmap = NativeMethods.CreateDIBSection(context.Handle, ref info, 0, out imageBits, IntPtr.Zero, 0);
                if (hBitmap == IntPtr.Zero) throw new System.ComponentModel.Win32Exception();

                int stride = width * 4;
                int cbImage = stride * height;

                try
                {
                    source.CopyPixels(IntPtr.Zero, stride, cbImage, imageBits);
                    return new Bitmap(hBitmap);
                }
                catch
                {
                    NativeMethods.DeleteObject(hBitmap);
                    throw;
                }
            }
        }

        /// <summary>
        /// Loads an image (of any format, including PNG or JPEG) from a file path.
        /// </summary>
        /// <param name="path">
        /// The path to the image to load.
        /// </param>
        /// <returns>
        /// A <see cref="Bitmap"/> instance that must be properly disposed to avoid leaking memory.
        /// </returns>
        public static Bitmap LoadImageFromFile(string path)
        {
            if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
                throw new PlatformNotSupportedException();

            NativeMethods.IWICImagingFactory factory = (NativeMethods.IWICImagingFactory)new NativeMethods.WICImagingFactory();
            NativeMethods.IWICBitmapDecoder decoder = factory.CreateBitmapFromFilename(path, desiredAccess: 0, options: NativeMethods.WICDecodeOptions.WICDecodeMetadataCacheOnDemand);
            NativeMethods.IWICBitmapFrameDecode frame = decoder.GetFrame(0);
            return LoadImageFromBitmapSource((NativeMethods.IWICBitmapSource)frame);
        }

        /// <summary>
        /// Loads an image (of any format, including PNG or JPEG) that is embedded in a DLL.
        /// </summary>
        /// <param name="loader">
        /// A <see cref="ResourceLoader"/> instance used to load the resource.
        /// </param>
        /// <param name="resourceTypeName">
        /// The name of the resource type (for example, <c>IMAGE</c>).
        /// </param>
        /// <param name="resourceId">
        /// The numerical index of the resource to load.
        /// </param>
        /// <returns>
        /// A <see cref="Bitmap"/> instance that must be properly disposed to avoid leaking memory.
        /// </returns>
        public static Bitmap LoadImageFromResource(ResourceLoader loader, string resourceTypeName, ushort resourceId)
        {
            IntPtr hResInfo = NativeMethods.FindResourceW(loader.ModuleHandle, resourceTypeName, (IntPtr)resourceId);
            IntPtr hResData = NativeMethods.LoadResource(loader.ModuleHandle, hResInfo);
            IntPtr data = NativeMethods.LockResource(hResData);
            int size = NativeMethods.SizeofResource(loader.ModuleHandle, hResData);

            if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
            {
                throw new PlatformNotSupportedException();
            }
            else
            {
                byte[] buffer = new byte[size];
                Marshal.Copy(data, buffer, 0, size);
                IntPtr hMem = Marshal.AllocHGlobal(size);
                Marshal.Copy(buffer, 0, hMem, size);

                NativeMethods.IStream stream = null;
                try
                {
                    int hr = NativeMethods.CreateStreamOnHGlobal(hMem, true, out stream);
                    if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                    NativeMethods.IWICImagingFactory factory = (NativeMethods.IWICImagingFactory)new NativeMethods.WICImagingFactory();
                    NativeMethods.IWICBitmapDecoder decoder = factory.CreateDecoderFromStream(stream, options: NativeMethods.WICDecodeOptions.WICDecodeMetadataCacheOnDemand);
                    NativeMethods.IWICBitmapFrameDecode frame = decoder.GetFrame(0);
                    return LoadImageFromBitmapSource((NativeMethods.IWICBitmapSource)frame);
                }
                finally
                {
                    if (stream != null) Marshal.ReleaseComObject(stream);
                }
            }
        }

        /// <summary>
        /// Loads an image (of any format, including PNG or JPEG) that is embedded in a DLL.
        /// </summary>
        /// <param name="loader">
        /// A <see cref="ResourceLoader"/> instance used to load the resource.
        /// </param>
        /// <param name="resourceTypeName">
        /// The name of the resource type (for example, <c>IMAGE</c>).
        /// </param>
        /// <param name="resourceId">
        /// The name (as used in the RC file) of the resource to load.
        /// </param>
        /// <returns>
        /// A <see cref="Bitmap"/> instance that must be properly disposed to avoid leaking memory.
        /// </returns>
        public static Bitmap LoadImageFromResource(ResourceLoader loader, string resourceTypeName, string resourceName)
        {
            IntPtr hResInfo;

            using (HGlobal namePtr = HGlobal.WithStringUni(resourceName))
            {
                hResInfo = NativeMethods.FindResourceW(loader.ModuleHandle, resourceTypeName, namePtr.Handle);
            }

            IntPtr hResData = NativeMethods.LoadResource(loader.ModuleHandle, hResInfo);
            IntPtr data = NativeMethods.LockResource(hResData);
            int size = NativeMethods.SizeofResource(loader.ModuleHandle, hResData);


            if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
            {
                throw new PlatformNotSupportedException();
            }
            else
            {
                byte[] buffer = new byte[size];
                Marshal.Copy(data, buffer, 0, size);
                IntPtr hMem = Marshal.AllocHGlobal(size);
                Marshal.Copy(buffer, 0, hMem, size);

                NativeMethods.IStream stream = null;
                try
                {
                    int hr = NativeMethods.CreateStreamOnHGlobal(hMem, true, out stream);
                    if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                    NativeMethods.IWICImagingFactory factory = (NativeMethods.IWICImagingFactory)new NativeMethods.WICImagingFactory();
                    NativeMethods.IWICBitmapDecoder decoder = factory.CreateDecoderFromStream(stream, options: NativeMethods.WICDecodeOptions.WICDecodeMetadataCacheOnDemand);
                    NativeMethods.IWICBitmapFrameDecode frame = decoder.GetFrame(0);
                    return LoadImageFromBitmapSource((NativeMethods.IWICBitmapSource)frame);
                }
                finally
                {
                    if (stream != null) Marshal.ReleaseComObject(stream);
                }
            }
        }
    }
}
