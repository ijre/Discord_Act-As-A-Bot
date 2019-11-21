namespace hatsune_miku_bot_display
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Output_Chat = new System.Windows.Forms.TextBox();
            this.version = new System.Windows.Forms.TextBox();
            this.start_button = new System.Windows.Forms.Button();
            this.Input_Chat = new System.Windows.Forms.TextBox();
            this.send_button = new System.Windows.Forms.Button();
            this.change_channel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Output_Chat
            // 
            this.Output_Chat.BackColor = System.Drawing.SystemColors.GrayText;
            this.Output_Chat.ForeColor = System.Drawing.Color.White;
            this.Output_Chat.Location = new System.Drawing.Point(12, 12);
            this.Output_Chat.Multiline = true;
            this.Output_Chat.Name = "Output_Chat";
            this.Output_Chat.ReadOnly = true;
            this.Output_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output_Chat.Size = new System.Drawing.Size(380, 257);
            this.Output_Chat.TabIndex = 1;
            this.Output_Chat.Visible = false;
            // 
            // version
            // 
            this.version.Enabled = false;
            this.version.Location = new System.Drawing.Point(389, 512);
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Size = new System.Drawing.Size(130, 20);
            this.version.TabIndex = 4;
            this.version.TabStop = false;
            this.version.Text = "Hatsune_Miku-Bot 0.0.2.0";
            // 
            // start_button
            // 
            this.start_button.Location = new System.Drawing.Point(389, 290);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(130, 120);
            this.start_button.TabIndex = 0;
            this.start_button.Text = "Launch Hatsune Miku Bot";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.Start);
            // 
            // Input_Chat
            // 
            this.Input_Chat.BackColor = System.Drawing.SystemColors.GrayText;
            this.Input_Chat.ForeColor = System.Drawing.Color.White;
            this.Input_Chat.Location = new System.Drawing.Point(516, 12);
            this.Input_Chat.Multiline = true;
            this.Input_Chat.Name = "Input_Chat";
            this.Input_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Input_Chat.Size = new System.Drawing.Size(387, 257);
            this.Input_Chat.TabIndex = 5;
            this.Input_Chat.Visible = false;
            this.Input_Chat.WordWrap = false;
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(658, 275);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(84, 85);
            this.send_button.TabIndex = 6;
            this.send_button.Text = "Send";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Visible = false;
            // 
            // change_channel
            // 
            this.change_channel.Location = new System.Drawing.Point(12, 290);
            this.change_channel.Name = "change_channel";
            this.change_channel.Size = new System.Drawing.Size(130, 120);
            this.change_channel.TabIndex = 7;
            this.change_channel.Text = "Change Channel";
            this.change_channel.UseVisualStyleBackColor = true;
            this.change_channel.Visible = false;
            this.change_channel.Click += new System.EventHandler(this.change_channel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(915, 544);
            this.Controls.Add(this.change_channel);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.Input_Chat);
            this.Controls.Add(this.version);
            this.Controls.Add(this.Output_Chat);
            this.Controls.Add(this.start_button);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(931, 583);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Output_Chat;
        private System.Windows.Forms.TextBox version;
        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.TextBox Input_Chat;
        private System.Windows.Forms.Button send_button;
        private System.Windows.Forms.Button change_channel;
    }
}

