namespace Red.WorkPoint.ProjectPlugin
{
    partial class PublishDialog
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
            this.publishButton = new System.Windows.Forms.Button();
            this.typeTM = new System.Windows.Forms.RadioButton();
            this.typeFB = new System.Windows.Forms.RadioButton();
            this.typeNB = new System.Windows.Forms.RadioButton();
            this.log = new System.Windows.Forms.ListBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.closeButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // publishButton
            // 
            this.publishButton.Location = new System.Drawing.Point(366, 55);
            this.publishButton.Name = "publishButton";
            this.publishButton.Size = new System.Drawing.Size(106, 23);
            this.publishButton.TabIndex = 2;
            this.publishButton.Text = "Publish";
            this.publishButton.UseVisualStyleBackColor = true;
            this.publishButton.Click += new System.EventHandler(this.PublishButton_Click);
            // 
            // typeTM
            // 
            this.typeTM.AutoSize = true;
            this.typeTM.Location = new System.Drawing.Point(16, 12);
            this.typeTM.Name = "typeTM";
            this.typeTM.Size = new System.Drawing.Size(102, 17);
            this.typeTM.TabIndex = 4;
            this.typeTM.TabStop = true;
            this.typeTM.Text = "Time && Materials";
            this.typeTM.UseVisualStyleBackColor = true;
            // 
            // typeFB
            // 
            this.typeFB.AutoSize = true;
            this.typeFB.Location = new System.Drawing.Point(16, 35);
            this.typeFB.Name = "typeFB";
            this.typeFB.Size = new System.Drawing.Size(68, 17);
            this.typeFB.TabIndex = 5;
            this.typeFB.TabStop = true;
            this.typeFB.Text = "Fixed Bid";
            this.typeFB.UseVisualStyleBackColor = true;
            // 
            // typeNB
            // 
            this.typeNB.AutoSize = true;
            this.typeNB.Location = new System.Drawing.Point(16, 58);
            this.typeNB.Name = "typeNB";
            this.typeNB.Size = new System.Drawing.Size(81, 17);
            this.typeNB.TabIndex = 6;
            this.typeNB.TabStop = true;
            this.typeNB.Text = "Non Billable";
            this.typeNB.UseVisualStyleBackColor = true;
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.Location = new System.Drawing.Point(16, 123);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(456, 147);
            this.log.TabIndex = 7;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(16, 90);
            this.progressBar.Maximum = 6;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(456, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 9;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(397, 276);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 10;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(285, 55);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // PublishDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 303);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.log);
            this.Controls.Add(this.typeNB);
            this.Controls.Add(this.typeFB);
            this.Controls.Add(this.typeTM);
            this.Controls.Add(this.publishButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PublishDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publish";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button publishButton;
        private System.Windows.Forms.RadioButton typeTM;
        private System.Windows.Forms.RadioButton typeFB;
        private System.Windows.Forms.RadioButton typeNB;
        private System.Windows.Forms.ListBox log;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button cancelButton;
    }
}