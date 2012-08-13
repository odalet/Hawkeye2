using System.Reflection;
using System.Windows.Forms;

namespace Hawkeye.UI
{
    // Inspiration found in Hawkeye's search box extender
    internal class PropertyGridEx : PropertyGrid
    {
        private bool alreadyInitialized = false;
        private ToolStrip thisToolStrip = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridEx"/> class.
        /// </summary>
        public PropertyGridEx()
        {
            base.ToolStripRenderer = new ToolStripProfessionalRenderer()
            {
                RoundedEdges = false
            };

            if (IsHandleCreated) Initialize();
            else
            {
                HandleCreated += (s, e) => Initialize();
                VisibleChanged += (s, e) => Initialize();
            }
        }

        protected ToolStrip ToolStrip
        {
            get
            {
                if (thisToolStrip == null)
                {
                    var field = typeof(PropertyGrid).GetField("toolStrip",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    thisToolStrip = (ToolStrip)field.GetValue(this);
                }

                return thisToolStrip;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            if (alreadyInitialized) return;
            if (ToolStrip == null) return;

            InitializeToolStrip();

            alreadyInitialized = true;
        }

        /// <summary>
        /// Initializes the tool strip.
        /// </summary>
        protected virtual void InitializeToolStrip()
        {
            ToolStrip.Items.Add(new ToolStripSeparator());
            ToolStrip.Items.AddRange(CreateToolStripItems());
        }

        /// <summary>
        /// Creates the tool strip items.
        /// </summary>
        /// <returns></returns>
        protected virtual ToolStripItem[] CreateToolStripItems()
        {
            return new ToolStripItem[] { };
        }
    }
}
