﻿namespace EventLogReader
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.gvEw = new System.Windows.Forms.DataGridView();
            this.gvFw = new System.Windows.Forms.DataGridView();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.filemenu = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.cbShowEvents = new System.Windows.Forms.CheckBox();
            this.cbSaveEwDb = new System.Windows.Forms.CheckBox();
            this.cbSaveFsDb = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvEw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFw)).BeginInit();
            this.filemenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(54, 45);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(145, 45);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // gvEw
            // 
            this.gvEw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvEw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvEw.Location = new System.Drawing.Point(42, 82);
            this.gvEw.Name = "gvEw";
            this.gvEw.Size = new System.Drawing.Size(1362, 172);
            this.gvEw.TabIndex = 2;
            // 
            // gvFw
            // 
            this.gvFw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvFw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFw.Location = new System.Drawing.Point(42, 284);
            this.gvFw.Name = "gvFw";
            this.gvFw.Size = new System.Drawing.Size(1362, 197);
            this.gvFw.TabIndex = 3;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(42, 510);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1362, 195);
            this.txtLog.TabIndex = 4;
            // 
            // filemenu
            // 
            this.filemenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem});
            this.filemenu.Location = new System.Drawing.Point(0, 0);
            this.filemenu.Name = "filemenu";
            this.filemenu.Size = new System.Drawing.Size(1436, 24);
            this.filemenu.TabIndex = 6;
            this.filemenu.Text = "menuStrip1";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(352, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbShowEvents
            // 
            this.cbShowEvents.AutoSize = true;
            this.cbShowEvents.Location = new System.Drawing.Point(563, 45);
            this.cbShowEvents.Name = "cbShowEvents";
            this.cbShowEvents.Size = new System.Drawing.Size(89, 17);
            this.cbShowEvents.TabIndex = 7;
            this.cbShowEvents.Text = "Show Events";
            this.cbShowEvents.UseVisualStyleBackColor = true;
            this.cbShowEvents.CheckedChanged += new System.EventHandler(this.cbShowEvents_CheckedChanged);
            // 
            // cbSaveEwDb
            // 
            this.cbSaveEwDb.AutoSize = true;
            this.cbSaveEwDb.Location = new System.Drawing.Point(670, 45);
            this.cbSaveEwDb.Name = "cbSaveEwDb";
            this.cbSaveEwDb.Size = new System.Drawing.Size(121, 17);
            this.cbSaveEwDb.TabIndex = 8;
            this.cbSaveEwDb.Text = "Save Events On Db";
            this.cbSaveEwDb.UseVisualStyleBackColor = true;
            this.cbSaveEwDb.CheckedChanged += new System.EventHandler(this.cbUpdateDb_CheckedChanged);
            // 
            // cbSaveFsDb
            // 
            this.cbSaveFsDb.AutoSize = true;
            this.cbSaveFsDb.Location = new System.Drawing.Point(808, 45);
            this.cbSaveFsDb.Name = "cbSaveFsDb";
            this.cbSaveFsDb.Size = new System.Drawing.Size(135, 17);
            this.cbSaveFsDb.TabIndex = 9;
            this.cbSaveFsDb.Text = "Save Fs Events On Db";
            this.cbSaveFsDb.UseVisualStyleBackColor = true;
            this.cbSaveFsDb.CheckedChanged += new System.EventHandler(this.cbSaveFsDb_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1436, 737);
            this.Controls.Add(this.cbSaveFsDb);
            this.Controls.Add(this.cbSaveEwDb);
            this.Controls.Add(this.cbShowEvents);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.gvFw);
            this.Controls.Add(this.gvEw);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.filemenu);
            this.MainMenuStrip = this.filemenu;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvEw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFw)).EndInit();
            this.filemenu.ResumeLayout(false);
            this.filemenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.DataGridView gvFw;
        private System.Windows.Forms.DataGridView gvEw;
        private System.Windows.Forms.MenuStrip filemenu;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbShowEvents;
        private System.Windows.Forms.CheckBox cbSaveEwDb;
        private System.Windows.Forms.CheckBox cbSaveFsDb;
    }
}

