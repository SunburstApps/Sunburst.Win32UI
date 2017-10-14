using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    internal struct SYSTEMTIME
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;

        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }

        public SYSTEMTIME(DateTime date)
        {
            wYear = Convert.ToInt16(date.Year);
            wMonth = Convert.ToInt16(date.Month);
            wDay = Convert.ToInt16(date.Day);
            wHour = Convert.ToInt16(date.Hour);
            wMinute = Convert.ToInt16(date.Minute);
            wSecond = Convert.ToInt16(date.Second);
            wMilliseconds = Convert.ToInt16(date.Millisecond);
            wDayOfWeek = Convert.ToInt16(date.DayOfWeek);
        }
    }
}
