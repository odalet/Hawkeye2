using System.Windows.Forms;

namespace Hawkeye.UI
{
    /// <summary>
    /// The property grid used to display native information about a window.
    /// </summary>
    internal class NativePropertyGrid : PropertyGridEx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NativePropertyGrid"/> class.
        /// </summary>
        public NativePropertyGrid() : base() { }

        /// <summary>
        /// Gets the detect button.
        /// </summary>
        public ToolStripButton DetectButton
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the dump button.
        /// </summary>
        public ToolStripButton DumpButton
        {
            get;
            private set;
        }

        protected override ToolStripItem[] CreateToolStripItems()
        {
            DetectButton = new ToolStripButton("Detect")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            
            DumpButton = new ToolStripButton("Dump")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            return new ToolStripItem[]
            {
                DetectButton, DumpButton
            };
        }
    }
}
