using System;

namespace Sunburst.Win32UI
{
    /// <summary>
    /// Contains the data of a Windows message.
    /// </summary>
    public struct Message
    {
        /// <summary>
        /// The <c>HWND</c> of the control the message was sent to.
        /// </summary>
        public IntPtr TargetHandle { get; set; }

        /// <summary>
        /// An integer that identifies the type of message.
        /// </summary>
        /// <seealso cref="WindowMessages"/>
        public uint MessageId { get; set; }

        /// <summary>
        /// The <c>wParam</c> of the message.
        /// </summary>
        public IntPtr WParam { get; set; }

        /// <summary>
        /// The <c>lParam</c> of the message.
        /// </summary>
        public IntPtr LParam { get; set; }

        /// <summary>
        /// The value to be returned to Windows.
        /// </summary>
        public IntPtr Result { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Message"/>.
        /// </summary>
        /// <param name="hWnd">
        /// The value for <see cref="TargetHandle"/>.
        /// </param>
        /// <param name="msg">
        /// The value for <see cref="MessageId"/>.
        /// </param>
        /// <param name="wParam">
        /// The value for <see cref="WParam"/>.
        /// </param>
        /// <param name="lParam">
        /// The value for <see cref="LParam"/>.
        /// </param>
        public Message(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            TargetHandle = hWnd;
            MessageId = msg;
            WParam = wParam;
            LParam = lParam;
            Result = IntPtr.Zero;
        }
    }
}
