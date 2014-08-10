using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using Hawkeye.Logging;
using Hawkeye.Extensibility;
using Hawkeye.UI.Controls;
using Hawkeye.UI.PropertyTabs;

namespace Hawkeye.UI
{
    /// <summary>
    /// The property grid used to display .NET related information about a window.
    /// </summary>
    internal class DotNetPropertyGrid : PropertyGridEx
    {
        private static readonly ILogService log = LogManager.GetLogger<DotNetPropertyGrid>();

        private readonly Dictionary<DotNetPropertyGridAction, ToolStripItem> actionButtons = new Dictionary<DotNetPropertyGridAction, ToolStripItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetPropertyGrid"/> class.
        /// </summary>
        public DotNetPropertyGrid() : base() { }

        public event EventHandler ControlCreated;
        public event DotNetPropertyGridActionEventHandler ActionClicked;

        internal void EnableAction(DotNetPropertyGridAction action, bool enabled = true)
        {
            if (actionButtons.ContainsKey(action))
                actionButtons[action].Enabled = enabled;
        }

        /// <summary>
        /// Called when the control is created.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            base.PropertyTabs.AddTabType(typeof(InstanceEventsTab));
            base.PropertyTabs.AddTabType(typeof(AllPropertiesTab));
            base.PropertyTabs.AddTabType(typeof(AllEventsTab));

            AddButtons();

            AddPlugins();

            if (ControlCreated != null)
                ControlCreated(this, EventArgs.Empty);
        }

        /////// <summary>
        /////// Initializes the tool strip.
        /////// </summary>
        ////protected override void InitializeToolStrip()
        ////{
        ////    // Nothing to do here.
        ////}

        private void AddButtons()
        {
            var parentButton = new ToolStripButton("P&arent", Properties.Resources.UpArrow)
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Enabled = false
            };

            var previousButton = new ToolStripButton("&Previous", Properties.Resources.LeftArrow)
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Enabled = false
            };

            var nextButton = new ToolStripButton("&Next", Properties.Resources.RightArrow)
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Enabled = false
            };

            var highlightButton = new ToolStripButton("&Highlight", Properties.Resources.Highlight)
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Enabled = false
            };

            var lastItemIndex = ToolStrip.Items.Count - 1; // This is the "Properties" button
            // Insert the additional buttons before this one
            ToolStrip.Items.Insert(lastItemIndex, new ToolStripSeparator());
            ToolStrip.Items.Insert(lastItemIndex, nextButton);
            ToolStrip.Items.Insert(lastItemIndex, previousButton);
            ToolStrip.Items.Insert(lastItemIndex, parentButton);
            ToolStrip.Items.Insert(lastItemIndex, highlightButton);

            parentButton.Click += (s, _) => RaiseActionClicked(DotNetPropertyGridAction.Parent);
            previousButton.Click += (s, _) => RaiseActionClicked(DotNetPropertyGridAction.Previous);
            nextButton.Click += (s, _) => RaiseActionClicked(DotNetPropertyGridAction.Next);
            highlightButton.Click += (s, _) => RaiseActionClicked(DotNetPropertyGridAction.Highlight);

            actionButtons.Add(DotNetPropertyGridAction.Previous, previousButton);
            actionButtons.Add(DotNetPropertyGridAction.Next, nextButton);
            actionButtons.Add(DotNetPropertyGridAction.Parent, parentButton);
            actionButtons.Add(DotNetPropertyGridAction.Highlight, highlightButton);
        }

        private void AddPlugins()
        {
            log.Debug("--------- Adding plugin buttons");
            var q = HawkeyeApplication.Shell.PluginManager.Plugins.Where(p => p is ICommandPlugin).Cast<ICommandPlugin>().ToArray();
            if (q.Length == 0) return;

            var lastItemIndex = ToolStrip.Items.Count - 1; // This is the "Properties" button
            ToolStrip.Items.Insert(lastItemIndex, new ToolStripSeparator());

            foreach (var plugin in q)
            {
                var adapter = new CommandPluginAdapter(plugin);
                adapter.InsertToolStripButton(ToolStrip, lastItemIndex);
            }
        }

        private void RaiseActionClicked(DotNetPropertyGridAction action)
        {
            if (ActionClicked != null)
                ActionClicked(this, new DotNetPropertyGridActionEventArgs(action));
        }
    }
}