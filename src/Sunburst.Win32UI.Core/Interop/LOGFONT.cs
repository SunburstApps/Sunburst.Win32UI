using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    /// <summary>
    /// Contains the data necessary to create a <see cref="Sunburst.Win32UI.Graphics.Font"/> instance.
    /// </summary>
    /// <remarks>
    /// This structure is public because wrapping it in a <c>FontDescriptor</c> class is
    /// not worth the effort it would take to create the wrapper.
    /// </remarks>
    public struct LOGFONT : IEquatable<LOGFONT>
    {
        public long lfHeight;
        public long lfEscapement;
        public long lfOrientation;
        public long lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.LPWStr, SizeConst = 32)]
        public string lfFaceName;

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType())) return false;
            return Equals((LOGFONT)obj);
        }

        public bool Equals(LOGFONT other)
        {
            return lfHeight == other.lfHeight && lfEscapement == other.lfEscapement &&
                lfOrientation == other.lfOrientation && lfWeight == other.lfWeight &&
                lfItalic == other.lfItalic && lfUnderline == other.lfUnderline &&
                lfStrikeOut == other.lfStrikeOut && lfCharSet == other.lfCharSet &&
                lfOutPrecision == other.lfOutPrecision && lfPitchAndFamily == other.lfPitchAndFamily &&
                lfFaceName.Equals(other.lfFaceName);
        }

        public override int GetHashCode()
        {
            return lfHeight.GetHashCode() ^ lfEscapement.GetHashCode() ^ lfOrientation.GetHashCode() ^
                lfWeight.GetHashCode() ^ lfItalic.GetHashCode() ^ lfStrikeOut.GetHashCode() ^
                lfCharSet.GetHashCode() ^ lfOutPrecision.GetHashCode() ^ lfPitchAndFamily.GetHashCode() ^
                lfFaceName.GetHashCode();
        }

        public override string ToString()
        {
            return $"<LOGFONT>({lfFaceName})";
        }
    }
}
