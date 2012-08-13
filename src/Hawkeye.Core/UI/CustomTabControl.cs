using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Hawkeye.UI
{
    public class CustomTabControl : TabControl
    {
        protected readonly Color DefaultBorderColor = Color.FromArgb(149, 169, 212);
        protected readonly Color DefaultTextColor = Color.FromArgb(77, 103, 162);

        private Color tabBorderColor = Color.Empty;
        private Color textColor = Color.Empty;
        private bool showTabSeparator = true;

        public CustomTabControl()
        {
            tabBorderColor = DefaultBorderColor;
            textColor = DefaultTextColor;

            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        [EditorBrowsable(EditorBrowsableState.Always), DefaultValue(typeof(Color), "149, 169, 212")]
        public Color TabBorderColor { get { return tabBorderColor; } set { tabBorderColor = value; } }

        [EditorBrowsable(EditorBrowsableState.Always), DefaultValue(typeof(Color), "77, 103, 162")]
        public Color TextColor { get { return textColor; } set { textColor = value; } }

        [EditorBrowsable(EditorBrowsableState.Always), DefaultValue("true")]
        public bool ShowTabSeparator
        {
            get { return showTabSeparator; }
            set
            {
                if (showTabSeparator != value)
                {
                    showTabSeparator = value;
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            PaintTabPageBorder(e);
            PaintAllTabs(e);
            PaintSelectedTab(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            var o = base.Parent.PointToClient(base.PointToScreen(new Point(0, 0)));
            pevent.Graphics.TranslateTransform((float)-o.X, (float)-o.Y);
            base.InvokePaintBackground(base.Parent, pevent);
            base.InvokePaint(base.Parent, pevent);
            pevent.Graphics.TranslateTransform((float)o.X, (float)o.Y);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (base.TabCount > 0 && base.SelectedTab != null) base.SelectedTab.Invalidate();
        }

        protected virtual GraphicsPath GetPath(int index)
        {
            var rect = base.GetTabRect(index);
            var gp = new GraphicsPath();

            switch (base.Alignment)
            {
                case TabAlignment.Top:
                    gp.AddLine(rect.Left + 1, rect.Bottom, rect.Left + 1, rect.Top);
                    gp.AddLine(rect.Right, rect.Top, rect.Right, rect.Bottom);
                    break;
                case TabAlignment.Bottom:
                    gp.AddLine(rect.Left + 1, rect.Top - 1, rect.Left + 1, rect.Bottom);
                    gp.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Top - 1);
                    break;
                case TabAlignment.Left:
                    gp.AddLine(rect.Right + 1, rect.Top + 1, rect.Left + 1, rect.Top + 1);
                    gp.AddLine(rect.Left + 1, rect.Bottom, rect.Right, rect.Bottom);
                    break;
                case TabAlignment.Right:
                    gp.AddLine(rect.Left - 1, rect.Top + 1, rect.Right - 2, rect.Top + 1);
                    gp.AddLine(rect.Right - 2, rect.Bottom, rect.Left - 1, rect.Bottom);
                    break;
            }

            return gp;
        }

        protected virtual void PaintTab(PaintEventArgs e, int index)
        {
            PaintTabText(e.Graphics, index);
        }

        protected virtual void PaintTabSeparator(PaintEventArgs e, int index)
        {
            var bounds = base.GetTabRect(index);
            var bounds2 = base.GetTabRect(index - 1);

            if ((base.Alignment == TabAlignment.Top) || (base.Alignment == TabAlignment.Bottom))
            {
                var gap = bounds.Left - bounds2.Right;
                float x = bounds.Left - gap / 2f;

                using (Pen pen = new Pen(tabBorderColor))
                    e.Graphics.DrawLine(pen, x, bounds.Top + 3f, x, bounds.Bottom - 3f);
            }
            else
            {
                var gap = bounds.Top - bounds2.Bottom;
                float y = bounds.Top - gap / 2f;

                using (Pen pen = new Pen(tabBorderColor))
                    e.Graphics.DrawLine(pen, bounds.Left + 3f, y, bounds.Right - 3f, y);
            }
        }

        protected virtual void PaintTabBorder(Graphics graph, int index, GraphicsPath path)
        {
            using (var pen = new Pen(tabBorderColor)) graph.DrawPath(pen, path);
        }

        private Image GetTabImage(int index)
        {
            if (base.ImageList == null) return null;
            var tab = base.TabPages[index];

            if ((tab.ImageIndex < 0) && string.IsNullOrEmpty(tab.ImageKey))
                return null;

            if (tab.ImageIndex >= 0)
                return base.ImageList.Images[tab.ImageIndex];
            else return base.ImageList.Images[tab.ImageKey];
        }

        protected virtual void PaintTabText(Graphics g, int index)
        {
            float gap = 0;
            var image = GetTabImage(index);
            var rect = base.GetTabRect(index);
            var bounds = new RectangleF(
                (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);

            if (image != null)
            {
                if (IsHorizontal)
                    gap = (bounds.Height - (float)image.Height) / 2f;
                else gap = (bounds.Width - (float)image.Width) / 2f;
            }

            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            sf.FormatFlags |= StringFormatFlags.LineLimit;
            if (!IsHorizontal)
                sf.FormatFlags |= StringFormatFlags.DirectionVertical;

            if (image == null) // simple: just center the text in the tab
            {
                using (SolidBrush brush = new SolidBrush(textColor)) g.DrawString(
                   base.TabPages[index].Text, Font, brush, (RectangleF)base.GetTabRect(index), sf);
            }
            else // We have an image, we try to measure the text.
            {
                var rectf = (RectangleF)base.GetTabRect(index);
                var text = base.TabPages[index].Text;
                var maxAvailableArea = SizeF.Empty;
                if (IsHorizontal)
                    maxAvailableArea = new SizeF(rectf.Width - 3f * gap - (float)image.Width, (float)image.Height);
                else maxAvailableArea = new SizeF((float)image.Width, rectf.Height - 3f * gap - (float)image.Height);

                var textSize = g.MeasureString(text, Font, maxAvailableArea, sf);

                if (textSize.Width > maxAvailableArea.Width) textSize.Width = maxAvailableArea.Width;
                if (textSize.Height > maxAvailableArea.Height) textSize.Height = maxAvailableArea.Height;

                var imageAndTextSize = SizeF.Empty;
                if (IsHorizontal)
                {
                    imageAndTextSize.Width = (float)image.Width + gap + textSize.Width;
                    imageAndTextSize.Height = (float)image.Height > textSize.Height ?
                        (float)image.Height : textSize.Height;
                }
                else
                {
                    imageAndTextSize.Height = (float)image.Height + gap + textSize.Height;
                    imageAndTextSize.Width = (float)image.Width > textSize.Width ?
                        (float)image.Width : textSize.Width;
                }

                var imageAndTextBounds = new RectangleF(
                    bounds.X + (bounds.Width - imageAndTextSize.Width) / 2f,
                    bounds.Y + (bounds.Height - imageAndTextSize.Height) / 2f,
                    imageAndTextSize.Width, imageAndTextSize.Height);

                // Draw the image
                var imageBounds = new Rectangle(
                    (int)imageAndTextBounds.X, (int)imageAndTextBounds.Y,
                    image.Width, image.Height);

                g.DrawImageUnscaledAndClipped(image, imageBounds);

                // Draw the text
                var textBounds = RectangleF.Empty;
                if (IsHorizontal) textBounds = new RectangleF(
                    imageAndTextBounds.X + (float)image.Width + gap,
                    imageAndTextBounds.Y,
                    imageAndTextBounds.Width - ((float)image.Width + gap),
                    imageAndTextBounds.Height);
                else textBounds = new RectangleF(
                   imageAndTextBounds.X,
                   imageAndTextBounds.Y + (float)image.Height + gap,
                   imageAndTextBounds.Width,
                   imageAndTextBounds.Height - ((float)image.Height + gap));

                using (var brush = new SolidBrush(textColor)) g.DrawString(
                   base.TabPages[index].Text, Font, brush, textBounds, sf);
            }
        }

        private bool IsHorizontal
        {
            get { return (base.Alignment == TabAlignment.Top) || (base.Alignment == TabAlignment.Bottom); }
        }

        protected virtual void PaintSelectedTab(PaintEventArgs e)
        {
            if (base.SelectedIndex < 0) return;

            var rect = base.GetTabRect(base.SelectedIndex);
            using (GraphicsPath path = GetPath(base.SelectedIndex))
            {
                switch (base.Alignment)
                {
                    case TabAlignment.Top:
                        path.AddLine(rect.Right, rect.Bottom + 2, rect.Left + 2, rect.Bottom + 2);
                        break;
                    case TabAlignment.Bottom:
                        path.AddLine(rect.Right, rect.Top - 3, rect.Left + 2, rect.Top - 3);
                        break;
                    case TabAlignment.Left:
                        path.AddLine(rect.Right + 2, rect.Bottom, rect.Right + 2, rect.Top + 2);
                        break;
                    case TabAlignment.Right:
                        path.AddLine(rect.Left - 3, rect.Bottom, rect.Left - 3, rect.Top + 2);
                        break;
                }

                e.Graphics.FillPath(Brushes.White, path);
                PaintTabBorder(e.Graphics, base.SelectedIndex, path);
                PaintTabText(e.Graphics, base.SelectedIndex);
            }
        }

        protected virtual void PaintTabPageBorder(PaintEventArgs e)
        {
            if (base.TabCount <= 0) return;

            var rect = new Rectangle(
                new Point(base.SelectedTab.Left, base.SelectedTab.Top), base.SelectedTab.Size);
            rect.Inflate(1, 1);
            ControlPaint.DrawBorder(e.Graphics, rect, tabBorderColor, ButtonBorderStyle.Solid);
        }

        private void PaintAllTabs(PaintEventArgs e)
        {
            for (int i = base.TabCount - 1; i >= 0; i--)
            {
                PaintTab(e, i);
                if (ShouldPaintTabSeparator(i)) PaintTabSeparator(e, i);
            }
        }

        /// <summary>
        /// Get a value indicating if we should paint a tab separator <c>before</c>
        /// the tab identified by the index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The tab index.</param>
        /// <returns></returns>
        private bool ShouldPaintTabSeparator(int index)
        {
            return ShowTabSeparator &&
                base.TabCount > 2 &&
                index != base.SelectedIndex &&
                index - 1 != base.SelectedIndex &&
                index != 0;
        }
    }
}
