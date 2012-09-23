using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hawkeye.UI
{
    /// <summary>
    /// Holds parameters describing Hawkeye's Main form
    /// </summary>
    internal class MainFormSettings
    {
        //TODO: should be serialized to disk so that Hawkeye restores itself
        // at the right place when creating a new instance.

        public IntPtr SpiedWindow { get; set; }
        public Size Size { get; set; }
        public Point Location { get; set; }
        public FormWindowState WindowState { get; set; }
        public Screen Screen { get; set; } 
    }
}
