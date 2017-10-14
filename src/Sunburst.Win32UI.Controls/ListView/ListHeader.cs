using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class ListHeader : Window
    {
        #region Messages
        private const uint HDM_FIRST = 0x1200;
        private const uint HDM_GETITEMCOUNT = (HDM_FIRST + 0);
        private const uint HDM_INSERTITEM = (HDM_FIRST + 10);
        private const uint HDM_DELETEITEM = (HDM_FIRST + 2);
        private const uint HDM_GETITEM = (HDM_FIRST + 11);
        private const uint HDM_SETITEM = (HDM_FIRST + 12);
        private const uint HDM_LAYOUT = (HDM_FIRST + 5);
        private const uint HDM_HITTEST = (HDM_FIRST + 6);
        private const uint HDM_GETITEMRECT = (HDM_FIRST + 7);
        private const uint HDM_SETIMAGELIST = (HDM_FIRST + 8);
        private const uint HDM_GETIMAGELIST = (HDM_FIRST + 9);
        private const uint HDM_ORDERTOINDEX = (HDM_FIRST + 15);
        private const uint HDM_CREATEDRAGIMAGE = (HDM_FIRST + 16);
        private const uint HDM_GETORDERARRAY = (HDM_FIRST + 17);
        private const uint HDM_SETORDERARRAY = (HDM_FIRST + 18);
        private const uint HDM_SETHOTDIVIDER = (HDM_FIRST + 19);
        private const uint HDM_SETBITMAPMARGIN = (HDM_FIRST + 20);
        private const uint HDM_GETBITMAPMARGIN = (HDM_FIRST + 21);
        private const uint HDM_SETFILTERCHANGETIMEOUT = (HDM_FIRST + 22);
        private const uint HDM_EDITFILTER = (HDM_FIRST + 23);
        private const uint HDM_CLEARFILTER = (HDM_FIRST + 24);
        private const uint HDM_GETITEMDROPDOWNRECT = (HDM_FIRST + 25);
        private const uint HDM_GETOVERFLOWRECT = (HDM_FIRST + 26);
        private const uint HDM_GETFOCUSEDITEM = (HDM_FIRST + 27);
        private const uint HDM_SETFOCUSEDITEM = (HDM_FIRST + 28);
        #endregion

        public const string WindowClass = "SysHeader32";
        public override string WindowClassName => WindowClass;

        public int ColumnCount => (int)SendMessage(HDM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
        public int AddColumn(ListHeaderColumn column) => InsertColumn(ColumnCount, column);

        public ListHeaderColumn GetColumn(int index)
        {
            using (StructureBuffer<HDITEM> ptr = new StructureBuffer<HDITEM>())
            {
                SendMessage(HDM_GETITEM, (IntPtr)index, ptr.Handle);
                return new ListHeaderColumn(ptr.Value);
            }
        }

        public void SetColumn(int index, ListHeaderColumn column)
        {
            using (StructureBuffer<HDITEM> ptr = new StructureBuffer<HDITEM>())
            {
                HDITEM item = column.ToNativeStruct();
                ptr.Value = item;
                SendMessage(HDM_SETITEM, (IntPtr)index, ptr.Handle);
                item.ClearFilterData();
            }
        }

        public int InsertColumn(int index, ListHeaderColumn column)
        {
            using (StructureBuffer<HDITEM> ptr = new StructureBuffer<HDITEM>())
            {
                HDITEM item = column.ToNativeStruct();
                ptr.Value = item;
                int ret = (int)SendMessage(HDM_INSERTITEM, (IntPtr)index, ptr.Handle);
                item.ClearFilterData();
                return ret;
            }
        }

        public NonOwnedImageList ImageList
        {
            get
            {
                return new NonOwnedImageList() { Handle = SendMessage(HDM_GETIMAGELIST, IntPtr.Zero, IntPtr.Zero) };
            }

            set
            {
                SendMessage(HDM_SETIMAGELIST, IntPtr.Zero, value.Handle);
            }
        }

        public IReadOnlyList<int> ColumnOrder
        {
            get
            {
                var count = ColumnCount;
                using (HGlobal buffer = new HGlobal(count * Marshal.SizeOf<int>()))
                {
                    SendMessage(HDM_GETORDERARRAY, (IntPtr)count, buffer.Handle);
                    int[] array = new int[count];
                    Marshal.Copy(buffer.Handle, array, 0, count);
                    return array;
                }
            }

            set
            {
                var count = ColumnCount;
                if (value.Count != count) throw new ArgumentException($"ColumnOrder array must have exactly {count} elements");

                using (HGlobal buffer = new HGlobal(count * Marshal.SizeOf<int>()))
                {
                    Marshal.Copy(value.ToArray(), 0, buffer.Handle, count);
                    SendMessage(HDM_SETORDERARRAY, (IntPtr)count, buffer.Handle);
                }
            }
        }

        public int OrderToIndex(int order) => (int)SendMessage(HDM_ORDERTOINDEX, (IntPtr)order, IntPtr.Zero);

        public Rect GetColumnFrame(int column)
        {
            if (column >= ColumnCount) throw new ArgumentException("Column index out of range", nameof(column));

            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                SendMessage(HDM_GETITEMRECT, (IntPtr)column, ptr.Handle);
                return ptr.Value;
            }
        }

        public int HighlightDivider(Point mouseLocation)
        {
            int combined = (mouseLocation.x & 0xFFFF) | ((mouseLocation.y & 0xFFFF) << 16);
            return (int)SendMessage(HDM_SETHOTDIVIDER, (IntPtr)1, (IntPtr)combined);
        }

        public void SetHighlightedDivider(int dividerIndex)
        {
            SendMessage(HDM_SETHOTDIVIDER, IntPtr.Zero, (IntPtr)dividerIndex);
        }

        public int BitmapMargin
        {
            get
            {
                return (int)SendMessage(HDM_GETBITMAPMARGIN, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(HDM_SETBITMAPMARGIN, (IntPtr)value, IntPtr.Zero);
            }
        }

        public void SetFilterChangeTimeout(TimeSpan timeout)
        {
            int interval = Convert.ToInt32(Math.Round(timeout.TotalMilliseconds));
            SendMessage(HDM_SETFILTERCHANGETIMEOUT, IntPtr.Zero, (IntPtr)interval);
        }

        public bool GetSplitButtonRect(int columnIndex, out Rect rect)
        {
            rect = default(Rect);

            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                bool success = (int)SendMessage(HDM_GETITEMDROPDOWNRECT, (IntPtr)columnIndex, ptr.Handle) == 1;
                if (!success) return false;

                rect = ptr.Value;
                return true;
            }
        }

        public bool GetOverflowRect(out Rect rect)
        {
            rect = default(Rect);

            using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
            {
                bool success = (int)SendMessage(HDM_GETOVERFLOWRECT, IntPtr.Zero, ptr.Handle) == 1;
                if (!success) return false;

                rect = ptr.Value;
                return true;
            }
        }

        public int FocusedColumn
        {
            get
            {
                return (int)SendMessage(HDM_GETFOCUSEDITEM, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(HDM_SETFOCUSEDITEM, IntPtr.Zero, (IntPtr)value);
            }
        }

        public void AutoLayout(Rect containerRect)
        {
            using (StructureBuffer<Rect> containerRectPtr = new StructureBuffer<Rect>())
            {
                containerRectPtr.Value = containerRect;
                HDLAYOUT layout = new HDLAYOUT();
                layout.prc = containerRectPtr.Handle;

                using (StructureBuffer<HDLAYOUT> layoutPtr = new StructureBuffer<HDLAYOUT>())
                {
                    layoutPtr.Value = layout;
                    SendMessage(HDM_LAYOUT, IntPtr.Zero, layoutPtr.Handle);

                    WINDOWPOS wpos = Marshal.PtrToStructure<WINDOWPOS>(layoutPtr.Value.pwpos);
                    wpos.Apply();
                }
            }
        }

        public int EditFilter(int column, bool discardChanges) => (int)SendMessage(HDM_EDITFILTER, (IntPtr)column, (IntPtr)(discardChanges ? 1 : 0));
        public void ClearFilter(int column) => SendMessage(HDM_CLEARFILTER, (IntPtr)column, IntPtr.Zero);
        public void ClearAllFilters() => ClearFilter(-1);
    }
}
