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
            this.dumpButton = new System.Windows.Forms.Button();
            this.hwndBox = new System.Windows.Forms.TextBox();
            this.rootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.windowFinderControl = new Hawkeye.UI.WindowFinderControl();
            this.topRightPanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.tabs = new Hawkeye.UI.TabControlEx();
            this.nativeTabPage = new System.Windows.Forms.TabPage();
            this.nativePropertyGrid = new Hawkeye.UI.NativePropertyGrid();
            this.dotNetTabPage = new System.Windows.Forms.TabPage();
            this.dotNetPropertyGrid = new Hawkeye.UI.DotNetPropertyGrid();
            this.rootPanel.SuspendLayout();
            this.topRightPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.tabs.SuspendLayout();
            this.nativeTabPage.SuspendLayout();
            this.dotNetTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // dumpButton
            // 
            this.dumpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dumpButton.Location = new System.Drawing.Point(402, 6);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new System.Drawing.Size(75, 23);
            this.dumpButton.TabIndex = 10;
            this.dumpButton.Text = "Dump...";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new System.EventHandler(this.dumpButton_Click);
            // 
            // hwndBox
            // 
            this.hwndBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hwndBox.Location = new System.Drawing.Point(3, 8);
            this.hwndBox.Name = "hwndBox";
            this.hwndBox.ReadOnly = true;
            this.hwndBox.Size = new System.Drawing.Size(393, 20);
            this.hwndBox.TabIndex = 9;
            // 
            // rootPanel
            // 
            this.rootPanel.ColumnCount = 2;
            this.rootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.rootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.rootPanel.Controls.Add(this.windowFinderControl, 0, 0);
            this.rootPanel.Controls.Add(this.topRightPanel, 1, 0);
            this.rootPanel.Controls.Add(this.mainPanel, 1, 1);
            this.rootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootPanel.Location = new System.Drawing.Point(0, 0);
            this.rootPanel.Name = "rootPanel";
            this.rootPanel.RowCount = 2;
            this.rootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.rootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.rootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.rootPanel.Size = new System.Drawing.Size(526, 190);
            this.rootPanel.TabIndex = 13;
            // 
            // windowFinderControl
            // 
            this.windowFinderControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.windowFinderControl.BackColor = System.Drawing.Color.Transparent;
            this.windowFinderControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("windowFinderControl.BackgroundImage")));
            this.windowFinderControl.Location = new System.Drawing.Point(4, 4);
            this.windowFinderControl.Name = "windowFinderControl";
            this.windowFinderControl.Size = new System.Drawing.Size(32, 32);
            this.windowFinderControl.TabIndex = 12;
            // 
            // topRightPanel
            // 
            this.topRightPanel.Controls.Add(this.hwndBox);
            this.topRightPanel.Controls.Add(this.dumpButton);
            this.topRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topRightPanel.Location = new System.Drawing.Point(43, 3);
            this.topRightPanel.Name = "topRightPanel";
            this.topRightPanel.Size = new System.Drawing.Size(480, 34);
            this.topRightPanel.TabIndex = 13;
            // 
            // mainPanel
            // 
            this.rootPanel.SetColumnSpan(this.mainPanel, 2);
            this.mainPanel.Controls.Add(this.tabs);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(3, 43);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(520, 144);
            this.mainPanel.TabIndex = 14;
            // 
            // tabs
            // 
            this.tabs.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabs.Controls.Add(this.nativeTabPage);
            this.tabs.Controls.Add(this.dotNetTabPage);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(520, 144);
            this.tabs.TabIndex = 12;
            // 
            // nativeTabPage
            // 
            this.nativeTabPage.Controls.Add(this.nativePropertyGrid);
            this.nativeTabPage.Location = new System.Drawing.Point(4, 4);
            this.nativeTabPage.Name = "nativeTabPage";
            this.nativeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.nativeTabPage.Size = new System.Drawing.Size(512, 118);
            this.nativeTabPage.TabIndex = 0;
            this.nativeTabPage.Text = "Native";
            this.nativeTabPage.UseVisualStyleBackColor = true;
            // 
            // nativePropertyGrid
            // 
            this.nativePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nativePropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.nativePropertyGrid.Name = "nativePropertyGrid";
            this.nativePropertyGrid.Size = new System.Drawing.Size(506, 112);
            this.nativePropertyGrid.TabIndex = 11;
            // 
            // dotNetTabPage
            // 
            this.dotNetTabPage.Controls.Add(this.dotNetPropertyGrid);
            this.dotNetTabPage.Location = new System.Drawing.Point(4, 4);
            this.dotNetTabPage.Name = "dotNetTabPage";
            this.dotNetTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.dotNetTabPage.Size = new System.Drawing.Size(512, 118);
            this.dotNetTabPage.TabIndex = 1;
            this.dotNetTabPage.Text = ".NET";
            this.dotNetTabPage.UseVisualStyleBackColor = true;
            // 
            // dotNetPropertyGrid
            // 
            this.dotNetPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dotNetPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.dotNetPropertyGrid.Name = "dotNetPropertyGrid";
            this.dotNetPropertyGrid.Size = new System.Drawing.Size(506, 112);
            this.dotNetPropertyGrid.TabIndex = 0;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rootPanel);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(526, 190);
            this.rootPanel.ResumeLayout(false);
            this.topRightPanel.ResumeLayout(false);
            this.topRightPanel.PerformLayout();
            this.mainPanel.ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel rootPanel;
        private System.Windows.Forms.Panel topRightPanel;
        private System.Windows.Forms.Panel mainPanel;
        private TabControlEx tabs;
        private System.Windows.Forms.TabPage nativeTabPage;
        private System.Windows.Forms.TabPage dotNetTabPage;
        private DotNetPropertyGrid dotNetPropertyGrid;
    }
}
