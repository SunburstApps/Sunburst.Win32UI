using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    public sealed class MenuItem
    {
        public static readonly MenuItem Separator = new MenuItem { IsSeparator = true };

        public MenuItem() { }
        internal MenuItem(MENUITEMINFO nativeStruct)
        {
            Text = nativeStruct.dwTypeData;
            ID = nativeStruct.wID;
            IsSeparator = (nativeStruct.fType & MenuConstants.MFT_SEPARATOR) != 0;
            IsOwnerDrawn = (nativeStruct.fType & MenuConstants.MFT_OWNERDRAW) != 0;
            IsDefault = (nativeStruct.fState & MenuConstants.MFS_DEFAULT) != 0;
            IsChecked = (nativeStruct.fState & MenuConstants.MFS_CHECKED) != 0;
            UseRadioCheck = (nativeStruct.fType & MenuConstants.MFT_RADIOCHECK) != 0;

            if (nativeStruct.hbmpChecked != IntPtr.Zero) CheckedBitmap = new Bitmap(nativeStruct.hbmpChecked);
            if (nativeStruct.hbmpUnchecked != IntPtr.Zero) UncheckedBitmap = new Bitmap(nativeStruct.hbmpUnchecked);
            if (nativeStruct.hbmpItem != IntPtr.Zero) ItemBitmap = new Bitmap(nativeStruct.hbmpItem);
            if (nativeStruct.hSubMenu != IntPtr.Zero) Submenu = new Menu(nativeStruct.hSubMenu);
        }

        internal MENUITEMINFO ToNativeStruct()
        {
            MENUITEMINFO info = new MENUITEMINFO();
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf<MENUITEMINFO>());
            info.fMask = MenuConstants.MIIM_ID | MenuConstants.MIIM_SUBMENU | MenuConstants.MIIM_STRING | MenuConstants.MIIM_BITMAP | MenuConstants.MIIM_CHECKMARKS |
                MenuConstants.MIIM_FTYPE | MenuConstants.MIIM_STATE;

            info.wID = ID;
            info.hSubMenu = Submenu?.Handle ?? IntPtr.Zero;
            info.dwTypeData = Text;
            info.cch = Convert.ToUInt32(Text.Length);
            info.fState = (IsDefault ? MenuConstants.MFS_DEFAULT : 0) | (IsEnabled ? MenuConstants.MFS_ENABLED : MenuConstants.MFS_DISABLED) | (IsChecked ? MenuConstants.MFS_CHECKED : MenuConstants.MFS_UNCHECKED);
            info.fType = (IsSeparator ? MenuConstants.MFT_SEPARATOR : 0) | (UseRadioCheck ? MenuConstants.MFT_RADIOCHECK : 0) | (IsOwnerDrawn ? MenuConstants.MFT_OWNERDRAW : 0);
            info.hbmpChecked = CheckedBitmap?.Handle ?? IntPtr.Zero;
            info.hbmpUnchecked = UncheckedBitmap?.Handle ?? IntPtr.Zero;
            
            if (ItemBitmap != null)
            {
                info.hbmpItem = ItemBitmap.Handle;
                info.fMask |= MenuConstants.MIIM_BITMAP;
            }

            return info;
        }

        public string Text { get; set; } = null;
        public uint ID { get; set; } = 0;
        public Menu Submenu { get; set; } = null;

        public bool IsSeparator { get; private set; } = false;
        public bool IsOwnerDrawn { get; set; } = false;
        public bool IsDefault { get; set; } = false;
        public bool IsChecked { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool UseRadioCheck { get; set; } = false;

        public Bitmap ItemBitmap { get; set; } = null;
        public Bitmap CheckedBitmap { get; set; } = null;
        public Bitmap UncheckedBitmap { get; set; } = null;
        public void SetBitmap(Bitmap bmp)
        {
            CheckedBitmap = UncheckedBitmap = bmp;
        }
    }
}
