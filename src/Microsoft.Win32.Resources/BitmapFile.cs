using System;
using System.Runtime.InteropServices;
using System.IO;

namespace Microsoft.Win32.Resources
{
    /// <summary>
    /// A bitmap file in a .bmp format.
    /// </summary>
    public class BitmapFile
    {
        private Gdi32.BITMAPFILEHEADER _header = new Gdi32.BITMAPFILEHEADER();
        private DeviceIndependentBitmap _bitmap = null;

        /// <summary>
        /// Device independent bitmap.
        /// </summary>
        public DeviceIndependentBitmap Bitmap
        {
            get
            {
                return _bitmap;
            }
        }

        /// <summary>
        /// An existing bitmap file.
        /// </summary>
        /// <param name="filename">A file in a .bmp format.</param>
        public BitmapFile(string filename)
        {
            byte[] data = File.ReadAllBytes(filename);
            
            IntPtr pFileHeaderData = Marshal.AllocHGlobal(Marshal.SizeOf(_header));
            try
            {
                Marshal.Copy(data, 0, pFileHeaderData, Marshal.SizeOf(_header));
                _header = Marshal.PtrToStructure<Gdi32.BITMAPFILEHEADER>(pFileHeaderData);
            }
            finally
            {
                Marshal.FreeHGlobal(pFileHeaderData);
            }

            Int32 size = data.Length - Marshal.SizeOf(_header);
            byte[] bitmapData = new byte[size];
            Buffer.BlockCopy(data, Marshal.SizeOf(_header), bitmapData, 0, size);
            _bitmap = new DeviceIndependentBitmap(bitmapData);
        }
    }
}
