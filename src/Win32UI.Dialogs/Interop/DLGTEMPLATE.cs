using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Interop
{
    /// <summary>
    /// The DIALOGTEMPLATE structure defines the dimensions and style of a dialog box. 
    /// This structure, always the first in a standard template for a dialog box, 
    /// also specifies the number of controls in the dialog box and therefore specifies 
    /// the number of subsequent DIALOGITEMTEMPLATE structures in the template.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct DIALOGTEMPLATE
    {
        /// <summary>
        /// Specifies the style of the dialog box.
        /// </summary>
        public UInt32 style;
        /// <summary>
        /// Extended styles for a window.
        /// </summary>
        public UInt32 dwExtendedStyle;
        /// <summary>
        /// Specifies the number of items in the dialog box. 
        /// </summary>
        public UInt16 cdit;
        /// <summary>
        /// Specifies the x-coordinate, in dialog box units, of the upper-left corner of the dialog box. 
        /// </summary>
        public Int16 x;
        /// <summary>
        /// Specifies the y-coordinate, in dialog box units, of the upper-left corner of the dialog box.
        /// </summary>
        public Int16 y;
        /// <summary>
        /// Specifies the width, in dialog box units, of the dialog box.
        /// </summary>
        public Int16 cx;
        /// <summary>
        /// Specifies the height, in dialog box units, of the dialog box.
        /// </summary>
        public Int16 cy;
    }

    /// <summary>
    /// The DIALOGITEMTEMPLATE structure defines the dimensions and style of a control in a dialog box.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct DIALOGITEMTEMPLATE
    {
        /// <summary>
        /// Specifies the style of the control.
        /// </summary>
        public UInt32 style;
        /// <summary>
        /// Extended styles for a window.
        /// </summary>
        public UInt32 dwExtendedStyle;
        /// <summary>
        /// Specifies the x-coordinate, in dialog box units, of the upper-left corner of the control. 
        /// </summary>
        public Int16 x;
        /// <summary>
        /// Specifies the y-coordinate, in dialog box units, of the upper-left corner of the control.
        /// </summary>
        public Int16 y;
        /// <summary>
        /// Specifies the width, in dialog box units, of the control.
        /// </summary>
        public Int16 cx;
        /// <summary>
        /// Specifies the height, in dialog box units, of the control.
        /// </summary>
        public Int16 cy;
        /// <summary>
        /// Specifies the control identifier.
        /// </summary>
        public Int16 id;
    }
}
