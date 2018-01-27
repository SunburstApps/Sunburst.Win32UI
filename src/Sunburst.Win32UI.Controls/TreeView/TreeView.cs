using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class TreeView : Control
    {
        #region Messages
        private const uint TV_FIRST = 0x1100;
        private const uint TVM_INSERTITEMA = (TV_FIRST + 0);
        private const uint TVM_INSERTITEMW = (TV_FIRST + 50);
        private const uint TVM_INSERTITEM = TVM_INSERTITEMW;
        private const uint TVM_DELETEITEM = (TV_FIRST + 1);
        private const uint TVM_EXPAND = (TV_FIRST + 2);
        private const uint TVM_GETITEMRECT = (TV_FIRST + 4);
        private const uint TVM_GETCOUNT = (TV_FIRST + 5);
        private const uint TVM_GETINDENT = (TV_FIRST + 6);
        private const uint TVM_SETINDENT = (TV_FIRST + 7);
        private const uint TVM_GETIMAGELIST = (TV_FIRST + 8);
        private const uint TVM_SETIMAGELIST = (TV_FIRST + 9);
        private const uint TVM_GETNEXTITEM = (TV_FIRST + 10);
        private const uint TVM_SELECTITEM = (TV_FIRST + 11);
        private const uint TVM_GETITEMA = (TV_FIRST + 12);
        private const uint TVM_GETITEMW = (TV_FIRST + 62);
        private const uint TVM_GETITEM = TVM_GETITEMW;
        private const uint TVM_SETITEMA = (TV_FIRST + 13);
        private const uint TVM_SETITEMW = (TV_FIRST + 63);
        private const uint TVM_SETITEM = TVM_SETITEMW;
        private const uint TVM_EDITLABELA = (TV_FIRST + 14);
        private const uint TVM_EDITLABELW = (TV_FIRST + 65);
        private const uint TVM_EDITLABEL = TVM_EDITLABELW;
        private const uint TVM_GETEDITCONTROL = (TV_FIRST + 15);
        private const uint TVM_GETVISIBLECOUNT = (TV_FIRST + 16);
        private const uint TVM_HITTEST = (TV_FIRST + 17);
        private const uint TVM_CREATEDRAGIMAGE = (TV_FIRST + 18);
        private const uint TVM_SORTCHILDREN = (TV_FIRST + 19);
        private const uint TVM_ENSUREVISIBLE = (TV_FIRST + 20);
        private const uint TVM_SORTCHILDRENCB = (TV_FIRST + 21);
        private const uint TVM_ENDEDITLABELNOW = (TV_FIRST + 22);
        private const uint TVM_GETISEARCHSTRINGA = (TV_FIRST + 23);
        private const uint TVM_GETISEARCHSTRINGW = (TV_FIRST + 64);
        private const uint TVM_GETISEARCHSTRING = TVM_GETISEARCHSTRINGW;
        private const uint TVM_SETTOOLTIPS = (TV_FIRST + 24);
        private const uint TVM_GETTOOLTIPS = (TV_FIRST + 25);
        private const uint TVM_SETINSERTMARK = (TV_FIRST + 26);
        private const uint TVM_SETUNICODEFORMAT = 0x2005;
        private const uint TVM_GETUNICODEFORMAT = 0x2006;
        private const uint TVM_SETITEMHEIGHT = (TV_FIRST + 27);
        private const uint TVM_GETITEMHEIGHT = (TV_FIRST + 28);
        private const uint TVM_SETBKCOLOR = (TV_FIRST + 29);
        private const uint TVM_SETTEXTCOLOR = (TV_FIRST + 30);
        private const uint TVM_GETBKCOLOR = (TV_FIRST + 31);
        private const uint TVM_GETTEXTCOLOR = (TV_FIRST + 32);
        private const uint TVM_SETSCROLLTIME = (TV_FIRST + 33);
        private const uint TVM_GETSCROLLTIME = (TV_FIRST + 34);
        private const uint TVM_SETINSERTMARKCOLOR = (TV_FIRST + 37);
        private const uint TVM_GETINSERTMARKCOLOR = (TV_FIRST + 38);
        private const uint TVM_SETBORDER = (TV_FIRST + 35);
        private const uint TVM_GETITEMSTATE = (TV_FIRST + 39);
        private const uint TVM_SETLINECOLOR = (TV_FIRST + 40);
        private const uint TVM_GETLINECOLOR = (TV_FIRST + 41);
        private const uint TVM_MAPACCIDTOHTREEITEM = (TV_FIRST + 42);
        private const uint TVM_MAPHTREEITEMTOACCID = (TV_FIRST + 43);
        private const uint TVM_SETEXTENDEDSTYLE = (TV_FIRST + 44);
        private const uint TVM_GETEXTENDEDSTYLE = (TV_FIRST + 45);
        private const uint TVM_SETAUTOSCROLLINFO = (TV_FIRST + 59);
        private const uint TVM_SETHOT = (TV_FIRST + 58);
        private const uint TVM_GETSELECTEDCOUNT = (TV_FIRST + 70);
        private const uint TVM_SHOWINFOTIP = (TV_FIRST + 71);
        private const uint TVM_GETITEMPARTRECT = (TV_FIRST + 72);
        #endregion

        public TreeView() : base() { }
        public TreeView(IntPtr hWnd) : base(hWnd) { }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassName = "SysTreeView32";
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.MessageId == WindowMessages.WM_CREATE)
            {
                NativeMethods.SetWindowTheme(Handle, "Explorer", null);
            }

            base.WndProc(ref m);
        }

        public uint Count
        {
            get
            {
                return (uint)SendMessage(TVM_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public uint IndentLevel
        {
            get
            {
                return (uint)SendMessage(TVM_GETINDENT, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TVM_SETINDENT, (IntPtr)value, IntPtr.Zero);
            }
        }

        public ImageList NormalImageList
        {
            get
            {
                const int TVSIL_NORMAL = 0;

                var list = new ImageList();
                list.Handle = SendMessage(TVM_GETIMAGELIST, (IntPtr)TVSIL_NORMAL, IntPtr.Zero);
                return list;
            }

            set
            {
                const int TVSIL_NORMAL = 0;
                SendMessage(TVM_SETIMAGELIST, (IntPtr)TVSIL_NORMAL, value.Handle);
            }
        }

        public ImageList SelectedImageList
        {
            get
            {
                const int TVSIL_STATE = 2;

                var list = new ImageList();
                list.Handle = SendMessage(TVM_GETIMAGELIST, (IntPtr)TVSIL_STATE, IntPtr.Zero);
                return list;
            }

            set
            {
                const int TVSIL_STATE = 2;
                SendMessage(TVM_SETIMAGELIST, (IntPtr)TVSIL_STATE, value.Handle);
            }
        }

        public TreeViewItemData GetItemData(TreeViewItemHandle itemHandle)
        {
            TVITEMEX nativeStruct = new TVITEMEX();
            nativeStruct.hTreeItem = itemHandle.Handle;
            nativeStruct.mask = TVITEMEX.TVIF_CHILDREN | TVITEMEX.TVIF_EXPANDEDIMAGE | TVITEMEX.TVIF_HANDLE |
                TVITEMEX.TVIF_IMAGE | TVITEMEX.TVIF_INTEGRAL | TVITEMEX.TVIF_SELECTEDIMAGE | TVITEMEX.TVIF_STATE |
                TVITEMEX.TVIF_STATEEX | TVITEMEX.TVIF_TEXT;
            nativeStruct.cchTextMax = (1024 * 1024);

            using (StructureBuffer<TVITEMEX> ptr = new StructureBuffer<TVITEMEX>())
            {
                ptr.Value = nativeStruct;
                SendMessage(TVM_GETITEM, IntPtr.Zero, ptr.Handle);
                return new TreeViewItemData(ptr.Value);
            }
        }

        public void SetItemData(TreeViewItemHandle itemHandle, TreeViewItemData data)
        {
            using (StructureBuffer<TVITEMEX> ptr = new StructureBuffer<TVITEMEX>())
            {
                var nativeStruct = data.ToNativeStruct();
                nativeStruct.hTreeItem = itemHandle.Handle;
                ptr.Value = nativeStruct;
                SendMessage(TVM_SETITEM, IntPtr.Zero, ptr.Handle);
            }
        }

        public TextBox TextBox
        {
            get
            {
                return new TextBox(SendMessage(TVM_GETEDITCONTROL, IntPtr.Zero, IntPtr.Zero));
            }
        }

        public int VisibleCount
        {
            get
            {
                return (int)SendMessage(TVM_GETVISIBLECOUNT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public Rect GetItemRect(TreeViewItemHandle item, bool textRectOnly)
        {
            // This one is weird. I must use an HGlobal that can hold a Rect, but it must
            // point to the value of the HTREEITEM when I send the message; it will hold
            // a valid Rect once the SendMessage() call returns.
            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                Marshal.WriteIntPtr(ptr.Handle, item.Handle);
                SendMessage(TVM_GETITEMRECT, (IntPtr)(textRectOnly ? 1 : 0), ptr.Handle);
                return ptr.Value;
            }
        }

        public ToolTip ToolTip
        {
            get
            {
                return new ToolTip(SendMessage(TVM_GETTOOLTIPS, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TVM_SETTOOLTIPS, value.Handle, IntPtr.Zero);
            }
        }

        public string IncrementalSearchString
        {
            get
            {
                int charCount = (int)SendMessage(TVM_GETISEARCHSTRINGW, IntPtr.Zero, IntPtr.Zero);
                if (charCount == 0) return null;

                using (HGlobal ptr = new HGlobal(charCount * Marshal.SystemDefaultCharSize))
                {
                    SendMessage(TVM_GETISEARCHSTRINGW, IntPtr.Zero, ptr.Handle);
                    return Marshal.PtrToStringUni(ptr.Handle);
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(TVM_GETBKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TVM_SETBKCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public Color InsertMarkColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(TVM_GETINSERTMARKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TVM_SETINSERTMARKCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public Color TextColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(TVM_GETTEXTCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TVM_SETTEXTCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public Color LineColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(TVM_GETLINECOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TVM_SETLINECOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public int ItemHeight
        {
            get
            {
                return (int)SendMessage(TVM_GETITEMHEIGHT, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TVM_SETITEMHEIGHT, (IntPtr)value, IntPtr.Zero);
            }
        }

        public TimeSpan MaximumScrollTime
        {
            get
            {
                return TimeSpan.FromMilliseconds((double)(int)SendMessage(TVM_GETSCROLLTIME, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                int span = (int)Math.Round(value.TotalMilliseconds);
                SendMessage(TVM_SETSCROLLTIME, (IntPtr)span, IntPtr.Zero);
            }
        }

        public bool SetAutoScrollParameters(uint pixelsPerSecond, uint updateTime)
        {
            return (int)SendMessage(TVM_SETAUTOSCROLLINFO, (IntPtr)(int)pixelsPerSecond, (IntPtr)(int)updateTime) != 0;
        }

        public int SelectedCount
        {
            get
            {
                return (int)SendMessage(TVM_GETSELECTEDCOUNT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public TreeViewItemHandle InsertItem(TreeViewItemHandle parentItem, TreeViewItemHandle previousItem, TreeViewItemData data)
        {
            TVINSERTSTRUCT insertStruct = new TVINSERTSTRUCT();
            insertStruct.hParent = parentItem.Handle;
            insertStruct.hInsertAfter = previousItem.Handle;
            insertStruct.item = data.ToNativeStruct();

            using (StructureBuffer<TVINSERTSTRUCT> ptr = new StructureBuffer<TVINSERTSTRUCT>())
            {
                ptr.Value = insertStruct;
                IntPtr hNewItem = SendMessage(TVM_INSERTITEMW, IntPtr.Zero, ptr.Handle);
                return new TreeViewItemHandle(hNewItem);
            }
        }

        public bool DeleteItem(TreeViewItemHandle item)
        {
            return (int)SendMessage(TVM_DELETEITEM, IntPtr.Zero, item.Handle) != 0;
        }

        public bool DeleteAllItems()
        {
            return DeleteItem(TreeViewItemHandle.Root);
        }

        public bool ExpandItem(TreeViewItemHandle item)
        {
            const int TVE_EXPAND = 0x2;
            return (int)SendMessage(TVM_EXPAND, (IntPtr)TVE_EXPAND, item.Handle) != 0;
        }

        public bool CollapseItem(TreeViewItemHandle item)
        {
            const int TVE_COLLAPSE = 0x1;
            return (int)SendMessage(TVM_EXPAND, (IntPtr)TVE_COLLAPSE, item.Handle) != 0;
        }

        public bool ToggleItemExpanded(TreeViewItemHandle item)
        {
            const int TVE_TOGGLE = 0x3;
            return (int)SendMessage(TVM_EXPAND, (IntPtr)TVE_TOGGLE, item.Handle) != 0;
        }

        public TreeViewItemHandle GetRootItem()
        {
            const int TVGN_ROOT = 0x0;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_ROOT, IntPtr.Zero));
        }

        public TreeViewItemHandle GetNextItem(TreeViewItemHandle item)
        {
            const int TVGN_NEXT = 0x1;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_NEXT, item.Handle));
        }

        public TreeViewItemHandle GetPreviousItem(TreeViewItemHandle item)
        {
            const int TVGN_PREVIOUS = 0x2;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_PREVIOUS, item.Handle));
        }

        public TreeViewItemHandle GetParentItem(TreeViewItemHandle item)
        {
            const int TVGN_PARENT = 0x3;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_PARENT, item.Handle));
        }

        public TreeViewItemHandle GetFirstChildItem(TreeViewItemHandle item)
        {
            const int TVGN_CHILD = 0x4;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_CHILD, item.Handle));
        }

        public TreeViewItemHandle GetFirstVisibleItem()
        {
            const int TVGN_FIRSTVISIBLE = 0x5;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_FIRSTVISIBLE, IntPtr.Zero));
        }

        public TreeViewItemHandle GetLastVisibleItem()
        {
            const int TVGN_LASTVISIBLE = 0xA;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_LASTVISIBLE, IntPtr.Zero));
        }

        public TreeViewItemHandle GetNextVisibleItem(TreeViewItemHandle item)
        {
            const int TVGN_NEXTVISIBLE = 0x6;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_NEXTVISIBLE, item.Handle));
        }

        public TreeViewItemHandle GetPreviousVisibleItem(TreeViewItemHandle item)
        {
            const int TVGN_PREVIOUSVISIBLE = 0x7;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_PREVIOUSVISIBLE, item.Handle));
        }

        public TreeViewItemHandle GetSelectedItem()
        {
            const int TVGN_CARET = 0x9;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_CARET, IntPtr.Zero));
        }

        public TreeViewItemHandle GetItemWithDropHighlight()
        {
            const int TVGN_DROPHILITE = 0x8;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_DROPHILITE, IntPtr.Zero));
        }

        public TreeViewItemHandle GetNextSelectedItem()
        {
            const int TVGN_NEXTSELECTED = 0xB;
            return new TreeViewItemHandle(SendMessage(TVM_GETNEXTITEM, (IntPtr)TVGN_NEXTSELECTED, IntPtr.Zero));
        }

        public bool SelectItem(TreeViewItemHandle item)
        {
            const int TVGN_CARET = 0x9;
            return (int)SendMessage(TVM_SELECTITEM, (IntPtr)TVGN_CARET, item.Handle) != 0;
        }

        public bool SetDropTarget(TreeViewItemHandle item)
        {
            const int TVGN_DROPHILITE = 0x8;
            return (int)SendMessage(TVM_SELECTITEM, (IntPtr)TVGN_DROPHILITE, item.Handle) != 0;
        }

        public bool SetFirstVisibleItem(TreeViewItemHandle item)
        {
            const int TVGN_FIRSTVISIBLE = 0x5;
            return (int)SendMessage(TVM_SELECTITEM, (IntPtr)TVGN_FIRSTVISIBLE, item.Handle) != 0;
        }

        public TextBox BeginEditItemLabel(TreeViewItemHandle item)
        {
            return new TextBox(SendMessage(TVM_EDITLABELW, IntPtr.Zero, item.Handle));
        }

        public bool EndEditItemLabel(bool discardChanges = false)
        {
            return (int)SendMessage(TVM_ENDEDITLABELNOW, (IntPtr)(discardChanges ? 1 : 0), IntPtr.Zero) != 0;
        }

        public TreeViewItemHandle HitTest(Point pt, out TreeViewHitTestLocation location, out TreeViewHitTestDirection direction)
        {
            TVHITTESTINFO hitTestInfo = new TVHITTESTINFO();
            hitTestInfo.pt = pt;
            IntPtr hItem;

            using (StructureBuffer<TVHITTESTINFO> ptr = new StructureBuffer<TVHITTESTINFO>())
            {
                ptr.Value = hitTestInfo;
                hItem = SendMessage(TVM_HITTEST, IntPtr.Zero, ptr.Handle);
                hitTestInfo = ptr.Value;
            }

            location = (TreeViewHitTestLocation)hitTestInfo.flags;
            direction = (TreeViewHitTestDirection)hitTestInfo.flags;
            return new TreeViewItemHandle(hItem);
        }

        public bool SortChildren(TreeViewItemHandle item, bool recursive = false)
        {
            return (int)SendMessage(TVM_SORTCHILDREN, (IntPtr)(recursive ? 1 : 0), item.Handle) != 0;
        }

        public bool EnsureItemIsVisible(TreeViewItemHandle item)
        {
            return (int)SendMessage(TVM_ENSUREVISIBLE, IntPtr.Zero, item.Handle) != 0;
        }

        // You must dispose the return value of this function.
        public ImageList CreateDragImage(TreeViewItemHandle item)
        {
            return new ImageList() { Handle = SendMessage(TVM_CREATEDRAGIMAGE, IntPtr.Zero, item.Handle) };
        }

        public bool SetInsertMarkBeforeItem(TreeViewItemHandle item)
        {
            return (int)SendMessage(TVM_SETINSERTMARK, (IntPtr)1, item.Handle) != 0;
        }

        public bool SetInsertMarkAfterItem(TreeViewItemHandle item)
        {
            return (int)SendMessage(TVM_SETINSERTMARK, IntPtr.Zero, item.Handle) != 0;
        }

        public bool RemoveInsertMark()
        {
            return (int)SendMessage(TVM_SETINSERTMARK, IntPtr.Zero, IntPtr.Zero) != 0;
        }

        public void ShowInfoTipForItem(TreeViewItemHandle item)
        {
            SendMessage(TVM_SHOWINFOTIP, IntPtr.Zero, item.Handle);
        }
    }

    public enum TreeViewHitTestLocation
    {
        // These must match the TVHT_* values in CommCtrl.h
        Nowhere = 0x1,
        OnItemIcon = 0x2,
        OnItemLabel = 0x4,
        OnItemIndent = 0x8,
        OnItemButton = 0x10,
        OnItemRight = 0x20,
        OnItemStateIcon = 0x40,

        OnItem = OnItemIcon | OnItemLabel | OnItemStateIcon
    }

    public enum TreeViewHitTestDirection
    {
        // These must also match the TVHT_* values in CommCtrl.h
        Above = 0x100,
        Below = 0x200,
        ToRight = 0x400,
        ToLeft = 0x800
    }
}
