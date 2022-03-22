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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtMaxImageDimension = new System.Windows.Forms.TextBox();
            this.chkUseMaxDimension = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTextureScale = new System.Windows.Forms.TextBox();
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
            this.rtxt.Size = new System.Drawing.Size(780, 370);
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
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(7, 422);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(781, 23);
            this.progressBar.TabIndex = 3;
            // 
            // txtMaxImageDimension
            // 
            this.txtMaxImageDimension.Location = new System.Drawing.Point(745, 15);
            this.txtMaxImageDimension.Name = "txtMaxImageDimension";
            this.txtMaxImageDimension.Size = new System.Drawing.Size(40, 20);
            this.txtMaxImageDimension.TabIndex = 4;
            this.txtMaxImageDimension.Text = "2048";
            // 
            // chkUseMaxDimension
            // 
            this.chkUseMaxDimension.AutoSize = true;
            this.chkUseMaxDimension.Checked = true;
            this.chkUseMaxDimension.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseMaxDimension.Location = new System.Drawing.Point(595, 17);
            this.chkUseMaxDimension.Name = "chkUseMaxDimension";
            this.chkUseMaxDimension.Size = new System.Drawing.Size(153, 17);
            this.chkUseMaxDimension.TabIndex = 6;
            this.chkUseMaxDimension.Text = "use Max Image Dimension:";
            this.chkUseMaxDimension.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Texture scale:";
            // 
            // txtTextureScale
            // 
            this.txtTextureScale.Location = new System.Drawing.Point(383, 17);
            this.txtTextureScale.Name = "txtTextureScale";
            this.txtTextureScale.Size = new System.Drawing.Size(55, 20);
            this.txtTextureScale.TabIndex = 8;
            this.txtTextureScale.Text = "10";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtTextureScale);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMaxImageDimension);
            this.Controls.Add(this.chkUseMaxDimension);
            this.Controls.Add(this.progressBar);
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
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtMaxImageDimension;
        private System.Windows.Forms.CheckBox chkUseMaxDimension;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTextureScale;
    }
}

