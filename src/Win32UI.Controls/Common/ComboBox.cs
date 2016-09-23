using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Handles;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class ComboBox : Window
    {
        #region Messages
        private const uint CB_GETEDITSEL = 0x0140;
        private const uint CB_LIMITTEXT = 0x0141;
        private const uint CB_SETEDITSEL = 0x0142;
        private const uint CB_ADDSTRING = 0x0143;
        private const uint CB_DELETESTRING = 0x0144;
        private const uint CB_DIR = 0x0145;
        private const uint CB_GETCOUNT = 0x0146;
        private const uint CB_GETCURSEL = 0x0147;
        private const uint CB_GETLBTEXT = 0x0148;
        private const uint CB_GETLBTEXTLEN = 0x0149;
        private const uint CB_INSERTSTRING = 0x014A;
        private const uint CB_RESETCONTENT = 0x014B;
        private const uint CB_FINDSTRING = 0x014C;
        private const uint CB_SELECTSTRING = 0x014D;
        private const uint CB_SETCURSEL = 0x014E;
        private const uint CB_SHOWDROPDOWN = 0x014F;
        private const uint CB_GETITEMDATA = 0x0150;
        private const uint CB_SETITEMDATA = 0x0151;
        private const uint CB_GETDROPPEDCONTROLRECT = 0x0152;
        private const uint CB_SETITEMHEIGHT = 0x0153;
        private const uint CB_GETITEMHEIGHT = 0x0154;
        private const uint CB_SETEXTENDEDUI = 0x0155;
        private const uint CB_GETEXTENDEDUI = 0x0156;
        private const uint CB_GETDROPPEDSTATE = 0x0157;
        private const uint CB_FINDSTRINGEXACT = 0x0158;
        private const uint CB_SETLOCALE = 0x0159;
        private const uint CB_GETLOCALE = 0x015A;
        private const uint CB_GETTOPINDEX = 0x015b;
        private const uint CB_SETTOPINDEX = 0x015c;
        private const uint CB_GETHORIZONTALEXTENT = 0x015d;
        private const uint CB_SETHORIZONTALEXTENT = 0x015e;
        private const uint CB_GETDROPPEDWIDTH = 0x015f;
        private const uint CB_SETDROPPEDWIDTH = 0x0160;
        private const uint CB_INITSTORAGE = 0x0161;
        private const uint CB_MULTIPLEADDSTRING = 0x0163;
        private const uint CB_GETCOMBOBOXINFO = 0x0164;
        private const uint CB_SETMINVISIBLE       = 0x1701;
        private const uint CB_GETMINVISIBLE       = 0x1702;  
        private const uint CB_SETCUEBANNER        = 0x1703;
        private const uint CB_GETCUEBANNER        = 0x1704;
        #endregion

        public const string WindowClass = "COMBOBOX";
        public override string WindowClassName => WindowClass;

        public int Count => (int)SendMessage(CB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);

        public int SelectedRow
        {
            get
            {
                return (int)SendMessage(CB_GETCURSEL, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(CB_SETCURSEL, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int LCID
        {
            get
            {
                return (int)SendMessage(CB_GETLOCALE, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(CB_SETLOCALE, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int TopIndex
        {
            get
            {
                return (int)SendMessage(CB_GETTOPINDEX, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(CB_SETTOPINDEX, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int HorizontalExtent
        {
            get
            {
                return (int)SendMessage(CB_GETHORIZONTALEXTENT, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(CB_SETHORIZONTALEXTENT, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int DroppedWidth
        {
            get
            {
                return (int)SendMessage(CB_GETDROPPEDWIDTH, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(CB_SETDROPPEDWIDTH, (IntPtr)value, IntPtr.Zero);
            }
        }

        public Range EditControlSelectedRange
        {
            get
            {
                int combined = (int)SendMessage(CB_GETEDITSEL, IntPtr.Zero, IntPtr.Zero);
                return new Range(Convert.ToUInt32(combined & 0xFFFF), Convert.ToUInt32((combined & 0xFFFF0000) >> 16));
            }

            set
            {
                uint combined = (value.Location & 0xFFFF) | ((value.MaxValue & 0xFFFF) << 16);
                SendMessage(CB_SETEDITSEL, IntPtr.Zero, (IntPtr)combined);
            }
        }

        public IntPtr GetItemData(int index) => SendMessage(CB_GETITEMDATA, (IntPtr)index, IntPtr.Zero);
        public void SetItemData(int index, IntPtr data) => SendMessage(CB_SETITEMDATA, (IntPtr)index, data);

        // TODO: Figure out what "LB" stands for
        public string GetItemText(int index)
        {
            int length = (int)SendMessage(CB_GETLBTEXTLEN, (IntPtr)index, IntPtr.Zero);
            using (HGlobal ptr = new HGlobal(length * Marshal.SystemDefaultCharSize))
            {
                SendMessage(CB_GETLBTEXT, (IntPtr)index, ptr.Handle);
                return Marshal.PtrToStringUni(ptr.Handle);
            }
        }

        public int GetItemHeight(int index) => (int)SendMessage(CB_GETITEMHEIGHT, (IntPtr)index, IntPtr.Zero);
        public void SetItemHeight(int index, int height) => SendMessage(CB_SETITEMHEIGHT, (IntPtr)index, (IntPtr)height);

        public bool UsesExtendedUI
        {
            get
            {
                return (int)SendMessage(CB_GETEXTENDEDUI, IntPtr.Zero, IntPtr.Zero) == 1;
            }

            set
            {
                SendMessage(CB_SETEXTENDEDUI, (IntPtr)(value ? 1 : 0), IntPtr.Zero);
            }
        }

        public Rect DroppedControlRect
        {
            get
            {
                using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
                {
                    SendMessage(CB_GETDROPPEDCONTROLRECT, IntPtr.Zero, ptr.Handle);
                    return ptr.Value;
                }
            }
        }

        public int MinimumVisibleItems
        {
            get
            {
                return (int)SendMessage(CB_GETMINVISIBLE, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(CB_SETMINVISIBLE, (IntPtr)value, IntPtr.Zero);
            }
        }

        public void SetCueBannerText(string text)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(text))
                SendMessage(CB_SETCUEBANNER, IntPtr.Zero, ptr.Handle);
        }

        public bool SetTextLengthLimit(int charCount) => (int)SendMessage(CB_LIMITTEXT, (IntPtr)charCount, IntPtr.Zero) == 1;
        public void ShowDropDown(bool show) => SendMessage(CB_SHOWDROPDOWN, (IntPtr)(show ? 1 : 0), IntPtr.Zero);

        public int AddString(string text)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(text))
                return (int)SendMessage(CB_ADDSTRING, IntPtr.Zero, ptr.Handle);
        }

        public int InsertString(int index, string text)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(text))
                return (int)SendMessage(CB_INSERTSTRING, (IntPtr)index, ptr.Handle);
        }

        public int PopulateWithWildcard(int attr, string wildcard)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(wildcard))
                return (int)SendMessage(CB_DIR, (IntPtr)attr, ptr.Handle);
        }

        public int FindString(int startAfter, string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(CB_FINDSTRING, (IntPtr)startAfter, ptr.Handle);
        }

        public int FindExactString(int startIndex, string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(CB_FINDSTRINGEXACT, (IntPtr)startIndex, ptr.Handle);
        }

        public int SelectString(int startAfter, string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(CB_SELECTSTRING, (IntPtr)startAfter, ptr.Handle);
        }

        public void Clear() => SendMessage(WindowMessages.WM_CLEAR, IntPtr.Zero, IntPtr.Zero);
        public void Cut() => SendMessage(WindowMessages.WM_CUT, IntPtr.Zero, IntPtr.Zero);
        public void Copy() => SendMessage(WindowMessages.WM_COPY, IntPtr.Zero, IntPtr.Zero);
        public void Paste() => SendMessage(WindowMessages.WM_PASTE, IntPtr.Zero, IntPtr.Zero);
    }
}
