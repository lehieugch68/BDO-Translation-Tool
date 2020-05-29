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
            this.selectTranslator = new System.Windows.Forms.ComboBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonMerge = new System.Windows.Forms.Button();
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.buttonContact = new System.Windows.Forms.Button();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.buttonRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 184);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(410, 23);
            this.progressBar.TabIndex = 0;
            // 
            // GamePath
            // 
            this.GamePath.Location = new System.Drawing.Point(109, 72);
            this.GamePath.Name = "GamePath";
            this.GamePath.ReadOnly = true;
            this.GamePath.Size = new System.Drawing.Size(313, 20);
            this.GamePath.TabIndex = 1;
            // 
            // Browser
            // 
            this.Browser.Location = new System.Drawing.Point(12, 70);
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
            this.Log.Location = new System.Drawing.Point(12, 213);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(410, 160);
            this.Log.TabIndex = 3;
            // 
            // Install
            // 
            this.Install.Location = new System.Drawing.Point(12, 105);
            this.Install.Name = "Install";
            this.Install.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Install.Size = new System.Drawing.Size(203, 25);
            this.Install.TabIndex = 4;
            this.Install.Text = "Cài đặt";
            this.Install.UseVisualStyleBackColor = true;
            this.Install.Click += new System.EventHandler(this.Install_Click);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(12, 168);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(44, 13);
            this.Status.TabIndex = 6;
            this.Status.Text = "Chưa rõ";
            // 
            // buttonDecompress
            // 
            this.buttonDecompress.Location = new System.Drawing.Point(219, 105);
            this.buttonDecompress.Name = "buttonDecompress";
            this.buttonDecompress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonDecompress.Size = new System.Drawing.Size(203, 25);
            this.buttonDecompress.TabIndex = 8;
            this.buttonDecompress.Text = "Giải nén";
            this.buttonDecompress.UseVisualStyleBackColor = true;
            this.buttonDecompress.Click += new System.EventHandler(this.buttonDecompress_Click);
            // 
            // linkGithub
            // 
            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(9, 386);
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
            this.linkVHG.Location = new System.Drawing.Point(349, 386);
            this.linkVHG.Name = "linkVHG";
            this.linkVHG.Size = new System.Drawing.Size(73, 13);
            this.linkVHG.TabIndex = 10;
            this.linkVHG.TabStop = true;
            this.linkVHG.Text = "VietHoaGame";
            this.linkVHG.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkVHG_LinkClicked);
            // 
            // selectTranslator
            // 
            this.selectTranslator.FormattingEnabled = true;
            this.selectTranslator.Location = new System.Drawing.Point(300, 26);
            this.selectTranslator.Name = "selectTranslator";
            this.selectTranslator.Size = new System.Drawing.Size(122, 21);
            this.selectTranslator.TabIndex = 11;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(12, 23);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonDownload.Size = new System.Drawing.Size(90, 25);
            this.buttonDownload.TabIndex = 12;
            this.buttonDownload.Text = "Tải bản dịch";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonMerge
            // 
            this.buttonMerge.Location = new System.Drawing.Point(108, 23);
            this.buttonMerge.Name = "buttonMerge";
            this.buttonMerge.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonMerge.Size = new System.Drawing.Size(90, 25);
            this.buttonMerge.TabIndex = 13;
            this.buttonMerge.Text = "Gộp bản dịch";
            this.buttonMerge.UseVisualStyleBackColor = true;
            this.buttonMerge.Click += new System.EventHandler(this.buttonMerge_Click);
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Location = new System.Drawing.Point(12, 54);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(410, 5);
            this.progressBarDownload.TabIndex = 14;
            // 
            // buttonContact
            // 
            this.buttonContact.Location = new System.Drawing.Point(204, 23);
            this.buttonContact.Name = "buttonContact";
            this.buttonContact.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonContact.Size = new System.Drawing.Size(90, 25);
            this.buttonContact.TabIndex = 15;
            this.buttonContact.Text = "Liên hệ";
            this.buttonContact.UseVisualStyleBackColor = true;
            this.buttonContact.Click += new System.EventHandler(this.buttonContact_Click);
            // 
            // buttonBackup
            // 
            this.buttonBackup.Location = new System.Drawing.Point(12, 133);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonBackup.Size = new System.Drawing.Size(203, 25);
            this.buttonBackup.TabIndex = 16;
            this.buttonBackup.Text = "Sao lưu";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // buttonRestore
            // 
            this.buttonRestore.Location = new System.Drawing.Point(219, 133);
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonRestore.Size = new System.Drawing.Size(203, 25);
            this.buttonRestore.TabIndex = 17;
            this.buttonRestore.Text = "Khôi phục";
            this.buttonRestore.UseVisualStyleBackColor = true;
            this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
            // 
            // BDOTranslationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 411);
            this.Controls.Add(this.buttonRestore);
            this.Controls.Add(this.buttonBackup);
            this.Controls.Add(this.buttonContact);
            this.Controls.Add(this.progressBarDownload);
            this.Controls.Add(this.buttonMerge);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.selectTranslator);
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
            this.MaximumSize = new System.Drawing.Size(450, 450);
            this.MinimumSize = new System.Drawing.Size(450, 450);
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
        private System.Windows.Forms.ComboBox selectTranslator;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonMerge;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.Button buttonContact;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.Button buttonRestore;
    }
}

