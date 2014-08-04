using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Hawkeye.UI.Controls
{
    // Inspiration found in Hawkeye's search box extender
    internal partial class PropertyGridEx : PropertyGrid
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

        protected bool IsProcessingSelection
        {
            get;
            private set;
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

            InitializeSearchBox();
        }

        /// <summary>
        /// Creates the tool strip items.
        /// </summary>
        /// <returns></returns>
        protected virtual ToolStripItem[] CreateToolStripItems()
        {
            return new ToolStripItem[] { };
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.PropertyGrid.SelectedObjectsChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            if (IsProcessingSelection)
            {
                base.OnSelectedObjectsChanged(e);
                return;
            }

            IsProcessingSelection = true;
            try
            {
                var selection = base.SelectedObjects;
                base.SelectedObjects = ProcessSelection(selection).ToArray();                
            }
            finally
            {
                IsProcessingSelection = false;
            }
        }

        /// <summary>
        /// Processes the current property grid <see cref="SelectedObjects"/>, giving a chance to alter it.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <returns>The altered selection</returns>
        protected virtual IEnumerable<object> ProcessSelection(IEnumerable<object> selection)
        {
            if (selection == null) return selection;
            return selection
                .Select(item => item.GetInnerObject())
                .Where(item => item != null);
        }
    }
}
