using System;
using System.Windows.Forms;

using Hawkeye.Extensibility;

namespace Hawkeye.UI
{
    internal class CommandPluginAdapter
    {
        private readonly ICommandPlugin plugin;
        private bool enabled = false;
        private ToolStripButton button = null;

        public CommandPluginAdapter(ICommandPlugin commandPlugin)
        {
            if (commandPlugin == null) throw new ArgumentNullException("commandPlugin");
            plugin = commandPlugin;

            CreateControls();
        }
        
        public bool Enabled
        {
            get { return enabled; }
            private set
            {
                if (enabled == value) return;
                enabled = value;
                EnableControls(enabled);
            }
        }

        internal void InsertToolStripButton(ToolStrip strip, int index)
        {
            strip.Items.Insert(index, button);
        }

        private void CreateControls()
        {
            button = CreateToolStripButton();
            button.Click += (s, _) => plugin.Execute(); 

            plugin.CanExecuteChanged += (s, _) =>
                Enabled = plugin.CanExecute();
        }

        private void EnableControls(bool enable)
        {
            button.Enabled = enable;
        }

        private ToolStripButton CreateToolStripButton()
        {
            var label = plugin.Label;
            if (string.IsNullOrEmpty(label))
                label = plugin.Descriptor.Name;

            var button = new ToolStripButton(label);

            if (plugin.Image != null)
            {
                button.Image = plugin.Image;
                button.DisplayStyle = ToolStripItemDisplayStyle.Image;
            }
            else button.DisplayStyle = ToolStripItemDisplayStyle.Text;

            return button;
        }
    }
}
