using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.Entities;

using discord_puppet.utils;

namespace discord_puppet
{
    public partial class MainDisplayForm : Form
    {
        public DiscordGuild guild;
        public DiscordChannel channel;

#if !_FINAL
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        public readonly DebugForm debuger;
#endif

        private readonly DiscordClient client;
        private readonly Dictionary<string, Stream> file = new Dictionary<string, Stream>();

        private IOF iof;
        private readonly IntPtr[] handles = new IntPtr[100];
        private int availableSpace;

        public MainDisplayForm(DiscordClient mainClient)
        {
            InitializeComponent();
            Text = $"Act As A Discord Bot (v{Application.ProductVersion})";
            client = mainClient;

#if !_FINAL
            debuger = new DebugForm();
            debuger.Show();
#endif
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
                file.Clear();

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

            if (!string.IsNullOrWhiteSpace(diag.FileName))
            {
                for (int i = 0; i < diag.FileNames.Length; i++)
                    file.Add(diag.FileNames[i], new FileStream(diag.FileNames[i], FileMode.Open));

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
            {
                Output_ChatCM.Enabled = false;
            }
            else if (Output_ChatText.SelectedIndex == 100) // index 100 is the "end of prev 100 messages" message
            {
                e.Cancel = true;
            }
            else
            {
                Output_ChatCM.Enabled = true;
                CMReact.Enabled = true;
                var message = MessageUtils.GetMessage(Output_ChatText.SelectedItem.ToString(), channel);

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
        #region MultiHandlingFunctions
        private void Send_Button_Click(object sender, EventArgs e)
        {
            if (Send_Button.Text == "Send")
                SendMessage();
            else
                EditMessage();
        }

        private void Input_Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Back
                || e.KeyData == Keys.Shift || e.KeyData == Keys.Alt || e.KeyData == Keys.Control
                || Send_Button.Text == "Edit")
            {
                HandleTyping.Enabled = false;
                return;
            }

            if (e.KeyData == Keys.Return)
            {
                if (Send_Button.Text == "Send")
                    SendMessage();
                else
                    EditMessage();

                HandleTyping.Enabled = false;
            }
            else
                HandleTyping.Enabled = true;
        }
        #endregion

        #region TypingEventHandling
        private string prevText = "";
        private void HandleTyping_Tick(object sender, EventArgs e)
        {
            if (prevText != Input_Chat.Text)
                channel.TriggerTypingAsync();

            prevText = Input_Chat.Text;
        }
        #endregion

        #region SendMessageHandling
        private async Task SendMessage()
        {
            Input_Chat.Enabled = false;

            try
            {
                if (string.IsNullOrWhiteSpace(Input_Chat.Text) && file.Count == 0)
                {
                    MessageBox.Show("Message cannot be empty unless you have a file attached.", "Empty Message Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (file.Count >= 1)
                {
                    await channel.SendMultipleFilesAsync(file, Input_Chat.Text);

                    file.Clear();
                    Add_Image.Text = "Add Image/File";
                }
                else
                    await client.SendMessageAsync(channel, Input_Chat.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Input_Chat.Enabled = true;
            Input_Chat.Clear();

            Input_Chat.Focus(); // disabling pulls away focus
        }
        #endregion

        #region EditMessageHandling
        private void EditMessage()
        {
            var message = MessageUtils.GetMessage(Output_ChatText.SelectedItem.ToString(), channel);
            message.ModifyAsync(Input_Chat.Text);

            Input_Chat.Text = oldInput;
            oldInput = null;

            Send_Button.Text = "Send";
            CancelEdit.Visible = false;
            Output_ChatText.Enabled = true;
        }

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
            MessageUtils.GetMessage(Output_ChatText.SelectedItem.ToString(), channel).DeleteAsync();
        }
        #endregion

        #region ViewImageHandling
        private void CMViewImage_Click(object sender, EventArgs e)
        {
            var message = MessageUtils.GetMessage(Output_ChatText.SelectedItem.ToString(), channel);

            if (message.Attachments.Count == 1)
            {
                iof = new IOF(message.Attachments[0].Url, message.Attachments[0].FileName);

                handles[availableSpace] = iof.Handle;
                availableSpace++;

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
                            Multiple_ImagesLB.Items.Add($"{i + 1}th file ({message.Attachments[i].FileName})");
                            break;
                    }
                }

                Multiple_ImagesLB.BringToFront();
                Multiple_ImagesLB.Visible = true;

                Multiple_ImagesOpen.BringToFront();
                Multiple_ImagesOpen.Visible = true;

                Multiple_ImagesCancel.BringToFront();
                Multiple_ImagesCancel.Visible = true;

                CloseAllImages.Visible = true;
            }
        }

        private void Multiple_ImagesOpen_Click(object sender, EventArgs e)
        {
            bool allowFiles = false;
            bool allowFilesAnswered = false;

            for (int i = 0; i < Multiple_ImagesLB.SelectedIndices.Count; i++)
            {
                var attachment = MessageUtils.GetMessage(Output_ChatText.SelectedItem.ToString(), channel)
                    .Attachments[Multiple_ImagesLB.SelectedIndices[i]];

                // width == 0 means it's not an image
                if (attachment.Width != 0)
                {
                    iof = new IOF(attachment.Url, attachment.FileName);

                    handles[availableSpace] = iof.Handle;
                    availableSpace++;

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
            for (int i = 0; i < handles.Length; i++)
                if (handles[i] != IntPtr.Zero)
                    try
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        FromHandle(handles[i]).FindForm().Close();
                        handles[i] = IntPtr.Zero;
                    }
                    catch (NullReferenceException) { }

            CloseAllImages.Visible = false;
            availableSpace = 0;
            Array.Clear(handles, 0, handles.Length);
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

                MessageUtils.GetMessage(Output_ChatText.SelectedItem.ToString(), channel).
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

        #endregion
    }
    #endregion
}