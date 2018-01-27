using System;
using System.Collections.Generic;
using System.Linq;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class ImageList : IDisposable
    {
        public ImageList()
        {
            Handle = IntPtr.Zero;
        }

        public ImageList(Size imageSize, int maximumCount) : this(imageSize, 0, maximumCount)
        {
        }

        public ImageList(Size imageSize, int initialCount, int maximumCount)
        {
            const uint ILC_COLOR32 = 0x20;
            Handle = NativeMethods.ImageList_Create(imageSize.width, imageSize.height, ILC_COLOR32, initialCount, maximumCount);
        }

        public void Dispose()
        {
            NativeMethods.ImageList_Destroy(Handle);
            Handle = IntPtr.Zero;
        }

        public IntPtr Handle { get; set; }

        public int ImageCount
        {
            get
            {
                return NativeMethods.ImageList_GetImageCount(Handle);
            }

            set
            {
                NativeMethods.ImageList_SetImageCount(Handle, value);
            }
        }

        public Size ImageSize
        {
            get
            {
                Size size = new Size();
                NativeMethods.ImageList_GetIconSize(Handle, out size.width, out size.height);
                return size;
            }

            set
            {
                NativeMethods.ImageList_SetIconSize(Handle, value.width, value.height);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return Color.FromWin32Color(NativeMethods.ImageList_GetBkColor(Handle));
            }

            set
            {
                NativeMethods.ImageList_SetBkColor(Handle, Color.ToWin32Color(value));
            }
        }

        public ImageListItem GetItemInfo(int index)
        {
            IMAGEINFO info = new IMAGEINFO();
            NativeMethods.ImageList_GetImageInfo(Handle, index, ref info);
            return new ImageListItem(info);
        }

        public Icon GetIcon(int index)
        {
            return new Icon(NativeMethods.ImageList_GetIcon(Handle, index, 0U));
        }

        public bool SetImageOverlay(int baseImageIndex, int overlayIndex)
        {
            return NativeMethods.ImageList_SetOverlayImage(Handle, baseImageIndex, overlayIndex);
        }

        public int AddImage(Bitmap image, Bitmap mask)
        {
            return NativeMethods.ImageList_Add(Handle, image.Handle, mask.Handle);
        }

        public int AddImage(Bitmap image, Color maskColor)
        {
            return NativeMethods.ImageList_AddMasked(Handle, image.Handle, Color.ToWin32Color(maskColor));
        }

        public int AddImage(NonOwnedIcon icon)
        {
            return NativeMethods.ImageList_ReplaceIcon(Handle, -1, icon.Handle);
        }

        public int ReplaceImage(int index, NonOwnedIcon icon)
        {
            return NativeMethods.ImageList_ReplaceIcon(Handle, index, icon.Handle);
        }

        public bool DrawImage(NonOwnedGraphicsContext dc, int index, Point location)
        {
            return NativeMethods.ImageList_Draw(Handle, index, dc.Handle, location.x, location.y, 0U);
        }

        public bool DrawImage(NonOwnedGraphicsContext dc, int index, Rect frame)
        {
            const int CLR_NONE = unchecked((int)0xFFFFFFFF);
            return NativeMethods.ImageList_DrawEx(Handle, index, dc.Handle, frame.left, frame.top, frame.Width, frame.Height, CLR_NONE, CLR_NONE, 0U);
        }

        public bool BeginDrag(int index, Point cursorLocation)
        {
            return NativeMethods.ImageList_BeginDrag(Handle, index, cursorLocation.x, cursorLocation.y);
        }

        public void EndDrag() => NativeMethods.ImageList_EndDrag();
        public bool DragMove(Point location) => NativeMethods.ImageList_DragMove(location.x, location.y);
        public bool DragEnterWindow(Control window, Point location) => NativeMethods.ImageList_DragEnter(window.Handle, location.x, location.y);
        public bool DragLeaveWindow(Control window) => NativeMethods.ImageList_DragLeave(window.Handle);
        public ImageList Duplicate() => new ImageList() { Handle = NativeMethods.ImageList_Duplicate(Handle) };
    }
}
