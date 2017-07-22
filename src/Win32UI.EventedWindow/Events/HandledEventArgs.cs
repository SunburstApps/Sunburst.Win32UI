using System;

namespace Microsoft.Win32.UserInterface.Events
{
    /// <summary>
    /// Provides a method for an event listener to indicate
    /// if the event was handled or not.
    /// </summary>
    /// <remarks>
    /// What "handled" means depends on the context, but for <see cref="Window"/>
    /// subclasses, it controls whether or not the message's result value will
    /// be returned to Windows immediately, or if further processing of the
    /// message should occur.
    /// </remarks>
    public class HandledEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;
    }
}
