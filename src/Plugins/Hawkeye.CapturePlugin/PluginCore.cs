using System.Drawing;
using System.Windows.Forms;

namespace Hawkeye.CapturePlugin
{
    internal class PluginCore
    {
        private ToolStripButton button = null;
        private IWindowInfo windowInfo = null;

        public void Initialize(IHawkeyeHost host)
        {
            CreateButton();

            host.CurrentWindowInfoChanged += (s, e) =>
            {
                windowInfo = host.CurrentWindowInfo;
                button.Enabled = CanCapture;
            };
        }

        private bool CanCapture
        {
            get
            {
                return
                    windowInfo != null &&
                    windowInfo.ControlInfo != null &&
                    windowInfo.ControlInfo.Control != null;
            }
        }

        private void CreateButton()
        {
            button = new ToolStripButton("&Capture", Properties.Resources.Camera);
            button.Enabled = CanCapture;
        }

        private void Capture()
        {
            if (!CanCapture) return;

            var control = windowInfo.ControlInfo.Control;
            using (var image = new Bitmap(control.Width, control.Height))
            {
                control.DrawToBitmap(image, new Rectangle(0, 0, control.Width, control.Height));
                Clipboard.SetImage(image);
            }
        }
    }
}
