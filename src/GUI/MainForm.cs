using System;
using System.ComponentModel;
using System.Diagnostics;
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
        private string file = "";

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
                file = "";
                Add_Image.Text = "Add Image/File";
                return;
            }

            using OpenFileDialog diag = new OpenFileDialog
            {
                RestoreDirectory = true
            };
            diag.ShowDialog();

            if (!string.IsNullOrWhiteSpace(diag.FileName))
            {
                file = diag.FileName;
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
        }

        private void Output_ChatCM_Opening(object sender, CancelEventArgs e)
        {
            if (Output_ChatText.SelectedIndex <= 99)
            {
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
            if (string.IsNullOrWhiteSpace(Input_Chat.Text) && string.IsNullOrWhiteSpace(file))
            {
                MessageBox.Show("Message cannot be empty.", "Empty Message Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(file))
            {
                using FileStream fstream = new FileStream(file, FileMode.Open);

                await channel.SendFileAsync(fstream, Input_Chat.Text);

                file = "";
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
                IOF iof = new IOF(message.Attachments[0].Url, message.Attachments[0].FileName);
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

            for (int i = 0; i < Multiple_ImagesLB.SelectedIndices.Count; i++)
            {
                var attachment = MessageUtils.GetMessage(client, MessageUtils.GetID(Output_ChatText.SelectedItem.ToString()), channel)
                    .Attachments[Multiple_ImagesLB.SelectedIndices[i]];

                // width == 0 means it's not an image
                if (attachment.Width != 0)
                {
                    IOF iof = new IOF(attachment.Url, attachment.FileName);
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