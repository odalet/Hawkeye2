using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using Hawkeye.UI.Controls;

namespace Hawkeye.UI
{
    /// <summary>
    /// Specialized tab control with a border drawn on three sides (bottom, left and right)
    /// </summary>
    internal class HawkeyeTabControl : TabControlEx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HawkeyeTabControl" /> class.
        /// </summary>
        public HawkeyeTabControl() : base() { }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            var topRectangle = new Rectangle(
                    base.SelectedTab.Left, 0, base.SelectedTab.Width, base.SelectedTab.Top);
            pevent.Graphics.FillRectangle(Brushes.White, topRectangle);
        }

        /// <summary>
        /// Paints the tab page border.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.</param>
        protected override void PaintTabPageBorder(PaintEventArgs e)
        {
            if (base.TabCount <= 0) return;

            var x1 = base.SelectedTab.Left - 1;
            var y1 = 0;
            var x2 = base.SelectedTab.Width + base.SelectedTab.Left;
            var y2 = base.SelectedTab.Height + base.SelectedTab.Top;

            var savedSmoothing = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            using (var p = new Pen(base.TabBorderColor, -1f))
            {
                e.Graphics.DrawLine(p, x1, y1, x1, y2);
                e.Graphics.DrawLine(p, x1, y2, x2, y2);
                e.Graphics.DrawLine(p, x2, y1, x2, y2);
            }
            e.Graphics.SmoothingMode = savedSmoothing;
        }
    }
}
