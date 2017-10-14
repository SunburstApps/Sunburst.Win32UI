using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class TabControl : Window
    {
        #region Messages
        private const uint TCM_FIRST = 0x1300;
        private const uint TCM_GETIMAGELIST = (TCM_FIRST + 2);
        private const uint TCM_SETIMAGELIST = (TCM_FIRST + 3);
        private const uint TCM_GETITEMCOUNT = (TCM_FIRST + 4);
        private const uint TCM_GETITEMW = (TCM_FIRST + 60);
        private const uint TCM_GETITEM = TCM_GETITEMW;
        private const uint TCM_SETITEMW = (TCM_FIRST + 61);
        private const uint TCM_SETITEM = TCM_SETITEMW;
        private const uint TCM_INSERTITEMW = (TCM_FIRST + 62);
        private const uint TCM_INSERTITEM = TCM_INSERTITEMW;
        private const uint TCM_DELETEITEM = (TCM_FIRST + 8);
        private const uint TCM_DELETEALLITEMS = (TCM_FIRST + 9);
        private const uint TCM_GETITEMRECT = (TCM_FIRST + 10);
        private const uint TCM_GETCURSEL = (TCM_FIRST + 11);
        private const uint TCM_SETCURSEL = (TCM_FIRST + 12);
        private const uint TCM_HITTEST = (TCM_FIRST + 13);
        private const uint TCM_SETITEMEXTRA = (TCM_FIRST + 14);
        private const uint TCM_ADJUSTRECT = (TCM_FIRST + 40);
        private const uint TCM_SETITEMSIZE = (TCM_FIRST + 41);
        private const uint TCM_REMOVEIMAGE = (TCM_FIRST + 42);
        private const uint TCM_SETPADDING = (TCM_FIRST + 43);
        private const uint TCM_GETROWCOUNT = (TCM_FIRST + 44);
        private const uint TCM_GETTOOLTIPS = (TCM_FIRST + 45);
        private const uint TCM_SETTOOLTIPS = (TCM_FIRST + 46);
        private const uint TCM_GETCURFOCUS = (TCM_FIRST + 47);
        private const uint TCM_SETCURFOCUS = (TCM_FIRST + 48);
        private const uint TCM_SETMINTABWIDTH = (TCM_FIRST + 49);
        private const uint TCM_DESELECTALL = (TCM_FIRST + 50);
        private const uint TCM_HIGHLIGHTITEM = (TCM_FIRST + 51);
        private const uint TCM_SETEXTENDEDSTYLE = (TCM_FIRST + 52);
        private const uint TCM_GETEXTENDEDSTYLE = (TCM_FIRST + 53);
        #endregion

        public const string WindowClass = "SysTabControl32";
        public override string WindowClassName => WindowClass;

        public NonOwnedImageList ImageList
        {
            get
            {
                return new NonOwnedImageList() { Handle = SendMessage(TCM_GETIMAGELIST, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(TCM_SETIMAGELIST, IntPtr.Zero, value.Handle);
            }
        }

        public int ItemCount => (int)SendMessage(TCM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);

        // Note: There is no GetItem() function because it is impossible to correctly
        // marshal TCITEM.pszText, as there is no way of getting the length of the string
        // to be stored there. Because of that, I cannot ensure that I am returning a
        // string that is not truncated.

        public int SetItem(int index, TabControlItem item)
        {
            TCITEM tci = new TCITEM();
            try
            {
                tci.mask = 0x3;
                tci.pszText = Marshal.StringToHGlobalUni(item.Text);
                tci.cchTextMax = item.Text.Length;
                tci.iImage = item.ImageListIndex;

                unsafe
                {
                    return (int)SendMessage(TCM_SETITEM, (IntPtr)index, new IntPtr(&tci));
                }
            }
            finally
            {
                if (tci.pszText != IntPtr.Zero) Marshal.FreeHGlobal(tci.pszText);
            }
        }

        public unsafe Rect GetItemRect(int index)
        {
            Rect retval = new Rect();
            SendMessage(TCM_GETITEMRECT, (IntPtr)index, new IntPtr(&retval));
            return retval;
        }

        public int CurrentTab
        {
            get
            {
                return (int)SendMessage(TCM_GETCURSEL, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TCM_SETCURSEL, (IntPtr)value, IntPtr.Zero);
            }
        }

        public void SetItemSize(int index, Size size)
        {
            int combined = (size.width & 0xFFFF) | ((size.height & 0xFFFF) << 16);
            SendMessage(TCM_SETITEMSIZE, (IntPtr)index, (IntPtr)combined);
        }

        public void SetPadding(Size size)
        {
            int combined = (size.width & 0xFFFF) | ((size.height & 0xFFFF) << 16);
            SendMessage(TCM_SETPADDING, IntPtr.Zero, (IntPtr)combined);
        }

        public int RowCount
        {
            get
            {
                return (int)SendMessage(TCM_GETROWCOUNT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public ToolTip ToolTip
        {
            get
            {
                return new ToolTip() { Handle = SendMessage(TCM_GETTOOLTIPS, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(TCM_SETTOOLTIPS, value.Handle, IntPtr.Zero);
            }
        }

        public int CurrentlyFocusedTab
        {
            get
            {
                return (int)SendMessage(TCM_GETCURFOCUS, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TCM_SETCURFOCUS, (IntPtr)value, IntPtr.Zero);
            }
        }

        public int SetMinimumTabWidth(int minimumWidth = -1)
        {
            return (int)SendMessage(TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)minimumWidth);
        }

        public int InsertItem(int index, TabControlItem item)
        {
            TCITEM tci = new TCITEM();
            try
            {
                tci.mask = 0x3;
                tci.pszText = Marshal.StringToHGlobalUni(item.Text);
                tci.cchTextMax = item.Text.Length;
                tci.iImage = item.ImageListIndex;

                unsafe
                {
                    return (int)SendMessage(TCM_INSERTITEM, (IntPtr)index, new IntPtr(&tci));
                }
            }
            finally
            {
                if (tci.pszText != IntPtr.Zero) Marshal.FreeHGlobal(tci.pszText);
            }
        }

        public int AddItem(TabControlItem item) => InsertItem(ItemCount, item);
        public bool DeleteItem(int index) => (int)SendMessage(TCM_DELETEITEM, (IntPtr)index, IntPtr.Zero) == 1;
        public bool DeleteAllItems() => (int)SendMessage(TCM_DELETEALLITEMS, IntPtr.Zero, IntPtr.Zero) == 1;

        public unsafe Rect DisplayArea
        {
            get
            {
                Rect rc = WindowRect;
                SendMessage(TCM_ADJUSTRECT, IntPtr.Zero, new IntPtr(&rc));
                return rc;
            }
        }
    }

    public sealed class TabControlItem
    {
        // Set this to -1 if no icon should be displayed.
        public int ImageListIndex { get; set; } = -1;
        public string Text { get; set; } = "";
    }
}
