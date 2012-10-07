namespace ScreenInfo
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
            this.closeButton = new System.Windows.Forms.Button();
            this.lvScreens = new System.Windows.Forms.ListView();
            this.chPrimary = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBpp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBounds = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWorkingArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.movePanel = new System.Windows.Forms.Panel();
            this.refreshScreensButton = new System.Windows.Forms.Button();
            this.trackCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.boundsTextBox = new System.Windows.Forms.TextBox();
            this.clientRectangleTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.clientRectangleInScreenCoordinatesTextBox = new System.Windows.Forms.TextBox();
            this.maximizedCheckBox = new System.Windows.Forms.CheckBox();
            this.minimizedCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.currentScreenTextBox = new System.Windows.Forms.TextBox();
            this.movePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(549, 422);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // lvScreens
            // 
            this.lvScreens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvScreens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPrimary,
            this.chName,
            this.chBpp,
            this.chBounds,
            this.chWorkingArea});
            this.lvScreens.FullRowSelect = true;
            this.lvScreens.Location = new System.Drawing.Point(12, 12);
            this.lvScreens.Name = "lvScreens";
            this.lvScreens.Size = new System.Drawing.Size(612, 243);
            this.lvScreens.TabIndex = 1;
            this.lvScreens.UseCompatibleStateImageBehavior = false;
            this.lvScreens.View = System.Windows.Forms.View.Details;
            // 
            // chPrimary
            // 
            this.chPrimary.Text = "";
            this.chPrimary.Width = 14;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 120;
            // 
            // chBpp
            // 
            this.chBpp.Text = "Bpp";
            this.chBpp.Width = 40;
            // 
            // chBounds
            // 
            this.chBounds.Text = "Bounds";
            this.chBounds.Width = 200;
            // 
            // chWorkingArea
            // 
            this.chWorkingArea.Text = "Working Area";
            this.chWorkingArea.Width = 200;
            // 
            // movePanel
            // 
            this.movePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.movePanel.BackColor = System.Drawing.SystemColors.Window;
            this.movePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.movePanel.Controls.Add(this.minimizedCheckBox);
            this.movePanel.Controls.Add(this.maximizedCheckBox);
            this.movePanel.Controls.Add(this.currentScreenTextBox);
            this.movePanel.Controls.Add(this.clientRectangleTextBox);
            this.movePanel.Controls.Add(this.clientRectangleInScreenCoordinatesTextBox);
            this.movePanel.Controls.Add(this.boundsTextBox);
            this.movePanel.Controls.Add(this.label3);
            this.movePanel.Controls.Add(this.label2);
            this.movePanel.Controls.Add(this.label1);
            this.movePanel.Location = new System.Drawing.Point(12, 261);
            this.movePanel.Name = "movePanel";
            this.movePanel.Size = new System.Drawing.Size(612, 155);
            this.movePanel.TabIndex = 2;
            // 
            // refreshScreensButton
            // 
            this.refreshScreensButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshScreensButton.Location = new System.Drawing.Point(12, 422);
            this.refreshScreensButton.Name = "refreshScreensButton";
            this.refreshScreensButton.Size = new System.Drawing.Size(132, 23);
            this.refreshScreensButton.TabIndex = 3;
            this.refreshScreensButton.Text = "&Refresh Screens List";
            this.refreshScreensButton.UseVisualStyleBackColor = true;
            this.refreshScreensButton.Click += new System.EventHandler(this.refreshScreensButton_Click);
            // 
            // trackCheckBox
            // 
            this.trackCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackCheckBox.AutoSize = true;
            this.trackCheckBox.Checked = true;
            this.trackCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackCheckBox.Location = new System.Drawing.Point(150, 426);
            this.trackCheckBox.Name = "trackCheckBox";
            this.trackCheckBox.Size = new System.Drawing.Size(104, 17);
            this.trackCheckBox.TabIndex = 4;
            this.trackCheckBox.Text = "Enable &Tracking";
            this.trackCheckBox.UseVisualStyleBackColor = true;
            this.trackCheckBox.CheckedChanged += new System.EventHandler(this.trackCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Window Bounds:";
            // 
            // boundsTextBox
            // 
            this.boundsTextBox.Location = new System.Drawing.Point(97, 3);
            this.boundsTextBox.Name = "boundsTextBox";
            this.boundsTextBox.Size = new System.Drawing.Size(200, 20);
            this.boundsTextBox.TabIndex = 1;
            // 
            // clientRectangleTextBox
            // 
            this.clientRectangleTextBox.Location = new System.Drawing.Point(97, 29);
            this.clientRectangleTextBox.Name = "clientRectangleTextBox";
            this.clientRectangleTextBox.Size = new System.Drawing.Size(200, 20);
            this.clientRectangleTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Client Rect:";
            // 
            // clientRectangleInScreenCoordinatesTextBox
            // 
            this.clientRectangleInScreenCoordinatesTextBox.Location = new System.Drawing.Point(303, 29);
            this.clientRectangleInScreenCoordinatesTextBox.Name = "clientRectangleInScreenCoordinatesTextBox";
            this.clientRectangleInScreenCoordinatesTextBox.Size = new System.Drawing.Size(200, 20);
            this.clientRectangleInScreenCoordinatesTextBox.TabIndex = 1;
            // 
            // maximizedCheckBox
            // 
            this.maximizedCheckBox.AutoSize = true;
            this.maximizedCheckBox.Location = new System.Drawing.Point(303, 5);
            this.maximizedCheckBox.Name = "maximizedCheckBox";
            this.maximizedCheckBox.Size = new System.Drawing.Size(75, 17);
            this.maximizedCheckBox.TabIndex = 2;
            this.maximizedCheckBox.Text = "Maximized";
            this.maximizedCheckBox.UseVisualStyleBackColor = true;
            // 
            // minimizedCheckBox
            // 
            this.minimizedCheckBox.AutoSize = true;
            this.minimizedCheckBox.Location = new System.Drawing.Point(384, 5);
            this.minimizedCheckBox.Name = "minimizedCheckBox";
            this.minimizedCheckBox.Size = new System.Drawing.Size(72, 17);
            this.minimizedCheckBox.TabIndex = 2;
            this.minimizedCheckBox.Text = "Minimized";
            this.minimizedCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Screen:";
            // 
            // currentScreenTextBox
            // 
            this.currentScreenTextBox.Location = new System.Drawing.Point(97, 55);
            this.currentScreenTextBox.Name = "currentScreenTextBox";
            this.currentScreenTextBox.Size = new System.Drawing.Size(200, 20);
            this.currentScreenTextBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 457);
            this.Controls.Add(this.trackCheckBox);
            this.Controls.Add(this.refreshScreensButton);
            this.Controls.Add(this.movePanel);
            this.Controls.Add(this.lvScreens);
            this.Controls.Add(this.closeButton);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScreenInfo";
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.movePanel.ResumeLayout(false);
            this.movePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ListView lvScreens;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chBpp;
        private System.Windows.Forms.ColumnHeader chBounds;
        private System.Windows.Forms.ColumnHeader chPrimary;
        private System.Windows.Forms.ColumnHeader chWorkingArea;
        private System.Windows.Forms.Panel movePanel;
        private System.Windows.Forms.Button refreshScreensButton;
        private System.Windows.Forms.CheckBox trackCheckBox;
        private System.Windows.Forms.TextBox boundsTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox clientRectangleTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox clientRectangleInScreenCoordinatesTextBox;
        private System.Windows.Forms.CheckBox maximizedCheckBox;
        private System.Windows.Forms.CheckBox minimizedCheckBox;
        private System.Windows.Forms.TextBox currentScreenTextBox;
        private System.Windows.Forms.Label label3;
    }
}

