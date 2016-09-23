using System;

namespace Microsoft.GDIPlus
{
    public enum FillMode
    {
        Alternate = 0,
        Winding = 1
    }

    public enum QualityMode
    {
        Invalid = -1,
        Default = 0,
        Low = 1, // optimize for best performance
        High = 2 // optimize for best rendering quality
    }

    public enum CompositingMode
    {
        SourceOver = 0,
        SourceCopy = 1
    }

    public enum CompositingQuality
    {
        Invalid = QualityMode.Invalid,
        Default = QualityMode.Default,
        HighSpeed = QualityMode.Low,
        HighQuality = QualityMode.High,
        GammaCorrected = 3,
        AssumeLinear = 4
    }

    public enum GraphicsUnit
    {
        World = 0, // non-physical unit
        Display = 1, // variable - used with PageTransform only 
        Pixel = 2,
        Point = 3, // equal to 1/72 inch
        Inch = 4,
        Document = 5, // equal to 1/300 inch
        Millimeter = 6
    }

    public enum MetafileFrameUnit
    {
        Pixel = GraphicsUnit.Pixel,
        Point = GraphicsUnit.Point,
        Inch = GraphicsUnit.Inch,
        Document = GraphicsUnit.Document,
        Millimeter = GraphicsUnit.Millimeter,
        GDI = 7 // GDI-compatible 0.01 MM units
    }

    public enum CoordinateSpace
    {
        World = 0,
        Page = 1,
        Device = 2
    }

    public enum WrapMode
    {
        Tile = 0,
        FlipX = 1,
        FlipY = 2,
        FlipXY = 3,
        Clamp = 4
    }

    public enum HatchStyle
    {

        Horizontal = 0,
        Vertical = 1,
        ForwardDiagonal = 2,
        BackwardDiagonal = 3,
        Cross = 4,
        DiagonalCross = 5,
        Percent05 = 6,
        Percent10 = 7,
        Percent20 = 8,
        Percent25 = 9,
        Percent30 = 10,
        Percent40 = 11,
        Percent50 = 12,
        Percent60 = 13,
        Percent70 = 14,
        Percent75 = 15,
        Percent80 = 16,
        Percent90 = 17,
        LightDownwardDiagonal = 18,
        LightUpwardDiagonal = 19,
        DarkDownwardDiagonal = 20,
        DarkUpwardDiagonal = 21,
        WideDownwardDiagonal = 22,
        WideUpwardDiagonal = 23,
        LightVertical = 24,
        LightHorizontal = 25,
        NarrowVertical = 26,
        NarrowHorizontal = 27,
        DarkVertical = 28,
        DarkHorizontal = 29,
        DashedDownwardDiagonal = 30,
        DashedUpwardDiagonal = 31,
        DashedHorizontal = 32,
        DashedVertical = 33,
        SmallConfetti = 34,
        LargeConfetti = 35,
        ZigZag = 36,
        Wave = 37,
        DiagonalBrick = 38,
        HorizontalBrick = 39,
        Weave = 40,
        Plaid = 41,
        Divot = 42,
        DottedGrid = 43,
        DottedDiamond = 44,
        Shingle = 45,
        Trellis = 46,
        Sphere = 47,
        SmallGrid = 48,
        SmallCheckerBoard = 49,
        LargeCheckerBoard = 50,
        OutlinedDiamond = 51,
        SolidDiamond = 52,

        _MaxValid = 53,
        _MinValid = Horizontal,
        LargeGrid = Cross
    }

    public enum DashStyle
    {
        Solid = 0,
        Dash = 1,
        Dot = 2,
        DashDot = 3,
        DashDotDot = 4,
        Custom = 5
    }

    public enum DashCap
    {
        Flat = 0,
        Round = 2,
        Triangle = 3
    }

    public enum LineCap
    {
        Flat = 0,
        Square = 1,
        Round = 2,
        Triangle = 3,

        NoAnchor = 0x10,
        SquareAnchor = 0x11,
        RoundAnchor = 0x12,
        DiamondAnchor = 0x13,
        ArrowAnchor = 0x14,

        Custom = 0xFF
    }

    public enum CustomLineCapType
    {
        Default = 0,
        AdjustableArrow = 1
    }

    public enum LineJoin
    {
        Miter = 0,
        Bevel = 1,
        Round = 2,
        ClippedMiter = 3
    }

    public enum PathPointType
    {
        Start = 0,
        Line = 1,
        Bezier = 3,

        PathTypeMask = 0x07,
        DashMode = 0x10,
        PathMarker = 0x20,
        CloseSubpath = 0x80,
        CubicBezier = 3
    }

    public enum WarpMode
    {
        Perspective = 0,
        Bilinear = 1
    }

    public enum LinearGradientMode
    {
        Horizontal = 0,
        Vertical = 1,
        ForwardDiagonal = 2,
        BackwardDiagonal = 3
    }

    public enum RegionCombineMode
    {
        Replace = 0,
        Intersect = 1,
        Union = 2,
        Xor = 3,
        Exclude = 4,
        Complement = 5
    }

    public enum ImageType
    {
        Unknown = 0,
        Bitmap = 1,
        Metafile = 2
    }

    public enum InterpolationMode
    {
        Invalid = QualityMode.Invalid,
        Default = QualityMode.Default,
        LowQuality = QualityMode.Low,
        HighQuality = QualityMode.High,
        Bilinear = 3,
        Bicubic = 4,
        NearestNeighbor = 5,
        HighQualityBilinear = 6,
        HighQualityBicubic = 7
    }

    public enum PenAlignment
    {
        Center = 0,
        Inset = 1
    }

    public enum BrushType
    {
        SolidColor = 0,
        HatchFill = 1,
        TextureFill = 2,
        PathGradient = 3,
        LinearGradient = 4
    }

    public enum PenType
    {
        SolidColor = BrushType.SolidColor,
        HatchFill = BrushType.HatchFill,
        TextureFill = BrushType.TextureFill,
        PathGradient = BrushType.PathGradient,
        LinearGradient = BrushType.LinearGradient,
        Unknown = -1
    }

    public enum MatrixOrder
    {
        Prepend = 0,
        Append = 1
    }

    public enum GenericFontFamily
    {
        Serif,
        SansSerif,
        Monospaced
    }

    public enum FontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        BoldItalic = 3,
        Underline = 4,
        Strikeout = 8
    }

    public enum SmoothingMode
    {
        Invalid = QualityMode.Invalid,
        Default = QualityMode.Default,
        HighSpeed = QualityMode.Low,
        HighQuality = QualityMode.High,
        None = 3,
        AntiAlias = 4,
        AntiAlias8x4 = AntiAlias,
        AntiAlias8x8 = 5
    }

    public enum PixelOffsetMode
    {
        Invalid = QualityMode.Invalid,
        Default = QualityMode.Default,
        HighSpeed = QualityMode.Low,
        HighQuality = QualityMode.High,
        None = 3,
        ModeHalf = 4
    }

    public enum TextRenderingHint
    {
        SystemDefault = 0,
        SingleBitPerPixelGridFit = 1,
        SingleBitPerPixel = 2,
        AntiAliasGridFit = 3,
        AntiAlias = 4,
        ClearTypeGridFit = 5
    }

    public enum MetafileType
    {
        Invalid = 0,
        WMF = 1,
        PlaceableWMF = 2,
        EMF = 3,
        EMFPlusOnly = 4,
        EMFPlusDual = 5
    }

    public enum EMFType
    {
        EMFOnly = MetafileType.EMF,
        EMFPlusOnly = MetafileType.EMFPlusOnly,
        EMFPlusDual = MetafileType.EMFPlusDual
    }

    public enum ObjectType
    {
        Invalid = 0,
        Brush = 1,
        Pen = 2,
        Path = 3,
        Region = 4,
        Image = 5,
        Font = 6,
        StringFormat = 7,
        ImageAttributes = 8,
        CustomLineCap = 9,
        Graphics = 10,

        _MinValid = Brush,
        _MaxValid = Graphics
    }

    enum _EMFPlusRecordTypeHelpers
    {
        EMFPlusRecordBase = 0x00004000,
        WMFRecordBase = 0x00010000
    }

    public enum StringrFormatFlags
    {
        DirectionRightToLeft = 0x00000001,
        DirectionVertical = 0x00000002,
        NoFitBlackBox = 0x00000004,
        DisplayFormatControl = 0x00000020,
        NoFontFallback = 0x00000400,
        MeasureTrailingSpaces = 0x00000800,
        NoWrap = 0x00001000,
        LineLimit = 0x00002000,
        NoClip = 0x00004000,
        BypassGDI = unchecked((int)0x80000000)
    }

    public enum StringTrimming
    {
        None = 0,
        Character = 1,
        Word = 2,
        EllipsisCharacter = 3,
        EllipsisWord = 4,
        EllipsisPath = 5
    }

    public enum StringDigitSubstitution
    {
        User = 0,  // As NLS setting
        None = 1,
        National = 2,
        Traditional = 3
    }

    public enum HotkeyPrefix
    {
        None = 0,
        Show = 1,
        Hide = 2
    }

    public enum StringAlignment
    {
        /// <summary>
        /// Left edge for left-to-right text, right edge for right-to-left text,
        /// and top for vertical.
        /// </summary>
        Near = 0,

        /// <summary>
        /// Centered.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Right edge for left-to-right text, left edge for right-to-left text,
        /// and bottom for vertical.
        /// </summary>
        Far = 2
    }

    public enum DriverStringOptions
    {
        CmapLookup = 1,
        Vertical = 2,
        RealizedAdvance = 4,
        LimitSubpixel = 8
    }

    public enum FlushIntention
    {
        Normal = 0,
        Sync = 1
    }

    public enum EncoderParameterValueType
    {
        EncoderParameterValueTypeByte = 1,
        EncoderParameterValueTypeASCII = 2,
        EncoderParameterValueTypeShort = 3,
        EncoderParameterValueTypeLong = 4,
        EncoderParameterValueTypeRational = 5,
        EncoderParameterValueTypeLongRange = 6,
        EncoderParameterValueTypeUndefined = 7,
        EncoderParameterValueTypeRationalRange = 8,
        EncoderParameterValueTypePointer = 9
    }

    public enum EncoderValueType
    {
        ColorTypeCMYK,
        ColorTypeYCCK,
        CompressionLZW,
        CompressionCCITT3,
        CompressionCCITT4,
        CompressionRle,
        CompressionNone,
        ScanMethodInterlaced,
        ScanMethodNonInterlaced,
        VersionGif87,
        VersionGif89,
        RenderProgressive,
        RenderNonProgressive,
        TransformRotate90,
        TransformRotate180,
        TransformRotate270,
        TransformFlipHorizontal,
        TransformFlipVertical,
        MultiFrame,
        LastFrame,
        Flush,
        FrameDimensionTime,
        FrameDimensionResolution,
        FrameDimensionPage,
        ColorTypeGray,
        ColorTypeRGB,
    }

    internal enum TestControl
    {
        ForceBilinear = 0,
        NoICM = 1,
        GetBuildNumber = 2
    }
}
