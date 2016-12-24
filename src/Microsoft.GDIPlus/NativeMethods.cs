using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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
    }
}
