using System;
using System.Drawing;

namespace Sunburst.WindowsForms
{
    public sealed class CreateParams : IEquatable<CreateParams>
    {
        // This is actually the name of the desired superclass, NOT the window class to be created.
        // If null, no superclass will be used.
        public string ClassName { get; set; }

        public string Caption { get; set; }

        public int Style { get; set; }

        public int ExtendedStyle { get; set; }

        public Rectangle Frame { get; set; }

        public IntPtr ParentHandle { get; set; }

        public object CreationParameter { get; set; }

        public bool Equals(CreateParams other)
        {
            return ClassName == other.ClassName && Caption == other.Caption && Style == other.Style && Frame.Equals(other.Frame) && ParentHandle == other.ParentHandle && CreationParameter.Equals(other.CreationParameter);
        }

        public override bool Equals(object obj)
        {
            CreateParams other = obj as CreateParams;
            if (other == null) return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return ClassName.GetHashCode() ^ Caption.GetHashCode() ^ Style.GetHashCode() ^ ExtendedStyle.GetHashCode() ^ Frame.GetHashCode() ^ ParentHandle.GetHashCode() ^ CreationParameter.GetHashCode();
        }
    }
}
