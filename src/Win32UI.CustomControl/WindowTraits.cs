using System;

namespace Microsoft.Win32.UserInterface
{
    /// <summary>
    /// Contains styling data common to a specific type of <see cref="Window"/>.
    /// </summary>
    public class WindowTraits : IEquatable<WindowTraits>
    {
        /// <summary>
        /// An instance of ControlTraits containing recommended defaults for use with child controls.
        /// </summary>
        public static readonly WindowTraits ChildControl = new WindowTraits(WindowStyles.WS_CHILD | WindowStyles.WS_VISIBLE | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS, 0);

        /// <summary>
        /// An instance of ControlTraits containing recommended defaults for use with top-level windows.
        /// </summary>
        public static readonly WindowTraits TopLevelWindow = new WindowTraits(WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS, WindowStyles.WS_EX_APPWINDOW | WindowStyles.WS_EX_WINDOWEDGE);

        /// <summary>
        /// Creates a new instance of ControlTraits.
        /// </summary>
        /// <remarks>
        /// Both <see cref="DefaultStyle"/> and <see cref="DefaultExtendedStyle"/>
        /// will be set to zero.
        /// </remarks>
        public WindowTraits() : this(0, 0) { }

        /// <summary>
        /// Creates a new instance of ControlTraits.
        /// </summary>
        /// <param name="style">
        /// The new instance's <see cref="DefaultStyle"/>.
        /// </param>
        /// <param name="extendedStyle">
        /// The new instance's <see cref="DefaultExtendedStyle"/>.
        /// </param>
        public WindowTraits(uint style, uint extendedStyle)
        {
            DefaultStyle = style;
            DefaultExtendedStyle = extendedStyle;
        }

        /// <summary>
        /// The default style flags to be assigned to controls created with these traits.
        /// </summary>
        public uint DefaultStyle { get; protected set; }

        /// <summary>
        /// The default extended style flags to be assigned to controls created with these traits.
        /// </summary>
        public uint DefaultExtendedStyle { get; protected set; }

        /// <summary>
        /// Calculates the style flags to be used with a control.
        /// </summary>
        /// <param name="style">
        /// Any extra flags to be applied on top of the defaults.
        /// </param>
        /// <returns>
        /// The style flags to be applied to the control.
        /// </returns>
        public virtual uint CalculateStyle(uint style)
        {
            return style | DefaultStyle;
        }

        /// <summary>
        /// Calculates the extended style flags to be used with a control.
        /// </summary>
        /// <param name="style">
        /// Any extra flags to be applied on top of the defaults.
        /// </param>
        /// <returns>
        /// The extended style flags to be applied to the control.
        /// </returns>
        public virtual uint CalculateExtendedStyle(uint style)
        {
            return style | DefaultExtendedStyle;
        }

        /// <summary>
        /// Determines if the given object is equal to the receiver.
        /// </summary>
        /// <param name="obj">
        /// An object.
        /// </param>
        /// <returns>
        /// <c>true</c> if the given object is non-<c>null</c>, an instance
        /// of <see cref="WindowTraits"/>, and is equal in value to the receiver.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            WindowTraits other = obj as WindowTraits;
            if (other == null) return false;
            return Equals(other);
        }

        /// <summary>
        /// Compares two <see cref="WindowTraits"/> instances for equality.
        /// </summary>
        /// <param name="other">
        /// An instance of ControlTraits.
        /// </param>
        /// <returns>
        /// <c>true</c> if the two instances are equal, or <c>false</c> otherwise.
        /// </returns>
        public bool Equals(WindowTraits other)
        {
            return other.DefaultStyle == DefaultStyle && other.DefaultExtendedStyle == DefaultExtendedStyle;
        }

        /// <summary>
        /// Calculates a hash code for the receiver.
        /// </summary>
        /// <returns>
        /// A hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return DefaultStyle.GetHashCode() ^ DefaultExtendedStyle.GetHashCode();
        }

        /// <summary>
        /// Converts the receiver to a string suitable for debugging purposes.
        /// </summary>
        /// <returns>
        /// A string representing the receiver.
        /// </returns>
        public override string ToString()
        {
            return $"<{GetType().FullName}>(style={DefaultStyle}, exstyle={DefaultExtendedStyle})";
        }
    }
}
