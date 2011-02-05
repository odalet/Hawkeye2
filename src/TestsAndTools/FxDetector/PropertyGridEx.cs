using System;
using System.Windows.Forms;
using System.Reflection;

namespace FxDetector
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
            : base()
        {
            base.ToolStripRenderer = new ToolStripProfessionalRenderer()
            {
                RoundedEdges = false
            };

            if (IsHandleCreated)
                InitializeToolStrip();
            else
            {
                HandleCreated += (s, e) => InitializeToolStrip();
                VisibleChanged += (s, e) => InitializeToolStrip();
            }
        }

        private ToolStrip ToolStrip
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

        public ToolStripButton DetectButton
        {
            get;
            private set;
        }

        public ToolStripButton DumpButton
        {
            get;
            private set;
        }

        private void InitializeToolStrip()
        {
            if (alreadyInitialized) return;
            if (ToolStrip == null) return;

            DetectButton = new ToolStripButton("Detect")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            DumpButton = new ToolStripButton("Dump")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            ToolStrip.Items.Add(new ToolStripSeparator());
            ToolStrip.Items.Add(DetectButton);
            ToolStrip.Items.Add(DumpButton);
            alreadyInitialized = true;
        }
    }
}
