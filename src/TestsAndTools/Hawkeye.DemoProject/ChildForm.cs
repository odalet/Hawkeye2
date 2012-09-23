using System;
using System.Windows.Forms;

namespace Hawkeye.DemoProject
{
    public partial class ChildForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildForm" /> class.
        /// </summary>
        public ChildForm()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;
            Left += 10;
            Top += 10;
        }

        /// <summary>
        /// Handles the Click event of the showMeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void showMeButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new ChildForm())
            {
                dialog.ShowDialog(this);
            }
        }
    }
}
