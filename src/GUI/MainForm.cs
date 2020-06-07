using System;
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
        private readonly DiscordClient client;
        private readonly MainDisplayForm display;

        public MainForm()
        {
            InitializeComponent();

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

            client = new DiscordClient(new DiscordConfiguration
            {
#if !_DEBUG
                Token = File.ReadAllText("./deps/id.txt")
#else
                Token = File.ReadAllText("../deps/id.txt")
#endif
            });

#if !_DEBUG
            Servers.Items.Clear();
#endif

            client.MessageCreated += OnMessageCreated;
            client.MessageUpdated += OnMessageUpdated;
            client.MessageDeleted += OnMessageDeleted;
            client.Ready += OnReady;

            client.ConnectAsync();
            client.InitializeAsync();


            display = new MainDisplayForm(client);

            display.Move += ResizeHandler;
            display.ResizeEnd += ResizeHandler;

            display.Show();

            display.Location = new Point(display.Location.X + 33, display.Location.Y); // offset because display opens in the center, need to move else we get cutoff
        }

        #region DiscordEvents
        private Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (e.Message.Channel.Id != display.channel.Id)
                return Task.CompletedTask;

            var message = e.Message;

            display.Output_ChatText.Items.Add(AddAttachmentText(message));

            if (display.Output_ChatText.Items.Count - 1 - display.Output_ChatText.TopIndex == 18) // if we're scrolled to the very bottom of the chat
                display.Output_ChatText.TopIndex = display.Output_ChatText.Items.Count - 1; // scroll to adjust to new message

            return Task.CompletedTask;
        }

        private Task OnMessageUpdated(MessageUpdateEventArgs e)
        {
            if (e.Message.Channel.Id != display.channel.Id)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < display.Output_ChatText.Items.Count; i++)
                if (i != 100 && MessageUtils.GetID(display.Output_ChatText.Items[i].ToString()) == message.Id)
                    display.Output_ChatText.Items[i] = AddAttachmentText(message).Insert(display.Output_ChatText.Items[i].ToString().LastIndexOf("[") - 4, " (edited)");

            return Task.CompletedTask;
        }

        private Task OnMessageDeleted(MessageDeleteEventArgs e)
        {
            if (e.Message.Channel.Id != display.channel.Id)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < display.Output_ChatText.Items.Count; i++)
                if (i != 100 && MessageUtils.GetID(display.Output_ChatText.Items[i].ToString()) == message.Id)
                {
                    display.Output_ChatText.Items[i] = display.Output_ChatText.Items[i].ToString().Substring(0, display.Output_ChatText.Items[i].ToString().LastIndexOf("[") - 1); // delete everything past the message's body
                    display.Output_ChatText.Items[i] += " {MESSAGE DELETED} []"; // and add a note that it's deleted
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
            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                try
                {
                    Servers.Items.Add(e.Data);
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

        #region WinForms Events
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

        private void ResizeHandler(object sender, EventArgs e)
        {
            const int defaultWidth = 550;

            Location = new Point((display.Location.X - defaultWidth) + 15, display.Location.Y);

            Channels.Size = new Size(Channels.Width, display.Height);
            Size = new Size(defaultWidth, Channels.Height);
            Channels.Size = new Size(Channels.Width, Servers.Height);
        }
        #endregion

        #region Helpers
        private static string AddAttachmentText(DiscordMessage message)
        {
            string attachments = "";

            if (message.Attachments.Count == 1)
                attachments = " (IMAGE ATTACHED)";
            else if (message.Attachments.Count > 1)
                attachments = " (MULTIPLE IMAGES ATTACHED)";

            return $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content}{attachments}    [{message.Id}]";
        }

        private async void DoStuffSync(bool server)
        {
            if (server)
            {
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

                display.guild = chosenGuild;
            }
            else
            {
                display.Output_ChatText.Items.Clear();
                display.channel = client.GetChannelAsync(MessageUtils.GetID(Channels.SelectedItem.ToString())).Result;

                var getMessages = await display.channel.GetMessagesAsync();

                IEnumerable<DiscordMessage> messages = from value in getMessages
                                                       select value;
                var messagesArray = messages.ToArray();

                for (int i = messagesArray.Length - 1; 0 <= i; i--)
                    display.Output_ChatText.Items.Add(AddAttachmentText(messagesArray[i]));

                display.Output_ChatText.Items.Add("((((((((((END OF PREVIOUS 100 MESSAGES)))))))))");

                display.Output_ChatText.TopIndex = display.Output_ChatText.Items.Count - 1;
            }
        }
        #endregion
    }
}