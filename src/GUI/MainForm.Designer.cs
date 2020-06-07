namespace discord_puppet
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Channels = new System.Windows.Forms.ListBox();
            this.Servers = new System.Windows.Forms.ListBox();
            this.MainForm_ThinkTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Channels
            // 
            this.Channels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Channels.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.Channels.ForeColor = System.Drawing.Color.White;
            this.Channels.FormattingEnabled = true;
            this.Channels.Location = new System.Drawing.Point(285, 0);
            this.Channels.Name = "Channels";
            this.Channels.Size = new System.Drawing.Size(251, 472);
            this.Channels.TabIndex = 1;
            this.Channels.SelectedIndexChanged += new System.EventHandler(this.Channels_SelectedIndexChanged);
            // 
            // Servers
            // 
            this.Servers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Servers.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.Servers.ForeColor = System.Drawing.Color.White;
            this.Servers.FormattingEnabled = true;
            this.Servers.Items.AddRange(new object[] {
            "test serv [511026398429839371]"});
            this.Servers.Location = new System.Drawing.Point(0, 0);
            this.Servers.Name = "Servers";
            this.Servers.Size = new System.Drawing.Size(251, 472);
            this.Servers.TabIndex = 0;
            this.Servers.SelectedIndexChanged += new System.EventHandler(this.Servers_SelectedIndexChanged);
            // 
            // MainForm_ThinkTimer
            // 
            this.MainForm_ThinkTimer.Enabled = true;
            this.MainForm_ThinkTimer.Tick += new System.EventHandler(this.MainForm_Think);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = global::discord_puppet.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(534, 471);
            this.Controls.Add(this.Servers);
            this.Controls.Add(this.Channels);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Channels;
        private System.Windows.Forms.ListBox Servers;
        private System.Windows.Forms.Timer MainForm_ThinkTimer;
    }
}