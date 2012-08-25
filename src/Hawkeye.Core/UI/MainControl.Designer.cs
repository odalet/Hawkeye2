namespace Hawkeye.UI
{
    partial class MainControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainControl));
            this.panel1 = new System.Windows.Forms.Panel();
            this.innerPanel = new Hawkeye.UI.HawkeyePanel();
            this.hwndBox = new System.Windows.Forms.TextBox();
            this.dumpButton = new System.Windows.Forms.Button();
            this.windowFinderControl = new Hawkeye.UI.WindowFinderControl();
            this.tabs = new Hawkeye.UI.HawkeyeTabControl();
            this.nativeTabPage = new System.Windows.Forms.TabPage();
            this.nativePropertyGrid = new Hawkeye.UI.NativePropertyGrid();
            this.dotNetTabPage = new System.Windows.Forms.TabPage();
            this.dotNetPropertyGrid = new Hawkeye.UI.DotNetPropertyGrid();
            this.panel1.SuspendLayout();
            this.innerPanel.SuspendLayout();
            this.tabs.SuspendLayout();
            this.nativeTabPage.SuspendLayout();
            this.dotNetTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.innerPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 40);
            this.panel1.TabIndex = 12;
            // 
            // innerPanel
            // 
            this.innerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.innerPanel.BackColor = System.Drawing.Color.White;
            this.innerPanel.Controls.Add(this.hwndBox);
            this.innerPanel.Controls.Add(this.dumpButton);
            this.innerPanel.Controls.Add(this.windowFinderControl);
            this.innerPanel.Location = new System.Drawing.Point(3, 2);
            this.innerPanel.Name = "innerPanel";
            this.innerPanel.Size = new System.Drawing.Size(764, 38);
            this.innerPanel.TabIndex = 13;
            // 
            // hwndBox
            // 
            this.hwndBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.hwndBox.Location = new System.Drawing.Point(42, 13);
            this.hwndBox.Name = "hwndBox";
            this.hwndBox.ReadOnly = true;
            this.hwndBox.Size = new System.Drawing.Size(635, 20);
            this.hwndBox.TabIndex = 9;
            // 
            // dumpButton
            // 
            this.dumpButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dumpButton.Location = new System.Drawing.Point(683, 11);
            this.dumpButton.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new System.Drawing.Size(75, 23);
            this.dumpButton.TabIndex = 10;
            this.dumpButton.Text = "Dump...";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new System.EventHandler(this.dumpButton_Click);
            // 
            // windowFinderControl
            // 
            this.windowFinderControl.BackColor = System.Drawing.Color.Transparent;
            this.windowFinderControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("windowFinderControl.BackgroundImage")));
            this.windowFinderControl.Location = new System.Drawing.Point(4, 6);
            this.windowFinderControl.Name = "windowFinderControl";
            this.windowFinderControl.Size = new System.Drawing.Size(32, 32);
            this.windowFinderControl.TabIndex = 12;
            // 
            // tabs
            // 
            this.tabs.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabs.Controls.Add(this.nativeTabPage);
            this.tabs.Controls.Add(this.dotNetTabPage);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 40);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.ShowTabSeparator = true;
            this.tabs.Size = new System.Drawing.Size(770, 427);
            this.tabs.TabIndex = 12;
            // 
            // nativeTabPage
            // 
            this.nativeTabPage.Controls.Add(this.nativePropertyGrid);
            this.nativeTabPage.Location = new System.Drawing.Point(4, 4);
            this.nativeTabPage.Name = "nativeTabPage";
            this.nativeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.nativeTabPage.Size = new System.Drawing.Size(762, 398);
            this.nativeTabPage.TabIndex = 0;
            this.nativeTabPage.Text = "Native";
            this.nativeTabPage.UseVisualStyleBackColor = true;
            // 
            // nativePropertyGrid
            // 
            this.nativePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nativePropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.nativePropertyGrid.Name = "nativePropertyGrid";
            this.nativePropertyGrid.Size = new System.Drawing.Size(756, 392);
            this.nativePropertyGrid.TabIndex = 11;
            // 
            // dotNetTabPage
            // 
            this.dotNetTabPage.Controls.Add(this.dotNetPropertyGrid);
            this.dotNetTabPage.Location = new System.Drawing.Point(4, 4);
            this.dotNetTabPage.Name = "dotNetTabPage";
            this.dotNetTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.dotNetTabPage.Size = new System.Drawing.Size(762, 398);
            this.dotNetTabPage.TabIndex = 1;
            this.dotNetTabPage.Text = ".NET";
            this.dotNetTabPage.UseVisualStyleBackColor = true;
            // 
            // dotNetPropertyGrid
            // 
            this.dotNetPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dotNetPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.dotNetPropertyGrid.Name = "dotNetPropertyGrid";
            this.dotNetPropertyGrid.Size = new System.Drawing.Size(756, 392);
            this.dotNetPropertyGrid.TabIndex = 0;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.panel1);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(770, 467);
            this.panel1.ResumeLayout(false);
            this.innerPanel.ResumeLayout(false);
            this.innerPanel.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.nativeTabPage.ResumeLayout(false);
            this.dotNetTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NativePropertyGrid nativePropertyGrid;
        private System.Windows.Forms.Button dumpButton;
        private System.Windows.Forms.TextBox hwndBox;
        private WindowFinderControl windowFinderControl;
        private HawkeyeTabControl tabs;
        private System.Windows.Forms.TabPage nativeTabPage;
        private System.Windows.Forms.TabPage dotNetTabPage;
        private DotNetPropertyGrid dotNetPropertyGrid;
        private HawkeyePanel innerPanel;
        private System.Windows.Forms.Panel panel1;
    }
}
