using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    internal static class NativeMethods
    {
        #region Resource Loading

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResourceW(IntPtr hModule, string lpType, IntPtr lpName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LockResource(IntPtr hResourceData);

        [DllImport("kernel32.dll")]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

        #endregion

        [DllImport("ole32.dll", PreserveSig = true)]
        public static extern int CreateStreamOnHGlobal(IntPtr hGlobal, bool deleteOnFree, [MarshalAs(UnmanagedType.Interface)] out IStream stream);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr iconName);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadBitmap(IntPtr hInstance, IntPtr bitmapId);

        [DllImport("user32.dll", EntryPoint = "LoadCursorW")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr cursorId);

        [DllImport("gdi32.dll")]
        public static extern void DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateFontIndirect(ref LOGFONT font_struct);

        [DllImport("gdi32.dll")]
        public static extern int GetObject(IntPtr hObject, int structSize, IntPtr structPtr);

        [DllImport("kernel32.dll")]
        public static extern int MulDiv(int number, int numer, int denom);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int penStyle, int width, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern IntPtr ExtCreatePen(int penStyle, int width, ref LOGBRUSH logBrush, int styleCount, [In, MarshalAs(UnmanagedType.LPArray)] int[] style);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int color_ref);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateHatchBrush(int index, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePatternBrush(IntPtr hBitmap);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSysColorBrush(int colorId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hDC, int index);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);

        [DllImport("gdi32.dll")]
        public static extern int GetDIBits(IntPtr hDC, IntPtr hBitmap, uint startScanline, uint scanlineCount, [Out] IntPtr bits, ref BITMAPINFO info, uint usage);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgnIndirect(ref Rect frame);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateEllipticRgnIndirect(ref Rect frame);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePolygonRgn([MarshalAs(UnmanagedType.LPArray)] Point[] points, int pointCount, int fillMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePolyPolygonRgn([MarshalAs(UnmanagedType.LPArray)] Point[] points, [MarshalAs(UnmanagedType.LPArray)] int[] polygonCounts, int pointCount, int fillMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int widthRadius, int heightRadius);

        [DllImport("gdi32.dll")]
        public static extern IntPtr PathToRegion(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int CombineRgn(IntPtr destination, IntPtr source, IntPtr otherSource, int combineMode);

        [DllImport("gdi32.dll")]
        public static extern bool EqualRgn(IntPtr leftRegion, IntPtr rightRegion);

        [DllImport("gdi32.dll")]
        public static extern int OffsetRgn(IntPtr hRegion, int x, int y);

        [DllImport("gdi32.dll")]
        public static extern int GetRgnBox(IntPtr hRegion, ref Rect box);

        [DllImport("gdi32.dll")]
        public static extern bool PtInRegion(IntPtr hRegion, int x, int y);

        [DllImport("gdi32.dll")]
        public static extern bool RectInRegion(IntPtr hRegion, ref Rect frame);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetCurrentObject(IntPtr hDC, int typeId);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int SaveDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool RestoreDC(IntPtr hDC, int saveId);

        [DllImport("gdi32.dll")]
        public static extern uint GetBoundsRect(IntPtr hDC, ref Rect rect, uint flags);

        [DllImport("gdi32.dll")]
        public static extern uint SetBoundsRect(IntPtr hDC, ref Rect rect, uint flags);

        [DllImport("gdi32.dll")]
        public static extern int GetBkColor(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int SetBkColor(IntPtr hDC, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern int GetTextColor(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int SetTextColor(IntPtr hDC, int color_ref);

        [DllImport("gdi32.dll")]
        public static extern bool FillRgn(IntPtr hDC, IntPtr hRegion, IntPtr hBrush);

        [DllImport("gdi32.dll")]
        public static extern bool FrameRgn(IntPtr hDC, IntPtr hRegion, IntPtr hBrush, int width, int height);

        [DllImport("gdi32.dll")]
        public static extern bool InvertRgn(IntPtr hDC, IntPtr hRegion);

        [DllImport("gdi32.dll")]
        public static extern int GetClipRgn(IntPtr hDC, ref IntPtr hRegion);

        [DllImport("gdi32.dll")]
        public static extern int ExtSelectClipRgn(IntPtr hDC, IntPtr hRegion, int mode);

        [DllImport("user32.dll")]
        public static extern bool FillRect(IntPtr hDC, ref Rect rect, IntPtr hBrush);

        [DllImport("user32.dll")]
        public static extern bool FrameRect(IntPtr hDC, ref Rect rect, IntPtr hBrush);

        [DllImport("gdi32.dll")]
        public static extern bool DrawIconEx(IntPtr hDC, int x, int y, IntPtr hIcon, int width, int height, uint stepForAnimatedCursor, IntPtr hBrushForNonFlicker, uint flags = 3);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hDC, int x, int y, int width, int height, IntPtr hSourceDC, int sourceX, int sourceY, int rop);

        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hDC, int x, int y, int width, int height, IntPtr hSourceDC, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int rop);

        [DllImport("gdi32.dll")]
        public static extern bool GradientFill(IntPtr hDC, [MarshalAs(UnmanagedType.LPArray)] TRIVERTEX[] points, int pointCount, ref GRADIENT_RECT rect, int gradientRectCount, int orientation);

        [DllImport("gdi32.dll")]
        public static extern bool AlphaBlend(IntPtr hDC, int x, int y, int width, int height, IntPtr hSourceDC, int sourceX, int sourceY, int sourceWidth, int sourceHeight, BLENDFUNCTION bf);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ExtTextOut(IntPtr hDC, int x, int y, uint options, ref Rect rect, string text, int textLength, [MarshalAs(UnmanagedType.LPArray)] int[] lpDx);

        [DllImport("user32.dll")]
        public static extern int DrawText(IntPtr hDC, string text, int charCount, ref Rect frame, uint formatFlags);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetTextExtentPoint32(IntPtr hDC, string text, int textLength, ref Size size);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hDC, ref BITMAPINFO bitmapInfo, int colors, out IntPtr imageBits, IntPtr hSection, int offset);

        [DllImport("gdi32.dll")]
        public static extern bool SetViewportOrgEx(IntPtr hDC, int x, int y, out Point oldOrg);

        [DllImport("user32.dll")]
        public static extern IntPtr GetCursor();

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        #region Windows Imaging Component

        public enum WICDecodeOptions
        {
            WICDecodeMetadataCacheOnDemand = 0,
            WICDecodeMetadataCacheOnLoad = 0x1,
            WICMETADATACACHEOPTION_FORCE_DWORD = 0x7fffffff
        }

        [ComImport, Guid("0000000c-0000-0000-C000-000000000046")]
        public interface IStream { }

        [ComImport, Guid("ec5ec8a9-c395-4314-9c77-54d7a935ff70")]
        public interface IWICImagingFactory
        {
            IWICBitmapDecoder CreateBitmapFromFilename(
                [MarshalAs(UnmanagedType.LPWStr)] string filename,
                [In, Optional] ref Guid pguidVendor,
                int desiredAccess,
                WICDecodeOptions options);

            IWICBitmapDecoder CreateDecoderFromStream(
                [In, MarshalAs(UnmanagedType.Interface)] IStream stream,
                [In, Optional] ref Guid pguidVendor,
                WICDecodeOptions options);
        }

        [ComImport, Guid("9EDDE9E7-8DEE-47ea-99DF-E6FAF2ED44BF")]
        public interface IWICBitmapDecoder
        {
            void QueryCapability(IStream stream, out int capability);
            void Initialize(IStream stream, WICDecodeOptions options);
            void GetContainerFormat(out Guid formatGuid);
            void GetDecoderInfo([MarshalAs(UnmanagedType.Interface)] out object decoderInfo);
            void CopyPalette([MarshalAs(UnmanagedType.Interface)] object palette);
            void GetMetadataQueryReader([MarshalAs(UnmanagedType.Interface)] out object queryReader);
            void GetPreview(out IWICBitmapSource source);
            void GetColorContexts(uint count, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface, SizeParamIndex = 0)] object[] colorContexts, out uint actualCount);
            void GetThumbnail(out IWICBitmapSource thumbnail);
            void GetFrameCount(out uint count);
            void GetFrame(uint index, out IWICBitmapFrameDecode frame);
        }

        [ComImport, Guid("00000120-a8f2-4877-ba0a-fd2b6645fb94")]
        public interface IWICBitmapSource
        {
            void GetSize(out int width, out int height);
            void GetPixelFormat(IntPtr pPixelFormat);
            void GetResolution(out double dpX, out double dpiY);
            void CopyPalette([MarshalAs(UnmanagedType.Interface)] object palette);
            void CopyPixels(IntPtr pWicRect, int stride, int bufferSize, IntPtr pbBuffer);
        }

        [ComImport, Guid("3B16811B-6A43-4ec9-A813-3D930C13B940")]
        public interface IWICBitmapFrameDecode { }

        [ComImport, Guid("317d06e8-5f24-433d-bdf7-79ce68d8abc2")]
        [ClassInterface(ClassInterfaceType.None)]
        public class WICImagingFactory { }

        #endregion
    }
}
