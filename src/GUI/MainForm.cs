﻿using System;
using System.Diagnostics;
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
        private ulong channel;
        private ulong guild;

        private readonly ulong[] messageIds = new ulong[500];
        private string file = "";

        public MainForm()
        {
            InitializeComponent();
            ServerChannelList.BringToFront();
            Text = $"Act As A Discord Bot (v{Application.ProductVersion})";

            if (!Directory.Exists("./deps/"))
#if !_DEBUG
                MessageBox.Show("Could not find deps folder, this folder and its original contents are required for the program to run.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#else
                Directory.CreateDirectory("./deps/");
#endif

            TrueStart();
        }

        private readonly DiscordClient client = new DiscordClient(new DiscordConfiguration
        {
#if !_DEBUG
            Token = File.ReadAllText("./deps/id.txt")
#else
            Token = File.ReadAllText("../deps/id.txt")
#endif
        });

        private async Task TrueStart()
        {
            await client.ConnectAsync();
            await client.InitializeAsync();

            client.Ready += OnReady;
            client.MessageCreated += OnMessageCreated;
            client.MessageUpdated += OnMessageUpdated;
            client.MessageDeleted += OnMessageDeleted;

            bool server = true;
            ServerChannelList.DoubleClick += async (object sender, EventArgs e) =>
            {
                string item2S;
                try
                {
                    item2S = ServerChannelList.SelectedItem.ToString();
                }
                catch (Exception ex)
                {
                    ExceptionUtils.IgnoreSpecificException(new NullReferenceException(), ex, true, "Fatal Error");
                    return;
                }

                if (server)
                {
                    var chosenGuild = await client.GetGuildAsync(ulong.Parse(item2S.Substring(item2S.LastIndexOf("(") + 1, item2S.Length - item2S.LastIndexOf("(") - 2)));
                    IEnumerable<DiscordChannel> channels = from value in await chosenGuild.GetChannelsAsync()
                                                           select value;
                    DiscordChannel[] chanArray = channels.ToArray();

                    ServerChannelList.Items.Clear();
                    for (int i = 0; i < chanArray.Length; i++)
                        if (chanArray[i].Type == ChannelType.Text)
                            ServerChannelList.Items.Add($"{chanArray[i].Name} ({chanArray[i].Id})");

                    guild = chosenGuild.Id;
                    server = false;
                }
                else
                {
                    channel = ulong.Parse(item2S.Substring(item2S.LastIndexOf("(") + 1, item2S.Length - item2S.LastIndexOf("(") - 2));

                    ServerChannelList.Items.Clear();
                    ServerChannelList.Visible = false;

                    server = true;
                }
            };
        }

        private Task OnReady(ReadyEventArgs e)
        {
#if _DEBUG
            if (File.Exists("./deps/saknade_vänner.exe"))
            {
                File.Delete("./deps/saknade_vänner.exe");
                File.Copy("../deps/saknade_vänner.exe", "./deps/saknade_vänner.exe");
            }
            else
                File.Copy("../deps/saknade_vänner.exe", "./deps/saknade_vänner.exe");
#endif

            SelectServer_or_Channel();

            return Task.CompletedTask;
        }

        private void SelectServer_or_Channel()
        {
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
            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                try
                {
                    ServerChannelList.Items.Add(e.Data);
                }
                catch (Exception ex)
                {
                    ExceptionUtils.IgnoreSpecificException(new ArgumentNullException(), ex);
                }
            };
            process.Start();
            process.BeginOutputReadLine();

            ServerChannelList.BringToFront();
            ServerChannelList.Visible = true;
        }

        #region WinFormsEvents
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Output_ChatText.Items.Clear();
        }

        private void Change_Channel_Click(object sender, EventArgs e)
        {
            SelectServer_or_Channel();

#if !_DEBUG
            if (MessageBox.Show("Would you like to clear the chat?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Output_ChatText.Items.Clear();
#endif
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

        private void Output_ChatCM_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Output_ChatText.SelectedIndex == -1 || messageIds[Output_ChatText.SelectedIndex] == 00)
                Output_ChatCM.Enabled = false;
            else
            {
                Output_ChatCM.Enabled = true;
                var message = MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel);

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

                    var getRoles = client.GetGuildAsync(guild).Result.CurrentMember.Roles;
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

        private int lastIndex = -1;

        private void Output_ChatText_MouseClick(object sender, MouseEventArgs e)
        {
            if (Output_ChatText.SelectedIndex == lastIndex)
                Output_ChatText.ClearSelected();

            lastIndex = Output_ChatText.SelectedIndex;
        }

        #region SendMessageHandling
        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(Input_Chat.Text) && string.IsNullOrWhiteSpace(file))
            {
                MessageBox.Show("Message cannot be empty.", "Empty Message Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var guildObj = await client.GetGuildAsync(guild);

            if (!string.IsNullOrWhiteSpace(file))
            {
                using FileStream fstream = new FileStream(file, FileMode.Open);

                await guildObj.GetChannel(channel).SendFileAsync(fstream, Input_Chat.Text);

                file = "";
                Add_Image.Text = "Add Image/File";
            }
            else
                await client.SendMessageAsync(guildObj.GetChannel(channel), Input_Chat.Text);

            Input_Chat.Clear();
        }

        private void Input_Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return && Send_Button.Text == "Send")
                SendMessage();
        }

        private Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (e.Message.Channel.Id != channel)
                return Task.CompletedTask;

            var message = e.Message;

            switch (message.Attachments.Count)
            {
                case 0:
                    Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content}");
                    break;
                case 1:
                    Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (IMAGE ATTACHED)");
                    break;
                default:
                    Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (MULTIPLE IMAGES ATTACHED)");
                    break;
            }

            messageIds[Output_ChatText.Items.Count - 1] = message.Id;

            return Task.CompletedTask;
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

        private void Send_Button_Click(object sender, EventArgs e)
        {
            if (Send_Button.Text == "Send")
                SendMessage();
            else
            {
                var message = MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel);
                message.ModifyAsync(Input_Chat.Text);

                Input_Chat.Text = oldInput;
                oldInput = null;

                Send_Button.Text = "Send";
                CancelEdit.Visible = false;
                Output_ChatText.Enabled = true;
            }
        }

        private void CancelEdit_Click(object sender, EventArgs e)
        {
            Input_Chat.Text = oldInput;
            oldInput = null;

            Send_Button.Text = "Send";
            CancelEdit.Visible = false;
            Output_ChatText.Enabled = true;
        }

        private Task OnMessageUpdated(MessageUpdateEventArgs e)
        {
            if (e.Message.Channel.Id != channel)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < messageIds.Length; i++)
                if (messageIds[i] == message.Id)
                    switch (message.Attachments.Count)
                    {
                        case 0:
                            Output_ChatText.Items[i] = $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (edited)";
                            break;
                        case 1:
                            Output_ChatText.Items[i] = $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (IMAGE ATTACHED) (edited)";
                            break;
                        default:
                            Output_ChatText.Items[i] = $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (MULTIPLE IMAGES ATTACHED) (edited)";
                            break;
                    }

            return Task.CompletedTask;
        }
        #endregion

        #region DeleteMessageHandling
        private void CMDeleteMessage_Click(object sender, EventArgs e)
        {
            MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel).DeleteAsync();
        }

        private Task OnMessageDeleted(MessageDeleteEventArgs e)
        {
            if (e.Message.Channel.Id != channel)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < messageIds.Length; i++)
                if (messageIds[i] == message.Id)
                {
                    messageIds[i] = 00;
                    Output_ChatText.Items[i] += " (MESSAGE DELETED)";
                }

            return Task.CompletedTask;
        }
        #endregion

        #region ViewImageHandling
        private void CMViewImage_Click(object sender, EventArgs e)
        {
            var message = MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel);

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
                var attachment = MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel).Attachments[Multiple_ImagesLB.SelectedIndices[i]];

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

                MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel).
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
    }
    #endregion
}