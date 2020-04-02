// todo: ADD A GODDAMN CLOSE ALL FOR THE IMAGE VIEWER FFS YOU'VE LITERALLY THOUGHT ABOUT IT EVERY TIME YOU'VE TESTED IT CmonBruh
// todo: add ability to send multiple files at once

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.Entities;

using discord_puppet.utils;

namespace discord_puppet
{
    public partial class MainForm : Form
    {
        public DiscordGuild guild;
        public DiscordChannel channel;

        private readonly DiscordClient client;
        private string[] file = new string[10];
        private IOF iof;
        private readonly IntPtr[] openedImages = new IntPtr[100];

        public MainForm(DiscordClient mainClient)
        {
            InitializeComponent();
            Text = $"Act As A Discord Bot (v{Application.ProductVersion})";
            client = mainClient;
        }

        #region WinFormsEvents
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Output_ChatText.Items.Clear();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Add_Image_Click(object sender, EventArgs e)
        {
            if (Add_Image.Text.Contains("Remove"))
            {
                for (int i = 0; i < file.Length; i++)
                    file[i] = "";

                Add_Image.Text = "Add Image/File";
                return;
            }

            using OpenFileDialog diag = new OpenFileDialog
            {
                RestoreDirectory = true,
                Multiselect = true
            };
            diag.ShowDialog();
            if (diag.FileNames.Length > 10)
            {
                MessageBox.Show("Discord only allows 10 attachments per message.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (!string.IsNullOrWhiteSpace(diag.FileNames[0]))
            {
                for (int i = 0; i < diag.FileNames.Length; i++)
                    file[i] = diag.FileNames[i];

                Add_Image.Text = "Remove Image/File";
            }
        }

        #region ContextMenuEvents
        private void CMGreyedOut_Click(object sender, EventArgs e)
        {
            MessageBox.Show("For some reason, Discord will not allow you to interact with messages sent before the connection of your bot.", "Discord is dumb, sorry.",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void Output_ChatCM_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            CMGreyedOut.Visible = false;
            Output_ChatCM.Size = new Size(234, 114);
        }

        private void Output_ChatCM_Opening(object sender, CancelEventArgs e)
        {
            if (Output_ChatText.SelectedIndex <= 99)
            {
                Output_ChatCM.Size = new Size(234, 136);
                CMGreyedOut.Visible = true;

                CMViewImage.Enabled = false;
                CMReact.Enabled = false;
                CMEditMessage.Enabled = false;
                CMDeleteMessage.Enabled = false;
            }
            else if (Output_ChatText.SelectedIndex == -1 || Output_ChatText.SelectedItem.ToString().EndsWith("{MESSAGE DELETED} []"))
                Output_ChatCM.Enabled = false;
            else if (Output_ChatText.SelectedIndex == 100) // index 100 is the "end of prev 100 messages" message
                e.Cancel = true;
            else
            {
                Output_ChatCM.Enabled = true;
                CMReact.Enabled = true;
                var message = MessageUtils.GetMessage(client, MessageUtils.GetID(Output_ChatText.SelectedItem.ToString()), channel);

                switch (message.Attachments.Count)
                {
                    case 0:
                        CMViewImage.Enabled = false;
                        break;
                    case 1:
                        CMViewImage.Enabled = true;
                        CMViewImage.Text = "View Image";
                        break;
                    default:
                        CMViewImage.Enabled = true;
                        CMViewImage.Text = "View Images";
                        break;
                }

                if (message.Author.Id != client.CurrentUser.Id)
                {
                    CMEditMessage.Enabled = false;

                    var getRoles = guild.CurrentMember.Roles;
                    var roles = getRoles.ToArray();

                    for (int i = 0; i < roles.Count(); i++)
                        if (roles.ToArray()[i].Permissions.HasPermission(Permissions.ManageMessages))
                        {
                            CMDeleteMessage.Enabled = true;
                            break;
                        }
                        else
                            CMDeleteMessage.Enabled = false;
                }
                else
                {
                    CMEditMessage.Enabled = true;
                    CMDeleteMessage.Enabled = true;
                }
            }
        }
        #endregion

        private int lastIndex = -1;

        private void Output_ChatText_MouseClick(object sender, MouseEventArgs e)
        {
            if (Output_ChatText.SelectedIndex == lastIndex)
                Output_ChatText.ClearSelected();

            lastIndex = Output_ChatText.SelectedIndex;
        }

        #region Handlers
        #region SendMessageHandling
        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(Input_Chat.Text) && string.IsNullOrWhiteSpace(file[0]))
            {
                MessageBox.Show("Message cannot be empty.", "Empty Message Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(file[0]) && string.IsNullOrWhiteSpace(file[1]))
            {
                using FileStream fstream = new FileStream(file[0], FileMode.Open);

                await channel.SendFileAsync(fstream, Input_Chat.Text);

                file[0] = "";
                Add_Image.Text = "Add Image/File";
            }
            else if (!string.IsNullOrWhiteSpace(file[0]) && !string.IsNullOrWhiteSpace(file[1]))
            {
                Dictionary<string, Stream> files = file.ToDictionary(fileArr => fileArr, stream => Stream.Null);

                await channel.SendMultipleFilesAsync(files, Input_Chat.Text);

                for (int i = 0; i < files.Count; i++)
                    file[i] = "";

                Add_Image.Text = "Add Image/File";
            }
            else
                await client.SendMessageAsync(channel, Input_Chat.Text);

            Input_Chat.Clear();
        }

        #endregion

        #region EditMessageHandling
        private string oldInput = "";
        private void CMEditMessage_Click(object sender, EventArgs e)
        {
            oldInput = Input_Chat.Text;
            Input_Chat.Clear();

            Send_Button.Text = "Edit";
            CancelEdit.Visible = true;
            Output_ChatText.Enabled = false;

            MessageBox.Show("Use the input box to submit your changes.");
        }

        private void CancelEdit_Click(object sender, EventArgs e)
        {
            Input_Chat.Text = oldInput;
            oldInput = null;

            Send_Button.Text = "Send";
            CancelEdit.Visible = false;
            Output_ChatText.Enabled = true;
        }
        #endregion

        #region DeleteMessageHandling
        private void CMDeleteMessage_Click(object sender, EventArgs e)
        {
            MessageUtils.GetMessage(client, MessageUtils.GetID(Output_ChatText.SelectedItem.ToString()), channel).DeleteAsync();
        }
        #endregion

        #region ViewImageHandling
        private void CMViewImage_Click(object sender, EventArgs e)
        {
            var message = MessageUtils.GetMessage(client, MessageUtils.GetID(Output_ChatText.SelectedItem.ToString()), channel);

            if (message.Attachments.Count == 1)
            {
                iof = new IOF(message.Attachments[0].Url, message.Attachments[0].FileName);
                iof.Show();
            }
            else
            {
                MessageBox.Show("Selected message has more than one attachment. Please select which attachments you would like to open.", "Multi-image message",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                for (int i = 0; i < message.Attachments.Count; i++)
                {
                    switch (i + 1)
                    {
                        case 1:
                            Multiple_ImagesLB.Items.Add($"1st file ({message.Attachments[i].FileName})");
                            break;
                        case 2:
                            Multiple_ImagesLB.Items.Add($"2nd file ({message.Attachments[i].FileName})");
                            break;
                        case 3:
                            Multiple_ImagesLB.Items.Add($"3rd file ({message.Attachments[i].FileName})");
                            break;
                        default:
                            Multiple_ImagesLB.Items.Add($"{i}th file ({message.Attachments[i].FileName})");
                            break;
                    }
                }

                Multiple_ImagesLB.BringToFront();
                Multiple_ImagesLB.Visible = true;

                Multiple_ImagesOpen.BringToFront();
                Multiple_ImagesOpen.Visible = true;

                Multiple_ImagesCancel.BringToFront();
                Multiple_ImagesCancel.Visible = true;
            }
        }

        private void Multiple_ImagesOpen_Click(object sender, EventArgs e)
        {
            bool allowFiles = false;
            bool allowFilesAnswered = false;
            ushort availableSpace = 0;
            for (ushort i = 0; i < openedImages.Length; i++)
                if (openedImages[i] == (IntPtr)0)
                {
                    availableSpace = i;
                    break;
                }

            for (int i = 0; i < Multiple_ImagesLB.SelectedIndices.Count; i++)
            {
                var attachment = MessageUtils.GetMessage(client, MessageUtils.GetID(Output_ChatText.SelectedItem.ToString()), channel)
                    .Attachments[Multiple_ImagesLB.SelectedIndices[i]];

                // width == 0 means it's not an image
                if (attachment.Width != 0)
                {
                    iof = new IOF(attachment.Url, attachment.FileName);

                    openedImages[availableSpace] = iof.Handle;
                    iof.Show();
                }
                else
                {
                    if (!allowFilesAnswered)
                        if (MessageBox.Show(
                                "One or more of the selected files is not an image.\n" +
                                "Downloading/Viewing these files requires opening your browser; would you still like to open these files?", "Non-image in selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            allowFiles = true;
                            allowFilesAnswered = true;
                        }

                    if (allowFiles)
                        Process.Start(attachment.Url);
                }
            }

            Multiple_ImagesLB.Visible = false;
            Multiple_ImagesOpen.Visible = false;
            Multiple_ImagesCancel.Visible = false;

            Multiple_ImagesLB.Items.Clear();
        }

        private void Multiple_ImagesCancel_Click(object sender, EventArgs e)
        {
            Multiple_ImagesLB.Visible = false;
            Multiple_ImagesOpen.Visible = false;
            Multiple_ImagesCancel.Visible = false;

            Multiple_ImagesLB.Items.Clear();
        }

        private void CloseAllImages_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < openedImages.Length; i++)
                if (openedImages[i] != IntPtr.Zero)
                    MessageBox.Show(FromHandle(openedImages[i]).FindForm().Text);
        }
        #endregion

        #region ReactionHandling
        private void CMReactText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Return)
                return;

            CMReactText.Enabled = false;

            try
            {
                if (!CMReactText.Text.StartsWith(":") || !CMReactText.Text.EndsWith(":"))
                {
                    while (CMReactText.Text.Contains(":"))
                        CMReactText.Text = CMReactText.Text.Remove(CMReactText.Text.IndexOf(":"), 1);

                    CMReactText.Text = CMReactText.Text.Insert(0, ":") + ":";
                }

                MessageUtils.GetMessage(client, MessageUtils.GetID(Output_ChatText.SelectedItem.ToString()), channel).
                        CreateReactionAsync(DiscordEmoji.FromName(client, CMReactText.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CMReactText.Enabled = true;
            CMReactText.Text = "Input Emoji Here";
        }

        private void CMReactText_Click(object sender, EventArgs e)
        {
            if (CMReactText.Text == "Input Emoji Here")
                CMReactText.Clear();
        }
        #endregion

        #region MultiHandlingFunctions
        private void Send_Button_Click(object sender, EventArgs e)
        {
            if (Send_Button.Text == "Send")
                SendMessage();
            else
            {
                var message = MessageUtils.GetMessage(client, MessageUtils.GetID(Output_ChatText.SelectedItem.ToString()), channel);
                message.ModifyAsync(Input_Chat.Text);

                Input_Chat.Text = oldInput;
                oldInput = null;

                Send_Button.Text = "Send";
                CancelEdit.Visible = false;
                Output_ChatText.Enabled = true;
            }
        }

        private void Input_Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Return)
                return;

            if (Send_Button.Text == "Send")
                SendMessage();
            else
                Send_Button_Click(sender, e);
        }
        #endregion

        #endregion
    }
    #endregion
}