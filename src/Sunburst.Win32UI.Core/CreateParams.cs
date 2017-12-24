using System;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI
{
    /// <summary>
    /// Contains data required to create a Win32 control.
    /// </summary>
    public sealed class CreateParams : IEquatable<CreateParams>
    {
        /// <summary>
        /// The name of the desired superclass, NOT the window class to be created.
        /// The created control will inherit the styles and behaviors of its superclass.
        /// If <c>null</c>, no superclass will be used.
        /// </summary>
        public string ClassName { get; set; } = null;

        /// <summary>
        /// The text to be displayed on the control.
        /// </summary>
        public string Caption { get; set; } = "";

        /// <summary>
        /// The Windows style (<c>WS_*</c>) flags.
        /// </summary>
        /// <seealso cref="WindowStyles"/>
        public int Style { get; set; } = 0;

        /// <summary>
        /// The Windows extended style (<c>WS_EX_*</c>) flags.
        /// </summary>
        /// <seealso cref="WindowStyles"/>
        public int ExtendedStyle { get; set; } = 0;

        /// <summary>
        /// The Windows class style flags.
        /// </summary>
        public int ClassStyle { get; set; } = 0;

        /// <summary>
        /// The frame of the control will have once it is created.
        /// </summary>
        public Rect Frame { get; set; } = Rect.Zero;

        /// <summary>
        /// The <c>HWND</c> to the control's parent, or <see cref="IntPtr.Zero"/> if it should be created without one.
        /// </summary>
        public IntPtr ParentHandle { get; set; } = IntPtr.Zero;

        public bool Equals(CreateParams other)
        {
            return ClassName == other.ClassName && Caption == other.Caption && Style == other.Style && ExtendedStyle == other.ExtendedStyle && ClassStyle == other.ClassStyle && Frame.Equals(other.Frame) && ParentHandle == other.ParentHandle;
        }

        public override bool Equals(object obj)
        {
            CreateParams other = obj as CreateParams;
            if (other == null) return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return ClassName.GetHashCode() ^ Caption.GetHashCode() ^ Style.GetHashCode() ^ ExtendedStyle.GetHashCode() ^ ClassStyle.GetHashCode() ^ Frame.GetHashCode() ^ ParentHandle.GetHashCode();
        }
    }
}
