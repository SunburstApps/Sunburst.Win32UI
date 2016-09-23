using Microsoft.Win32.UserInterface.Graphics;

#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct TPMPARAMS
    {
        public uint cbStruct;
        public Rect rcExclude;
    }
}
