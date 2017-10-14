using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Handles;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Menus
{
    public class NonOwnedMenu : IMenuHandle
    {
        public IntPtr Handle { get; set; }

        public void InsertItem(uint position, MenuItem item)
        {
            MENUITEMINFO nativeItem = item.ToNativeStruct();
            bool success = NativeMethods.InsertMenuItemW(Handle, position, true, ref nativeItem);
            if (!success) throw new System.ComponentModel.Win32Exception();
        }

        public MenuItem GetItem(uint position)
        {
            MENUITEMINFO nativeItem = new MENUITEMINFO();
            nativeItem.cbSize = Convert.ToUInt32(Marshal.SizeOf<MENUITEMINFO>());
            nativeItem.fMask = MenuConstants.MIIM_BITMAP | MenuConstants.MIIM_CHECKMARKS | MenuConstants.MIIM_FTYPE | MenuConstants.MIIM_ID | MenuConstants.MIIM_STATE | MenuConstants.MIIM_SUBMENU;

            bool success = NativeMethods.GetMenuItemInfoW(Handle, position, true, ref nativeItem);
            if (!success) throw new System.ComponentModel.Win32Exception();
            return new MenuItem(nativeItem);
        }

        public void SetItem(uint position, MenuItem item)
        {
            MENUITEMINFO nativeItem = item.ToNativeStruct();
            bool success = NativeMethods.SetMenuItemInfoW(Handle, position, true, ref nativeItem);
            if (!success) throw new NotImplementedException();
        }

        public void RemoveItem(int position)
        {
            bool success = NativeMethods.DeleteMenu(Handle, position, MenuConstants.MF_BYPOSITION);
            if (!success) throw new System.ComponentModel.Win32Exception();
        }

        public void ShowPopupMenu(Point windowRelativePosition, Window owner, PopupMenuFlags flags)
        {
            Point screenPosition = windowRelativePosition;
            bool success = NativeMethods.ClientToScreen(owner.Handle, ref screenPosition);
            if (!success) throw new System.ComponentModel.Win32Exception();

            NativeMethods.TrackPopupMenuEx(Handle, (uint)flags, screenPosition.x, screenPosition.y, owner.Handle, IntPtr.Zero);
        }

        public void ShowPopupMenu(Point windowRelativePosition, Window owner, PopupMenuFlags flags, Rect excludeRect)
        {
            Point screenPosition = windowRelativePosition;
            bool success = NativeMethods.ClientToScreen(owner.Handle, ref screenPosition);
            if (!success) throw new System.ComponentModel.Win32Exception();

            TPMPARAMS paramStruct = new TPMPARAMS();
            paramStruct.cbStruct = Convert.ToUInt32(Marshal.SizeOf<TPMPARAMS>());
            paramStruct.rcExclude = excludeRect;

            using (StructureBuffer<TPMPARAMS> ptr = new StructureBuffer<TPMPARAMS>())
            {
                ptr.Value = paramStruct;
                NativeMethods.TrackPopupMenuEx(Handle, (uint)flags, screenPosition.x, screenPosition.y, owner.Handle, ptr.Handle);
            }
        }
    }
}
