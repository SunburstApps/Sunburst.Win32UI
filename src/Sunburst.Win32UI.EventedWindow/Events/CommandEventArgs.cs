using System.ComponentModel;

namespace Sunburst.Win32UI.Events
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
