using System;

namespace Sunburst.WindowsForms
{
    public interface IWin32Window
    {
        IntPtr Handle { get; }
    }
}
