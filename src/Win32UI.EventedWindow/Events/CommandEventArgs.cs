using System.ComponentModel;

namespace Microsoft.Win32.UserInterface.Events
{
    public class CommandEventArgs : HandledEventArgs
    {
        public CommandEventArgs(int id, Window window)
        {
            ControlId = id;
            Window = window;
        }

        public int ControlId { get; private set; }
        public Window Window { get; private set; }
    }
}
