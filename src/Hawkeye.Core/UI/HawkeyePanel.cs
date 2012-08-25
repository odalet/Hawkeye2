using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Hawkeye.UI
{
    /// <summary>
    /// Specialized panel with a border drawn on three sides (top, left and right)
    /// </summary>
    internal class HawkeyePanel : Panel
    {
        private readonly Color borderColor = Color.FromArgb(149, 169, 212);

        /// <summary>
        /// Initializes a new instance of the <see cref="HawkeyePanel" /> class.
        /// </summary>
        public HawkeyePanel() : base()
        {
            base.BorderStyle = BorderStyle.None;
            base.BackColor = Color.White;
            
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true); 
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.None;
            var rect = new Rectangle(0, 0, base.Width, base.Height);
            ControlPaint.DrawBorder(e.Graphics, rect,
                borderColor, 1, ButtonBorderStyle.Solid,    // left
                borderColor, 1, ButtonBorderStyle.Solid,    // top
                borderColor, 1, ButtonBorderStyle.Solid,    // right
                borderColor, 0, ButtonBorderStyle.Solid);   // bottom

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }
    }
}
