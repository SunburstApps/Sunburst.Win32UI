using System;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.CommonControls
{
    public class Label : Control
    {
        #region Messages

        private const uint STM_SETICON = 0x0170;
        private const uint STM_GETICON = 0x0171;
        private const uint STM_SETIMAGE = 0x0172;
        private const uint STM_GETIMAGE = 0x0173;

        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassName = "STATIC";
                return cp;
            }
        }

        public NonOwnedIcon Icon
        {
            get
            {
                return new NonOwnedIcon(SendMessage(STM_GETICON, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(STM_SETICON, value?.Handle ?? IntPtr.Zero, IntPtr.Zero);
            }
        }

        public NonOwnedBitmap Bitmap
        {
            get
            {
                return new NonOwnedBitmap(SendMessage(STM_GETIMAGE, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(STM_SETIMAGE, IntPtr.Zero, value?.Handle ?? IntPtr.Zero);
            }
        }

        public Cursor Cursor
        {
            get
            {
                return new Cursor(SendMessage(STM_GETIMAGE, (IntPtr)2, IntPtr.Zero));
            }

            set
            {
                SendMessage(STM_SETIMAGE, (IntPtr)2, value?.Handle ?? IntPtr.Zero);
            }
        }
    }
}
