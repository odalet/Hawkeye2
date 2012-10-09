using System;
using System.IO;
using System.Windows.Forms;
using Hawkeye.Configuration;

namespace Hawkeye.UI
{
    public partial class AboutForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutForm" /> class.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
            LayoutManager.RegisterForm("HawkeyeAboutForm", this);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            applicationDataDirectoryBox.Text = 
                HawkeyeApplication.ApplicationInfo.ApplicationDataDirectory;
        }
    }
}
