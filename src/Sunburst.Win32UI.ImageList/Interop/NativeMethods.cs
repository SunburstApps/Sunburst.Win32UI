using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    internal static class NativeMethods
    {
        [DllImport("comctl32.dll")]
        public static extern int ImageList_GetImageCount(IntPtr hImageList);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_GetBkColor(IntPtr hImageList);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_SetBkColor(IntPtr hImageList, int cr);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_GetImageInfo(IntPtr hImageList, int i, ref IMAGEINFO info);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_GetIcon(IntPtr hImageList, int i, uint flags);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_GetIconSize(IntPtr hImageList, out int cx, out int cy);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_SetIconSize(IntPtr hImageList, int cx, int cy);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_SetImageCount(IntPtr hImageList, int count);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_SetOverlayImage(IntPtr hImageList, int nImage, int nOverlay);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_Destroy(IntPtr hImageList);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_Create(int cx, int cy, uint flags, int cInitial, int cGrow);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_Add(IntPtr hImageList, IntPtr hBitmap, IntPtr hBitmapMask);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_AddMasked(IntPtr hImageList, IntPtr hBitmap, int crMask);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_ReplaceIcon(IntPtr hImageList, int nImage, IntPtr hIcon);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_GetIcon(IntPtr hImageList, int nImage, int flags);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_Draw(IntPtr hImageList, int image, IntPtr hDC, int x, int y, uint style);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DrawEx(IntPtr hImageList, int index, IntPtr hDC, int x, int y, int width, int height, int backColor, int foreColor, uint style);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_BeginDrag(IntPtr hImageList, int index, int hotspotX, int hotspotY);

        [DllImport("comctl32.dll")]
        public static extern void ImageList_EndDrag();

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragMove(int x, int y);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragEnter(IntPtr hWnd, int x, int y);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragLeave(IntPtr hWnd);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_Duplicate(IntPtr hImageList);
    }
}
