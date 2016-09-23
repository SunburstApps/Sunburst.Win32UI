using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class Button : Window
    {
        #region Messages

        private const uint BM_GETCHECK = 0x00F0;
        private const uint BM_SETCHECK = 0x00F1;
        private const uint BM_GETSTATE = 0x00F2;
        private const uint BM_SETSTATE = 0x00F3;
        private const uint BM_SETSTYLE = 0x00F4;
        private const uint BM_CLICK = 0x00F5;
        private const uint BM_GETIMAGE = 0x00F6;
        private const uint BM_SETIMAGE = 0x00F7;
        private const uint BM_SETDONTCLICK = 0x00F8;
        private const uint BCM_FIRST = 0x1600;
        private const uint BCM_GETIDEALSIZE = (BCM_FIRST + 0x0001);
        private const uint BCM_SETIMAGELIST = (BCM_FIRST + 0x0002);
        private const uint BCM_GETIMAGELIST = (BCM_FIRST + 0x0003);
        private const uint BCM_SETTEXTMARGIN = (BCM_FIRST + 0x0004);
        private const uint BCM_GETTEXTMARGIN = (BCM_FIRST + 0x0005);
        private const uint BCM_SETDROPDOWNSTATE = (BCM_FIRST + 0x0006);
        private const uint BCM_SETSPLITINFO = (BCM_FIRST + 0x0007);
        private const uint BCM_GETSPLITINFO = (BCM_FIRST + 0x0008);
        private const uint BCM_SETNOTE = (BCM_FIRST + 0x0009);
        private const uint BCM_GETNOTE = (BCM_FIRST + 0x000A);
        private const uint BCM_GETNOTELENGTH = (BCM_FIRST + 0x000B);
        private const uint BCM_SETSHIELD = (BCM_FIRST + 0x000C);

        #endregion

        public override string WindowClassName => "BUTTON";

        public bool Highlighted
        {
            get
            {
                return (int)SendMessage(BM_GETSTATE, IntPtr.Zero, IntPtr.Zero) == 1;
            }

            set
            {
                SendMessage(BM_SETSTATE, (IntPtr)(value ? 1 : 0), IntPtr.Zero);
            }
        }

        public bool Checked
        {
            get
            {
                return (int)SendMessage(BM_GETCHECK, IntPtr.Zero, IntPtr.Zero) == 1;
            }

            set
            {
                SendMessage(BM_SETCHECK, (IntPtr)(value ? 1 : 0), IntPtr.Zero);
            }
        }

        public NonOwnedIcon Icon
        {
            get
            {
                return new NonOwnedIcon(SendMessage(BM_GETIMAGE, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(BM_SETIMAGE, IntPtr.Zero, value.Handle);
            }
        }

        public NonOwnedBitmap Bitmap
        {
            get
            {
                return new NonOwnedBitmap(SendMessage(BM_GETIMAGE, (IntPtr)2, IntPtr.Zero));
            }

            set
            {
                SendMessage(BM_SETIMAGE, (IntPtr)2, value?.Handle ?? IntPtr.Zero);
            }
        }

        public Size IdealSize
        {
            get
            {
                using (StructureBuffer<Size> sizePtr = new StructureBuffer<Size>())
                {
                    SendMessage(BCM_GETIDEALSIZE, IntPtr.Zero, sizePtr.Handle);
                    return sizePtr.Value;
                }
            }
        }

        public Rect TextMargin
        {
            get
            {
                using (StructureBuffer<Rect> rectPtr = new StructureBuffer<Rect>())
                {
                    SendMessage(BCM_GETTEXTMARGIN, IntPtr.Zero, rectPtr.Handle);
                    return rectPtr.Value;
                }
            }
            
            set
            {
                using (StructureBuffer<Rect> rectPtr = new StructureBuffer<Rect>())
                {
                    rectPtr.Value = value;
                    SendMessage(BCM_SETTEXTMARGIN, IntPtr.Zero, rectPtr.Handle);
                }
            }
        }

        public void SetDontClick(bool dontClick)
        {
            SendMessage(BM_SETDONTCLICK, (IntPtr)(dontClick ? 1 : 0), IntPtr.Zero);
        }

        public void SetDropDownState(bool isDropDown)
        {
            if ((GetStyle() & (CommonControlStyles.BS_DEFCOMMANDLINK | CommonControlStyles.BS_DEFCOMMANDLINK)) != 0)
                throw new InvalidOperationException($"{nameof(SetDropDownState)}() is not valid on command links");

            SendMessage(BCM_SETDROPDOWNSTATE, (IntPtr)(isDropDown ? 1 : 0), IntPtr.Zero);
        }

        public string CommandLinkNoteText
        {
            get
            {
                int length = (int)SendMessage(BCM_GETNOTELENGTH, IntPtr.Zero, IntPtr.Zero);
                using (HGlobal ptr = new HGlobal((length + 1) * Marshal.SystemDefaultCharSize))
                {
                    SendMessage(BCM_GETNOTE, (IntPtr)(length + 1), ptr.Handle);
                    return Marshal.PtrToStringUni(ptr.Handle);
                }
            }

            set
            {
                using (HGlobal ptr = HGlobal.WithStringUni(value))
                {
                    SendMessage(BCM_SETNOTE, IntPtr.Zero, ptr.Handle);
                }
            }
        }

        public void SetShowsElevationShield(bool showShield)
        {
            SendMessage(BCM_SETSHIELD, IntPtr.Zero, (IntPtr)(showShield ? 1 : 0));
        }

        public void Click()
        {
            SendMessage(BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
