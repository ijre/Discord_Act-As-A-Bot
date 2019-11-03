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
            this.browse_button = new System.Windows.Forms.Button();
            this.Output_Chat = new System.Windows.Forms.TextBox();
            this.version = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(366, 275);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(189, 171);
            this.browse_button.TabIndex = 0;
            this.browse_button.Text = "search for hatsune_miku.exe";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.Start);
            // 
            // Output_Chat
            // 
            this.Output_Chat.BackColor = System.Drawing.SystemColors.GrayText;
            this.Output_Chat.ForeColor = System.Drawing.SystemColors.Window;
            this.Output_Chat.Location = new System.Drawing.Point(12, 12);
            this.Output_Chat.Multiline = true;
            this.Output_Chat.Name = "Output_Chat";
            this.Output_Chat.ReadOnly = true;
            this.Output_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output_Chat.Size = new System.Drawing.Size(891, 257);
            this.Output_Chat.TabIndex = 1;
            this.Output_Chat.Visible = false;
            // 
            // version
            // 
            this.version.Location = new System.Drawing.Point(389, 512);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(130, 20);
            this.version.TabIndex = 4;
            this.version.Text = "Hatsune_Miku-Bot 0.0.1.0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(915, 544);
            this.Controls.Add(this.version);
            this.Controls.Add(this.Output_Chat);
            this.Controls.Add(this.browse_button);
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

        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox Output_Chat;
        private System.Windows.Forms.TextBox version;
    }
}

