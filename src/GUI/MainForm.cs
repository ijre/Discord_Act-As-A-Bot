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

            if (!Directory.Exists("./deps/"))
#if !_DEBUG
                MessageBox.Show("Could not find deps folder, this folder and its original contents are required for the program to run.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#else
                Directory.CreateDirectory("./deps/");
#endif

            TrueStart();
        }

        private enum FormControlGroups
        {
            ShowMain,
            HideMain
        }

        private void Hide_Show_Controls(FormControlGroups group)
        {
            switch (group)
            {
                case FormControlGroups.ShowMain:
                    Output_ChatText.Visible = true;
                    Change_Channel.Visible = true;
                    Clear_Button.Visible = true;
                    Input_Chat.Visible = true;
                    Send_Button.Visible = true;
                    Add_Image.Visible = true;


                    ServerChannelList.Visible = false;

                    Multiple_ImagesLB.Visible = false;
                    Multiple_ImagesOpen.Visible = false;
                    Multiple_ImagesCancel.Visible = false;
                    break;
                case FormControlGroups.HideMain:
                    Output_ChatText.Visible = false;
                    Change_Channel.Visible = false;
                    Clear_Button.Visible = false;
                    Input_Chat.Visible = false;
                    Send_Button.Visible = false;
                    Add_Image.Visible = false;


                    ServerChannelList.Visible = true;

                    Multiple_ImagesLB.Visible = true;
                    Multiple_ImagesOpen.Visible = true;
                    Multiple_ImagesCancel.Visible = true;
                    break;
            }
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

                    Hide_Show_Controls(FormControlGroups.ShowMain);

                    server = true;
                }
            };
        }

        private readonly ulong[] messageIds = new ulong[500];

        #region DiscordEvents
        private async Task<int> OnMessage(MessageCreateEventArgs e)
        {
            if (e.Message.Channel.Id != channel)
                return 1;

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
                    ExceptionUtils.IgnoreSpecificException(new ArgumentNullException(), ex);
                }
            };
            process.Start();
            process.BeginOutputReadLine();

            Hide_Show_Controls(FormControlGroups.HideMain);
            ServerChannelList.BringToFront();
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

        #region ViewImageHandling
        private void CMViewImage_Click(object sender, EventArgs e)
        {
            var message = MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel);

            if (message.Attachments.Count == 1)
            {
                IOF iof = new IOF(message.Attachments[0].Url);
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

                Hide_Show_Controls(FormControlGroups.HideMain);

                Multiple_ImagesLB.BringToFront();
                Multiple_ImagesOpen.BringToFront();
                Multiple_ImagesCancel.BringToFront();
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
                    IOF iof = new IOF(attachment.Url);
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

            Hide_Show_Controls(FormControlGroups.ShowMain);

            Multiple_ImagesLB.Items.Clear();
        }

        private void Multiple_ImagesCancel_Click(object sender, EventArgs e)
        {
            Hide_Show_Controls(FormControlGroups.ShowMain);

            Multiple_ImagesLB.Items.Clear();
        }
        #endregion

        private void CMReact_Click(object sender, EventArgs e)
        {
            string oldInput = Input_Chat.Text;

            //MessageBox.Show("Use the input ")
        }

        private void Output_ChatCM_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Output_ChatText.SelectedIndex == -1)
            {
                CMReact.Enabled = false;
                CMViewImage.Enabled = false;
            }
            else
            {
                switch (MessageUtils.GetMessage(client, messageIds[Output_ChatText.SelectedIndex], channel).
                    Attachments.Count)
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
                CMReact.Enabled = true;
            }
        }

        private int lastIndex = -1;

        private void Output_ChatText_MouseClick(object sender, MouseEventArgs e)
        {
            if (Output_ChatText.SelectedIndex == lastIndex)
                Output_ChatText.ClearSelected();

            lastIndex = Output_ChatText.SelectedIndex;
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            MessageBox.Show("e");
        }
    }
    #endregion
}