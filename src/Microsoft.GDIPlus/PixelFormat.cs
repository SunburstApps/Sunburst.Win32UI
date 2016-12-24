using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.GDIPlus
{
    public static class PixelFormatUtility
    {
        internal const int Indexed = 0x00010000;
        internal const int GDICompatible = 0x00020000;
        internal const int HasAlpha = 0x00040000;
        internal const int PremultipliedAlpha = 0x00080000;
        internal const int Extended = 0x00100000;
        internal const int Canonical = 0x00200000;

        public static int GetSizeInBits(this PixelFormat format)
        {
            int value = (int)format;
            return (value >> 8) & 0xFF;
        }

        public static bool IsValid(this PixelFormat format)
        {
            int value = (int)format;
            return (value & 0xFF) < 16;
        }

        public static bool IsIndexedFormat(this PixelFormat format)
        {
            int value = (int)format;
            return (value & Indexed) != 0;
        }

        public static bool FormatHasAlpha(this PixelFormat format)
        {
            int value = (int)format;
            return (value & HasAlpha) != 0;
        }

        public static bool IsExtendedFormat(this PixelFormat format)
        {
            int value = (int)format;
            return (value & Extended) != 0;
        }

        public static bool IsCanonicalFormat(this PixelFormat format)
        {
            int value = (int)format;
            return (value & Canonical) != 0;
        }
    }

    public enum PixelFormat
    {
        Undefined = 0,
        Format1bppIndexed = (1 | 1 << 8 | PixelFormatUtility.Indexed | PixelFormatUtility.GDICompatible),
        Format4bppIndexed = (2 | 4 << 8 | PixelFormatUtility.Indexed | PixelFormatUtility.GDICompatible),
        Format8bppIndexed = (3 | 8 << 8 | PixelFormatUtility.Indexed | PixelFormatUtility.GDICompatible),
        Format16bppGrayscale = (4 | 8 << 8 | PixelFormatUtility.Extended),
        FormatRGB555 = (5 | 16 << 8 | PixelFormatUtility.GDICompatible),
        FormatRGB565 = (6 | 16 << 8 | PixelFormatUtility.GDICompatible),
        FormatARGB1555 = (7 | 16 << 8 | PixelFormatUtility.HasAlpha | PixelFormatUtility.GDICompatible),
        Format24bppRGB = (8 | 24 << 8 | PixelFormatUtility.GDICompatible),
        Format32bppRGB = (9 | 24 << 8 | PixelFormatUtility.GDICompatible),
        Format32bppARGB = (10 | 32 << 8 | PixelFormatUtility.HasAlpha | PixelFormatUtility.GDICompatible),
        Format32bppPremultipliedARGB = (11 | 32 << 8 | PixelFormatUtility.HasAlpha | PixelFormatUtility.GDICompatible | PixelFormatUtility.PremultipliedAlpha),
        Format48bppRGB = (12 | 48 << 8 | PixelFormatUtility.Extended),
        Format64bppARGB = (13 | 64 << 8 | PixelFormatUtility.PremultipliedAlpha | PixelFormatUtility.Canonical | PixelFormatUtility.Extended),
        Formar64bppPremultipliedARGB = (12 | 64 << 8 | PixelFormatUtility.PremultipliedAlpha | PixelFormatUtility.Canonical | PixelFormatUtility.Extended),
        Format32bppCMYK = (15 | 32 << 8)
    }
}
