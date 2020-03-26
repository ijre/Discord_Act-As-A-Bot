namespace hatsune_miku
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Input_Chat = new System.Windows.Forms.TextBox();
            this.Send_Button = new System.Windows.Forms.Button();
            this.Change_Channel = new System.Windows.Forms.Button();
            this.Add_Image = new System.Windows.Forms.Button();
            this.Clear_Button = new System.Windows.Forms.Button();
            this.ServerChannelList = new System.Windows.Forms.ListBox();
            this.Output_ChatCM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CMViewImage = new System.Windows.Forms.ToolStripMenuItem();
            this.CMReact = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Output_ChatText = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Output_ChatCM.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Input_Chat
            // 
            this.Input_Chat.AcceptsReturn = true;
            this.Input_Chat.AcceptsTab = true;
            this.Input_Chat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Input_Chat.BackColor = System.Drawing.SystemColors.GrayText;
            this.Input_Chat.ForeColor = System.Drawing.Color.White;
            this.Input_Chat.Location = new System.Drawing.Point(524, 10);
            this.Input_Chat.Multiline = true;
            this.Input_Chat.Name = "Input_Chat";
            this.Input_Chat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Input_Chat.Size = new System.Drawing.Size(380, 260);
            this.Input_Chat.TabIndex = 1;
            this.Input_Chat.Visible = false;
            this.Input_Chat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_Chat_KeyDown);
            // 
            // Send_Button
            // 
            this.Send_Button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Send_Button.Location = new System.Drawing.Point(659, 276);
            this.Send_Button.Name = "Send_Button";
            this.Send_Button.Size = new System.Drawing.Size(84, 84);
            this.Send_Button.TabIndex = 3;
            this.Send_Button.Text = "Send";
            this.Send_Button.UseVisualStyleBackColor = true;
            this.Send_Button.Click += new System.EventHandler(this.Send_Button_Click);
            // 
            // Change_Channel
            // 
            this.Change_Channel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Change_Channel.Location = new System.Drawing.Point(9, 276);
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
            this.Add_Image.Location = new System.Drawing.Point(749, 276);
            this.Add_Image.Name = "Add_Image";
            this.Add_Image.Size = new System.Drawing.Size(84, 84);
            this.Add_Image.TabIndex = 4;
            this.Add_Image.Text = "Add Image/File";
            this.Add_Image.UseVisualStyleBackColor = true;
            this.Add_Image.Visible = false;
            this.Add_Image.Click += new System.EventHandler(this.Add_Image_Click);
            // 
            // Clear_Button
            // 
            this.Clear_Button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Clear_Button.Location = new System.Drawing.Point(259, 276);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(130, 120);
            this.Clear_Button.TabIndex = 14;
            this.Clear_Button.Text = "Clear Messages";
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // ServerChannelList
            // 
            this.ServerChannelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerChannelList.BackColor = System.Drawing.SystemColors.GrayText;
            this.ServerChannelList.ForeColor = System.Drawing.Color.White;
            this.ServerChannelList.FormattingEnabled = true;
            this.ServerChannelList.HorizontalScrollbar = true;
            this.ServerChannelList.Location = new System.Drawing.Point(62, 17);
            this.ServerChannelList.Name = "ServerChannelList";
            this.ServerChannelList.Size = new System.Drawing.Size(777, 472);
            this.ServerChannelList.TabIndex = 0;
            // 
            // Output_ChatCM
            // 
            this.Output_ChatCM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CMViewImage,
            this.CMReact});
            this.Output_ChatCM.Name = "Output_ChatCM";
            this.Output_ChatCM.Size = new System.Drawing.Size(136, 48);
            this.Output_ChatCM.Opening += new System.ComponentModel.CancelEventHandler(this.Output_ChatCM_Opening);
            // 
            // CMViewImage
            // 
            this.CMViewImage.Name = "CMViewImage";
            this.CMViewImage.Size = new System.Drawing.Size(135, 22);
            this.CMViewImage.Text = "View Image";
            this.CMViewImage.Click += new System.EventHandler(this.CMViewImage_Click);
            // 
            // CMReact
            // 
            this.CMReact.Name = "CMReact";
            this.CMReact.Size = new System.Drawing.Size(135, 22);
            this.CMReact.Text = "React";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(918, 549);
            this.tabControl1.TabIndex = 20;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.tabPage1.BackgroundImage = global::hatsune_miku.Properties.Resources.corrupted_pic;
            this.tabPage1.Controls.Add(this.Button1);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Controls.Add(this.Output_ChatText);
            this.tabPage1.Controls.Add(this.Input_Chat);
            this.tabPage1.Controls.Add(this.Clear_Button);
            this.tabPage1.Controls.Add(this.Send_Button);
            this.tabPage1.Controls.Add(this.Add_Image);
            this.tabPage1.Controls.Add(this.Change_Channel);
            this.tabPage1.Controls.Add(this.ServerChannelList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(910, 523);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(410, 134);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(93, 44);
            this.Button1.TabIndex = 21;
            this.Button1.Text = "Button1";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Visible = false;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(244, 43);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(341, 212);
            this.listBox1.TabIndex = 20;
            this.listBox1.TabStop = false;
            this.listBox1.Visible = false;
            // 
            // Output_ChatText
            // 
            this.Output_ChatText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Output_ChatText.BackColor = System.Drawing.SystemColors.GrayText;
            this.Output_ChatText.ContextMenuStrip = this.Output_ChatCM;
            this.Output_ChatText.ForeColor = System.Drawing.Color.White;
            this.Output_ChatText.FormattingEnabled = true;
            this.Output_ChatText.Location = new System.Drawing.Point(9, 6);
            this.Output_ChatText.Name = "Output_ChatText";
            this.Output_ChatText.Size = new System.Drawing.Size(380, 264);
            this.Output_ChatText.TabIndex = 19;
            this.Output_ChatText.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.tabPage2.BackgroundImage = global::hatsune_miku.Properties.Resources.corrupted_pic;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(910, 523);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(915, 547);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(931, 586);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hatsune Miku";
            this.Output_ChatCM.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox Input_Chat;
        private System.Windows.Forms.Button Send_Button;
        private System.Windows.Forms.Button Change_Channel;
        private System.Windows.Forms.Button Add_Image;
        private System.Windows.Forms.Button Clear_Button;
        private System.Windows.Forms.ListBox ServerChannelList;
        private System.Windows.Forms.ContextMenuStrip Output_ChatCM;
        private System.Windows.Forms.ToolStripMenuItem CMViewImage;
        private System.Windows.Forms.ToolStripMenuItem CMReact;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox Output_ChatText;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Button1;
    }
}

