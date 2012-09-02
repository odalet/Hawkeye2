using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Hawkeye.UI.Controls
{
    /// <summary>
    /// Hawkeye-style Tab control
    /// </summary>
    internal class TabControlEx : CustomTabControl
    {
        private const int outerBezier = 5;
        private const int innerBezier = 2;

        private int bezierInnerCurveOffset = innerBezier;
        private int bezierOuterCurveOffset = outerBezier;

        /// <summary>
        /// Gets or sets the bezier inner curve offset.
        /// </summary>
        /// <value>
        /// The bezier inner curve offset.
        /// </value>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
         EditorBrowsable(EditorBrowsableState.Always), DefaultValue(innerBezier)]
        public int BezierInnerCurveOffset 
        { 
            get { return bezierInnerCurveOffset; } 
            set { bezierInnerCurveOffset = value; } 
        }

        /// <summary>
        /// Gets or sets the bezier outer curve offset.
        /// </summary>
        /// <value>
        /// The bezier outer curve offset.
        /// </value>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
         EditorBrowsable(EditorBrowsableState.Always), DefaultValue(outerBezier)]
        public int BezierOuterCurveOffset 
        { 
            get { return bezierOuterCurveOffset; } 
            set { bezierOuterCurveOffset = value; } 
        }

        /// <summary>
        /// Gets a graphics path representing how to draw the tab at the specified index.
        /// </summary>
        /// <param name="index">The tab index.</param>
        /// <returns>
        /// A <see cref="GraphicsPath"/> object.
        /// </returns>
        protected override GraphicsPath GetPath(int index)
        {
            var rect = base.GetTabRect(index);
            var gp = new GraphicsPath();

            var ptBottomLeft = Point.Empty;
            var ptTopLeft = Point.Empty;
            var ptTopRight = Point.Empty;
            var ptBottomRight = Point.Empty;

            var ptB1 = Point.Empty;
            var ptB2 = Point.Empty;
            var ptB3 = Point.Empty;
            var ptB4 = Point.Empty;

            switch (Alignment)
            {
                case TabAlignment.Top:
                    ptBottomLeft = new Point(rect.Left + 1, rect.Bottom);
                    ptTopLeft = new Point(rect.Left + 1, rect.Top);
                    ptTopRight = new Point(rect.Right, rect.Top);
                    ptBottomRight = new Point(rect.Right, rect.Bottom);

                    ptB1 = Point.Add(ptTopLeft, new Size(0, bezierOuterCurveOffset));
                    ptB2 = Point.Add(ptTopLeft, new Size(0, bezierInnerCurveOffset));
                    ptB3 = Point.Add(ptTopLeft, new Size(bezierInnerCurveOffset, 0));
                    ptB4 = Point.Add(ptTopLeft, new Size(bezierOuterCurveOffset, 0));

                    gp.AddLine(ptBottomLeft, ptB1);
                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);

                    ptB1 = Point.Add(ptTopRight, new Size(-bezierOuterCurveOffset, 0));
                    ptB2 = Point.Add(ptTopRight, new Size(-bezierInnerCurveOffset, 0));
                    ptB3 = Point.Add(ptTopRight, new Size(0, bezierInnerCurveOffset));
                    ptB4 = Point.Add(ptTopRight, new Size(0, bezierOuterCurveOffset));

                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);
                    gp.AddLine(ptB4, ptBottomRight);
                    break;

                case TabAlignment.Bottom:
                    ptBottomLeft = new Point(rect.Left + 1, rect.Bottom);
                    ptTopLeft = new Point(rect.Left + 1, rect.Top - 1);
                    ptTopRight = new Point(rect.Right, rect.Top - 1);
                    ptBottomRight = new Point(rect.Right, rect.Bottom);

                    ptB1 = Point.Add(ptBottomLeft, new Size(0, -bezierOuterCurveOffset));
                    ptB2 = Point.Add(ptBottomLeft, new Size(0, -bezierInnerCurveOffset));
                    ptB3 = Point.Add(ptBottomLeft, new Size(bezierInnerCurveOffset, 0));
                    ptB4 = Point.Add(ptBottomLeft, new Size(bezierOuterCurveOffset, 0));

                    gp.AddLine(ptTopLeft, ptB1);
                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);

                    ptB1 = Point.Add(ptBottomRight, new Size(-bezierOuterCurveOffset, 0));
                    ptB2 = Point.Add(ptBottomRight, new Size(-bezierInnerCurveOffset, 0));
                    ptB3 = Point.Add(ptBottomRight, new Size(0, -bezierInnerCurveOffset));
                    ptB4 = Point.Add(ptBottomRight, new Size(0, -bezierOuterCurveOffset));

                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);
                    gp.AddLine(ptB4, ptTopRight);
                    break;

                case TabAlignment.Left:
                    ptBottomLeft = new Point(rect.Left, rect.Bottom);
                    ptTopLeft = new Point(rect.Left, rect.Top + 1);
                    ptTopRight = new Point(rect.Right + 1, rect.Top + 1);
                    ptBottomRight = new Point(rect.Right + 1, rect.Bottom);

                    ptB1 = Point.Add(ptTopLeft, new Size(bezierOuterCurveOffset, 0));
                    ptB2 = Point.Add(ptTopLeft, new Size(bezierInnerCurveOffset, 0));
                    ptB3 = Point.Add(ptTopLeft, new Size(0, bezierInnerCurveOffset));
                    ptB4 = Point.Add(ptTopLeft, new Size(0, bezierOuterCurveOffset));

                    gp.AddLine(ptTopRight, ptB1);
                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);

                    ptB1 = Point.Add(ptBottomLeft, new Size(0, -bezierOuterCurveOffset));
                    ptB2 = Point.Add(ptBottomLeft, new Size(0, -bezierInnerCurveOffset));
                    ptB3 = Point.Add(ptBottomLeft, new Size(bezierInnerCurveOffset, 0));
                    ptB4 = Point.Add(ptBottomLeft, new Size(bezierOuterCurveOffset, 0));

                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);
                    gp.AddLine(ptB4, ptBottomRight);
                    break;

                case TabAlignment.Right:
                    ptBottomLeft = new Point(rect.Left - 2, rect.Bottom);
                    ptTopLeft = new Point(rect.Left - 2, rect.Top + 1);
                    ptTopRight = new Point(rect.Right, rect.Top + 1);
                    ptBottomRight = new Point(rect.Right, rect.Bottom);

                    ptB1 = Point.Add(ptTopRight, new Size(-bezierOuterCurveOffset, 0));
                    ptB2 = Point.Add(ptTopRight, new Size(-bezierInnerCurveOffset, 0));
                    ptB3 = Point.Add(ptTopRight, new Size(0, bezierInnerCurveOffset));
                    ptB4 = Point.Add(ptTopRight, new Size(0, bezierOuterCurveOffset));

                    gp.AddLine(ptTopLeft, ptB1);
                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);

                    ptB1 = Point.Add(ptBottomRight, new Size(0, -bezierOuterCurveOffset));
                    ptB2 = Point.Add(ptBottomRight, new Size(0, -bezierInnerCurveOffset));
                    ptB3 = Point.Add(ptBottomRight, new Size(-bezierInnerCurveOffset, 0));
                    ptB4 = Point.Add(ptBottomRight, new Size(-bezierOuterCurveOffset, 0));

                    gp.AddBezier(ptB1, ptB2, ptB3, ptB4);
                    gp.AddLine(ptB4, ptBottomLeft);
                    break;
            }

            return gp;
        }
    }
}
