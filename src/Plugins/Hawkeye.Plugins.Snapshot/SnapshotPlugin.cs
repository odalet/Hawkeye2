// Code by Olivier DALET based on code & idea by Sharpmao on Codeplex.
// See http://hawkeye.codeplex.com/workitem/2783

using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

using Hawkeye.Api;

namespace Hawkeye.Plugins.Snapshot
{
    /// <summary>
    /// This extender adds a "Snapshot" button allowing to capture an image of the currently selected control.
    /// </summary>
    public class SnapshotPlugin : IDynamicSubclass
    {
        private IHawkeyeEditor editor = null;
        private ToolBarButton button = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotPlugin"/> class.
        /// </summary>
        public SnapshotPlugin()
        {
            Trace.WriteLine("SnapshotPlugin: Loaded.");
        }

        #region IDynamicSubclass Members

        /// <summary>
        /// Attaches to the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        public void Attach(object target)
        {
            if (target == null)
            {
                Trace.WriteLine("SnapshotPlugin: Can't attach: target is null.");
                return;
            }

            if (!(target is IHawkeyeEditor))
            {
                Trace.WriteLine("SnapshotPlugin: Can't attach: target is not a IHawkeyeEditor.");
                return;
            }

            editor = (IHawkeyeEditor)target;
            ImageList imageList = editor.ToolBar.ImageList;

            imageList.Images.Add(Properties.Resources.Camera);
            int imageIndex = imageList.Images.Count - 1;

            button = new ToolBarButton();
            button.ToolTipText = "Snapshot";
            button.ImageIndex = imageIndex;
            button.Tag = new EventHandler(OnToolBarClick);
            editor.ToolBar.Buttons.Add(button);
        }

        #endregion

        /// <summary>
        /// Called when a button of Hawkeye's toolbar is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnToolBarClick(object sender, EventArgs e)
        {
            object selection = editor.PropertyGrid.SelectedObject;
            if (selection == null)
            {
                Trace.WriteLine("SnapshotExtender: Current selection is null. Nothing to capture.");
                return;
            }

            if (!(selection is Control))
            {
                Trace.WriteLine("SnapshotExtender: Current selection is not a control. Nothing to capture.");
                return;
            }

            Control selectedControl = (Control)selection;
            using (Bitmap image = new Bitmap(selectedControl.Width, selectedControl.Height))
            {
                selectedControl.DrawToBitmap(image, new Rectangle(0, 0, selectedControl.Width, selectedControl.Height));
                Clipboard.SetImage(image);
            }
        }
    }
}
