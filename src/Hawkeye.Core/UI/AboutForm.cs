using System;
using System.Reflection;
using System.Windows.Forms;

using Hawkeye.Logging;

namespace Hawkeye.UI
{
    public partial class AboutForm : Form
    {
        private static readonly ILogService log = LogManager.GetLogger<AboutForm>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutForm" /> class.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
            //LayoutManager.RegisterForm("HawkeyeAboutForm", this);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            versionBox.Text = GetVersion();

            applicationDataDirectoryBox.Text =
                HawkeyeApplication.Shell.ApplicationInfo.ApplicationDataDirectory;
        }

        private static string GetVersion()
        {
            try
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
            catch (Exception ex)
            {
                log.Error(string.Format(
                    "Could not retrieve assembly version: {0}", ex.Message), ex);
            }

            return string.Empty;
        }
    }
}
