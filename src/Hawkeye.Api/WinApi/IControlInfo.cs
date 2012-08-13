using System.Windows.Forms;
using System.ComponentModel;

namespace Hawkeye
{
    /// <summary>
    /// Stores properties of a Window Forms Control
    /// </summary>
    public interface IControlInfo
    {
        /// <summary>
        /// Gets the Windows Forms Control.
        /// </summary>
        Control Control { get; }
    }
}
