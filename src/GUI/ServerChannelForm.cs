using System;
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
    public partial class ServerChannelForm : Form
    {
        private readonly DiscordClient client;
        private readonly MainForm display;

        public ServerChannelForm()
        {
            InitializeComponent();

            if (!Directory.Exists("./deps/"))
#if !_DEBUG
                MessageBox.Show("Could not find deps folder, this folder and its original contents are required for the program to run.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            client.MessageCreated += OnMessageCreated;
            client.MessageUpdated += OnMessageUpdated;
            client.MessageDeleted += OnMessageDeleted;

            client.ConnectAsync();
            client.InitializeAsync();

            SelectServer();
            display = new MainForm(client);
            display.Show();
        }

        #region DiscordEvents
        private Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (e.Message.Channel.Id != display.channel)
                return Task.CompletedTask;

            var message = e.Message;

            switch (message.Attachments.Count)
            {
                case 0:
                    display.Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content}   [{message.Id}]");
                    break;
                case 1:
                    display.Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (IMAGE ATTACHED)   [{message.Id}]");
                    break;
                default:
                    display.Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (MULTIPLE IMAGES ATTACHED)   [{message.Id}]");
                    break;
            }

            return Task.CompletedTask;
        }

        private Task OnMessageUpdated(MessageUpdateEventArgs e)
        {
            if (e.Message.Channel.Id != display.channel)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < display.Output_ChatCM.Items.Count; i++)
                if (MessageUtils.GetID(display.Output_ChatCM.Items[i].Text) == message.Id)
                    switch (message.Attachments.Count)
                    {
                        case 0:
                            display.Output_ChatText.Items[i] = $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (edited)   [{message.Id}]";
                            break;
                        case 1:
                            display.Output_ChatText.Items[i] = $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (IMAGE ATTACHED) (edited)   [{message.Id}]";
                            break;
                        default:
                            display.Output_ChatText.Items[i] = $"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (MULTIPLE IMAGES ATTACHED) (edited)   [{message.Id}]";
                            break;
                    }

            return Task.CompletedTask;
        }

        private Task OnMessageDeleted(MessageDeleteEventArgs e)
        {
            if (e.Message.Channel.Id != display.channel)
                return Task.CompletedTask;

            var message = e.Message;

            for (int i = 0; i < display.Output_ChatCM.Items.Count; i++)
                if (MessageUtils.GetID(display.Output_ChatCM.Items[i].Text) == message.Id)
                {
                    display.Output_ChatCM.Items[i].Text = display.Output_ChatCM.Items[i].Text.Substring(0, display.Output_ChatCM.Items[i].Text.LastIndexOf("[") - 1);
                    display.Output_ChatText.Items[i] += " {MESSAGE DELETED} []";
                }

            return Task.CompletedTask;
        }
        #endregion

        private void SelectServer()
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
        }

        private async void DoStuffSync()
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
                if (chanArray[i].Type == ChannelType.Text)
                    Channels.Items.Add($"{chanArray[i].Name} [{chanArray[i].Id}]");

            display.guild = chosenGuild.Id;
        }

        private int lastIndex = -1;

        private void Servers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Servers.SelectedIndex == lastIndex)
                return;

            lastIndex = Servers.SelectedIndex;
            DoStuffSync();
        }

        private void Channels_SelectedIndexChanged(object sender, EventArgs e)
        {
            display.channel = MessageUtils.GetID(Channels.SelectedItem.ToString());
        }
    }
}