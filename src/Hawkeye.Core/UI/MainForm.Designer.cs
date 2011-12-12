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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.injectN2x86Button = new System.Windows.Forms.Button();
            this.injectN2X64Button = new System.Windows.Forms.Button();
            this.injectN4x86Button = new System.Windows.Forms.Button();
            this.injectN4x64Button = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(608, 437);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.injectN4x64Button);
            this.tabPage1.Controls.Add(this.injectN4x86Button);
            this.tabPage1.Controls.Add(this.injectN2X64Button);
            this.tabPage1.Controls.Add(this.injectN2x86Button);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(600, 411);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // injectN2x86Button
            // 
            this.injectN2x86Button.Location = new System.Drawing.Point(8, 6);
            this.injectN2x86Button.Name = "injectN2x86Button";
            this.injectN2x86Button.Size = new System.Drawing.Size(75, 23);
            this.injectN2x86Button.TabIndex = 0;
            this.injectN2x86Button.Text = "N2 x86";
            this.injectN2x86Button.UseVisualStyleBackColor = true;
            this.injectN2x86Button.Click += new System.EventHandler(this.injectN2x86Button_Click);
            // 
            // injectN2X64Button
            // 
            this.injectN2X64Button.Location = new System.Drawing.Point(8, 35);
            this.injectN2X64Button.Name = "injectN2X64Button";
            this.injectN2X64Button.Size = new System.Drawing.Size(75, 23);
            this.injectN2X64Button.TabIndex = 0;
            this.injectN2X64Button.Text = "N2 x64";
            this.injectN2X64Button.UseVisualStyleBackColor = true;
            this.injectN2X64Button.Click += new System.EventHandler(this.injectN2X64Button_Click);
            // 
            // injectN4x86Button
            // 
            this.injectN4x86Button.Location = new System.Drawing.Point(6, 64);
            this.injectN4x86Button.Name = "injectN4x86Button";
            this.injectN4x86Button.Size = new System.Drawing.Size(75, 23);
            this.injectN4x86Button.TabIndex = 0;
            this.injectN4x86Button.Text = "N4 x86";
            this.injectN4x86Button.UseVisualStyleBackColor = true;
            this.injectN4x86Button.Click += new System.EventHandler(this.injectN4x86Button_Click);
            // 
            // injectN4x64Button
            // 
            this.injectN4x64Button.Location = new System.Drawing.Point(6, 93);
            this.injectN4x64Button.Name = "injectN4x64Button";
            this.injectN4x64Button.Size = new System.Drawing.Size(75, 23);
            this.injectN4x64Button.TabIndex = 0;
            this.injectN4x64Button.Text = "N4 x64";
            this.injectN4x64Button.UseVisualStyleBackColor = true;
            this.injectN4x64Button.Click += new System.EventHandler(this.injectN4x64Button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 437);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "HawkEye";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button injectN2x86Button;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button injectN4x64Button;
        private System.Windows.Forms.Button injectN4x86Button;
        private System.Windows.Forms.Button injectN2X64Button;
    }
}