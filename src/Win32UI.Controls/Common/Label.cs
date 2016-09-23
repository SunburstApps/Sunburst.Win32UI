using System;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class Label : Window
    {
        #region Messages

        private const uint STM_SETICON = 0x0170;
        private const uint STM_GETICON = 0x0171;
        private const uint STM_SETIMAGE = 0x0172;
        private const uint STM_GETIMAGE = 0x0173;

        #endregion

        public override string WindowClassName => "STATIC";

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
