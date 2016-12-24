using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#pragma warning disable 0649
#pragma warning disable 0169
namespace Microsoft.GDIPlus
{
    internal static class NativeMethods
    {
        #region GpStatus

        internal enum GpStatus
        {
            Ok = 0,
            GenericError = 1,
            InvalidParameter = 2,
            OutOfMemory = 3,
            ObjectBusy = 4,
            InsufficientBuffer = 5,
            NotImplemented = 6,
            Win32Error = 7,
            WrongState = 8,
            Aborted = 9,
            FileNotFound = 10,
            ValueOverflow = 11,
            AccessDenied = 12,
            UnknownImageFormat = 13,
            FontFamilyNotFound = 14,
            FontStyleNotFound = 15,
            NotTrueTypeFont = 16,
            UnsupportedGdiplusVersion = 17,
            GdiplusNotInitialized = 18,
            PropertyNotFound = 19,
            PropertyNotSupported = 20,
            ProfileNotFound = 21
        }

        internal static void ThrowStatus(GpStatus status)
        {
            if (status == GpStatus.Ok) return;

            switch (status)
            {
                case GpStatus.GenericError: throw new Exception("Unspecified error returned by GDI+");
                case GpStatus.InvalidParameter: throw new ArgumentException("Invalid parameter error returned by GDI+");
                case GpStatus.OutOfMemory: throw new OutOfMemoryException();
                case GpStatus.ObjectBusy: throw new Exception("GDI+ object is busy");
                case GpStatus.InsufficientBuffer: throw new Exception("GDI+ buffer is too small");
                case GpStatus.NotImplemented: throw new NotImplementedException();
                case GpStatus.Win32Error: throw new System.ComponentModel.Win32Exception();
                case GpStatus.WrongState: throw new InvalidOperationException("GDI+ object reports it is in the wrong state");
                case GpStatus.Aborted: throw new OperationCanceledException();
                case GpStatus.FileNotFound: throw new System.IO.FileNotFoundException();
                case GpStatus.ValueOverflow: throw new OverflowException();
                case GpStatus.AccessDenied: throw new UnauthorizedAccessException();
                case GpStatus.FontFamilyNotFound: throw new ArgumentException("GDI+ font family not found");
                case GpStatus.FontStyleNotFound: throw new ArgumentException("GDI+ font style not found");
                case GpStatus.NotTrueTypeFont: throw new ArgumentException("Specified font is not a TrueType font");
                case GpStatus.UnsupportedGdiplusVersion: throw new InvalidOperationException("Unsupported GDI+ version");
                case GpStatus.GdiplusNotInitialized: throw new InvalidOperationException("GDI+ reports it is not initialized - this should not occur");
                case GpStatus.PropertyNotFound: throw new ArgumentException("GDI+ property not found");
                case GpStatus.PropertyNotSupported: throw new ArgumentException("GDI+ property not supported");
                case GpStatus.ProfileNotFound: throw new ArgumentException("GDI+ color profile not found");
                default: throw new Exception($"Unknown GDI+ error: {status.ToString()}");
            }
        }

        #endregion

        #region Metafile Structures

        public struct EHNMETAHEADER3
        {
            public uint iType;
            public uint nSize;

            public int rclBounds_Left;
            public int rclBounds_Top;
            public int rclBounds_Right;
            public int rclBounds_Bottom;

            public int rclFrame_Left;
            public int rclFrame_Top;
            public int rclFrame_Right;
            public int rclFrame_Bottom;

            public int dwSignature;
            public int nVersion;
            public int nBytes;
            public int nRecords;
            public short nHandles;
            private short _reserved;
            public int nDescription;
            public int offDescription;

            public int szlDevice_cx;
            public int szlDevice_cy;
            public int szlMillimeters_cx;
            public int szlMillimeters_cy;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct WmfPlaceableFileHeader
        {
            public WmfPlaceableFileHeader(bool initialize) : this()
            {
                Key = 0x9AC6CDD7;
                _reserved = 0;
            }

            public uint Key;
            public short Hmf;
            public short BoundingBox_Left;
            public short BoundingBox_Top;
            public short BoundingBox_Right;
            public short BoundingBox_Bottom;
            public short MetafileUnitsPerInch;
            private uint _reserved;
            public short Checksum;

            public void CalculateChecksum()
            {
                short sum = 0;
                sum ^= (short)((Key >> 16) & 0xFF);
                sum ^= (short)(Key & 0xFFFF);
                sum ^= BoundingBox_Left;
                sum ^= BoundingBox_Top;
                sum ^= BoundingBox_Right;
                sum ^= BoundingBox_Bottom;
                sum ^= MetafileUnitsPerInch;

                // _reserved is always 0, so we can skip xoring it into the checksum
                _reserved = 0;
                Checksum = sum;
            }
        }

        public struct METAHEADER
        {
            public short mtType;
            public short mtHeaderSize;
            public short mtVersion;
            public int mtSize;
            public short mtNoObjects;
            public int mtMaxRecord;
            public short mtNoParameters;
        }

        #endregion
    }
}
