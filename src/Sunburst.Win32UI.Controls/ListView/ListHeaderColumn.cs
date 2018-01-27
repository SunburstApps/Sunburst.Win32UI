using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public sealed class ListHeaderColumn
    {
        public ListHeaderColumn() { }

        internal ListHeaderColumn(HDITEM nativeItem)
        {
            Width = nativeItem.cxy;
            Text = nativeItem.pszText;
            Bitmap = (nativeItem.hbm != IntPtr.Zero) ? new Bitmap(nativeItem.hbm) : null;

            if ((nativeItem.fmt & HDITEM.HDF_CENTER) == HDITEM.HDF_CENTER) TextAlignment = TextAlignment.Center;
            else if ((nativeItem.fmt & HDITEM.HDF_RIGHT) == HDITEM.HDF_RIGHT) TextAlignment = TextAlignment.Right;
            else if ((nativeItem.fmt & HDITEM.HDF_LEFT) == HDITEM.HDF_LEFT) TextAlignment = TextAlignment.Left;

            ShowsBitmap = (nativeItem.fmt & HDITEM.HDF_BITMAP) == HDITEM.HDF_BITMAP;
            BitmapOnRight = (nativeItem.fmt & HDITEM.HDF_BITMAP_ON_RIGHT) == HDITEM.HDF_BITMAP_ON_RIGHT;

            if ((nativeItem.fmt & HDITEM.HDF_SORTUP) == HDITEM.HDF_SORTUP) SortOrder = ListHeaderColumnSortOrder.Up;
            else if ((nativeItem.fmt & HDITEM.HDF_SORTDOWN) == HDITEM.HDF_SORTUP) SortOrder = ListHeaderColumnSortOrder.Down;
            else SortOrder = ListHeaderColumnSortOrder.None;

            HasCheckbox = (nativeItem.fmt & HDITEM.HDF_CHECKBOX) == HDITEM.HDF_CHECKBOX;
            IsChecked = (nativeItem.fmt & HDITEM.HDF_CHECKED) == HDITEM.HDF_CHECKED;
            Resizable = !((nativeItem.fmt & HDITEM.HDF_FIXEDWIDTH) == HDITEM.HDF_FIXEDWIDTH);
            HasSplitButton = (nativeItem.fmt & HDITEM.HDF_SPLITBUTTON) == HDITEM.HDF_SPLITBUTTON;
            HasKeyboardFocus = (nativeItem.state & HDITEM.HDIS_FOCUSED) == HDITEM.HDIS_FOCUSED;

            Index = nativeItem.iOrder;

            if (nativeItem.filterType == HDITEM.HDFT_ISSTRING)
            {
                FilterKind = ListHeaderColumnFilterKind.StringData;
                HD_TEXTFILTERW filter = Marshal.PtrToStructure<HD_TEXTFILTERW>(nativeItem.pvFilter);
                FilterText = filter.pszText;
            }
            else if (nativeItem.filterType == HDITEM.HDFT_ISNUMBER)
            {
                FilterKind = ListHeaderColumnFilterKind.NumericData;
                FilterNumeric = (int)nativeItem.pvFilter;
            }
            else if (nativeItem.filterType == HDITEM.HDFT_ISDATE)
            {
                FilterKind = ListHeaderColumnFilterKind.Date;
                FilterDate = Marshal.PtrToStructure<SYSTEMTIME>(nativeItem.pvFilter).ToDateTime();
            }
            else
            {
                FilterKind = ListHeaderColumnFilterKind.None;
            }
        }

        internal HDITEM ToNativeStruct()
        {
            HDITEM item = new HDITEM();
            item.mask = HDITEM.HDI_WIDTH | HDITEM.HDI_TEXT | HDITEM.HDI_FORMAT | HDITEM.HDI_BITMAP | HDITEM.HDI_ORDER | HDITEM.HDI_FILTER | HDITEM.HDI_STATE;
            item.cxy = Width;
            item.pszText = Text;
            item.cchTextMax = Text.Length;
            item.iOrder = Index;
            item.hbm = Bitmap?.Handle ?? IntPtr.Zero;

            if (ShowsBitmap) item.fmt |= HDITEM.HDF_BITMAP;
            if (BitmapOnRight) item.fmt |= HDITEM.HDF_BITMAP_ON_RIGHT;
            if (HasCheckbox) item.fmt |= HDITEM.HDF_CHECKBOX;
            if (IsChecked) item.fmt |= HDITEM.HDF_CHECKED;
            if (!Resizable) item.fmt |= HDITEM.HDF_FIXEDWIDTH;
            if (HasSplitButton) item.fmt |= HDITEM.HDF_SPLITBUTTON;
            if (HasKeyboardFocus) item.state |= HDITEM.HDIS_FOCUSED;

            switch (TextAlignment)
            {
                case TextAlignment.Left: item.fmt |= HDITEM.HDF_LEFT; break;
                case TextAlignment.Center: item.fmt |= HDITEM.HDF_CENTER; break;
                case TextAlignment.Right: item.fmt |= HDITEM.HDF_RIGHT; break;
            }

            switch (SortOrder)
            {
                case ListHeaderColumnSortOrder.None: break; // no flag for this
                case ListHeaderColumnSortOrder.Up: item.fmt |= HDITEM.HDF_SORTUP; break;
                case ListHeaderColumnSortOrder.Down: item.fmt |= HDITEM.HDF_SORTDOWN; break;
            }

            switch (FilterKind)
            {
                case ListHeaderColumnFilterKind.None:
                    item.filterType = HDITEM.HDFT_HASNOVALUE;
                    break;
                case ListHeaderColumnFilterKind.StringData:
                    HD_TEXTFILTERW filterData = new HD_TEXTFILTERW();
                    filterData.pszText = FilterText;
                    filterData.cchTextMax = FilterText.Length;
                    item.filterType = HDITEM.HDFT_ISSTRING;
                    item.pvFilter = Marshal.AllocHGlobal(Marshal.SizeOf<HD_TEXTFILTERW>());
                    Marshal.StructureToPtr(filterData, item.pvFilter, false);
                    break;
                case ListHeaderColumnFilterKind.NumericData:
                    item.filterType = HDITEM.HDFT_ISNUMBER;
                    item.pvFilter = (IntPtr)FilterNumeric;
                    break;
                case ListHeaderColumnFilterKind.Date:
                    SYSTEMTIME time = new SYSTEMTIME(FilterDate);
                    item.filterType = HDITEM.HDFT_ISDATE;
                    item.pvFilter = Marshal.AllocHGlobal(Marshal.SizeOf<SYSTEMTIME>());
                    Marshal.StructureToPtr(time, item.pvFilter, false);
                    break;
            }

            return item;
        }

        public int Width { get; set; } = 100;
        public string Text { get; set; } = "";
        public bool ShowsBitmap { get; set; } = false;
        public Bitmap Bitmap { get; set; } = null;
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
        public bool BitmapOnRight { get; set; } = false;
        public ListHeaderColumnSortOrder SortOrder { get; set; } = ListHeaderColumnSortOrder.None;
        public bool HasCheckbox { get; set; } = false;
        public bool IsChecked { get; set; } = false;
        public bool Resizable { get; set; } = true;
        public bool HasSplitButton { get; set; } = false;
        public int Index { get; set; } = 0;
        public bool HasKeyboardFocus { get; set; } = false;
        public ListHeaderColumnFilterKind FilterKind { get; set; } = ListHeaderColumnFilterKind.None;
        public string FilterText { get; set; } = null; // ignored if FilterKind != StringData
        public long FilterNumeric { get; set; } = 0; // ignored if FilterKind != NumericData
        public DateTime FilterDate { get; set; } = default(DateTime); // ignored if FilterKind != Date
    }

    public enum ListHeaderColumnSortOrder
    {
        None,
        Up,
        Down
    }

    public enum ListHeaderColumnFilterKind
    {
        None,
        StringData,
        NumericData,
        Date
    }
}
