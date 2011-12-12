using System;
using System.Windows.Forms;

namespace Hawkeye.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Inject(Clr clr, Bitness bitness)
        {
        }

        private void injectN2x86Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net2, Bitness.x86);
        }

        private void injectN2X64Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net2, Bitness.x64);
        }

        private void injectN4x86Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net4, Bitness.x86);
        }

        private void injectN4x64Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net4, Bitness.x64);
        }
    }
}
