using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class ListView : Window
    {
        #region Messages
        private const uint LVM_FIRST = 0x1000;
        private const uint LVM_GETBKCOLOR = (LVM_FIRST + 0);
        private const uint LVM_SETBKCOLOR = (LVM_FIRST + 1);
        private const uint LVM_GETIMAGELIST = (LVM_FIRST + 2);
        private const uint LVM_SETIMAGELIST = (LVM_FIRST + 3);
        private const uint LVM_GETITEMCOUNT = (LVM_FIRST + 4);
        private const uint LVM_GETITEMA = (LVM_FIRST + 5);
        private const uint LVM_GETITEMW = (LVM_FIRST + 75);
        private const uint LVM_GETITEM = LVM_GETITEMW;
        private const uint LVM_SETITEMA = (LVM_FIRST + 6);
        private const uint LVM_SETITEMW = (LVM_FIRST + 76);
        private const uint LVM_SETITEM = LVM_SETITEMW;
        private const uint LVM_INSERTITEMA = (LVM_FIRST + 7);
        private const uint LVM_INSERTITEMW = (LVM_FIRST + 77);
        private const uint LVM_INSERTITEM = LVM_INSERTITEMW;
        private const uint LVM_DELETEITEM = (LVM_FIRST + 8);
        private const uint LVM_DELETEALLITEMS = (LVM_FIRST + 9);
        private const uint LVM_GETCALLBACKMASK = (LVM_FIRST + 10);
        private const uint LVM_SETCALLBACKMASK = (LVM_FIRST + 11);
        private const uint LVM_GETNEXTITEM = (LVM_FIRST + 12);
        private const uint LVM_FINDITEMA = (LVM_FIRST + 13);
        private const uint LVM_FINDITEMW = (LVM_FIRST + 83);
        private const uint LVM_FINDITEM = LVM_FINDITEMW;
        private const uint LVM_GETITEMRECT = (LVM_FIRST + 14);
        private const uint LVM_SETITEMPOSITION = (LVM_FIRST + 15);
        private const uint LVM_GETITEMPOSITION = (LVM_FIRST + 16);
        private const uint LVM_GETSTRINGWIDTHA = (LVM_FIRST + 17);
        private const uint LVM_GETSTRINGWIDTHW = (LVM_FIRST + 87);
        private const uint LVM_GETSTRINGWIDTH = LVM_GETSTRINGWIDTHW;
        private const uint LVM_HITTEST = (LVM_FIRST + 18);
        private const uint LVM_ENSUREVISIBLE = (LVM_FIRST + 19);
        private const uint LVM_SCROLL = (LVM_FIRST + 20);
        private const uint LVM_REDRAWITEMS = (LVM_FIRST + 21);
        private const uint LVM_ARRANGE = (LVM_FIRST + 22);
        private const uint LVM_EDITLABELA = (LVM_FIRST + 23);
        private const uint LVM_EDITLABELW = (LVM_FIRST + 118);
        private const uint LVM_EDITLABEL = LVM_EDITLABELW;
        private const uint LVM_GETEDITCONTROL = (LVM_FIRST + 24);
        private const uint LVM_GETCOLUMNA = (LVM_FIRST + 25);
        private const uint LVM_GETCOLUMNW = (LVM_FIRST + 95);
        private const uint LVM_GETCOLUMN = LVM_GETCOLUMNW;
        private const uint LVM_SETCOLUMNA = (LVM_FIRST + 26);
        private const uint LVM_SETCOLUMNW = (LVM_FIRST + 96);
        private const uint LVM_SETCOLUMN = LVM_SETCOLUMNW;
        private const uint LVM_INSERTCOLUMNA = (LVM_FIRST + 27);
        private const uint LVM_INSERTCOLUMNW = (LVM_FIRST + 97);
        private const uint LVM_DELETECOLUMN = (LVM_FIRST + 28);
        private const uint LVM_GETCOLUMNWIDTH = (LVM_FIRST + 29);
        private const uint LVM_SETCOLUMNWIDTH = (LVM_FIRST + 30);
        private const uint LVM_GETHEADER = (LVM_FIRST + 31);
        private const uint LVM_CREATEDRAGIMAGE = (LVM_FIRST + 33);
        private const uint LVM_GETVIEWRECT = (LVM_FIRST + 34);
        private const uint LVM_GETTEXTCOLOR = (LVM_FIRST + 35);
        private const uint LVM_SETTEXTCOLOR = (LVM_FIRST + 36);
        private const uint LVM_GETTEXTBKCOLOR = (LVM_FIRST + 37);
        private const uint LVM_SETTEXTBKCOLOR = (LVM_FIRST + 38);
        private const uint LVM_GETTOPINDEX = (LVM_FIRST + 39);
        private const uint LVM_GETCOUNTPERPAGE = (LVM_FIRST + 40);
        private const uint LVM_GETORIGIN = (LVM_FIRST + 41);
        private const uint LVM_UPDATE = (LVM_FIRST + 42);
        private const uint LVM_SETITEMSTATE = (LVM_FIRST + 43);
        private const uint LVM_GETITEMSTATE = (LVM_FIRST + 44);
        private const uint LVM_GETITEMTEXTA = (LVM_FIRST + 45);
        private const uint LVM_GETITEMTEXTW = (LVM_FIRST + 115);
        private const uint LVM_GETITEMTEXT = LVM_GETITEMTEXTW;
        private const uint LVM_SETITEMTEXTA = (LVM_FIRST + 46);
        private const uint LVM_SETITEMTEXTW = (LVM_FIRST + 116);
        private const uint LVM_SETITEMTEXT = LVM_SETITEMTEXTW;
        private const uint LVM_SETITEMCOUNT = (LVM_FIRST + 47);
        private const uint LVM_SORTITEMS = (LVM_FIRST + 48);
        private const uint LVM_SETITEMPOSITION32 = (LVM_FIRST + 49);
        private const uint LVM_GETSELECTEDCOUNT = (LVM_FIRST + 50);
        private const uint LVM_GETITEMSPACING = (LVM_FIRST + 51);
        private const uint LVM_GETISEARCHSTRINGA = (LVM_FIRST + 52);
        private const uint LVM_GETISEARCHSTRINGW = (LVM_FIRST + 117);
        private const uint LVM_GETISEARCHSTRING = LVM_GETISEARCHSTRINGW;
        private const uint LVM_SETICONSPACING = (LVM_FIRST + 53);
        private const uint LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54);
        private const uint LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55);
        private const uint LVM_GETSUBITEMRECT = (LVM_FIRST + 56);
        private const uint LVM_SUBITEMHITTEST = (LVM_FIRST + 57);
        private const uint LVM_SETCOLUMNORDERARRAY = (LVM_FIRST + 58);
        private const uint LVM_GETCOLUMNORDERARRAY = (LVM_FIRST + 59);
        private const uint LVM_SETHOTITEM = (LVM_FIRST + 60);
        private const uint LVM_GETHOTITEM = (LVM_FIRST + 61);
        private const uint LVM_SETHOTCURSOR = (LVM_FIRST + 62);
        private const uint LVM_GETHOTCURSOR = (LVM_FIRST + 63);
        private const uint LVM_APPROXIMATEVIEWRECT = (LVM_FIRST + 64);
        private const uint LVM_SETWORKAREAS = (LVM_FIRST + 65);
        private const uint LVM_GETWORKAREAS = (LVM_FIRST + 70);
        private const uint LVM_GETNUMBEROFWORKAREAS = (LVM_FIRST + 73);
        private const uint LVM_GETSELECTIONMARK = (LVM_FIRST + 66);
        private const uint LVM_SETSELECTIONMARK = (LVM_FIRST + 67);
        private const uint LVM_SETHOVERTIME = (LVM_FIRST + 71);
        private const uint LVM_GETHOVERTIME = (LVM_FIRST + 72);
        private const uint LVM_SETTOOLTIPS = (LVM_FIRST + 74);
        private const uint LVM_GETTOOLTIPS = (LVM_FIRST + 78);
        private const uint LVM_SORTITEMSEX = (LVM_FIRST + 81);
        private const uint LVM_SETBKIMAGEA = (LVM_FIRST + 68);
        private const uint LVM_SETBKIMAGEW = (LVM_FIRST + 138);
        private const uint LVM_GETBKIMAGEA = (LVM_FIRST + 69);
        private const uint LVM_GETBKIMAGEW = (LVM_FIRST + 139);
        private const uint LVM_SETSELECTEDCOLUMN = (LVM_FIRST + 140);
        private const uint LVM_SETVIEW = (LVM_FIRST + 142);
        private const uint LVM_GETVIEW = (LVM_FIRST + 143);
        private const uint LVM_INSERTGROUP = (LVM_FIRST + 145);
        private const uint LVM_SETGROUPINFO = (LVM_FIRST + 147);
        private const uint LVM_GETGROUPINFO = (LVM_FIRST + 149);
        private const uint LVM_REMOVEGROUP = (LVM_FIRST + 150);
        private const uint LVM_MOVEGROUP = (LVM_FIRST + 151);
        private const uint LVM_GETGROUPCOUNT = (LVM_FIRST + 152);
        private const uint LVM_GETGROUPINFOBYINDEX = (LVM_FIRST + 153);
        private const uint LVM_MOVEITEMTOGROUP = (LVM_FIRST + 154);
        private const uint LVM_GETGROUPRECT = (LVM_FIRST + 98);
        private const uint LVM_SETGROUPMETRICS = (LVM_FIRST + 155);
        private const uint LVM_GETGROUPMETRICS = (LVM_FIRST + 156);
        private const uint LVM_ENABLEGROUPVIEW = (LVM_FIRST + 157);
        private const uint LVM_SORTGROUPS = (LVM_FIRST + 158);
        private const uint LVM_INSERTGROUPSORTED = (LVM_FIRST + 159);
        private const uint LVM_REMOVEALLGROUPS = (LVM_FIRST + 160);
        private const uint LVM_HASGROUP = (LVM_FIRST + 161);
        private const uint LVM_GETGROUPSTATE = (LVM_FIRST + 92);
        private const uint LVM_GETFOCUSEDGROUP = (LVM_FIRST + 93);
        private const uint LVM_SETTILEVIEWINFO = (LVM_FIRST + 162);
        private const uint LVM_GETTILEVIEWINFO = (LVM_FIRST + 163);
        private const uint LVM_SETTILEINFO = (LVM_FIRST + 164);
        private const uint LVM_GETTILEINFO = (LVM_FIRST + 165);
        private const uint LVM_SETINSERTMARK = (LVM_FIRST + 166);
        private const uint LVM_GETINSERTMARK = (LVM_FIRST + 167);
        private const uint LVM_INSERTMARKHITTEST = (LVM_FIRST + 168);
        private const uint LVM_GETINSERTMARKRECT = (LVM_FIRST + 169);
        private const uint LVM_SETINSERTMARKCOLOR = (LVM_FIRST + 170);
        private const uint LVM_GETINSERTMARKCOLOR = (LVM_FIRST + 171);
        private const uint LVM_SETINFOTIP = (LVM_FIRST + 173);
        private const uint LVM_GETSELECTEDCOLUMN = (LVM_FIRST + 174);
        private const uint LVM_ISGROUPVIEWENABLED = (LVM_FIRST + 175);
        private const uint LVM_GETOUTLINECOLOR = (LVM_FIRST + 176);
        private const uint LVM_SETOUTLINECOLOR = (LVM_FIRST + 177);
        private const uint LVM_CANCELEDITLABEL = (LVM_FIRST + 179);
        private const uint LVM_MAPINDEXTOID = (LVM_FIRST + 180);
        private const uint LVM_MAPIDTOINDEX = (LVM_FIRST + 181);
        private const uint LVM_ISITEMVISIBLE = (LVM_FIRST + 182);
        private const uint LVM_GETEMPTYTEXT = (LVM_FIRST + 204);
        private const uint LVM_GETFOOTERRECT = (LVM_FIRST + 205);
        private const uint LVM_GETFOOTERINFO = (LVM_FIRST + 206);
        private const uint LVM_GETFOOTERITEMRECT = (LVM_FIRST + 207);
        private const uint LVM_GETFOOTERITEM = (LVM_FIRST + 208);
        private const uint LVM_GETITEMINDEXRECT = (LVM_FIRST + 209);
        private const uint LVM_SETITEMINDEXSTATE = (LVM_FIRST + 210);
        private const uint LVM_GETNEXTITEMINDEX = (LVM_FIRST + 211);
        private const uint LVM_SETBKIMAGE = LVM_SETBKIMAGEW;
        private const uint LVM_GETBKIMAGE = LVM_GETBKIMAGEW;
        #endregion

        public ListView() { }
        public ListView(IntPtr hWnd) : base(hWnd) { }
        public const string WindowClass = "SysListView32";
        public override string WindowClassName => WindowClassName;

        public Color BackgroundColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(LVM_GETBKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(LVM_SETBKCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public Color TextColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(LVM_GETTEXTCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(LVM_SETTEXTCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public Color TextBackgroundColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(LVM_GETTEXTBKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(LVM_SETTEXTBKCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public void SetTheme() => NativeMethods.SetWindowTheme(Handle, "Explorer", null);

        public NonOwnedImageList GetImageList(ListViewImageListType type)
        {
            return new NonOwnedImageList() { Handle = SendMessage(LVM_GETIMAGELIST, (IntPtr)(int)type, IntPtr.Zero) };
        }

        public void SetImageList(ListViewImageListType type, NonOwnedImageList list)
        {
            SendMessage(LVM_SETIMAGELIST, (IntPtr)(int)type, list.Handle);
        }

        public void RemoveImageList(ListViewImageListType type) => SendMessage(LVM_SETIMAGELIST, (IntPtr)(int)type, IntPtr.Zero);

        public int ItemCount
        {
            get
            {
                return (int)SendMessage(LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LVM_SETITEMCOUNT, (IntPtr)value, IntPtr.Zero);
            }
        }

        public unsafe bool GetItem(ref LVITEM item)
        {
            LVITEM tempItem = item;
            bool ok = (int)SendMessage(LVM_GETITEM, IntPtr.Zero, new IntPtr(&tempItem)) == 1;
            item = tempItem;
            return ok;
        }

        public unsafe bool SetItem(LVITEM item)
        {
            return (int)SendMessage(LVM_SETITEM, IntPtr.Zero, new IntPtr(&item)) == 1;
        }

        public unsafe string GetItemText(int item, int subItem)
        {
            LVITEM lvi = new LVITEM();
            lvi.iItem = item;
            lvi.iSubItem = subItem;

            for (int length = 256; ; length *= 2)
            {
                try
                {
                    lvi.cchTextMax = length;
                    lvi.lpszText = Marshal.AllocHGlobal(length * Marshal.SystemDefaultCharSize);
                    if (lvi.lpszText == IntPtr.Zero) throw new OutOfMemoryException();

                    int size = (int)SendMessage(LVM_GETITEMTEXT, (IntPtr)item, new IntPtr(&lvi));
                    if (size < (length - 1))
                    {
                        string retval = Marshal.PtrToStringUni(lvi.lpszText);
                        Marshal.FreeHGlobal(lvi.lpszText);
                        return retval;
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(lvi.lpszText);
                }
            }
        }

        public void SetItemText(int item, int subItem, string text)
        {
            LVITEM lvi = new LVITEM();
            lvi.iItem = item;
            lvi.iSubItem = subItem;
            lvi.mask = LVITEM.LVIF_TEXT;

            try
            {
                lvi.lpszText = Marshal.StringToHGlobalUni(text);
                SetItem(lvi);
            }
            finally
            {
                Marshal.FreeHGlobal(lvi.lpszText);
            }
        }

        public IntPtr GetItemData(int item)
        {
            LVITEM lvi = new LVITEM();
            lvi.mask = LVITEM.LVIF_PARAM;
            lvi.iItem = item;

            bool ok = GetItem(ref lvi);
            return ok ? lvi.lpszText : IntPtr.Zero;
        }

        public bool SetItemData(int item, IntPtr data)
        {
            LVITEM lvi = new LVITEM();
            lvi.mask = LVITEM.LVIF_PARAM;
            lvi.iItem = item;
            lvi.lpszText = data;

            return SetItem(lvi);
        }

        // The values for this are the LVIS_* constants in the LVITEM struct.
        public uint StateCallbackMask
        {
            get
            {
                return (uint)(int)SendMessage(LVM_GETCALLBACKMASK, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LVM_SETCALLBACKMASK, (IntPtr)(int)value, IntPtr.Zero);
            }
        }

        public unsafe bool GetItemPosition(int item, out Point pos)
        {
            Point point = default(Point);
            bool success = (int)SendMessage(LVM_GETITEMPOSITION, (IntPtr)item, new IntPtr(&point)) == 1;
            pos = point;
            return success;
        }

        public unsafe bool SetItemPosition(int item, Point pos)
        {
            bool iconView = ((GetStyle() & ListViewStyles.LVS_TYPEMASK) == ListViewStyles.LVS_ICON || (GetStyle() & ListViewStyles.LVS_TYPEMASK) == ListViewStyles.LVS_SMALLICON);
            if (!iconView) throw new InvalidOperationException("You can only set the position of an item in Icon or Small Icon view.");

            return (int)SendMessage(LVM_SETITEMPOSITION32, (IntPtr)item, new IntPtr(&pos)) == 1;
        }

        public TextBox GetEditControl() => new TextBox() { Handle = SendMessage(LVM_GETEDITCONTROL, IntPtr.Zero, IntPtr.Zero) };
        public ListHeader GetHeader() => new ListHeader() { Handle = SendMessage(LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero) };

        public unsafe bool GetColumn(int index, ref LVCOLUMN column)
        {
            LVCOLUMN tempColumn = column;
            bool ret = (int)SendMessage(LVM_GETCOLUMN, (IntPtr)index, new IntPtr(&tempColumn)) == 1;
            column = tempColumn;
            return ret;
        }

        public unsafe bool SetColumn(int index, LVCOLUMN column)
        {
            return (int)SendMessage(LVM_SETCOLUMN, (IntPtr)index, new IntPtr(&column)) == 1;
        }

        public int GetColumnWidth(int column) => (int)SendMessage(LVM_GETCOLUMNWIDTH, (IntPtr)column, IntPtr.Zero);
        public bool SetColumnWidth(int column, int width) => (int)SendMessage(LVM_SETCOLUMNWIDTH, (IntPtr)column, (IntPtr)width) == 1;
        public int TopIndex => (int)SendMessage(LVM_GETTOPINDEX, IntPtr.Zero, IntPtr.Zero);
        public int CountPerPage => (int)SendMessage(LVM_GETCOUNTPERPAGE, IntPtr.Zero, IntPtr.Zero);
        public int SelectedCount => (int)SendMessage(LVM_GETSELECTEDCOUNT, IntPtr.Zero, IntPtr.Zero);

        public unsafe Rect ViewRect
        {
            get
            {
                Rect rc = new Rect();
                bool ok = (int)SendMessage(LVM_GETVIEWRECT, IntPtr.Zero, new IntPtr(&rc)) == 1;
                if (!ok) throw new System.ComponentModel.Win32Exception();
                return rc;
            }
        }

        public unsafe Point Origin
        {
            get
            {
                Point point = new Point();
                bool success = (int)SendMessage(LVM_GETORIGIN, IntPtr.Zero, new IntPtr(&point)) == 1;
                if (!success) throw new System.ComponentModel.Win32Exception();
                return point;
            }
        }

        public unsafe Rect GetItemRect(int item, ListViewItemRectType type)
        {
            Rect rc = new Rect();
            rc.left = (int)type;
            bool ok = (int)SendMessage(LVM_GETITEMRECT, (IntPtr)item, new IntPtr(&rc)) == 1;
            if (!ok) throw new System.ComponentModel.Win32Exception();
            return rc;
        }

        public Cursor HotCursor
        {
            get
            {
                return new Cursor(SendMessage(LVM_GETHOTCURSOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(LVM_SETHOTCURSOR, IntPtr.Zero, value.Handle);
            }
        }

        public int HotItem
        {
            get
            {
                return (int)SendMessage(LVM_GETHOTITEM, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LVM_SETHOTITEM, IntPtr.Zero, (IntPtr)value);
            }
        }

        public IReadOnlyList<int> GetColumnOrder(int columnCount)
        {
            using (HGlobal ptr = new HGlobal(columnCount * Marshal.SizeOf<int>()))
            {
                bool ok = (int)SendMessage(LVM_GETCOLUMNORDERARRAY, (IntPtr)columnCount, ptr.Handle) == 1;
                if (!ok) throw new System.ComponentModel.Win32Exception();

                int[] block = new int[columnCount];
                Marshal.Copy(ptr.Handle, block, 0, columnCount);
                return block;
            }
        }

        public bool SetColumnOrder(IReadOnlyList<int> order)
        {
            using (HGlobal ptr = new HGlobal(order.Count * Marshal.SizeOf<int>()))
            {
                Marshal.Copy(order.ToArray(), 0, ptr.Handle, order.Count);
                return (int)SendMessage(LVM_SETCOLUMNORDERARRAY, (IntPtr)order.Count, ptr.Handle) == 1;
            }
        }

        public unsafe Rect GetSubItemRect(int item, int subItem, ListViewSubItemRectType type)
        {
            if ((GetStyle() & ListViewStyles.LVS_TYPEMASK) != ListViewStyles.LVS_REPORT)
                throw new InvalidOperationException($"You can only call {nameof(GetSubItemRect)} when in report (details) view.");

            Rect rc = new Rect();
            rc.left = (int)type;
            rc.top = subItem;

            bool ok = (int)SendMessage(LVM_GETSUBITEMRECT, (IntPtr)item, new IntPtr(&rc)) == 1;
            if (!ok) throw new System.ComponentModel.Win32Exception();
            return rc;
        }

        public void SetIconSpacing(Size spacing)
        {
            var typemask = GetStyle() & ListViewStyles.LVS_TYPEMASK;
            if (typemask != ListViewStyles.LVS_ICON && typemask != ListViewStyles.LVS_SMALLICON)
                throw new InvalidOleVariantTypeException($"You can only call {nameof(SetIconSpacing)} when in icon or small-icon view.");

            int combined = (spacing.width & 0xFFFF) | ((spacing.height & 0xFFFF) << 16);
            SendMessage(LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)combined);
        }

        public Size GetSmallIconViewIconSpacing()
        {
            int combined = (int)SendMessage(LVM_GETITEMSPACING, (IntPtr)1, IntPtr.Zero);
            return new Size(combined & 0xFFFF, (combined >> 16) & 0xFFFF);
        }

        public Size GetLargeIconViewIconSpacing()
        {
            int combined = (int)SendMessage(LVM_GETITEMSPACING, IntPtr.Zero, IntPtr.Zero);
            return new Size(combined & 0xFFFF, (combined >> 16) & 0xFFFF);
        }

        public int SelectedIndex
        {
            get
            {
                if ((GetStyle() & ListViewStyles.LVS_SINGLESEL) == 0)
                    throw new InvalidOperationException($"{nameof(SelectedIndex)} can be called only on ListView instances configured as single-selection.");

                return (int)SendMessage(LVM_GETNEXTITEM, (IntPtr)(-1), (IntPtr)(int)(ListViewDirection.All | ListViewDirection.IsSelected));
            }
        }

        public bool GetSelectedItem(ref LVITEM lvi)
        {
            int item = SelectedIndex;
            if (item == -1) return false;
            lvi.iItem = item;
            return GetItem(ref lvi);
        }

        public bool GetItemChecked(int index)
        {
            if ((GetExtendedStyle() & ListViewStyles.LVS_EX_CHECKBOXES) == 0)
                throw new InvalidOperationException($"{nameof(GetItemChecked)}() can only be called on ListView instances with {nameof(ListViewStyles.LVS_EX_CHECKBOXES)}.");

            int state = (int)SendMessage(LVM_GETITEMSTATE, (IntPtr)index, (IntPtr)(int)LVITEM.LVIS_STATEIMAGEMASK);
            return ((state >> 12) - 1) != 0;
        }

        public void SetItemChecked(int index, bool is_checked)
        {
            int nCheck = is_checked ? 2 : 1; // one-based index
            SendMessage(LVM_SETITEMSTATE, (IntPtr)(nCheck << 12), (IntPtr)(int)LVITEM.LVIS_STATEIMAGEMASK);
        }

        public int ViewType
        {
            get
            {
                return GetStyle() & ListViewStyles.LVS_TYPEMASK;
            }

            set
            {
                int oldType = GetStyle() & ListViewStyles.LVS_TYPEMASK;
                if (value != oldType)
                {
                    int style = GetStyle();
                    style &= ~ListViewStyles.LVS_TYPEMASK;
                    style |= value;
                    SetStyle(style);
                    Invalidate();
                }
            }
        }

        public unsafe ListViewBackground BackgroundImage
        {
            get
            {
                LVBKIMAGE image = new LVBKIMAGE();
                SendMessage(LVM_GETBKIMAGE, IntPtr.Zero, new IntPtr(&image));
                return new ListViewBackground(image);
            }

            set
            {
                // IMPORTANT NOTE: Even though I am using an "owned" bitmap here,
                // I must not Dispose() it, for ownership of the underlying handle
                // is assumed by the control. Disposing it now will result in a crash
                // later, due to referencing freed memory.
                Bitmap bmp = value.Bitmap.Duplicate();
                LVBKIMAGE image = value.ToNativeStruct();
                image.hbm = bmp.Handle;
                bool success = (int)SendMessage(LVM_SETBKIMAGE, IntPtr.Zero, new IntPtr(&image)) == 1;

                // However, that being said, ownership is only taken if the call succeeds.
                // If it failed, we must call Dispose() ourselves to avoid leaking memory.
                if (!success) bmp.Dispose();
            }
        }

        public int MultipleSelectionStartIndex
        {
            get
            {
                return (int)SendMessage(LVM_GETSELECTIONMARK, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LVM_SETSELECTIONMARK, IntPtr.Zero, (IntPtr)value);
            }
        }

        public IReadOnlyList<Rect> GetWorkAreas()
        {
            int workAreaCount = WorkAreaCount;
            if (workAreaCount == -1) throw new System.ComponentModel.Win32Exception($"{WorkAreaCount} failed");

            using (HGlobal ptr = new HGlobal(workAreaCount * Marshal.SizeOf<Rect>()))
            {
                bool success = (int)SendMessage(LVM_GETWORKAREAS, (IntPtr)workAreaCount, ptr.Handle) == 1;
                if (!success) return null;

                List<Rect> list = new List<Rect>();
                for (int i = 0; i < workAreaCount; i++)
                {
                    list.Add(Marshal.PtrToStructure<Rect>(ptr.Handle + (i * Marshal.SizeOf<Rect>())));
                }

                return list;
            }
        }

        public unsafe bool SetWorkAreas(IReadOnlyList<Rect> workAreas)
        {
            Rect[] rectArray = workAreas.ToArray();
            fixed (Rect* rectPtr = rectArray)
            {
                return (int)SendMessage(LVM_SETWORKAREAS, (IntPtr)workAreas.Count, new IntPtr(rectPtr)) == 1;
            }
        }

        public unsafe int WorkAreaCount
        {
            get
            {
                int count = 0;
                bool success = (int)SendMessage(LVM_GETNUMBEROFWORKAREAS, IntPtr.Zero, new IntPtr(&count)) == 0;
                return success ? count : -1;
            }
        }

        public int HoverTime
        {
            get
            {
                bool supported = (GetExtendedStyle() & (ListViewStyles.LVS_EX_TRACKSELECT | ListViewStyles.LVS_EX_ONECLICKACTIVATE | ListViewStyles.LVS_EX_TWOCLICKACTIVATE)) != 0;
                if (!supported) throw new InvalidCastException($"{nameof(HoverTime)} not supported with these styles");

                return (int)SendMessage(LVM_GETHOVERTIME, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                bool supported = (GetExtendedStyle() & (ListViewStyles.LVS_EX_TRACKSELECT | ListViewStyles.LVS_EX_ONECLICKACTIVATE | ListViewStyles.LVS_EX_TWOCLICKACTIVATE)) != 0;
                if (!supported) throw new InvalidCastException($"{nameof(HoverTime)} not supported with these styles");

                SendMessage(LVM_SETHOVERTIME, IntPtr.Zero, (IntPtr)value);
            }
        }

        public ToolTip ToolTip
        {
            get
            {
                return new ToolTip() { Handle = SendMessage(LVM_GETTOOLTIPS, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(LVM_SETTOOLTIPS, value.Handle, IntPtr.Zero);
            }
        }

        public int SelectedColumn
        {
            get
            {
                return (int)SendMessage(LVM_GETSELECTEDCOLUMN, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LVM_SETSELECTEDCOLUMN, (IntPtr)value, IntPtr.Zero);
            }
        }

        public ListViewView CurrentView
        {
            get
            {
                return (ListViewView)(int)SendMessage(LVM_GETVIEW, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(LVM_SETVIEW, (IntPtr)(int)value, IntPtr.Zero);
            }
        }

        public bool IsGroupViewEnabled => (int)SendMessage(LVM_ISGROUPVIEWENABLED, IntPtr.Zero, IntPtr.Zero) == 1;

        public unsafe int GetGroupInfo(int groupId, ref LVGROUP groupInfo)
        {
            LVGROUP tempGroup = groupInfo;
            int ret = (int)SendMessage(LVM_GETGROUPINFO, (IntPtr)groupId, new IntPtr(&tempGroup));
            groupInfo = tempGroup;
            return ret;
        }

        public unsafe int SetGroupInfo(int groupId, LVGROUP groupInfo)
        {
            return (int)SendMessage(LVM_SETGROUPINFO, (IntPtr)groupId, new IntPtr(&groupInfo));
        }

        public unsafe void GetGroupMetrics(int groupId, ref LVGROUPMETRICS metrics)
        {
            LVGROUPMETRICS tempMetrics = metrics;
            SendMessage(LVM_GETGROUPMETRICS, (IntPtr)groupId, new IntPtr(&tempMetrics));
            metrics = tempMetrics;
        }

        public unsafe void SetGroupMetrics(int groupId, LVGROUPMETRICS metrics)
        {
            SendMessage(LVM_SETGROUPMETRICS, (IntPtr)groupId, new IntPtr(&metrics));
        }

        public unsafe LVTILEVIEWINFO TileViewInfo
        {
            get
            {
                LVTILEVIEWINFO info = new LVTILEVIEWINFO();
                SendMessage(LVM_GETTILEVIEWINFO, IntPtr.Zero, new IntPtr(&info));
                return info;
            }

            set
            {
                SendMessage(LVM_SETTILEVIEWINFO, IntPtr.Zero, new IntPtr(&value));
            }
        }

        public unsafe LVTILEINFO TileInfo
        {
            get
            {
                LVTILEINFO info = new LVTILEINFO();
                SendMessage(LVM_GETTILEINFO, IntPtr.Zero, new IntPtr(&info));
                return info;
            }

            set
            {
                SendMessage(LVM_SETTILEINFO, IntPtr.Zero, new IntPtr(&value));
            }
        }

        public unsafe bool GetInsertMark(out int location, out bool displayedAfter)
        {
            LVINSERTMARK mark = new LVINSERTMARK();
            mark.cbSize = Convert.ToUInt32(Marshal.SizeOf<LVINSERTMARK>());
            bool success = (int)SendMessage(LVM_GETINSERTMARK, IntPtr.Zero, new IntPtr(&mark)) == 1;
            location = mark.iItem;
            displayedAfter = (mark.dwFlags & LVINSERTMARK.LVIM_AFTER) == LVINSERTMARK.LVIM_AFTER;
            return success;
        }

        public unsafe bool SetInsertMark(int location, bool displayAfter)
        {
            LVINSERTMARK mark = new LVINSERTMARK();
            mark.cbSize = Convert.ToUInt32(Marshal.SizeOf<LVINSERTMARK>());
            mark.iItem = location;
            mark.dwFlags = displayAfter ? LVINSERTMARK.LVIM_AFTER : 0;
            return (int)SendMessage(LVM_SETINSERTMARK, IntPtr.Zero, new IntPtr(&mark)) == 1;
        }

        public unsafe Rect InsertMarkRect
        {
            get
            {
                Rect rc = new Rect();
                SendMessage(LVM_GETINSERTMARKRECT, IntPtr.Zero, new IntPtr(&rc));
                return rc;
            }
        }

        public Color InsertMarkColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(LVM_GETINSERTMARKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(LVM_SETINSERTMARKCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public Color OutlineColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(LVM_GETOUTLINECOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(LVM_SETOUTLINECOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public int GroupCount => (int)SendMessage(LVM_GETGROUPCOUNT, IntPtr.Zero, IntPtr.Zero);

        public unsafe int GetGroupInfoByIndex(int index, ref LVGROUP group)
        {
            LVGROUP tempGroup = group;
            int retval = (int)SendMessage(LVM_GETGROUPINFOBYINDEX, (IntPtr)index, new IntPtr(&tempGroup));
            group = tempGroup;
            return retval;
        }

        public unsafe int InsertGroup(int item, LVGROUP group) => (int)SendMessage(LVM_INSERTGROUP, (IntPtr)item, new IntPtr(&group));
        public int AddGroup(LVGROUP group) => InsertGroup(-1, group);
        public void RemoveAllGroups() => SendMessage(LVM_REMOVEALLGROUPS, IntPtr.Zero, IntPtr.Zero);
        public int RemoveGroup(int groupId) => (int)SendMessage(LVM_REMOVEGROUP, (IntPtr)groupId, IntPtr.Zero);
        public void MoveGroup(int groupId, int item) => SendMessage(LVM_MOVEGROUP, (IntPtr)groupId, (IntPtr)item);
        public void MoveItemToGroup(int item, int groupId) => SendMessage(LVM_MOVEITEMTOGROUP, (IntPtr)groupId, (IntPtr)item);
        public int EnableGroupView(bool enable) => (int)SendMessage(LVM_ENABLEGROUPVIEW, (IntPtr)(enable ? 1 : 0), IntPtr.Zero);
        public bool HasGroup(int groupId) => (int)SendMessage(LVM_HASGROUP, (IntPtr)groupId, IntPtr.Zero) == 1;

        public unsafe Rect GetGroupRect(int groupId, ListViewGroupRectType type)
        {
            Rect rc = new Rect();
            rc.top = (int)type;
            bool success = (int)SendMessage(LVM_GETGROUPRECT, (IntPtr)groupId, new IntPtr(&rc)) == 1;
            if (!success) throw new System.ComponentModel.Win32Exception();
            else return rc;
        }

        public unsafe string GetEmptyText(int maxLength)
        {
            using (HGlobal ptr = new HGlobal(maxLength * Marshal.SystemDefaultCharSize))
            {
                bool success = (int)SendMessage(LVM_GETEMPTYTEXT, ptr.Handle, (IntPtr)maxLength) == 1;
                return success ? Marshal.PtrToStringUni(ptr.Handle) : null;
            }
        }

        public unsafe Rect FooterRect
        {
            get
            {
                Rect rc = new Rect();
                bool success = (int)SendMessage(LVM_GETFOOTERRECT, IntPtr.Zero, new IntPtr(&rc)) == 1;
                if (!success) throw new System.ComponentModel.Win32Exception();
                else return rc;
            }
        }

        public unsafe int FooterItemCount
        {
            get
            {
                LVFOOTERINFO info = new LVFOOTERINFO();
                info.mask = LVFOOTERINFO.LVFF_ITEMCOUNT;
                SendMessage(LVM_GETFOOTERINFO, IntPtr.Zero, new IntPtr(&info));
                return info.cItems;
            }
        }

        public unsafe Rect GetFooterItemRect(int index)
        {
            Rect rc = new Rect();
            bool success = (int)SendMessage(LVM_GETFOOTERITEMRECT, (IntPtr)index, new IntPtr(&rc)) == 1;
            if (!success) throw new System.ComponentModel.Win32Exception();
            else return rc;
        }

        public unsafe bool GetFooterItem(int item, ref LVFOOTERITEM itemInfo)
        {
            LVFOOTERITEM tempInfo = itemInfo;
            bool ret = (int)SendMessage(LVM_GETFOOTERITEM, (IntPtr)item, new IntPtr(&tempInfo)) == 1;
            itemInfo = tempInfo;
            return ret;
        }

        public unsafe int InsertColumn(int columnIndex, LVCOLUMN column)
        {
            return (int)SendMessage(LVM_INSERTCOLUMNW, (IntPtr)columnIndex, new IntPtr(&column));
        }

        public bool DeleteColumn(int columnIndex) => (int)SendMessage(LVM_DELETECOLUMN, (IntPtr)columnIndex, IntPtr.Zero) == 1;
        public unsafe int InsertItem(LVITEM item) => (int)SendMessage(LVM_INSERTITEM, IntPtr.Zero, new IntPtr(&item));
        public bool DeleteItem(int itemIndex) => (int)SendMessage(LVM_DELETEITEM, (IntPtr)itemIndex, IntPtr.Zero) == 1;
        public bool DeleteAllItems() => (int)SendMessage(LVM_DELETEALLITEMS, IntPtr.Zero, IntPtr.Zero) == 1;

        public int FindItem(string itemToFind, bool allowPartialMatch = true, bool wrapAround = false, int start = -1)
        {
            LVFINDINFO findInfo = new LVFINDINFO();
            findInfo.flags = LVFINDINFO.LVFI_STRING | (wrapAround ? LVFINDINFO.LVFI_WRAP : 0) | (allowPartialMatch ? LVFINDINFO.LVFI_PARTIAL : 0);
            findInfo.psz = itemToFind;

            using (StructureBuffer<LVFINDINFO> ptr = new StructureBuffer<LVFINDINFO>())
            {
                ptr.Value = findInfo;
                return (int)SendMessage(LVM_FINDITEM, (IntPtr)start, ptr.Handle);
            }
        }

        public unsafe LVHITTESTINFO HitTest(Point pt)
        {
            LVHITTESTINFO info = new LVHITTESTINFO();
            info.pt.x = pt.x; info.pt.y = pt.y;
            SendMessage(LVM_HITTEST, (IntPtr)(-1), new IntPtr(&info));
            return info;
        }

        public bool EnsureItemVisible(int item, bool allowPartialVisibility)
        {
            return (int)SendMessage(LVM_ENSUREVISIBLE, (IntPtr)item, (IntPtr)(allowPartialVisibility ? 1 : 0)) == 1;
        }

        public bool Scroll(Size scrollSize) => (int)SendMessage(LVM_SCROLL, (IntPtr)scrollSize.width, (IntPtr)scrollSize.height) == 1;
        public bool Redraw(Range itemRange) => (int)SendMessage(LVM_REDRAWITEMS, (IntPtr)itemRange.Location, (IntPtr)itemRange.MaxValue) == 1;
        public void SnapItemsToGrid() => SendMessage(LVM_ARRANGE, (IntPtr)0x5, IntPtr.Zero);
        public bool UpdateItem(int item) => (int)SendMessage(LVM_UPDATE, (IntPtr)item, IntPtr.Zero) == 1;
        public unsafe ImageList CreateDragImage(int item, Point pt) => new ImageList() { Handle = SendMessage(LVM_CREATEDRAGIMAGE, (IntPtr)item, new IntPtr(&pt)) };
        public TextBox EditLabel(int item) => new TextBox() { Handle = SendMessage(LVM_EDITLABEL, (IntPtr)item, IntPtr.Zero) };
        public void CancelEditLabel() => SendMessage(LVM_CANCELEDITLABEL, IntPtr.Zero, IntPtr.Zero);
        
        public Size EstimateDisplaySize(int itemCount = -1) => EstimateDisplaySize(itemCount, new Point(ClientRect.Width, ClientRect.Height));
        public Size EstimateDisplaySize(int itemCount, Point position)
        {
            int combined = (position.x & 0xFFFF) | ((position.y & 0xFFFF) << 16);
            int result = (int)SendMessage(LVM_APPROXIMATEVIEWRECT, (IntPtr)ItemCount, (IntPtr)combined);
            return new Size(result & 0xFFFF, (result >> 16) & 0xFFFF);
        }

        public bool SetInfoTip(int item, int subItem, string text)
        {
            LVSETINFOTIP tipStruct = new LVSETINFOTIP();
            tipStruct.cbSize = Convert.ToUInt32(Marshal.SizeOf<LVSETINFOTIP>());
            tipStruct.dwFlags = 0;
            tipStruct.pszText = text;
            tipStruct.iItem = item;
            tipStruct.iSubItem = subItem;

            using (StructureBuffer<LVSETINFOTIP> tipPtr = new StructureBuffer<LVSETINFOTIP>())
            {
                tipPtr.Value = tipStruct;
                return (int)SendMessage(LVM_SETINFOTIP, IntPtr.Zero, tipPtr.Handle) == 1;
            }
        }

        public int MapIndexToID(int index) => (int)SendMessage(LVM_MAPINDEXTOID, (IntPtr)index, IntPtr.Zero);
        public int MapIDToIndex(int id) => (int)SendMessage(LVM_MAPIDTOINDEX, (IntPtr)id, IntPtr.Zero);

        public unsafe bool SetItemState(int item, uint state, uint stateMask)
        {
            LVITEM lvi = new LVITEM();
            lvi.state = state;
            lvi.stateMask = stateMask;
            return (int)SendMessage(LVM_SETITEMSTATE, (IntPtr)item, new IntPtr(&lvi)) == 1;
        }

        public bool SelectItem(int index)
        {
            if ((GetStyle() & ListViewStyles.LVS_SINGLESEL) == 0) SetItemState(-1, 0, LVITEM.LVIS_SELECTED);
            bool ret = SetItemState(index, LVITEM.LVIS_SELECTED | LVITEM.LVIS_FOCUSED, LVITEM.LVIS_SELECTED | LVITEM.LVIS_FOCUSED);
            if (ret) EnsureItemVisible(index, false);
            return ret;
        }
    }

    public sealed class ListViewBackground
    {
        public ListViewBackground() { }
        internal ListViewBackground(LVBKIMAGE nativeStruct)
        {
            if ((nativeStruct.ulFlags & LVBKIMAGE.LVBKIF_SOURCE_HBITMAP) == 0)
                throw new InvalidOperationException($"{nameof(ListViewBackground)} only supports ${nameof(LVBKIMAGE)} structures that contain an HBITMAP");

            Bitmap = new NonOwnedBitmap(nativeStruct.hbm);
            Tile = (nativeStruct.ulFlags & LVBKIMAGE.LVBKIF_STYLE_TILE) != 0;
            BitmapIsWatermark = (nativeStruct.ulFlags & LVBKIMAGE.LVBKIF_TYPE_WATERMARK) != 0;
            HorizontalOffset = nativeStruct.xOffsetPercent;
            VerticalOffset = nativeStruct.yOffsetPercent;
        }

        internal LVBKIMAGE ToNativeStruct()
        {
            // IMPORTANT NOTE: This method does *NOT* copy over the Bitmap, as ownership of its memory will
            // be claimed by the list view control. You must create -- and then "leak" -- an "owned" Bitmap
            // for the control to function correctly, and your app to not crash due to referencing freed memory.
            LVBKIMAGE nativeStruct = new LVBKIMAGE();
            nativeStruct.ulFlags |= LVBKIMAGE.LVBKIF_SOURCE_HBITMAP | LVBKIMAGE.LVBKIF_FLAG_ALPHABLEND;
            if (Tile) nativeStruct.ulFlags |= LVBKIMAGE.LVBKIF_FLAG_TILEOFFSET | LVBKIMAGE.LVBKIF_FLAG_TILEOFFSET;
            if (BitmapIsWatermark) nativeStruct.ulFlags |= LVBKIMAGE.LVBKIF_TYPE_WATERMARK;
            nativeStruct.xOffsetPercent = HorizontalOffset;
            nativeStruct.yOffsetPercent = VerticalOffset;
            return nativeStruct;
        }

        public NonOwnedBitmap Bitmap { get; set; } = null;
        public bool Tile { get; set; } = false;
        public bool BitmapIsWatermark { get; set; } = false;
        public int HorizontalOffset { get; set; } = 0; // In pixels if Tile is true, in percent (0-100 left-to-right) if it is false
        public int VerticalOffset { get; set; } = 0; // In pixels if Tile is true, in percent (0-100 top-to-bottom) if it is false
    }

    public enum ListViewImageListType
    {
        Normal = 0,
        Small = 1,
        State = 2,
        GroupHeader = 3
    }

    public enum ListViewItemRectType
    {
        // These must match the LVIR_* values in CommCtrl.h
        Bounds = 0,
        Icon = 1,
        Label = 2,
        BoundsExceptColumns = 3
    }

    public enum ListViewSubItemRectType
    {
        EntireItem = 0,
        IconOnly = 1
    }

    [Flags]
    public enum ListViewDirection
    {
        // These must match the LVNI_* values in CommCtrl.h
        All = 0,
        IsFocused = 1,
        IsSelected = 2,
        IsCut = 4,
        IsDropTarget = 8,

        UseVisibleOrder = 16,
        FindPrevious = 32,
        FindVisibleOnly = 64,
        FindInSameGroupOnly = 128,

        Above = 256,
        Below = 512,
        ToLeft = 1024,
        ToRight = 2048
    }

    public enum ListViewView
    {
        Icon = 0,
        Details = 1,
        SmallIcon = 2,
        List = 3,
        Tile = 4
    }

    public enum ListViewGroupRectType
    {
        EntireGroup = 0,
        HeaderOnly = 1,
        LabelOnly = 2,
        SubsetLink = 3
    }
}
