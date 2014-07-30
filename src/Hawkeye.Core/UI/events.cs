using System;

namespace Hawkeye.UI
{
    /// <summary>
    /// Represents the methods that handle events related to <see cref="DotNetPropertyGridAction"/>.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="DotNetPropertyGridActionEventArgs"/> instance containing the event data.</param>
    public delegate void DotNetPropertyGridActionEventHandler(object sender, DotNetPropertyGridActionEventArgs e);

    /// <summary>
    /// Event Data for events related to <see cref="DotNetPropertyGridAction"/>.
    /// </summary>
    public class DotNetPropertyGridActionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetPropertyGridActionEventArgs"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public DotNetPropertyGridActionEventArgs(DotNetPropertyGridAction action)
        {
            Action = action;
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        public DotNetPropertyGridAction Action { get; private set; }
    }
}
