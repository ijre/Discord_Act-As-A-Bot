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
            this.Start_Button = new System.Windows.Forms.Button();
            this.Input_Chat = new System.Windows.Forms.TextBox();
            this.Send_Button = new System.Windows.Forms.Button();
            this.Change_Channel = new System.Windows.Forms.Button();
            this.Add_Image = new System.Windows.Forms.Button();
            this.File_Name = new System.Windows.Forms.TextBox();
            this.React = new System.Windows.Forms.Button();
            this.Output_Chat = new System.Windows.Forms.TextBox();
            this.ReactText = new System.Windows.Forms.TextBox();
            this.Please_Wait = new System.Windows.Forms.TextBox();
            this.React_Confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Start_Button
            // 
            this.Start_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Start_Button.Location = new System.Drawing.Point(389, 290);
            this.Start_Button.Name = "Start_Button";
            this.Start_Button.Size = new System.Drawing.Size(130, 120);
            this.Start_Button.TabIndex = 0;
            this.Start_Button.TabStop = false;
            this.Start_Button.Text = "Launch Hatsune Miku Bot";
            this.Start_Button.UseVisualStyleBackColor = true;
            this.Start_Button.Click += new System.EventHandler(this.Start);
            // 
            // Input_Chat
            // 
            this.Input_Chat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Input_Chat.BackColor = System.Drawing.SystemColors.GrayText;
            this.Input_Chat.ForeColor = System.Drawing.Color.White;
            this.Input_Chat.Location = new System.Drawing.Point(516, 12);
            this.Input_Chat.Multiline = true;
            this.Input_Chat.Name = "Input_Chat";
            this.Input_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Input_Chat.Size = new System.Drawing.Size(387, 257);
            this.Input_Chat.TabIndex = 5;
            this.Input_Chat.Visible = false;
            // 
            // Send_Button
            // 
            this.Send_Button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Send_Button.Location = new System.Drawing.Point(658, 275);
            this.Send_Button.Name = "Send_Button";
            this.Send_Button.Size = new System.Drawing.Size(84, 84);
            this.Send_Button.TabIndex = 6;
            this.Send_Button.Text = "Send";
            this.Send_Button.UseVisualStyleBackColor = true;
            this.Send_Button.Visible = false;
            // 
            // Change_Channel
            // 
            this.Change_Channel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Change_Channel.Location = new System.Drawing.Point(12, 290);
            this.Change_Channel.Name = "Change_Channel";
            this.Change_Channel.Size = new System.Drawing.Size(130, 120);
            this.Change_Channel.TabIndex = 7;
            this.Change_Channel.Text = "Change Server/Channel";
            this.Change_Channel.UseVisualStyleBackColor = true;
            this.Change_Channel.Visible = false;
            this.Change_Channel.Click += new System.EventHandler(this.Change_Channel_Click);
            // 
            // Add_Image
            // 
            this.Add_Image.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Add_Image.Location = new System.Drawing.Point(748, 275);
            this.Add_Image.Name = "Add_Image";
            this.Add_Image.Size = new System.Drawing.Size(84, 84);
            this.Add_Image.TabIndex = 8;
            this.Add_Image.Text = "Add Image/File";
            this.Add_Image.UseVisualStyleBackColor = true;
            this.Add_Image.Visible = false;
            this.Add_Image.Click += new System.EventHandler(this.Add_Image_Click);
            // 
            // File_Name
            // 
            this.File_Name.CausesValidation = false;
            this.File_Name.Location = new System.Drawing.Point(881, 529);
            this.File_Name.Name = "File_Name";
            this.File_Name.Size = new System.Drawing.Size(8, 20);
            this.File_Name.TabIndex = 9;
            this.File_Name.TabStop = false;
            this.File_Name.Visible = false;
            // 
            // React
            // 
            this.React.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.React.Location = new System.Drawing.Point(262, 290);
            this.React.Name = "React";
            this.React.Size = new System.Drawing.Size(130, 120);
            this.React.TabIndex = 10;
            this.React.Text = "React to a message";
            this.React.UseVisualStyleBackColor = true;
            this.React.Visible = false;
            // 
            // Output_Chat
            // 
            this.Output_Chat.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            // ReactText
            // 
            this.ReactText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ReactText.BackColor = System.Drawing.SystemColors.GrayText;
            this.ReactText.ForeColor = System.Drawing.Color.White;
            this.ReactText.Location = new System.Drawing.Point(262, 416);
            this.ReactText.Multiline = true;
            this.ReactText.Name = "ReactText";
            this.ReactText.Size = new System.Drawing.Size(130, 42);
            this.ReactText.TabIndex = 12;
            this.ReactText.TabStop = false;
            this.ReactText.Text = "Type in your reaction here.";
            this.ReactText.Visible = false;
            this.ReactText.Enter += new System.EventHandler(this.ReactText_Enter);
            // 
            // Please_Wait
            // 
            this.Please_Wait.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Please_Wait.BackColor = System.Drawing.SystemColors.GrayText;
            this.Please_Wait.ForeColor = System.Drawing.Color.White;
            this.Please_Wait.Location = new System.Drawing.Point(-1, -2);
            this.Please_Wait.Multiline = true;
            this.Please_Wait.Name = "Please_Wait";
            this.Please_Wait.ReadOnly = true;
            this.Please_Wait.Size = new System.Drawing.Size(916, 551);
            this.Please_Wait.TabIndex = 11;
            this.Please_Wait.TabStop = false;
            this.Please_Wait.Text = resources.GetString("Please_Wait.Text");
            this.Please_Wait.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Please_Wait.Visible = false;
            // 
            // React_Confirm
            // 
            this.React_Confirm.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.React_Confirm.Location = new System.Drawing.Point(262, 461);
            this.React_Confirm.Name = "React_Confirm";
            this.React_Confirm.Size = new System.Drawing.Size(130, 45);
            this.React_Confirm.TabIndex = 13;
            this.React_Confirm.TabStop = false;
            this.React_Confirm.Text = "Confirm";
            this.React_Confirm.UseVisualStyleBackColor = true;
            this.React_Confirm.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(915, 547);
            this.Controls.Add(this.React_Confirm);
            this.Controls.Add(this.React);
            this.Controls.Add(this.ReactText);
            this.Controls.Add(this.File_Name);
            this.Controls.Add(this.Add_Image);
            this.Controls.Add(this.Change_Channel);
            this.Controls.Add(this.Send_Button);
            this.Controls.Add(this.Input_Chat);
            this.Controls.Add(this.Output_Chat);
            this.Controls.Add(this.Start_Button);
            this.Controls.Add(this.Please_Wait);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MinimumSize = new System.Drawing.Size(931, 586);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hatsune Miku (version 1.0.0.1)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Start_Button;
        private System.Windows.Forms.TextBox Input_Chat;
        private System.Windows.Forms.Button Send_Button;
        private System.Windows.Forms.Button Change_Channel;
        private System.Windows.Forms.Button Add_Image;
        private System.Windows.Forms.TextBox File_Name;
        private System.Windows.Forms.Button React;
        private System.Windows.Forms.TextBox Output_Chat;
        private System.Windows.Forms.TextBox ReactText;
        private System.Windows.Forms.TextBox Please_Wait;
        private System.Windows.Forms.Button React_Confirm;
    }
}

