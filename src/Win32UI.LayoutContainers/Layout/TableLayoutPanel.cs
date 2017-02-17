using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Layout
{
    public class TableLayoutPanel : Window
    {
    }

    public struct TableCellSize : IEquatable<TableCellSize>
    {
        private readonly TableCellMeasurementUnit _unit;
        private readonly double _value;

        public static readonly TableCellSize Auto = new TableCellSize(0, TableCellMeasurementUnit.AutoSize);

        public TableCellSize(double value) : this(value, TableCellMeasurementUnit.Pixel) { }

        public TableCellSize(double value, TableCellMeasurementUnit unit)
        {
            if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid size value", nameof(value));

            if (unit < TableCellMeasurementUnit.AutoSize || unit > TableCellMeasurementUnit.WeightedProportion)
                throw new ArgumentOutOfRangeException($"Invalid {nameof(TableCellMeasurementUnit)} value", nameof(unit));

            _unit = unit;
            _value = value;
        }

        public TableCellMeasurementUnit MeasurementUnit => _unit;
        public double Value => _value;

        internal bool IsAbsolute => MeasurementUnit == TableCellMeasurementUnit.Pixel;
        internal bool IsAuto => MeasurementUnit == TableCellMeasurementUnit.AutoSize;
        internal bool IsStar => MeasurementUnit == TableCellMeasurementUnit.WeightedProportion;

        #region Equality Comparison

        public static bool operator ==(TableCellSize lhs, TableCellSize rhs) => (lhs.IsAuto && rhs.IsAuto) || (lhs.Value == rhs.Value && lhs.MeasurementUnit == rhs.MeasurementUnit);
        public static bool operator !=(TableCellSize lhs, TableCellSize rhs) => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is TableCellSize)) return false;

            return this == (TableCellSize)obj;
        }

        public bool Equals(TableCellSize other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ MeasurementUnit.GetHashCode();
        }

        #endregion

        #region String Conversion

        public override string ToString()
        {
            if (IsAuto) return "Auto";

            string valueStr = Value.ToString();
            return IsStar ? valueStr + "*" : valueStr;
        }

        public static TableCellSize Parse(string str)
        {
            str = str.ToUpperInvariant();

            if (str == "AUTO") return TableCellSize.Auto;

            if (str.EndsWith("*"))
            {
                var valueString = str.Substring(0, str.Length - 1).Trim();
                var value = valueString.Length > 0 ? double.Parse(valueString) : 1;
                return new TableCellSize(value, TableCellMeasurementUnit.WeightedProportion);
            }
            else
            {
                var value = double.Parse(str);
                return new TableCellSize(value, TableCellMeasurementUnit.Pixel);
            }
        }

        public static IReadOnlyList<TableCellSize> ParseMultiple(string str)
        {
            return str.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Parse(x)).ToArray();
        }

        #endregion
    }

    public enum TableCellMeasurementUnit
    {
        AutoSize = 0,
        Pixel,
        WeightedProportion
    }
}
