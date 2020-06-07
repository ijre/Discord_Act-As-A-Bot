namespace discord_puppet
{
    partial class MemberListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemberListForm));
            this.MemberList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // MemberList
            // 
            this.MemberList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.MemberList.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.MemberList.ForeColor = System.Drawing.Color.White;
            this.MemberList.FormattingEnabled = true;
            this.MemberList.Items.AddRange(new object[] {
            "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee#2036 [148082086731972608]"});
            this.MemberList.Location = new System.Drawing.Point(0, 0);
            this.MemberList.Name = "MemberList";
            this.MemberList.Size = new System.Drawing.Size(232, 433);
            this.MemberList.TabIndex = 0;
            // 
            // MemberListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = global::discord_puppet.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(230, 433);
            this.Controls.Add(this.MemberList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MemberListForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MemberListForm_FormClosing);
            this.Resize += new System.EventHandler(this.MemberListForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox MemberList;
    }
}