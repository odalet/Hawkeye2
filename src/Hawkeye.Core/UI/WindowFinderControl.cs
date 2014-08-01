using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Hawkeye.WinApi;

namespace Hawkeye.UI
{
    /// <summary>
    /// The "Window finder" user control.
    /// </summary>
    [DefaultEvent("ActiveWindowChanged")]
    internal partial class WindowFinderControl : UserControl
    {
        private bool searching = false;
        
        private IntPtr windowHandle = IntPtr.Zero;
        private Point lastLocationOnScreen = Point.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowFinderControl"/> class.
        /// </summary>
        public WindowFinderControl()
        {
            InitializeComponent();
        }

        public event EventHandler ActiveWindowChanged;
        public event EventHandler WindowSelected;

        public IntPtr ActiveWindowHandle
        {
            get { return windowHandle; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;
            
            base.MouseDown += (s, _) => { if (!searching) StartSearch(); };
            base.MouseMove += (s, ev) =>
            {
                if (searching) Search(ev.Location);
                //if (!searching) StopSearch();
                //else Search(ev.Location);
            };
            base.MouseUp += (s, _) => StopSearch();
        }

        private void StartSearch()
        {
            searching = true;
            Cursor.Current = CursorHelper.LoadFrom(Properties.Resources.TargetIcon);
            base.Capture = true;
        }

        private void Search(Point mouseLocation)
        {
            // Grab the window from the screen location of the mouse.
            var locationOnScreen = PointToScreen(mouseLocation);
            var foundWindowHandle = WindowHelper.FindWindow(locationOnScreen);
            
            // We found a handle.
            if (foundWindowHandle != IntPtr.Zero)
            {
                // give it another try, it might be a child window (disabled, hidden .. something else)
                // offset the point to be a client point of the active window
                var locationInWindow = WindowHelper.ScreenToClient(foundWindowHandle, locationOnScreen);
                if (locationInWindow != Point.Empty)
                {
                    // check if there is some hidden/disabled child window at this point
                    IntPtr childWindowHandle = WindowHelper.FindChildWindow(foundWindowHandle, locationInWindow);
                    if (childWindowHandle != IntPtr.Zero) foundWindowHandle = childWindowHandle;
                }
            }

            // Is this the same window as the last detected one?
            if (lastLocationOnScreen != locationOnScreen)
            {
                lastLocationOnScreen = locationOnScreen;
                if (windowHandle != foundWindowHandle)
                {
                    if (windowHandle != IntPtr.Zero)
                        WindowHelper.RemoveAdorner(windowHandle); // Remove highlight

                    windowHandle = foundWindowHandle;
                    WindowHelper.DrawAdorner(windowHandle); // highlight the window                    
                    OnActiveWindowChanged();
                }
            }
        }

        private void StopSearch()
        {
            searching = false;
            base.Cursor = Cursors.Default;
            base.Capture = false;

            if (windowHandle != IntPtr.Zero)
                WindowHelper.RemoveAdorner(windowHandle); // Remove highlight
            OnWindowSelected();
        }

        private void OnWindowSelected()
        {
            if (WindowSelected != null) WindowSelected(this, EventArgs.Empty);
        }

        private void OnActiveWindowChanged()
        {
            if (ActiveWindowChanged != null) ActiveWindowChanged(this, EventArgs.Empty);
        }        
    }
}
