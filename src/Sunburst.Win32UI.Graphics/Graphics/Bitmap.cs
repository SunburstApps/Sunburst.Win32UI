using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Win32 bitmap (GDI <c>HBITMAP</c>).
    /// </summary>
    public class Bitmap : IDisposable
    {
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
            decoder.GetFrame(0, out NativeMethods.IWICBitmapFrameDecode frame);
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
                    decoder.GetFrame(0, out NativeMethods.IWICBitmapFrameDecode frame);
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
        /// <param name="resourceName">
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
                    decoder.GetFrame(0, out NativeMethods.IWICBitmapFrameDecode frame);
                    return LoadImageFromBitmapSource((NativeMethods.IWICBitmapSource)frame);
                }
                finally
                {
                    if (stream != null) Marshal.ReleaseComObject(stream);
                }
            }
        }

        public static Bitmap Load(ResourceLoader loader, string resourceName)
        {
            using (HGlobal buffer = HGlobal.WithStringUni(resourceName))
            {
                return new Bitmap(NativeMethods.LoadBitmap(loader.ModuleHandle, buffer.Handle));
            }
        }

        public static Bitmap Load(ResourceLoader loader, ushort resourceId)
        {
            return new Bitmap(NativeMethods.LoadBitmap(loader.ModuleHandle, (IntPtr)resourceId));
        }

        public Bitmap(IntPtr ptr)
        {
            Handle = ptr;
        }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }

        public IntPtr Handle { get; private set; }

        private BitmapHeader Header
        {
            get
            {
                using (StructureBuffer<BitmapHeader> ptr = new StructureBuffer<BitmapHeader>())
                {
                    int returnedSize = NativeMethods.GetObject(Handle, ptr.Size, ptr.Handle);
                    if (returnedSize != ptr.Size) throw new InvalidOperationException($"GetObject() returned unexpected size {returnedSize}");
                    return ptr.Value;
                }
            }
        }

        public Size Size
        {
            get
            {
                var header = Header;
                return new Size(header.bmWidth, header.bmHeight);
            }
        }

        public Bitmap Duplicate()
        {
            // Technique taken from here: http://stackoverflow.com/questions/5687263/copying-a-bitmap-from-another-hbitmap
            using (var sourceContext = GraphicsContext.CreateOffscreenContext())
            {
                using (var destContext = sourceContext.CreateCompatibleContext())
                {
                    const int SRCCOPY = 0x00CC0020;
                    var header = Header;

                    sourceContext.CurrentBitmap = this;
                    NativeMethods.BitBlt(sourceContext.Handle, 0, 0, header.bmWidth, header.bmHeight, destContext.Handle, 0, 0, SRCCOPY);
                    return destContext.CreateBitmap(header.bmWidth, header.bmHeight);
                }
            }
        }
    }
}
