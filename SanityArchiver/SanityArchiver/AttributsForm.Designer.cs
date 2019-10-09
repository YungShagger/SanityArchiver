namespace SanityArchiver
{
    partial class AttributsForm
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
            this.AtributbeCheckList = new System.Windows.Forms.CheckedListBox();
            this.AcceptButton = new System.Windows.Forms.Button();
            this.CancellButton = new System.Windows.Forms.Button();
            this.pathLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AtributbeCheckList
            // 
            this.AtributbeCheckList.FormattingEnabled = true;
            this.AtributbeCheckList.Items.AddRange(new object[] {
            "Archive",
            "Hidden",
            "Normal",
            "ReadOnly",
            "Compressed",
            "Directory",
            "Encrypted",
            "System",
            "Temporary"});
            this.AtributbeCheckList.Location = new System.Drawing.Point(12, 27);
            this.AtributbeCheckList.Name = "AtributbeCheckList";
            this.AtributbeCheckList.Size = new System.Drawing.Size(246, 244);
            this.AtributbeCheckList.TabIndex = 0;
            this.AtributbeCheckList.SelectedIndexChanged += new System.EventHandler(this.AtributbeCheckList_SelectedIndexChanged);
            // 
            // AcceptButton
            // 
            this.AcceptButton.Location = new System.Drawing.Point(12, 277);
            this.AcceptButton.Name = "AcceptButton";
            this.AcceptButton.Size = new System.Drawing.Size(120, 31);
            this.AcceptButton.TabIndex = 1;
            this.AcceptButton.Text = "Accept";
            this.AcceptButton.UseVisualStyleBackColor = true;
            // 
            // CancellButton
            // 
            this.CancellButton.Location = new System.Drawing.Point(138, 277);
            this.CancellButton.Name = "CancellButton";
            this.CancellButton.Size = new System.Drawing.Size(120, 31);
            this.CancellButton.TabIndex = 2;
            this.CancellButton.Text = "Cancel";
            this.CancellButton.UseVisualStyleBackColor = true;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(12, 9);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(54, 13);
            this.pathLabel.TabIndex = 3;
            this.pathLabel.Text = "pathLabel";
            // 
            // AttributsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Salmon;
            this.ClientSize = new System.Drawing.Size(268, 316);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.CancellButton);
            this.Controls.Add(this.AcceptButton);
            this.Controls.Add(this.AtributbeCheckList);
            this.Name = "AttributsForm";
            this.Text = "Atributes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox AtributbeCheckList;
        private System.Windows.Forms.Button AcceptButton;
        private System.Windows.Forms.Button CancellButton;
        private System.Windows.Forms.Label pathLabel;
    }
}