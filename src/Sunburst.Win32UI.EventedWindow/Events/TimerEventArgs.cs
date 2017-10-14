using System.ComponentModel;
namespace Sunburst.Win32UI.Events
{
    public sealed class TimerEventArgs : HandledEventArgs
    {
        public TimerEventArgs(uint id)
        {
            TimerId = id;
        }

        public uint TimerId { get; private set; }
    }
}
