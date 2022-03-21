namespace SketchupMaterialGenerator
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
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.rtxt = new System.Windows.Forms.RichTextBox();
            this.chkSearchSubfolders = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(7, 9);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(94, 31);
            this.btnSelectFolder.TabIndex = 0;
            this.btnSelectFolder.Text = "Select folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // rtxt
            // 
            this.rtxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxt.Location = new System.Drawing.Point(8, 46);
            this.rtxt.Name = "rtxt";
            this.rtxt.Size = new System.Drawing.Size(780, 392);
            this.rtxt.TabIndex = 1;
            this.rtxt.Text = "";
            // 
            // chkSearchSubfolders
            // 
            this.chkSearchSubfolders.AutoSize = true;
            this.chkSearchSubfolders.Location = new System.Drawing.Point(114, 18);
            this.chkSearchSubfolders.Name = "chkSearchSubfolders";
            this.chkSearchSubfolders.Size = new System.Drawing.Size(113, 17);
            this.chkSearchSubfolders.TabIndex = 2;
            this.chkSearchSubfolders.Text = "Search Subfolders";
            this.chkSearchSubfolders.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkSearchSubfolders);
            this.Controls.Add(this.rtxt);
            this.Controls.Add(this.btnSelectFolder);
            this.Name = "MainForm";
            this.Text = "Sketchup Material Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.RichTextBox rtxt;
        private System.Windows.Forms.CheckBox chkSearchSubfolders;
    }
}

