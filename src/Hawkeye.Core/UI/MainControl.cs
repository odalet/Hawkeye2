using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

using Hawkeye.WinApi;
using Hawkeye.Logging;
using Hawkeye.ComponentModel;

namespace Hawkeye.UI
{
    /// <summary>
    /// The main Hawkeye UI control.
    /// </summary>
    internal partial class MainControl : UserControl
    {
        private static readonly ILogService log = LogManager.GetLogger<MainControl>();

        ////private WindowInfo currentInfo = null;
        private History<WindowInfo> history = new History<WindowInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainControl"/> class.
        /// </summary>
        public MainControl()
        {
            InitializeComponent();
            
            // Remove the .NET Tab (to hide it)
            tabs.SuspendLayout();
            tabs.TabPages.Remove(dotNetTabPage);
            tabs.ResumeLayout(false);

            dotNetPropertyGrid.ActionClicked += (s, e) => HandleDotNetPropertyGridAction(e.Action);
            dotNetPropertyGrid.ControlCreated += (s, e) => RefreshDotNetPropertyGridActions();
        }

        /// <summary>
        /// Occurs when the <see cref="CurrentInfo"/> member changes.
        /// </summary>
        public event EventHandler CurrentInfoChanged;

        /// <summary>
        /// Gets or sets the target Window handle.
        /// </summary>
        /// <value>
        /// The handle of the spied window.
        /// </value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr Target
        {
            get
            {
                return CurrentInfo == null ? IntPtr.Zero : CurrentInfo.Handle;
            }
            set
            {
                if (value != IntPtr.Zero)
                    BuildCurrentWindowInfo(value);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;


            windowFinderControl.ActiveWindowChanged += (s, _) =>
                hwndBox.Text = windowFinderControl.ActiveWindowHandle.ToString();

            windowFinderControl.WindowSelected += (s, _) =>
            {
                var hwnd = windowFinderControl.ActiveWindowHandle;
                if (hwnd == IntPtr.Zero) CurrentInfo = null;
                else BuildCurrentWindowInfo(hwnd);
            };
        }

        public WindowInfo CurrentInfo
        {
            get { return history.Current; }
            set
            {
                history.Push(value);
                OnCurrentInfoChanged();
            }
        }

        protected void OnCurrentInfoChanged()
        {
            nativePropertyGrid.SelectedObject = CurrentInfo;

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

            // Update the hwnd box in case we detected .NET properties.
            hwndBox.Text = CurrentInfo.ToShortString();

            if (CurrentInfoChanged != null)
                CurrentInfoChanged(this, EventArgs.Empty);
        }

        private void FillControlInfo(IControlInfo controlInfo)
        {
            dotNetPropertyGrid.SelectedObject = controlInfo;
            RefreshDotNetPropertyGridActions();
        }

        #region DotNetPropertyGridAction Handling

        private void HandleDotNetPropertyGridAction(DotNetPropertyGridAction action)
        {
            switch (action)
            {
                case DotNetPropertyGridAction.Previous:
                    if (history.HasPrevious) history.MoveToPrevious();
                    OnCurrentInfoChanged();
                    break;
                case DotNetPropertyGridAction.Next:
                    if (history.HasNext) history.MoveToNext();
                    OnCurrentInfoChanged();
                    break;
                case DotNetPropertyGridAction.Parent:
                    if (CanExecuteAction(DotNetPropertyGridAction.Parent))
                        Target = CurrentInfo.ControlInfo.Control.Parent.Handle;
                    break;
                case DotNetPropertyGridAction.Highlight:
                    if (CanExecuteAction(DotNetPropertyGridAction.Highlight))
                        WindowHelper.DrawAdorner(Target);
                    break;
            }
        }

        private bool CanExecuteAction(DotNetPropertyGridAction action)
        {
            switch (action)
            {
                case DotNetPropertyGridAction.Previous: return history.HasPrevious;                    
                case DotNetPropertyGridAction.Next: return history.HasNext;
                case DotNetPropertyGridAction.Parent:
                    return 
                        CurrentInfo != null &&
                        CurrentInfo.ControlInfo != null &&
                        CurrentInfo.ControlInfo.Control != null &&
                        CurrentInfo.ControlInfo.Control.Parent != null;
                case DotNetPropertyGridAction.Highlight:
                    return Target != IntPtr.Zero;
            }

            return false;
        }

        private void RefreshDotNetPropertyGridActions()
        {
            foreach (var action in Enum.GetValues(typeof(DotNetPropertyGridAction)).Cast<DotNetPropertyGridAction>())
                dotNetPropertyGrid.EnableAction(action, CanExecuteAction(action));
        }
        
        #endregion

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

        /// <summary>
        /// Dumps the currently selected window information.
        /// </summary>
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

        /// <summary>
        /// Gets the name of the file to save log to.
        /// </summary>
        /// <returns>A file name.</returns>
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

        /// <summary>
        /// Handles the Click event of the dumpButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void dumpButton_Click(object sender, EventArgs e) { Dump(); }
    }
}
