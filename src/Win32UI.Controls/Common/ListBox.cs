using System;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Handles;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class ListBox : Window
    {
        #region Messages
        private const uint LB_ADDSTRING = 0x0180;
        private const uint LB_INSERTSTRING = 0x0181;
        private const uint LB_DELETESTRING = 0x0182;
        private const uint LB_SELITEMRANGEEX = 0x0183;
        private const uint LB_RESETCONTENT = 0x0184;
        private const uint LB_SETSEL = 0x0185;
        private const uint LB_SETCURSEL = 0x0186;
        private const uint LB_GETSEL = 0x0187;
        private const uint LB_GETCURSEL = 0x0188;
        private const uint LB_GETTEXT = 0x0189;
        private const uint LB_GETTEXTLEN = 0x018A;
        private const uint LB_GETCOUNT = 0x018B;
        private const uint LB_SELECTSTRING = 0x018C;
        private const uint LB_DIR = 0x018D;
        private const uint LB_GETTOPINDEX = 0x018E;
        private const uint LB_FINDSTRING = 0x018F;
        private const uint LB_GETSELCOUNT = 0x0190;
        private const uint LB_GETSELITEMS = 0x0191;
        private const uint LB_SETTABSTOPS = 0x0192;
        private const uint LB_GETHORIZONTALEXTENT = 0x0193;
        private const uint LB_SETHORIZONTALEXTENT = 0x0194;
        private const uint LB_SETCOLUMNWIDTH = 0x0195;
        private const uint LB_ADDFILE = 0x0196;
        private const uint LB_SETTOPINDEX = 0x0197;
        private const uint LB_GETITEMRECT = 0x0198;
        private const uint LB_GETITEMDATA = 0x0199;
        private const uint LB_SETITEMDATA = 0x019A;
        private const uint LB_SELITEMRANGE = 0x019B;
        private const uint LB_SETANCHORINDEX = 0x019C;
        private const uint LB_GETANCHORINDEX = 0x019D;
        private const uint LB_SETCARETINDEX = 0x019E;
        private const uint LB_GETCARETINDEX = 0x019F;
        private const uint LB_SETITEMHEIGHT = 0x01A0;
        private const uint LB_GETITEMHEIGHT = 0x01A1;
        private const uint LB_FINDSTRINGEXACT = 0x01A2;
        private const uint LB_SETLOCALE = 0x01A5;
        private const uint LB_GETLOCALE = 0x01A6;
        private const uint LB_SETCOUNT = 0x01A7;
        private const uint LB_INITSTORAGE = 0x01A8;
        private const uint LB_ITEMFROMPOINT = 0x01A9;
        private const uint LB_GETLISTBOXINFO = 0x01B2;
        #endregion

        public const string WindowClass = "LISTBOX";
        public override string WindowClassName => WindowClass;

        public void CreateHandle(Rect frame, string text, Window parent = null, IMenuHandle hMenu = null)
        {
            CreateHandle(WindowClassName, frame, text, CommonControlStyles.LBS_STANDARD, 0, parent, hMenu);
        }

        public int Count
        {
            get
            {
                return (int)SendMessage(LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                bool valid = ((GetStyle() & CommonControlStyles.LBS_NODATA) != 0) && ((GetStyle() & CommonControlStyles.LBS_HASSTRINGS) == 0);
                if (!valid) throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(Count)} setter with these window styles");
                SendMessage(LB_SETCOUNT, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int HorizontalExtent
        {
            get
            {
                return (int)SendMessage(LB_GETHORIZONTALEXTENT, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LB_SETHORIZONTALEXTENT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public int TopIndex
        {
            get
            {
                return (int)SendMessage(LB_GETTOPINDEX, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LB_SETTOPINDEX, (IntPtr)value, IntPtr.Zero);
            }
        }

        // Note: This value is not converted into a CultureInfo because CultureInfo/LCID conversion is not available on .NET Core.
        public int LCID
        {
            get
            {
                return (int)SendMessage(LB_GETLOCALE, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LB_SETLOCALE, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int CurrentSelection
        {
            get
            {
                bool valid = (GetStyle() & (CommonControlStyles.LBS_MULTIPLESEL | CommonControlStyles.LBS_EXTENDEDSEL)) == 0;
                if (!valid) throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(CurrentSelection)} with these window styles");
                return (int)SendMessage(LB_GETCURSEL, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                bool valid = (GetStyle() & (CommonControlStyles.LBS_MULTIPLESEL | CommonControlStyles.LBS_EXTENDEDSEL)) == 0;
                if (!valid) throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(CurrentSelection)} with these window styles");
                SendMessage(LB_SETCURSEL, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int GetSelection(int selectionIndex)
        {
            // Note: This method also works for single-selection list boxes.
            return (int)SendMessage(LB_GETSEL, (IntPtr)selectionIndex, IntPtr.Zero);
        }

        public void SetSelection(int selectionIndex, bool selected = true)
        {
            bool valid = (GetStyle() & (CommonControlStyles.LBS_MULTIPLESEL | CommonControlStyles.LBS_EXTENDEDSEL)) != 0;
            if (!valid) throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(SetSelection)}() with these window styles");
            SendMessage(LB_SETSEL, (IntPtr)(selected ? 1 : 0), (IntPtr)selectionIndex);
        }

        public int SelectionCount
        {
            get
            {
                bool valid = (GetStyle() & (CommonControlStyles.LBS_MULTIPLESEL | CommonControlStyles.LBS_EXTENDEDSEL)) != 0;
                if (!valid) throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(SelectionCount)} with these window styles");
                return (int)SendMessage(LB_GETSELCOUNT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public int AnchorIndex
        {
            get
            {
                bool valid = (GetStyle() & (CommonControlStyles.LBS_MULTIPLESEL | CommonControlStyles.LBS_EXTENDEDSEL)) != 0;
                if (!valid) throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(AnchorIndex)} with these window styles");
                return (int)SendMessage(LB_GETANCHORINDEX, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                bool valid = (GetStyle() & (CommonControlStyles.LBS_MULTIPLESEL | CommonControlStyles.LBS_EXTENDEDSEL)) != 0;
                if (!valid) throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(SelectionCount)} with these window styles");
                SendMessage(LB_SETANCHORINDEX, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int GetCaretIndex() => (int)SendMessage(LB_GETCARETINDEX, IntPtr.Zero, IntPtr.Zero);
        public void SetCaretIndex(int index, bool scroll = true) => SendMessage(LB_SETCARETINDEX, (IntPtr)index, (IntPtr)(scroll ? 1 : 0));

        public IntPtr GetItemData(int index) => SendMessage(LB_GETITEMDATA, (IntPtr)index, IntPtr.Zero);
        public void SetItemData(int index, IntPtr data) => SendMessage(LB_SETITEMDATA, (IntPtr)index, data);

        public Rect GetItemRect(int index)
        {
            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                SendMessage(LB_GETITEMRECT, (IntPtr)index, ptr.Handle);
                return ptr.Value;
            }
        }

        public string GetItemText(int index)
        {
            int length = (int)SendMessage(LB_GETTEXTLEN, (IntPtr)index, IntPtr.Zero);
            using (HGlobal ptr = new HGlobal(length * Marshal.SystemDefaultCharSize))
            {
                SendMessage(LB_GETTEXT, (IntPtr)index, ptr.Handle);
                return Marshal.PtrToStringUni(ptr.Handle);
            }
        }

        public int GetItemHeight(int index)
        {
            return (int)SendMessage(LB_GETITEMHEIGHT, (IntPtr)index, IntPtr.Zero);
        }

        public void SetItemHeight(int index, int height)
        {
            SendMessage(LB_SETITEMHEIGHT, (IntPtr)index, (IntPtr)height);
        }

        public void SetColumnWidth(int width) => SendMessage(LB_SETCOLUMNWIDTH, (IntPtr)width, IntPtr.Zero);

        public bool SetTabStops(int[] stops)
        {
            if ((GetStyle() & CommonControlStyles.LBS_USETABSTOPS) == 0)
                throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(SetTabStops)}() with these window styles");

            using (HGlobal buffer = new HGlobal(stops.Length * Marshal.SizeOf<int>()))
            {
                Marshal.Copy(stops, 0, buffer.Handle, stops.Length);
                return (int)SendMessage(LB_SETTABSTOPS, (IntPtr)stops.Length, buffer.Handle) == 1;
            }
        }

        public bool SetTabStops(int uniform)
        {
            if ((GetStyle() & CommonControlStyles.LBS_USETABSTOPS) == 0)
                throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(SetTabStops)}() with these window styles");

            using (HGlobal buffer = new HGlobal(Marshal.SizeOf<int>()))
            {
                Marshal.WriteInt32(buffer.Handle, uniform);
                return (int)SendMessage(LB_SETTABSTOPS, (IntPtr)1, buffer.Handle) == 1;
            }
        }

        public bool ClearTabStops()
        {
            if ((GetStyle() & CommonControlStyles.LBS_USETABSTOPS) == 0)
                throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(SetTabStops)}() with these window styles");
            return (int)SendMessage(LB_SETTABSTOPS, IntPtr.Zero, IntPtr.Zero) == 1;
        }

        public int ItemFromPoint(Point pt, out bool outside)
        {
            int lParam = (pt.x & 0xFFFF) | ((pt.y & 0xFFFF) << 16);
            int dw = (int)SendMessage(LB_ITEMFROMPOINT, IntPtr.Zero, (IntPtr)lParam);
            outside = ((dw & 0xFFFF0000) >> 16) == 1;
            return dw & 0xFFFF;
        }

        public int AddString(string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(LB_ADDSTRING, IntPtr.Zero, ptr.Handle);
        }

        public int DeleteString(int index) => (int)SendMessage(LB_DELETESTRING, (IntPtr)index, IntPtr.Zero);

        public int InsertString(int index, string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(LB_INSERTSTRING, (IntPtr)index, ptr.Handle);
        }

        public int PopulateWithWildcard(int attr, string wildcard)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(wildcard))
                return (int)SendMessage(LB_DIR, (IntPtr)attr, ptr.Handle);
        }

        public int AddFile(string filename)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(filename))
                return (int)SendMessage(LB_ADDFILE, IntPtr.Zero, ptr.Handle);
        }

        public int FindString(int startAfter, string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(LB_FINDSTRING, (IntPtr)startAfter, ptr.Handle);
        }

        public int FindExactString(int startIndex, string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(LB_FINDSTRINGEXACT, (IntPtr)startIndex, ptr.Handle);
        }

        public int SelectString(int startAfter, string item)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(item))
                return (int)SendMessage(LB_SELECTSTRING, (IntPtr)startAfter, ptr.Handle);
        }

        public int SelectRange(bool select, int firstItem, int lastItem)
        {
            if (firstItem > lastItem) throw new ArgumentException($"{nameof(firstItem)} cannot be greater than {nameof(lastItem)}");
            if ((GetStyle() & (CommonControlStyles.LBS_MULTIPLESEL | CommonControlStyles.LBS_EXTENDEDSEL)) == 0)
                throw new InvalidOperationException($"Cannot call {nameof(ListBox)}.{nameof(SetTabStops)}() with these window styles");

            if (select)
            {
                return (int)SendMessage(LB_SELITEMRANGEEX, (IntPtr)firstItem, (IntPtr)lastItem);
            }
            else
            {
                return (int)SendMessage(LB_SELITEMRANGEEX, (IntPtr)lastItem, (IntPtr)firstItem);
            }
        }
    }
}
