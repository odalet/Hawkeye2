using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Hawkeye.DemoProject
{
    /// <summary>
    /// Main form
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            string fx = "2"; 
#if DOTNET4
            fx= "4";
#endif

#if X64
            theLabel.TheText = "I am an x64 Test application - " + theLabel.Handle;
#elif X86
            theLabel.TheText = "I am an x86 Test application - " + theLabel.Handle;
#else
            theLabel.TheText = "I am an 'Any CPU' Test application - " + theLabel.Handle;
#endif
            exitButton.Click += delegate { Close(); };
            exitButton.Text += " " + exitButton.Handle;
            base.Text = string.Format("Test: {0} bits; HWND={1} - .NET {2}", 
                8 * Marshal.SizeOf(typeof(IntPtr)),
                Handle,
                fx);
        }

        /// <summary>
        /// Handles the Click event of the showDialogButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void showDialogButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new ChildForm())
                dialog.ShowDialog(this);
        }
    }
}