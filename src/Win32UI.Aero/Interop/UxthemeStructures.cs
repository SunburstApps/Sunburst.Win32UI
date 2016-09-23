using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    [Flags]
    internal enum WindowThemeNonClientAttributes : uint
    {
        /// <summary>Do Not Draw The Caption (Text)</summary>
        NoDrawCaption = 0x00000001,
        /// <summary>Do Not Draw the Icon</summary>
        NoDrawIcon = 0x00000002,
        /// <summary>Do Not Show the System Menu</summary>
        NoSysMenu = 0x00000004,
        /// <summary>Do Not Mirror the Question mark Symbol</summary>
        NoMirrorHelp = 0x00000008
    }

    internal enum WindowThemeAttributeType
    {
        WTA_NONCLIENT = 1,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WTA_OPTIONS
    {
        public WindowThemeNonClientAttributes Flags;
        public uint Mask;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DrawThemeTextOptions
    {
        int dwSize;
        DrawThemeTextOptionsFlags dwFlags;
        int crText;
        int crBorder;
        int crShadow;
        TextShadowType iTextShadowType;
        Point ptShadowOffset;
        int iBorderSize;
        int iFontPropId;
        int iColorPropId;
        int iStateId;
        bool fApplyOverlay;
        int iGlowSize;
        int pfnDrawTextCallback;
        IntPtr lParam;

        public DrawThemeTextOptions(bool init) : this()
        {
            dwSize = Marshal.SizeOf<DrawThemeTextOptions>();
        }

        public Color AlternateColor
        {
            get { return Color.FromWin32Color(iColorPropId); }
            set
            {
                iColorPropId = Color.ToWin32Color(value);
                dwFlags |= DrawThemeTextOptionsFlags.ColorProp;
            }
        }

        public DrawThemeTextSystemFonts AlternateFont
        {
            get { return (DrawThemeTextSystemFonts)iFontPropId; }
            set
            {
                iFontPropId = (int)value;
                dwFlags |= DrawThemeTextOptionsFlags.FontProp;
            }
        }

        public bool AntiAliasedAlpha
        {
            get { return (dwFlags & DrawThemeTextOptionsFlags.Composited) == DrawThemeTextOptionsFlags.Composited; }
            set
            {
                if (value)
                    dwFlags |= DrawThemeTextOptionsFlags.Composited;
                else
                    dwFlags &= ~DrawThemeTextOptionsFlags.Composited;
            }
        }

        public bool ApplyOverlay
        {
            get { return fApplyOverlay; }
            set
            {
                fApplyOverlay = value;
                dwFlags |= DrawThemeTextOptionsFlags.ApplyOverlay;
            }
        }

        public Color BorderColor
        {
            get { return Color.FromWin32Color(crBorder); }
            set
            {
                crBorder = Color.ToWin32Color(value);
                dwFlags |= DrawThemeTextOptionsFlags.BorderColor;
            }
        }

        public int BorderSize
        {
            get { return iBorderSize; }
            set
            {
                iBorderSize = value;
                dwFlags |= DrawThemeTextOptionsFlags.BorderSize;
            }
        }

        public int GlowSize
        {
            get { return iGlowSize; }
            set
            {
                iGlowSize = value;
                dwFlags |= DrawThemeTextOptionsFlags.GlowSize;
            }
        }

        public bool ReturnCalculatedRectangle
        {
            get { return (dwFlags & DrawThemeTextOptionsFlags.CalcRect) == DrawThemeTextOptionsFlags.CalcRect; }
            set
            {
                if (value)
                    dwFlags |= DrawThemeTextOptionsFlags.CalcRect;
                else
                    dwFlags &= ~DrawThemeTextOptionsFlags.CalcRect;
            }
        }

        public Color ShadowColor
        {
            get { return Color.FromWin32Color(crShadow); }
            set
            {
                crShadow = Color.ToWin32Color(value);
                dwFlags |= DrawThemeTextOptionsFlags.ShadowColor;
            }
        }

        public Point ShadowOffset
        {
            get { return new Point(ptShadowOffset.x, ptShadowOffset.y); }
            set
            {
                ptShadowOffset = value;
                dwFlags |= DrawThemeTextOptionsFlags.ShadowOffset;
            }
        }

        public TextShadowType ShadowType
        {
            get { return iTextShadowType; }
            set
            {
                iTextShadowType = value;
                dwFlags |= DrawThemeTextOptionsFlags.ShadowType;
            }
        }

        public Color TextColor
        {
            get { return Color.FromWin32Color(crText); }
            set
            {
                crText = Color.ToWin32Color(value);
                dwFlags |= DrawThemeTextOptionsFlags.TextColor;
            }
        }
    }

    internal enum DrawThemeTextSystemFonts
    {
        Caption = 801,
        SmallCaption = 802,
        Menu = 803,
        Status = 804,
        MessageBox = 805,
        IconTitle = 806
    }

    internal enum TextShadowType : int
    {
        None = 0,
        Single = 1,
        Continuous = 2
    }

    [Flags]
    internal enum DrawThemeTextOptionsFlags : int
    {
        TextColor = 1,
        BorderColor = 2,
        ShadowColor = 4,
        ShadowType = 8,
        ShadowOffset = 16,
        BorderSize = 32,
        FontProp = 64,
        ColorProp = 128,
        StateId = 256,
        CalcRect = 512,
        ApplyOverlay = 1024,
        GlowSize = 2048,
        Callback = 4096,
        Composited = 8192
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
        public byte bmiColors_rgbBlue;
        public byte bmiColors_rgbGreen;
        public byte bmiColors_rgbRed;
        public byte bmiColors_rgbReserved;

        public BITMAPINFO(int width, int height) : this()
        {
            biSize = Marshal.SizeOf<BITMAPINFO>();
            biWidth = width;
            biHeight = height;
            biPlanes = 1;
            biBitCount = 32;
        }
    }
}
