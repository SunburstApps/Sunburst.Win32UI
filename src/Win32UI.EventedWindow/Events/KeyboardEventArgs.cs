namespace Microsoft.Win32.UserInterface.Events
{
    public sealed class KeyboardEventArgs : HandledEventArgs
    {
        public KeyboardEventArgs(char text, int repeatCount)
        {
            Text = text;
            RepeatCount = repeatCount;
        }

        public char Text { get; private set; }
        public int RepeatCount { get; private set; }
    }
}
