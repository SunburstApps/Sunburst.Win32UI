using System;
using System.Drawing;

namespace Sunburst.WindowsForms
{
    public sealed class CreateParams : IEquatable<CreateParams>
    {
        // This is actually the name of the desired superclass, NOT the window class to be created.
        // If null, no superclass will be used.
        public string ClassName { get; set; } = null;

        public string Caption { get; set; } = "";

        public int Style { get; set; } = 0;

        public int ExtendedStyle { get; set; } = 0;

        public int ClassStyle { get; set; } = 0;

        public Rectangle Frame { get; set; } = Rectangle.Empty;

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
