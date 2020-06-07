namespace discord_puppet
{
    partial class MainDisplayForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDisplayForm));
            this.Output_ChatCM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CMGreyedOut = new System.Windows.Forms.ToolStripMenuItem();
            this.CMViewImage = new System.Windows.Forms.ToolStripMenuItem();
            this.CMReact = new System.Windows.Forms.ToolStripMenuItem();
            this.CMReactText = new System.Windows.Forms.ToolStripTextBox();
            this.CMEditMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.CMDeleteMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.Add_Image = new System.Windows.Forms.Button();
            this.Send_Button = new System.Windows.Forms.Button();
            this.Clear_Button = new System.Windows.Forms.Button();
            this.Input_Chat = new System.Windows.Forms.TextBox();
            this.Output_ChatText = new System.Windows.Forms.ListBox();
            this.Multiple_ImagesOpen = new System.Windows.Forms.Button();
            this.Multiple_ImagesCancel = new System.Windows.Forms.Button();
            this.Multiple_ImagesLB = new System.Windows.Forms.ListBox();
            this.CancelEdit = new System.Windows.Forms.Button();
            this.CloseAllImages = new System.Windows.Forms.Button();
            this.HandleTyping = new System.Windows.Forms.Timer(this.components);
            this.Output_ChatCM.SuspendLayout();
            this.SuspendLayout();
            // 
            // Output_ChatCM
            // 
            this.Output_ChatCM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CMGreyedOut,
            this.CMViewImage,
            this.CMReact,
            this.CMEditMessage,
            this.CMDeleteMessage});
            this.Output_ChatCM.Name = "Output_ChatCM";
            this.Output_ChatCM.Size = new System.Drawing.Size(234, 114);
            this.Output_ChatCM.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.Output_ChatCM_Closing);
            this.Output_ChatCM.Opening += new System.ComponentModel.CancelEventHandler(this.Output_ChatCM_Opening);
            // 
            // CMGreyedOut
            // 
            this.CMGreyedOut.Name = "CMGreyedOut";
            this.CMGreyedOut.Size = new System.Drawing.Size(233, 22);
            this.CMGreyedOut.Text = "Why is everything greyed out?";
            this.CMGreyedOut.Visible = false;
            this.CMGreyedOut.Click += new System.EventHandler(this.CMGreyedOut_Click);
            // 
            // CMViewImage
            // 
            this.CMViewImage.Name = "CMViewImage";
            this.CMViewImage.Size = new System.Drawing.Size(233, 22);
            this.CMViewImage.Text = "View Image";
            this.CMViewImage.Click += new System.EventHandler(this.CMViewImage_Click);
            // 
            // CMReact
            // 
            this.CMReact.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CMReactText});
            this.CMReact.Name = "CMReact";
            this.CMReact.Size = new System.Drawing.Size(233, 22);
            this.CMReact.Text = "React";
            // 
            // CMReactText
            // 
            this.CMReactText.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CMReactText.Name = "CMReactText";
            this.CMReactText.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.CMReactText.Size = new System.Drawing.Size(100, 23);
            this.CMReactText.Text = "Input Emoji Here";
            this.CMReactText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CMReactText_KeyDown);
            this.CMReactText.Click += new System.EventHandler(this.CMReactText_Click);
            // 
            // CMEditMessage
            // 
            this.CMEditMessage.Name = "CMEditMessage";
            this.CMEditMessage.Size = new System.Drawing.Size(233, 22);
            this.CMEditMessage.Text = "Edit Message";
            this.CMEditMessage.Click += new System.EventHandler(this.CMEditMessage_Click);
            // 
            // CMDeleteMessage
            // 
            this.CMDeleteMessage.Name = "CMDeleteMessage";
            this.CMDeleteMessage.Size = new System.Drawing.Size(233, 22);
            this.CMDeleteMessage.Text = "Delete Message";
            this.CMDeleteMessage.Click += new System.EventHandler(this.CMDeleteMessage_Click);
            // 
            // Add_Image
            // 
            this.Add_Image.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Add_Image.Location = new System.Drawing.Point(773, 278);
            this.Add_Image.Name = "Add_Image";
            this.Add_Image.Size = new System.Drawing.Size(130, 120);
            this.Add_Image.TabIndex = 4;
            this.Add_Image.Text = "Add Image/File";
            this.Add_Image.UseVisualStyleBackColor = true;
            this.Add_Image.Click += new System.EventHandler(this.Add_Image_Click);
            // 
            // Send_Button
            // 
            this.Send_Button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Send_Button.Location = new System.Drawing.Point(523, 278);
            this.Send_Button.Name = "Send_Button";
            this.Send_Button.Size = new System.Drawing.Size(130, 120);
            this.Send_Button.TabIndex = 3;
            this.Send_Button.Text = "Send";
            this.Send_Button.UseVisualStyleBackColor = true;
            this.Send_Button.Click += new System.EventHandler(this.Send_Button_Click);
            // 
            // Clear_Button
            // 
            this.Clear_Button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Clear_Button.Location = new System.Drawing.Point(12, 282);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(130, 120);
            this.Clear_Button.TabIndex = 14;
            this.Clear_Button.Text = "Clear Messages";
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // Input_Chat
            // 
            this.Input_Chat.AcceptsReturn = true;
            this.Input_Chat.AcceptsTab = true;
            this.Input_Chat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Input_Chat.BackColor = System.Drawing.SystemColors.GrayText;
            this.Input_Chat.ForeColor = System.Drawing.Color.White;
            this.Input_Chat.Location = new System.Drawing.Point(523, 12);
            this.Input_Chat.Margin = new System.Windows.Forms.Padding(0);
            this.Input_Chat.Multiline = true;
            this.Input_Chat.Name = "Input_Chat";
            this.Input_Chat.Size = new System.Drawing.Size(380, 260);
            this.Input_Chat.TabIndex = 1;
            this.Input_Chat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_Chat_KeyDown);
            // 
            // Output_ChatText
            // 
            this.Output_ChatText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Output_ChatText.BackColor = System.Drawing.SystemColors.GrayText;
            this.Output_ChatText.ContextMenuStrip = this.Output_ChatCM;
            this.Output_ChatText.ForeColor = System.Drawing.Color.White;
            this.Output_ChatText.FormattingEnabled = true;
            this.Output_ChatText.HorizontalScrollbar = true;
            this.Output_ChatText.Location = new System.Drawing.Point(12, 12);
            this.Output_ChatText.Margin = new System.Windows.Forms.Padding(0);
            this.Output_ChatText.Name = "Output_ChatText";
            this.Output_ChatText.Size = new System.Drawing.Size(380, 264);
            this.Output_ChatText.TabIndex = 19;
            this.Output_ChatText.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Output_ChatText_MouseClick);
            // 
            // Multiple_ImagesOpen
            // 
            this.Multiple_ImagesOpen.BackColor = System.Drawing.SystemColors.Window;
            this.Multiple_ImagesOpen.Location = new System.Drawing.Point(0, 506);
            this.Multiple_ImagesOpen.Name = "Multiple_ImagesOpen";
            this.Multiple_ImagesOpen.Size = new System.Drawing.Size(93, 44);
            this.Multiple_ImagesOpen.TabIndex = 21;
            this.Multiple_ImagesOpen.Text = "Open";
            this.Multiple_ImagesOpen.UseVisualStyleBackColor = false;
            this.Multiple_ImagesOpen.Visible = false;
            this.Multiple_ImagesOpen.Click += new System.EventHandler(this.Multiple_ImagesOpen_Click);
            // 
            // Multiple_ImagesCancel
            // 
            this.Multiple_ImagesCancel.BackColor = System.Drawing.SystemColors.Window;
            this.Multiple_ImagesCancel.Location = new System.Drawing.Point(822, 506);
            this.Multiple_ImagesCancel.Name = "Multiple_ImagesCancel";
            this.Multiple_ImagesCancel.Size = new System.Drawing.Size(93, 44);
            this.Multiple_ImagesCancel.TabIndex = 22;
            this.Multiple_ImagesCancel.Text = "Cancel";
            this.Multiple_ImagesCancel.UseVisualStyleBackColor = false;
            this.Multiple_ImagesCancel.Visible = false;
            this.Multiple_ImagesCancel.Click += new System.EventHandler(this.Multiple_ImagesCancel_Click);
            // 
            // Multiple_ImagesLB
            // 
            this.Multiple_ImagesLB.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.Multiple_ImagesLB.ForeColor = System.Drawing.Color.White;
            this.Multiple_ImagesLB.FormattingEnabled = true;
            this.Multiple_ImagesLB.Location = new System.Drawing.Point(0, 0);
            this.Multiple_ImagesLB.Name = "Multiple_ImagesLB";
            this.Multiple_ImagesLB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Multiple_ImagesLB.Size = new System.Drawing.Size(915, 550);
            this.Multiple_ImagesLB.TabIndex = 20;
            this.Multiple_ImagesLB.TabStop = false;
            this.Multiple_ImagesLB.Visible = false;
            // 
            // CancelEdit
            // 
            this.CancelEdit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CancelEdit.Location = new System.Drawing.Point(523, 404);
            this.CancelEdit.Name = "CancelEdit";
            this.CancelEdit.Size = new System.Drawing.Size(130, 45);
            this.CancelEdit.TabIndex = 23;
            this.CancelEdit.Text = "Cancel Edit";
            this.CancelEdit.UseVisualStyleBackColor = true;
            this.CancelEdit.Visible = false;
            this.CancelEdit.Click += new System.EventHandler(this.CancelEdit_Click);
            // 
            // CloseAllImages
            // 
            this.CloseAllImages.Location = new System.Drawing.Point(392, 9);
            this.CloseAllImages.Margin = new System.Windows.Forms.Padding(0);
            this.CloseAllImages.Name = "CloseAllImages";
            this.CloseAllImages.Size = new System.Drawing.Size(131, 56);
            this.CloseAllImages.TabIndex = 24;
            this.CloseAllImages.Text = "Close All Images";
            this.CloseAllImages.UseVisualStyleBackColor = true;
            this.CloseAllImages.Visible = false;
            this.CloseAllImages.Click += new System.EventHandler(this.CloseAllImages_Click);
            // 
            // HandleTyping
            // 
            this.HandleTyping.Tick += new System.EventHandler(this.HandleTyping_Tick);
            // 
            // MainDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(915, 547);
            this.Controls.Add(this.CloseAllImages);
            this.Controls.Add(this.CancelEdit);
            this.Controls.Add(this.Multiple_ImagesCancel);
            this.Controls.Add(this.Clear_Button);
            this.Controls.Add(this.Add_Image);
            this.Controls.Add(this.Send_Button);
            this.Controls.Add(this.Multiple_ImagesOpen);
            this.Controls.Add(this.Output_ChatText);
            this.Controls.Add(this.Input_Chat);
            this.Controls.Add(this.Multiple_ImagesLB);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(931, 586);
            this.Name = "MainDisplayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Act As A Discord Bot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Output_ChatCM.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ContextMenuStrip Output_ChatCM;
        public System.Windows.Forms.ToolStripMenuItem CMViewImage;
        public System.Windows.Forms.ToolStripMenuItem CMReact;
        public System.Windows.Forms.Button Add_Image;
        public System.Windows.Forms.Button Send_Button;
        public System.Windows.Forms.Button Clear_Button;
        public System.Windows.Forms.TextBox Input_Chat;
        public System.Windows.Forms.ListBox Output_ChatText;
        public System.Windows.Forms.Button Multiple_ImagesOpen;
        public System.Windows.Forms.Button Multiple_ImagesCancel;
        public System.Windows.Forms.ToolStripTextBox CMReactText;
        public System.Windows.Forms.ListBox Multiple_ImagesLB;
        public System.Windows.Forms.ToolStripMenuItem CMEditMessage;
        public System.Windows.Forms.ToolStripMenuItem CMDeleteMessage;
        public System.Windows.Forms.Button CancelEdit;
        private System.Windows.Forms.ToolStripMenuItem CMGreyedOut;
        private System.Windows.Forms.Button CloseAllImages;
        private System.Windows.Forms.Timer HandleTyping;
    }
}

