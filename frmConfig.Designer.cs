namespace EventLogReader
{
    partial class frmConfig
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
            this.btnSave = new System.Windows.Forms.Button();
            this.cbTrusted = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbDb = new System.Windows.Forms.GroupBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDbServer = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtClearMinutesOld = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbDb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(538, 244);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbTrusted
            // 
            this.cbTrusted.AutoSize = true;
            this.cbTrusted.Location = new System.Drawing.Point(93, 88);
            this.cbTrusted.Name = "cbTrusted";
            this.cbTrusted.Size = new System.Drawing.Size(62, 17);
            this.cbTrusted.TabIndex = 1;
            this.cbTrusted.Text = "Trusted";
            this.cbTrusted.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server:";
            // 
            // gbDb
            // 
            this.gbDb.Controls.Add(this.txtPass);
            this.gbDb.Controls.Add(this.label4);
            this.gbDb.Controls.Add(this.txtDbUser);
            this.gbDb.Controls.Add(this.label3);
            this.gbDb.Controls.Add(this.txtDb);
            this.gbDb.Controls.Add(this.cbTrusted);
            this.gbDb.Controls.Add(this.label2);
            this.gbDb.Controls.Add(this.txtDbServer);
            this.gbDb.Controls.Add(this.label1);
            this.gbDb.Location = new System.Drawing.Point(30, 21);
            this.gbDb.Name = "gbDb";
            this.gbDb.Size = new System.Drawing.Size(223, 173);
            this.gbDb.TabIndex = 3;
            this.gbDb.TabStop = false;
            this.gbDb.Text = "Database";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(93, 137);
            this.txtPass.MaxLength = 127;
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(100, 20);
            this.txtPass.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Pass:";
            // 
            // txtDbUser
            // 
            this.txtDbUser.Location = new System.Drawing.Point(93, 111);
            this.txtDbUser.MaxLength = 127;
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(100, 20);
            this.txtDbUser.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "User:";
            // 
            // txtDb
            // 
            this.txtDb.Location = new System.Drawing.Point(93, 51);
            this.txtDb.MaxLength = 127;
            this.txtDb.Name = "txtDb";
            this.txtDb.Size = new System.Drawing.Size(100, 20);
            this.txtDb.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Database:";
            // 
            // txtDbServer
            // 
            this.txtDbServer.Location = new System.Drawing.Point(93, 25);
            this.txtDbServer.MaxLength = 127;
            this.txtDbServer.Name = "txtDbServer";
            this.txtDbServer.Size = new System.Drawing.Size(100, 20);
            this.txtDbServer.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtClearMinutesOld);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtOffset);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtDir);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(271, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 168);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File System";
            // 
            // txtClearMinutesOld
            // 
            this.txtClearMinutesOld.Location = new System.Drawing.Point(157, 99);
            this.txtClearMinutesOld.MaxLength = 127;
            this.txtClearMinutesOld.Name = "txtClearMinutesOld";
            this.txtClearMinutesOld.Size = new System.Drawing.Size(100, 20);
            this.txtClearMinutesOld.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Buffer Clear Older(Minutes):";
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(157, 61);
            this.txtOffset.MaxLength = 127;
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(100, 20);
            this.txtOffset.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Time Offset(Seconds):";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(281, 20);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(55, 20);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(73, 19);
            this.txtDir.MaxLength = 127;
            this.txtDir.Name = "txtDir";
            this.txtDir.ReadOnly = true;
            this.txtDir.Size = new System.Drawing.Size(203, 20);
            this.txtDir.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Directory:";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 319);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbDb);
            this.Controls.Add(this.btnSave);
            this.Name = "frmConfig";
            this.Text = "Config";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.gbDb.ResumeLayout(false);
            this.gbDb.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox cbTrusted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbDb;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDbUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDbServer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtOffset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtClearMinutesOld;
        private System.Windows.Forms.Label label7;
    }
}