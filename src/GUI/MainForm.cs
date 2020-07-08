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
using DSharpPlus.EventArgs;

using discord_puppet.utils;

namespace discord_puppet
{
    public partial class MainForm : Form
    {
        private DiscordGuild guild;
        private DiscordChannel channel;

#if !_FINAL
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DebugForm debuger;
#endif

        private readonly DiscordClient client;
        private readonly Dictionary<string, Stream> file = new Dictionary<string, Stream>();

        private IOF iof;
        private readonly IntPtr[] handles = new IntPtr[100];
        private int availableSpace;

        public MainForm()
        {
            InitializeComponent();
            Text = $"Act As A Discord Bot (v{Application.ProductVersion})";

#if !_FINAL
            debuger = new DebugForm();
            debuger.Show();
#endif

            if (!Directory.Exists("./deps/"))
#if !_DEBUG
            {
                MessageBox.Show("Could not find deps folder, this folder and its original contents are required for the program to run.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!File.Exists("./deps/id.txt") || !File.Exists("./deps/server_choose.exe"))
                MessageBox.Show("id.txt or server_choose.exe are missing from deps folder; these two files are required for the program to run.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#else
                Directory.CreateDirectory("./deps/");
#endif

#if !_OFF
            client = new DiscordClient(new DiscordConfiguration
            {
#if !_DEBUG
                Token = File.ReadAllText("./deps/id.txt")
#else
                Token = File.ReadAllText("../deps/id.txt")
#endif
            });
#endif

#if !_DEBUG
            Servers.Items.Clear();
            MemberList.Items.Clear();
#endif

#if !_OFF
            client.MessageCreated += OnMessageCreated;
            client.MessageUpdated += OnMessageUpdated;
            client.MessageDeleted += OnMessageDeleted;
            client.Ready += OnReady;

            client.ConnectAsync();
            client.InitializeAsync();
#endif
        }

        #region DiscordEvents
        private Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (e.Message.Channel.Id != channel.Id)
                return Task.CompletedTask;

            var message = e.Message;

            Output_ChatText.Items.Add(AddAttachmentText(message));

            if (Output_ChatText.Items.Count - 1 - Output_ChatText.TopIndex == 18) // if we're scrolled to the very bottom of the chat
                Output_ChatText.TopIndex = Output_ChatText.Items.Count - 1; // scroll to adjust to new message

            return Task.CompletedTask;
        }

        private Task OnMessageUpdated(MessageUpdateEventArgs e)
        {
            if (e.Message.Channel.Id != channel.Id)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < Output_ChatText.Items.Count; i++)
                if (i != 100 && MessageUtils.GetID(Output_ChatText.Items[i].ToString()) == message.Id)
                    Output_ChatText.Items[i] = AddAttachmentText(message).Insert(Output_ChatText.Items[i].ToString().LastIndexOf("[") - 4, " (edited)");

            return Task.CompletedTask;
        }

        private Task OnMessageDeleted(MessageDeleteEventArgs e)
        {
            if (e.Message.Channel.Id != channel.Id)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < Output_ChatText.Items.Count; i++)
                if (i != 100 && MessageUtils.GetID(Output_ChatText.Items[i].ToString()) == message.Id)
                {
                    Output_ChatText.Items[i] = Output_ChatText.Items[i].ToString().Substring(0, Output_ChatText.Items[i].ToString().LastIndexOf("[") - 1); // delete everything past the message's body
                    Output_ChatText.Items[i] += " {MESSAGE DELETED} []"; // and add a note that it's deleted
                }

            return Task.CompletedTask;
        }

        private Task OnReady(ReadyEventArgs e)
        {
#if !_DEBUG
            using Process process = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "./deps/server_choose.exe",
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.OutputDataReceived += (object sender, DataReceivedEventArgs ee) =>
            {
                try
                {
                    Servers.Items.Add(ee.Data);
                }
                catch (Exception ex)
                {
                    ExceptionUtils.IgnoreSpecificException(new ArgumentNullException(), ex);
                }
            };
            process.Start();
            process.BeginOutputReadLine();
#else
            MessageBox.Show("");
#endif

            return Task.CompletedTask;
        }
        #endregion

        #region WinFormsEvents
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Output_ChatText.Items.Clear();
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

            if (string.IsNullOrWhiteSpace(diag.FileName))
                return;

            if (diag.FileNames.Length > 10)
            {
                MessageBox.Show("Discord only allows 10 attachments per message.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }


            for (int i = 0; i < diag.FileNames.Length; i++)
                file.Add(diag.FileNames[i], new FileStream(diag.FileNames[i], FileMode.Open));

            Add_Image.Text = "Remove Image/File";
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
                var message = GetMessage(Output_ChatText.SelectedItem.ToString());

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

        private void MemberListCM_Opening(object sender, CancelEventArgs e)
        {

        }
        #endregion

        #region ListBoxIndexChanges
        private int lastIndex = -1;
        private void Output_ChatText_MouseClick(object sender, MouseEventArgs e)
        {
            if (Output_ChatText.SelectedIndex == lastIndex)
                Output_ChatText.ClearSelected();

            lastIndex = Output_ChatText.SelectedIndex;
        }

        private int lastIndexServers = -1;
        private void Servers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Servers.SelectedIndex == lastIndexServers)
                return;

            lastIndexServers = Servers.SelectedIndex;
            DoStuffSync(true);
        }


        private int lastIndexChannels = -1;
        private void Channels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Channels.SelectedIndex == lastIndexChannels)
                return;

            lastIndexChannels = Channels.SelectedIndex;
            DoStuffSync(false);
        }

        private int lastIndexMemberList = -1;
        private void MemberList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (MemberList.SelectedIndex != lastIndexMemberList)
                lastIndexMemberList = MemberList.SelectedIndex;
        }
        #endregion

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
                || e.KeyData == Keys.Shift || e.KeyData == Keys.Alt || e.KeyData == Keys.Control || e.KeyData == Keys.Tab
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
            var message = GetMessage(Output_ChatText.SelectedItem.ToString());
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
            GetMessage(Output_ChatText.SelectedItem.ToString()).DeleteAsync();
        }
        #endregion

        #region ViewImageHandling
        private void CMViewImage_Click(object sender, EventArgs e)
        {
            var message = GetMessage(Output_ChatText.SelectedItem.ToString());

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
            }

            CloseAllImages.Visible = true;
        }

        private void Multiple_ImagesOpen_Click(object sender, EventArgs e)
        {
            bool allowFiles = false;
            bool allowFilesAnswered = false;

            for (int i = 0; i < Multiple_ImagesLB.SelectedIndices.Count; i++)
            {
                var attachment = GetMessage(Output_ChatText.SelectedItem.ToString())
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

                GetMessage(Output_ChatText.SelectedItem.ToString())
                    .CreateReactionAsync(DiscordEmoji.FromName(client, CMReactText.Text));
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
        #endregion // Reaction Handling
        #endregion // Handlers
        #endregion // WinForms Events

        #region Helpers
        private DiscordMessage GetMessage(string message)
        {
            try
            {
                return channel.GetMessageAsync(MessageUtils.GetID(message)).Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return null;
            }
        }

        private async void DoStuffSync(bool server)
        {
            if (server)
            {
                if (Channels.Items.Count != 0)
                {
                    lastIndexChannels = -1;
                    Channels.ClearSelected();
                    Channels.Items.Clear();

                    lastIndexMemberList = -1;
                    MemberList.ClearSelected();
                    MemberList.Items.Clear();
                }

                string item2S = "";
                try
                {
                    item2S = Servers.SelectedItem.ToString();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.IgnoreSpecificException(new NullReferenceException(), ex, true, "Fatal Error");
                }

                var chosenGuild = await client.GetGuildAsync(MessageUtils.GetID(item2S));
                IEnumerable<DiscordChannel> channels = from value in await chosenGuild.GetChannelsAsync()
                                                       select value;
                DiscordChannel[] chanArray = channels.ToArray();

                for (int i = 0; i < chanArray.Length; i++)
                    if (chanArray[i].Type == ChannelType.Text && (chanArray[i].PermissionsFor(chosenGuild.CurrentMember) & Permissions.AccessChannels) != 0)
                        Channels.Items.Add($"{chanArray[i].Name} [{chanArray[i].Id}]");


                var membersArray = chosenGuild.Members.ToArray();

                for (int i = 0; i < membersArray.Length; i++)
                    MemberList.Items.Add($"{membersArray[i].DisplayName}#{membersArray[i].Discriminator} [{membersArray[i].Id}]");

                guild = chosenGuild;
            }
            else
            {
                Output_ChatText.Items.Clear();
                channel = client.GetChannelAsync(MessageUtils.GetID(Channels.SelectedItem.ToString())).Result;

                var getMessages = await channel.GetMessagesAsync();

                IEnumerable<DiscordMessage> messages = from value in getMessages
                                                       select value;
                var messagesArray = messages.ToArray();

                for (int i = messagesArray.Length - 1; 0 <= i; i--)
                    Output_ChatText.Items.Add(AddAttachmentText(messagesArray[i]));

                Output_ChatText.Items.Add("((((((((((END OF PREVIOUS 100 MESSAGES)))))))))");

                Output_ChatText.TopIndex = Output_ChatText.Items.Count - 1;

                Input_Chat.Enabled = true;
            }
        }

        private static string AddAttachmentText(DiscordMessage message)
        {
            string attachments = "";

            if (message.Attachments.Count == 1)
                attachments = " (IMAGE ATTACHED)";
            else if (message.Attachments.Count > 1)
                attachments = " (MULTIPLE IMAGES ATTACHED)";

            return $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content}{attachments}    [{message.Id}]";
        }
        #endregion
    }
}