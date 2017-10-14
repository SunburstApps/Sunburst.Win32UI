namespace Sunburst.Win32UI.Interop
{
    /// <summary>
    /// Represents the standardized weights that can be used in a <see cref="LOGFONT"/>.
    /// </summary>
    public enum FontWeight
    {
        FW_THIN = 100,
        FW_EXTRALIGHT = 200,
        FW_LIGHT = 300,
        FW_NORMAL = 400,
        FW_MEDIUM = 500,
        FW_SEMIBOLD = 600,
        FW_BOLD = 700,
        FW_EXTRABOLD = 800,
        FW_HEAVY = 900,
        FW_ULTRALIGHT = FW_EXTRALIGHT,
        FW_REGULAR = FW_NORMAL,
        FW_DEMIBOLD = FW_SEMIBOLD,
        FW_ULTRABOLD = FW_EXTRABOLD,
        FW_BLACK = FW_HEAVY,
    }
}
