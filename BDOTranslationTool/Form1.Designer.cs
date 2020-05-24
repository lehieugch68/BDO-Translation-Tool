namespace BDOTranslationTool
{
    partial class BDOTranslationTool
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.GamePath = new System.Windows.Forms.TextBox();
            this.Browser = new System.Windows.Forms.Button();
            this.Log = new System.Windows.Forms.ListBox();
            this.Install = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Label();
            this.buttonDecompress = new System.Windows.Forms.Button();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.linkVHG = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 140);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(410, 23);
            this.progressBar.TabIndex = 0;
            // 
            // GamePath
            // 
            this.GamePath.Location = new System.Drawing.Point(108, 24);
            this.GamePath.Name = "GamePath";
            this.GamePath.ReadOnly = true;
            this.GamePath.Size = new System.Drawing.Size(310, 20);
            this.GamePath.TabIndex = 1;
            // 
            // Browser
            // 
            this.Browser.Location = new System.Drawing.Point(12, 24);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(90, 23);
            this.Browser.TabIndex = 2;
            this.Browser.Text = "Chọn thư mục";
            this.Browser.UseVisualStyleBackColor = true;
            this.Browser.Click += new System.EventHandler(this.Browser_Click);
            // 
            // Log
            // 
            this.Log.FormattingEnabled = true;
            this.Log.Location = new System.Drawing.Point(12, 169);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(410, 160);
            this.Log.TabIndex = 3;
            // 
            // Install
            // 
            this.Install.Location = new System.Drawing.Point(12, 74);
            this.Install.Name = "Install";
            this.Install.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Install.Size = new System.Drawing.Size(202, 30);
            this.Install.TabIndex = 4;
            this.Install.Text = "Cài đặt";
            this.Install.UseVisualStyleBackColor = true;
            this.Install.Click += new System.EventHandler(this.Install_Click);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(12, 124);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(44, 13);
            this.Status.TabIndex = 6;
            this.Status.Text = "Chưa rõ";
            // 
            // buttonDecompress
            // 
            this.buttonDecompress.Location = new System.Drawing.Point(220, 74);
            this.buttonDecompress.Name = "buttonDecompress";
            this.buttonDecompress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonDecompress.Size = new System.Drawing.Size(202, 30);
            this.buttonDecompress.TabIndex = 8;
            this.buttonDecompress.Text = "Giải nén";
            this.buttonDecompress.UseVisualStyleBackColor = true;
            this.buttonDecompress.Click += new System.EventHandler(this.buttonDecompress_Click);
            // 
            // linkGithub
            // 
            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(9, 342);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(38, 13);
            this.linkGithub.TabIndex = 9;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "Github";
            this.linkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGithub_LinkClicked);
            // 
            // linkVHG
            // 
            this.linkVHG.AutoSize = true;
            this.linkVHG.Location = new System.Drawing.Point(349, 342);
            this.linkVHG.Name = "linkVHG";
            this.linkVHG.Size = new System.Drawing.Size(73, 13);
            this.linkVHG.TabIndex = 10;
            this.linkVHG.TabStop = true;
            this.linkVHG.Text = "VietHoaGame";
            this.linkVHG.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkVHG_LinkClicked);
            // 
            // BDOTranslationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 381);
            this.Controls.Add(this.linkVHG);
            this.Controls.Add(this.linkGithub);
            this.Controls.Add(this.buttonDecompress);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Install);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.GamePath);
            this.Controls.Add(this.progressBar);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 420);
            this.MinimumSize = new System.Drawing.Size(450, 420);
            this.Name = "BDOTranslationTool";
            this.Text = "BDO Translation Tool";
            this.Load += new System.EventHandler(this.BDOTranslationTool_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox GamePath;
        private System.Windows.Forms.Button Browser;
        private System.Windows.Forms.ListBox Log;
        private System.Windows.Forms.Button Install;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Button buttonDecompress;
        private System.Windows.Forms.LinkLabel linkGithub;
        private System.Windows.Forms.LinkLabel linkVHG;
    }
}

