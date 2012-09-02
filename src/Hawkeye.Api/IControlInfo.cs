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
#if DEBUG
        Control Control { get; set; } // needed for tests purpose
#else
        Control Control { get; }
#endif
    }
}
