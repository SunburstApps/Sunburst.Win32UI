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
using System.Runtime.InteropServices;
using System.Text;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Layout
{
    public partial class TableLayoutPanel : Window
    {
        private static readonly Window PlaceholderWindow = new Window();
        public bool SuspendLayout { get; set; } = false;

        private void PerformLayout()
        {
            if (SuspendLayout) return;
            throw new NotImplementedException();
        }

        private Window[,] CalculateControlPositions(int columns, int rows)
        {
            Window[,] grid = new Window[columns, rows];

            // First, place all controls that have an explicit column/row set.
            foreach (Window ctrl in ChildWindows)
            {
                int col = GetColumn(ctrl), row = GetRow(ctrl);
                if (col >= 0 && row >= 0)
                {
                    if (col >= columns) return CalculateControlPositions(col + 1, rows);
                    if (row >= rows) return CalculateControlPositions(columns, row + 1);

                    if (grid[col, row] == null)
                    {
                        int colSpan = Math.Min(GetColumnSpan(ctrl), columns);
                        int rowSpan = Math.Min(GetRowSpan(ctrl), rows);

                        if (col + colSpan > columns)
                        {
                            if (row + 1 < rows)
                            {
                                grid[col, row] = PlaceholderWindow;
                                row++;
                                col = 0;
                            }
                            else if (GrowStyle == TableLayoutPanelGrowStyle.AddColumns)
                            {
                                return CalculateControlPositions(columns + 1, rows);
                            }
                            else
                            {
                                throw new InvalidOperationException();
                            }
                        }

                        if (row + rowSpan > rows)
                        {
                            if (GrowStyle == TableLayoutPanelGrowStyle.AddRows)
                            {
                                return CalculateControlPositions(columns, rows + 1);
                            }
                            else
                            {
                                throw new InvalidOperationException();
                            }
                        }

                        grid[col, row] = ctrl;

                        // Now fill in the rest of this control's span extent with
                        // the placeholder control, to ensure that other controls
                        // don't get put there.
                        for (int i = 0; i < colSpan; i++)
                        {
                            for (int j = 0; j < rowSpan; j++)
                            {
                                if (i != 0 || j != 0) grid[col + i, row + j] = PlaceholderWindow;
                            }
                        }
                    }
                }
            }

            int x_pointer = 0, y_pointer = 0;
            // Next, fill in gaps with controls that do not have an explicit row or column set.
            foreach (Window ctrl in ChildWindows)
            {
                int col = GetColumn(ctrl), row = GetRow(ctrl);

                if ((col >= 0 && col < columns) && (row >= 0 && row < rows) && (grid[col, row] == ctrl || grid[col, row] == PlaceholderWindow))
                    continue;

                for (int y = y_pointer; y < rows; y++)
                {
                    y_pointer = y;
                    x_pointer = 0;

                    for (int x = x_pointer; x < columns; x++)
                    {
                        x_pointer = x;

                        if (grid[x, y] == null)
                        {
                            int colSpan = Math.Min(GetColumnSpan(ctrl), columns);
                            int rowSpan = Math.Min(GetRowSpan(ctrl), rows);

                            if (x + colSpan > columns)
                            {
                                if (y + 1 < rows) break;
                                else if (GrowStyle == TableLayoutPanelGrowStyle.AddColumns) return CalculateControlPositions(columns + 1, rows);
                                else throw new InvalidOperationException();
                            }

                            if (y + rowSpan > rows)
                            {
                                if (x + 1 < columns) break;
                                else if (GrowStyle == TableLayoutPanelGrowStyle.AddRows) return CalculateControlPositions(columns, rows + 1);
                                else throw new InvalidOperationException();
                            }

                            grid[x, y] = ctrl;

                            // Now fill in the rest of this control's span extent with
                            // the placeholder control, to ensure that other controls
                            // don't get put there.
                            for (int i = 0; i < colSpan; i++)
                            {
                                for (int j = 0; j < rowSpan; j++)
                                {
                                    if (i != 0 || j != 0) grid[x + i, y + j] = PlaceholderWindow;
                                }
                            }

                            goto Found;
                        }
                        else
                        {
                            if (GrowStyle == TableLayoutPanelGrowStyle.AddColumns && RowCount == 0) break;
                        }
                    }
                }

                TableLayoutPanelGrowStyle adjustedGrowStyle = GrowStyle;
                if (GrowStyle == TableLayoutPanelGrowStyle.AddColumns)
                {
                    if (RowCount == 0) adjustedGrowStyle = TableLayoutPanelGrowStyle.AddRows;
                }

                switch (adjustedGrowStyle)
                {
                    case TableLayoutPanelGrowStyle.FixedSize:
                        throw new InvalidOperationException();

                    case TableLayoutPanelGrowStyle.AddColumns:
                        return CalculateControlPositions(columns + 1, rows);

                    case TableLayoutPanelGrowStyle.AddRows:
                    default:
                        return CalculateControlPositions(columns, rows + 1);
                }

                Found:;
            }

            return grid;
        }
    }
}
