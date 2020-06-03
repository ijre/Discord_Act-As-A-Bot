namespace discord_puppet
{
    partial class DebugForm
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
            this.DebugOutput = new System.Windows.Forms.TextBox();
            this.Clear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DebugOutput
            // 
            this.DebugOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugOutput.Location = new System.Drawing.Point(14, 21);
            this.DebugOutput.Multiline = true;
            this.DebugOutput.Name = "DebugOutput";
            this.DebugOutput.ReadOnly = true;
            this.DebugOutput.Size = new System.Drawing.Size(766, 469);
            this.DebugOutput.TabIndex = 0;
            this.DebugOutput.WordWrap = false;
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(361, 495);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(72, 32);
            this.Clear.TabIndex = 1;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 531);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.DebugOutput);
            this.Name = "DebugForm";
            this.Text = "DebugForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox DebugOutput;
        private System.Windows.Forms.Button Clear;
    }
}