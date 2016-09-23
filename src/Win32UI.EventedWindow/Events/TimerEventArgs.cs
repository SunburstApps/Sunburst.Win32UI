namespace Microsoft.Win32.UserInterface.Events
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
