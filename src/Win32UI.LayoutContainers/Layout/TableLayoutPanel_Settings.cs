//
// This file is based on Mono's Windows Forms implementation.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Win32.UserInterface.Layout
{
    public partial class TableLayoutPanel
    {
        private List<TableCellSize> mColumnSizes;
        private List<TableCellSize> mRowSizes;
        private int mColumnCount;
        private int mRowCount;
        private Dictionary<Window, int> mColumns;
        private Dictionary<Window, int> mColumnSpans;
        private Dictionary<Window, int> mRows;
        private Dictionary<Window, int> mRowSpans;
        private TableLayoutPanelGrowStyle mGrowStyle = TableLayoutPanelGrowStyle.FixedSize;

        public int ColumnCount
        {
            get => mColumnCount;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                if (value != mColumnCount)
                {
                    mColumnCount = value;
                    PerformLayout();
                }
            }
        }

        public int RowCount
        {
            get => mRowCount;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                if (value != mRowCount)
                {
                    mRowCount = value;
                    PerformLayout();
                }
            }
        }

        public IList<TableCellSize> ColumnSizes => mColumnSizes;
        public IList<TableCellSize> RowSizes => mRowSizes;

        public TableLayoutPanelGrowStyle GrowStyle
        {
            get => mGrowStyle;
            set
            {
                if (!Enum.IsDefined(typeof(TableLayoutPanelGrowStyle), value))
                    throw new ArgumentException("Enum value not defined");

                if (value != mGrowStyle)
                {
                    mGrowStyle = value;
                    PerformLayout();
                }
            }
        }

        public int GetColumn(Window control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (mColumns.TryGetValue(control, out int value)) return value;
            else return -1;
        }

        public int GetRow(Window control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (mRows.TryGetValue(control, out int value)) return value;
            else return -1;
        }

        public int GetColumnSpan(Window control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (mColumnSpans.TryGetValue(control, out int value)) return value;
            else return -1;
        }

        public int GetRowSpan(Window control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (mRowSpans.TryGetValue(control, out int value)) return value;
            else return -1;
        }

        public void SetColumn(Window control, int column)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (column < -1) throw new ArgumentOutOfRangeException(nameof(column));

            mColumns[control] = column;
            PerformLayout();
        }

        public void SetRow(Window control, int row)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (row < -1) throw new ArgumentOutOfRangeException(nameof(row));

            mRows[control] = row;
            PerformLayout();
        }

        public void SetColumnSpan(Window control, int span)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (span < -1) throw new ArgumentOutOfRangeException(nameof(span));

            mColumnSpans[control] = span;
            PerformLayout();
        }

        public void SetRowSpan(Window control, int span)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (span < -1) throw new ArgumentOutOfRangeException(nameof(span));

            mRowSpans[control] = span;
            PerformLayout();
        }
    }

    public enum TableLayoutPanelGrowStyle
    {
        FixedSize,
        AddRows,
        AddColumns
    }
}
