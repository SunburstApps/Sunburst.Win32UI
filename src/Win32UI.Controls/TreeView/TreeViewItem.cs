using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class TreeViewItemHandle : IEquatable<TreeViewItemHandle>
    {
        public static TreeViewItemHandle Root = new TreeViewItemHandle((IntPtr)0xFFFFD8F0);

        internal TreeViewItemHandle(IntPtr hTreeItem)
        {
            Handle = hTreeItem;
        }

        internal IntPtr Handle { get; private set; }

        public bool Equals(TreeViewItemHandle other)
        {
            return Handle.Equals(other.Handle);
        }

        public override bool Equals(object obj)
        {
            TreeViewItemHandle other = obj as TreeViewItemHandle;
            if (other != null) return Equals(other);
            else return false;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }

    public class TreeViewItemData
    {
        public TreeViewItemData() { }

        internal TreeViewItemData(TVITEMEX nativeStruct)
        {
            HasChildren = nativeStruct.cChildren != 0;

            if ((nativeStruct.mask & TVITEMEX.TVIF_TEXT) == TVITEMEX.TVIF_TEXT) Text = nativeStruct.pszText;
            if ((nativeStruct.mask & TVITEMEX.TVIF_IMAGE) == TVITEMEX.TVIF_IMAGE) ImageIndex = nativeStruct.iImage;
            if ((nativeStruct.mask & TVITEMEX.TVIF_INTEGRAL) == TVITEMEX.TVIF_INTEGRAL) ItemHeight = nativeStruct.iIntegral;
            if ((nativeStruct.mask & TVITEMEX.TVIF_PARAM) == TVITEMEX.TVIF_PARAM) UserData = nativeStruct.lParam;

            if ((nativeStruct.mask & TVITEMEX.TVIF_SELECTEDIMAGE) == TVITEMEX.TVIF_SELECTEDIMAGE)
            {
                HasSelectedImage = true;
                SelectedImageIndex = nativeStruct.iSelectedImage;
            }

            if ((nativeStruct.mask & TVITEMEX.TVIF_EXPANDEDIMAGE) == TVITEMEX.TVIF_EXPANDEDIMAGE)
            {
                HasExpandedImage = true;
                ExpandedImageIndex = nativeStruct.iExpandedImage;
            }

            if ((nativeStruct.mask & TVITEMEX.TVIF_STATE) == TVITEMEX.TVIF_STATE)
            {
                IsSelected = ((nativeStruct.state & TVITEMEX.TVIS_SELECTED) == TVITEMEX.TVIS_SELECTED);
                IsCut = ((nativeStruct.state & TVITEMEX.TVIS_CUT) == TVITEMEX.TVIS_CUT);
                IsHighlightedForDrop = ((nativeStruct.state & TVITEMEX.TVIS_DROPHILITED) == TVITEMEX.TVIS_DROPHILITED);
                IsTextBold = ((nativeStruct.state & TVITEMEX.TVIS_BOLD) == TVITEMEX.TVIS_BOLD);
                IsExpanded = ((nativeStruct.state & TVITEMEX.TVIS_EXPANDED) == TVITEMEX.TVIS_EXPANDED);
                OverlayMask = (byte)((nativeStruct.state & TVITEMEX.TVIS_OVERLAYMASK) >> 8);
            }

            if ((nativeStruct.mask & TVITEMEX.TVIF_STATEEX) == TVITEMEX.TVIF_STATEEX)
            {
                const uint TVIS_EX_DISABLED = 0x0002;
                Disabled = (nativeStruct.uStateEx & TVIS_EX_DISABLED) == TVIS_EX_DISABLED;
            }
        }

        internal TVITEMEX ToNativeStruct()
        {
            TVITEMEX nativeStruct = new TVITEMEX();
            nativeStruct.mask = TVITEMEX.TVIF_TEXT | TVITEMEX.TVIF_IMAGE | TVITEMEX.TVIF_INTEGRAL | TVITEMEX.TVIF_STATEEX | TVITEMEX.TVIF_PARAM | TVITEMEX.TVIF_STATE;
            nativeStruct.pszText = Text; nativeStruct.cchTextMax = Text.Length;
            nativeStruct.iImage = ImageIndex;
            nativeStruct.lParam = UserData;

            if (HasSelectedImage)
            {
                nativeStruct.mask |= TVITEMEX.TVIF_SELECTEDIMAGE;
                nativeStruct.iSelectedImage = SelectedImageIndex;
            }

            if (HasExpandedImage)
            {
                nativeStruct.mask |= TVITEMEX.TVIF_EXPANDEDIMAGE;
                nativeStruct.iExpandedImage = ExpandedImageIndex;
            }

            nativeStruct.cChildren = HasChildren ? 1 : 0;
            nativeStruct.iIntegral = ItemHeight;

            if (IsSelected) nativeStruct.state |= TVITEMEX.TVIS_SELECTED;
            if (IsCut) nativeStruct.state |= TVITEMEX.TVIS_CUT;
            if (IsHighlightedForDrop) nativeStruct.state |= TVITEMEX.TVIS_DROPHILITED;
            if (IsTextBold) nativeStruct.state |= TVITEMEX.TVIS_BOLD;
            if (IsExpanded) nativeStruct.state |= TVITEMEX.TVIS_EXPANDED;

            if (OverlayMask > 16) throw new ArgumentOutOfRangeException(nameof(OverlayMask), $"{nameof(OverlayMask)} must be in the range 0 to 16");
            else nativeStruct.state |= (uint)(OverlayMask << 8);

            const uint TVIS_EX_DISABLED = 0x0002;
            if (Disabled) nativeStruct.uStateEx |= TVIS_EX_DISABLED;

            return nativeStruct;
        }

        public string Text { get; set; }
        public int ImageIndex { get; set; }
        public int SelectedImageIndex { get; set; }
        public bool HasSelectedImage { get; set; } = false;
        public int ExpandedImageIndex { get; set; }
        public bool HasExpandedImage { get; set; } = false;
        public bool HasChildren { get; set; } = false;
        public bool Disabled { get; set; } = false;
        public int ItemHeight { get; set; } = 1;
        public IntPtr UserData { get; set; } = IntPtr.Zero;
        public bool IsSelected { get; set; } = false;
        public bool IsCut { get; set; } = false;
        public bool IsHighlightedForDrop { get; set; } = false;
        public bool IsTextBold { get; set; } = false;
        public bool IsExpanded { get; set; } = false;

        // This must be in the range 0-16 only, any other value will result in an exception.
        public byte OverlayMask { get; set; } = 0;
    }
}
