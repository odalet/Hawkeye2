namespace HawkeyeApplication
{
    partial class Form1
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
            this.dumpButton = new System.Windows.Forms.Button();
            this.hwndBox = new System.Windows.Forms.TextBox();
            this.detectButton = new System.Windows.Forms.Button();
            this.pgrid = new Hawkeye.PropertyGridEx();
            this.SuspendLayout();
            // 
            // dumpButton
            // 
            this.dumpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dumpButton.Location = new System.Drawing.Point(325, 12);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new System.Drawing.Size(75, 23);
            this.dumpButton.TabIndex = 6;
            this.dumpButton.Text = "Dump...";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new System.EventHandler(this.dumpButton_Click);
            // 
            // hwndBox
            // 
            this.hwndBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hwndBox.Location = new System.Drawing.Point(12, 14);
            this.hwndBox.Name = "hwndBox";
            this.hwndBox.Size = new System.Drawing.Size(226, 20);
            this.hwndBox.TabIndex = 5;
            // 
            // detectButton
            // 
            this.detectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.detectButton.Location = new System.Drawing.Point(244, 12);
            this.detectButton.Name = "detectButton";
            this.detectButton.Size = new System.Drawing.Size(75, 23);
            this.detectButton.TabIndex = 4;
            this.detectButton.Text = "&Detect";
            this.detectButton.UseVisualStyleBackColor = true;
            this.detectButton.Click += new System.EventHandler(this.detectButton_Click);
            // 
            // pgrid
            // 
            this.pgrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgrid.Location = new System.Drawing.Point(12, 40);
            this.pgrid.Name = "pgrid";
            this.pgrid.Size = new System.Drawing.Size(388, 416);
            this.pgrid.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 468);
            this.Controls.Add(this.pgrid);
            this.Controls.Add(this.dumpButton);
            this.Controls.Add(this.hwndBox);
            this.Controls.Add(this.detectButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button dumpButton;
        private System.Windows.Forms.TextBox hwndBox;
        private System.Windows.Forms.Button detectButton;
        private Hawkeye.PropertyGridEx pgrid;
    }
}

