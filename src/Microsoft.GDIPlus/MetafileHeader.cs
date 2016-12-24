using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.GDIPlus
{
    public sealed class MetafileHeader
    {
        public MetafileType Type { get; set; }
        public uint MetafileSize { get; set; }
        public uint Version { get; set; }
        public uint EmfPlusFlags { get; set; }
        public SizeF DPI { get; set; } = SizeF.Empty;
        public Rect Bounds { get; set; } = Rect.Empty;
        public int EmfPlusHeaderSize { get; set; }
        public Size LogicalDPI { get; set; } = Size.Empty;

        private NativeMethods.METAHEADER WmfHeader;
        private NativeMethods.EHNMETAHEADER3 EmfHeader;

        public MetafileHeader()
        {
            WmfHeader = new NativeMethods.METAHEADER();
            EmfHeader = new NativeMethods.EHNMETAHEADER3();
        }

        public bool IsWMF => Type == MetafileType.WMF || Type == MetafileType.PlaceableWMF;
        public bool IsPlaceableWMF => Type == MetafileType.PlaceableWMF;
        public bool IsEMF => Type == MetafileType.EMF;
        public bool IsEMFPlus => Type == MetafileType.EMFPlusDual || Type == MetafileType.EMFPlusOnly;
        public bool IsDisplay => IsEMFPlus && (EmfPlusFlags & 0x1) != 0;

        public HGlobal WMFHeader
        {
            get
            {
                if (!IsWMF) return null;

                HGlobal ptr = new HGlobal(Marshal.SizeOf<NativeMethods.METAHEADER>());
                Marshal.StructureToPtr(WmfHeader, ptr.Handle, false);
                return ptr;
            }
        }

        public HGlobal EMFHeader
        {
            get
            {
                if (!IsEMF && !IsEMFPlus) return null;

                HGlobal ptr = new HGlobal(Marshal.SizeOf<NativeMethods.EHNMETAHEADER3>());
                Marshal.StructureToPtr(EmfHeader, ptr.Handle, false);
                return ptr;
            }
        }
    }
}
