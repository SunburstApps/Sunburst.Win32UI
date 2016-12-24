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

        #region Guids

        public static class Guids
        {
            public static readonly Guid ImageFormatUndefined = new Guid("b96b3ca9-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatMemoryBMP = new Guid("b96b3caa-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatBMP = new Guid("b96b3cab-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatEMF = new Guid("b96b3cac-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatWMF = new Guid("b96b3cad-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatJPEG = new Guid("b96b3cae-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatPNG = new Guid("b96b3caf-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatGIF = new Guid("b96b3cb0-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatTIFF = new Guid("b96b3cb1-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatEXIF = new Guid("b96b3cb2-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid ImageFormatIcon = new Guid("b96b3cb5-0728-11d3-9d7b-0000f81ef32e");
            public static readonly Guid FrameDimensionTime = new Guid("6aedbd6d-3fb5-418a-83a6-7f45229dc872");
            public static readonly Guid FrameDimensionResolution = new Guid("84236f7b-3bd3-428f-8dab-4ea1439ca315");
            public static readonly Guid FrameDimensionPage = new Guid("7462dc86-6180-4c7e-8e3f-ee7333a7a483");
            public static readonly Guid FormatIDImageInformation = new Guid("e5836cbe-5eef-4f1d-acde-ae4c43b608ce");
            public static readonly Guid FormatIDJpegAppHeaders = new Guid("1c4afdcd-6177-43cf-abc7-5f51af39ee85");
            public static readonly Guid EncoderCompression = new Guid("e09d739d-ccd4-44ee-8eba-3fbf8be4fc58");
            public static readonly Guid EncoderColorDepth = new Guid("66087055-ad66-4c7c-9a18-38a2310b8337");
            public static readonly Guid EncoderScanMethod = new Guid("3a4e2661-3109-4e56-8536-42c156e7dcfa");
            public static readonly Guid EncoderVersion = new Guid("24d18c76-814a-41a4-bf53-1c219cccf797");
            public static readonly Guid EncoderRenderMethod = new Guid("6d42c53a-229a-4825-8bb7-5c99e2b9a8b8");
            public static readonly Guid EncoderQuality = new Guid("1d5be4b5-fa4a-452d-9cdd-5db35105e7eb");
            public static readonly Guid EncoderTransformation = new Guid("8d0eb2d1-a58e-4ea8-aa14-108074b7b6f9");
            public static readonly Guid EncoderLuminanceTable = new Guid("0xedb33bce-0266-4a77-b904-27216099e717");
            public static readonly Guid EncoderChrominanceTable = new Guid("0xf2e455dc-09b3-4316-8260-676ada32481c");
            public static readonly Guid EncoderSaveFlag = new Guid("292266fc-ac40-47bf-8cfca85b89a655de");
            public static readonly Guid CodecIImageBytes = new Guid("025d1823-6c7d-447b-bbdb-a3cbc3dfa2fc");
        }

        #endregion
    }
}
