using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using hatsune_miku.utils;

namespace hatsune_miku
{
    public partial class MainForm : Form
    {
        private ulong channel;
        private ulong guild;
        private string file = "";

        public MainForm()
        {
            InitializeComponent();
            Text = $"Hatsune Miku (v{Application.ProductVersion})";
            ServerChannelList.BringToFront();

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
            client.MessageCreated += OnMessage;

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
                    PickyCatch.IgnoreSpecificException(new NullReferenceException(), ex, true, "Fatal Error");
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

                    ServerChannelList.Visible = false;

                    Output_ChatText.Visible = true;
                    Input_Chat.Visible = true;
                    Change_Channel.Visible = true;
                    Add_Image.Visible = true;

                    server = true;
                }
            };
        }

        private readonly ulong[] messageIds = new ulong[500];

        #region DiscordEvents
        private async Task<int> OnMessage(MessageCreateEventArgs e)
        {
            var message = e.Message;

            if (message.Channel.Id != channel)
                return 1;

            if (message.Attachments.Count == 0)
                Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content}");
            else if (message.Attachments.Count == 1)
                Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (IMAGE ATTACHED)");
            else
            {
                Output_ChatText.Items.Add($"{message.Author.Username}#{message.Author.Discriminator}: {message.Content} (MULTIPLE IMAGES ATTACHED)");
                for (int i = 0; i < message.Attachments.Count; i++)
                    listBox1.Items.Add(message.Attachments[i].Url);
            }

            messageIds[Output_ChatText.Items.Count - 1] = message.Id;

            return 0;
        }

        private async Task<int> OnReady(ReadyEventArgs e)
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

            return 0;
        }
        #endregion

        private void SelectServer_or_Channel()
        {
            using Process process = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "./deps/saknade_vänner.exe",
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
                    PickyCatch.IgnoreSpecificException(new ArgumentNullException(), ex);
                }
            };
            process.Start();
            process.BeginOutputReadLine();

            Output_ChatText.Visible = false;
            Input_Chat.Visible = false;
            Change_Channel.Visible = false;
            Add_Image.Visible = false;

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

        private void CMViewImage_Click(object sender, EventArgs e)
        {
            var getMessage = client.GetChannelAsync(channel).Result.GetMessageAsync((messageIds[Output_ChatText.SelectedIndex])).Result;

            string attachment = "";
            if (getMessage.Attachments.Count == 1)
            {
                attachment = getMessage.Attachments[0].Url;
                IOF iof = new IOF(attachment);
                iof.Show();
            }
            else
            {
                MessageBox.Show("Selected message has more than one attachment. Please select which image(s) you would like to open.", "Multi-image message",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBox1.Visible = true;
                Button1.Visible = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.SelectedItems.Count; i++)
            {
                IOF iof = new IOF(listBox1.SelectedItems[i].ToString());
                iof.Show();
            }
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

            Input_Chat.Text = "";
        }

        private void Input_Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
                SendMessage();
        }

        private void Send_Button_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
        #endregion

        private void Output_ChatCM_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Output_ChatText.SelectedIndex == -1)
                e.Cancel = true;
            else
                switch (client.GetChannelAsync(channel).Result.
                    GetMessageAsync(messageIds[Output_ChatText.SelectedIndex]).Result.Attachments.Count)
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
        }
    }
    #endregion
}