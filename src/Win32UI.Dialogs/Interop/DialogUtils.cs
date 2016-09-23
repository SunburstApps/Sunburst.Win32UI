using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class DialogUtils
    {
        internal static IntPtr ReadResourceId(IntPtr ptr, out ResourceId rc)
        {
            rc = null;

            IntPtr adjustedPtr = ptr;
            switch ((ushort)Marshal.ReadInt16(adjustedPtr))
            {
                case 0x0000: // no predefined resource
                    adjustedPtr += 2;
                    break;

                case 0xFFFF: // one additional element that specifies the ordinal value of the resource
                    adjustedPtr += 2;
                    rc = new ResourceId((ushort)Marshal.ReadInt16(adjustedPtr));
                    adjustedPtr += 2;
                    break;

                default: // NUL-terminated Unicode string that specifies the name of the resource
                    rc = new ResourceId(Marshal.PtrToStringUni(adjustedPtr));
                    adjustedPtr += (rc.Name.Length + 1) * Marshal.SystemDefaultCharSize;
                    break;
            }

            return adjustedPtr;
        }

        internal static void WriteResourceId(BinaryWriter writer, ResourceId rc)
        {
            if (rc == null)
            {
                writer.Write((ushort)0);
            }
            else if (rc.IsIntResource())
            {
                writer.Write(0xFFFF);
                writer.Write((ushort)rc.Id);
            }
            else
            {
                PadToWORD(writer);
                writer.Write(Encoding.Unicode.GetBytes(rc.Name));
                writer.Write((ushort)0);
            }
        }

        internal static IntPtr Align(long p)
        {
            return new IntPtr((p + 3) & ~3);
        }

        internal static IntPtr Align(IntPtr p)
        {
            return Align(p.ToInt64());
        }

        internal static void PadToWORD(BinaryWriter writer)
        {
            long pos = writer.BaseStream.Position;
            if (pos % 2 != 0)
            {
                long count = 2 - pos % 2;
                Pad(ref writer, count);
            }
        }

        internal static void PadToDWORD(ref BinaryWriter writer)
        {
            long pos = writer.BaseStream.Position;
            if (pos % 4 != 0)
            {
                long count = 4 - pos % 4;
                Pad(ref writer, count);
            }
        }

        internal static void Pad(ref BinaryWriter writer, long count)
        {
            while (count-- > 0) writer.Write((byte)0);
        }
    }

    /// <summary>
    /// A resource Id.
    /// There're two types of resource Ids, reserved integer numbers (eg. RT_ICON) and custom string names (eg. "CUSTOM").
    /// </summary>
    internal class ResourceId
    {
        private IntPtr _name = IntPtr.Zero;

        /// <summary>
        /// A resource identifier.
        /// </summary>
        /// <param name="value">A integer or string resource id.</param>
        public ResourceId(IntPtr value)
        {
            Id = value;
        }

        /// <summary>
        /// A resource identifier.
        /// </summary>
        /// <param name="value">A integer resource id.</param>
        public ResourceId(uint value)
        {
            Id = new IntPtr(value);
        }

        /// <summary>
        /// A well-known resource-type identifier.
        /// </summary>
        /// <param name="value">A well known resource type.</param>
        public ResourceId(ResourceTypes value)
        {
            Id = (IntPtr)value;
        }

        /// <summary>
        /// A custom resource identifier.
        /// </summary>
        /// <param name="value"></param>
        public ResourceId(string value)
        {
            Name = value;
        }

        /// <summary>
        /// Resource Id.
        /// </summary>
        /// <remarks>
        /// If the resource Id is a string, it will be copied.
        /// </remarks>
        public IntPtr Id
        {
            get
            {
                return _name;
            }
            set
            {
                _name = IsIntResource(value)
                    ? value
                    : Marshal.StringToHGlobalUni(Marshal.PtrToStringUni(value));
            }
        }

        /// <summary>
        /// String representation of a resource type name.
        /// </summary>
        public string TypeName
        {
            get
            {
                return IsIntResource() ? ResourceType.ToString() : Name;
            }
        }

        /// <summary>
        /// An enumerated resource type for built-in resource types only.
        /// </summary>
        public ResourceTypes ResourceType
        {
            get
            {
                if (IsIntResource())
                    return (ResourceTypes)_name;

                throw new InvalidCastException(string.Format(
                    "Resource {0} is not of built-in type.", Name));
            }
            set
            {
                _name = (IntPtr)value;
            }
        }

        /// <summary>
        /// Returns true if the resource is an integer resource.
        /// </summary>
        public bool IsIntResource()
        {
            return IsIntResource(_name);
        }

        /// <summary>
        /// Returns true if the resource is an integer resource.
        /// </summary>
        /// <param name="value">Resource pointer.</param>
        internal static bool IsIntResource(IntPtr value)
        {
            return value.ToInt64() <= UInt16.MaxValue;
        }

        /// <summary>
        /// Resource Id in a string format.
        /// </summary>
        public string Name
        {
            get
            {
                return IsIntResource()
                    ? _name.ToString()
                    : Marshal.PtrToStringUni(_name);
            }
            set
            {
                _name = Marshal.StringToHGlobalUni(value);
            }
        }

        /// <summary>
        /// String representation of the resource Id.
        /// </summary>
        /// <returns>Resource name.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Resource Id hash code. 
        /// Resource Ids of the same type have the same hash code.
        /// </summary>
        /// <returns>Resource Id.</returns>
        public override int GetHashCode()
        {
            return IsIntResource()
                ? Id.ToInt32()
                : Name.GetHashCode();
        }

        /// <summary>
        /// Compares two resource Ids by value.
        /// </summary>
        /// <param name="obj">Resource Id.</param>
        /// <returns>True if both resource Ids represent the same resource.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ResourceId && obj == this)
                return true;

            if (obj is ResourceId && (obj as ResourceId).GetHashCode() == GetHashCode())
                return true;

            return false;
        }
    }

    /// <summary>
    /// Predefined resource types.
    /// </summary>
    internal enum ResourceTypes
    {
        /// <summary>
        /// Hardware-dependent cursor resource.
        /// </summary>
        RT_CURSOR = 1,
        /// <summary>
        /// Bitmap resource.
        /// </summary>
        RT_BITMAP = 2,
        /// <summary>
        /// Hardware-dependent icon resource.
        /// </summary>
        RT_ICON = 3,
        /// <summary>
        /// Menu resource.
        /// </summary>
        RT_MENU = 4,
        /// <summary>
        /// Dialog box.
        /// </summary>
        RT_DIALOG = 5,
        /// <summary>
        /// String-table entry.
        /// </summary>
        RT_STRING = 6,
        /// <summary>
        /// Font directory resource.
        /// </summary>
        RT_FONTDIR = 7,
        /// <summary>
        /// Font resource.
        /// </summary>
        RT_FONT = 8,
        /// <summary>
        /// Accelerator table.
        /// </summary>
        RT_ACCELERATOR = 9,
        /// <summary>
        /// Application-defined resource (raw data).
        /// </summary>
        RT_RCDATA = 10,
        /// <summary>
        /// Message-table entry.
        /// </summary>
        RT_MESSAGETABLE = 11,
        /// <summary>
        /// Hardware-independent cursor resource.
        /// </summary>
        RT_GROUP_CURSOR = 12,
        /// <summary>
        /// Hardware-independent icon resource.
        /// </summary>
        RT_GROUP_ICON = 14,
        /// <summary>
        /// Version resource.
        /// </summary>
        RT_VERSION = 16,
        /// <summary>
        /// Allows a resource editing tool to associate a string with an .rc file.
        /// </summary>
        RT_DLGINCLUDE = 17,
        /// <summary>
        /// Plug and Play resource.
        /// </summary>
        RT_PLUGPLAY = 19,
        /// <summary>
        /// VXD.
        /// </summary>
        RT_VXD = 20,
        /// <summary>
        /// Animated cursor.
        /// </summary>
        RT_ANICURSOR = 21,
        /// <summary>
        /// Animated icon.
        /// </summary>
        RT_ANIICON = 22,
        /// <summary>
        /// HTML.
        /// </summary>
        RT_HTML = 23,
        /// <summary>
        /// Microsoft Windows XP: Side-by-Side Assembly XML Manifest.
        /// </summary>
        RT_MANIFEST = 24,
    }
}
