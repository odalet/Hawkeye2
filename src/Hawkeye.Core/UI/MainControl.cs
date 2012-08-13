using System;
using System.IO;
using System.Windows.Forms;
using Hawkeye.WinApi;
using Hawkeye.Logging;
using System.Diagnostics;

namespace Hawkeye.UI
{
    public partial class MainControl : UserControl
    {
        private static readonly ILogService log = LogManager.GetLogger<MainControl>();

        private WindowInfo currentInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainControl"/> class.
        /// </summary>
        public MainControl()
        {
            InitializeComponent();
        }

        public void SetTarget(IntPtr hwnd)
        {
            BuildCurrentWindowInfo(hwnd);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            tabs.TabPages.Remove(dotNetTabPage);

            windowFinderControl.ActiveWindowChanged += (s, _) =>
                hwndBox.Text = windowFinderControl.ActiveWindowHandle.ToString();

            windowFinderControl.WindowSelected += (s, _) =>
            {
                var hwnd = windowFinderControl.ActiveWindowHandle;
                if (hwnd == IntPtr.Zero) CurrentInfo = null;
                else BuildCurrentWindowInfo(hwnd);
            };
        }

        private WindowInfo CurrentInfo
        {
            get { return currentInfo; }
            set
            {
                if (currentInfo == value) return;
                currentInfo = value;
                OnCurrentInfoChanged();
            }
        }

        private void OnCurrentInfoChanged()
        {
            nativePropertyGrid.SelectedObject = CurrentInfo;
            //pgrid.ExpandAllGridItems(); // this takes too much time!
            dumpButton.Enabled = CurrentInfo != null;
            if (CurrentInfo == null) return; // nope

            hwndBox.Text = CurrentInfo.ToShortString();

            // Inject ourself if possible
            if (HawkeyeApplication.CanInject(CurrentInfo))
            {
                HawkeyeApplication.Inject(CurrentInfo);
                return;
            }

            // Injection was not needed.

            CurrentInfo.DetectDotNetProperties();
            var controlInfo = CurrentInfo.ControlInfo;
            if (controlInfo != null)
            {
                if (!tabs.TabPages.Contains(dotNetTabPage))
                    tabs.TabPages.Add(dotNetTabPage);
                FillControlInfo(controlInfo);
                tabs.SelectedTab = dotNetTabPage;
            }
            else
            {
                if (tabs.TabPages.Contains(dotNetTabPage))
                    tabs.TabPages.Remove(dotNetTabPage);
                tabs.SelectedTab = nativeTabPage;
            }
        }
        
        private void FillControlInfo(IControlInfo controlInfo)
        {
            dotNetPropertyGrid.SelectedObject = controlInfo;
        }

        private void BuildCurrentWindowInfo(IntPtr hwnd)
        {
            base.Cursor = Cursors.WaitCursor;
            base.Enabled = false;
            base.Refresh();
            try
            {
                WindowInfo info = null;
                try
                {
                    info = new WindowInfo(hwnd);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format(
                        "Error while building window info: {0}", ex.Message), ex);
                }

                CurrentInfo = info;
            }
            finally
            {
                base.Enabled = true;
                base.Cursor = Cursors.Default;
            }
        }

        private void Dump()
        {
            if (CurrentInfo == null)
            {
                ErrorBox.Show(this, "Can't dump if no window selected.");
                return;
            }

            var filename = GetFileName();
            if (string.IsNullOrEmpty(filename)) return;

            try
            {
                var text = CurrentInfo.Dump();
                using (var writer = File.CreateText(filename))
                {
                    writer.Write(text);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                var message = string.Format("Could not create dump file for handle {0}: {1}", CurrentInfo.Handle, ex.Message);
                log.Error(message, ex);
                ErrorBox.Show(this, message);
            }
        }

        private string GetFileName()
        {
            using (var dialog = new SaveFileDialog()
            {
                //FileName = string.Format("dump_{0}.log", CurrentInfo.Hwnd),
                FileName = "dump.log",
                Filter = "Log files|*.log|All files|*.*"
            })
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    return dialog.FileName;
                else return string.Empty;
            }
        }

        private void dumpButton_Click(object sender, EventArgs e) { Dump(); }
    }
}
