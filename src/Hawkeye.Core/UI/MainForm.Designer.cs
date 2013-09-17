namespace Hawkeye.UI
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mstrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sstrip = new System.Windows.Forms.StatusStrip();
            this.tstripContainer = new System.Windows.Forms.ToolStripContainer();
            this.mainControl = new Hawkeye.UI.MainControl();
            this.mstrip.SuspendLayout();
            this.tstripContainer.ContentPanel.SuspendLayout();
            this.tstripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mstrip
            // 
            this.mstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mstrip.Location = new System.Drawing.Point(0, 0);
            this.mstrip.Name = "mstrip";
            this.mstrip.Size = new System.Drawing.Size(368, 24);
            this.mstrip.TabIndex = 1;
            this.mstrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F1)));
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(191, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // sstrip
            // 
            this.sstrip.Location = new System.Drawing.Point(0, 415);
            this.sstrip.Name = "sstrip";
            this.sstrip.Size = new System.Drawing.Size(368, 22);
            this.sstrip.TabIndex = 2;
            this.sstrip.Text = "statusStrip1";
            // 
            // tstripContainer
            // 
            // 
            // tstripContainer.ContentPanel
            // 
            this.tstripContainer.ContentPanel.Controls.Add(this.mainControl);
            this.tstripContainer.ContentPanel.Size = new System.Drawing.Size(368, 366);
            this.tstripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tstripContainer.Location = new System.Drawing.Point(0, 24);
            this.tstripContainer.Name = "tstripContainer";
            this.tstripContainer.Size = new System.Drawing.Size(368, 391);
            this.tstripContainer.TabIndex = 3;
            this.tstripContainer.Text = "toolStripContainer1";
            // 
            // mainControl
            // 
            this.mainControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainControl.Location = new System.Drawing.Point(0, 0);
            this.mainControl.Name = "mainControl";
            this.mainControl.Padding = new System.Windows.Forms.Padding(4);
            this.mainControl.Size = new System.Drawing.Size(368, 366);
            this.mainControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 437);
            this.Controls.Add(this.tstripContainer);
            this.Controls.Add(this.sstrip);
            this.Controls.Add(this.mstrip);
            this.MainMenuStrip = this.mstrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Hawkeye";
            this.mstrip.ResumeLayout(false);
            this.mstrip.PerformLayout();
            this.tstripContainer.ContentPanel.ResumeLayout(false);
            this.tstripContainer.ResumeLayout(false);
            this.tstripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mstrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip sstrip;
        private System.Windows.Forms.ToolStripContainer tstripContainer;
        private MainControl mainControl;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}