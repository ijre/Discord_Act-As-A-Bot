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

using static PickyCatch;
using IOF;

namespace hatsune_miku_bot_display
{
    public partial class MainForm : Form
    {
        private ulong channel = 0;
        private ulong guild = 0;
        private readonly string messages = "./deps/messages/";
        private readonly string[] file = new string[2] { "", "" };
        // index 0 is used for sending files, index 1 is used to store the message being reacted to

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

        private async Task<int> TrueStart()
        {
            DiscordClient client = new DiscordClient(new DiscordConfiguration
            {
#if !_DEBUG
                Token = File.ReadAllText("./deps/id.txt")
#else
                Token = File.ReadAllText("../deps/id.txt")
#endif
            });

            await client.ConnectAsync();
            await client.InitializeAsync();

            client.Ready += OnReady;
            client.MessageCreated += OnMessage;

            #region WinFormsClientAsyncEvents
            Send_Button.Click += async (sender, e) =>
            {
                var guildObj = await client.GetGuildAsync(guild);

                if (!string.IsNullOrWhiteSpace(file[0]))
                {
                    using FileStream fstream = new FileStream(file[0], FileMode.Open);

                    await guildObj.GetChannel(channel).SendFileAsync(fstream, Input_Chat.Text);

                    file[0] = "";
                    Add_Image.Text = "Add Image/File";
                }
                else
                    await client.SendMessageAsync(guildObj.GetChannel(channel), Input_Chat.Text);

                Input_Chat.Text = "";
            };

            React.Click += (sender, e) =>
            {
                Cancel_React.Visible = true;

                if (React.Text.Contains("React to"))
                {
                    React.Text = "Would you like to use an ID to specify which message, or pick one from the recent messages?";
                    ID_B.Visible = true;
                    Recent_Message_B.Visible = true;
                }
            };

            Cancel_React.Click += (sender2, eve) =>
            {
                file[1] = "";

                Cancel_React.Visible = false;

                ID_B.Visible = false;
                ID_TB.Visible = false;
                Recent_Message_B.Visible = false;

                ReactText.Visible = false;
                React_Confirm.Visible = false;
                React.Text = "React to a message";
                ReactText.Text = "Type in your reaction here.";
                ID_TB.Text = "Type in your message ID here.";
                return;
            };


            ID_B.Click += (bender, ee) =>
            {
                ID_TB.Visible = true;
                ID_B.Visible = false;
                Recent_Message_B.Visible = false;
                React_Confirm.Visible = true;
                React.Text = "Enter the ID in the box below.";
            };

            Recent_Message_B.Click += (zender, eee) =>
            {
                if (Directory.Exists(Application.StartupPath + "\\deps\\" + "messages"))
                    file[1] = Application.StartupPath + "\\deps\\" + "messages";
                else
                    file[1] = Application.StartupPath + "\\deps\\";

                using (OpenFileDialog diag = new OpenFileDialog
                {
                    DefaultExt = "txt",
                    Filter = "(*.txt) | *.txt",
                    InitialDirectory = file[1],
                    Title = "Choose which message you would like to react to"
                })
                {
                    diag.ShowDialog();

                    if (!string.IsNullOrEmpty(diag.FileName))
                    {
                        file[1] = diag.FileName;

                        React.Text = "Now, enter the name of your chosen emoji in the box below.";
                        ReactText.Visible = true;
                        React_Confirm.Visible = true;
                        ID_B.Visible = false;
                        Recent_Message_B.Visible = false;
                    }
                };
            };

            React_Confirm.Click += async (sender, e) =>
            {
                DiscordChannel chan = await client.GetChannelAsync(channel);
                DiscordMessage mess;

                if (React.Text.StartsWith("Enter"))
                {
                    ReactText.Visible = true;
                    ID_B.Visible = false;
                    Recent_Message_B.Visible = false;

                    ID_TB.Visible = false;
                    ReactText.Visible = true;
                    React.Text = "Now, enter the name of your chosen emoji in the box below.";
                    return;
                }
                else if (React.Text.StartsWith("Now"))
                {
                    if (String.IsNullOrWhiteSpace(file[1]))
                        mess = await chan.GetMessageAsync(ulong.Parse(ID_TB.Text));
                    else
                        mess = await chan.GetMessageAsync(ulong.Parse(File.ReadAllText(file[1])));

                    if (!ReactText.Text.Contains(":"))
                    {
                        ReactText.Text = ReactText.Text.Insert(0, ":");
                        ReactText.Text = ReactText.Text.Insert(ReactText.Text.Length, ":");
                    }

                    mess.CreateReactionAsync(DiscordEmoji.FromName(client, ReactText.Text));
                }

                ReactText.Visible = false;
                React_Confirm.Visible = false;
                Cancel_React.Visible = false;
                React.Text = "React to a message";
                ReactText.Text = "Type in your reaction here.";
                ID_TB.Text = "Type in your message ID here.";
            };

            bool server = true;
            ServerChannelList.DoubleClick += async (object sender, EventArgs e) =>
            {
                string item2S = "";
                try
                {
                    item2S = ServerChannelList.SelectedItem.ToString();
                }
                catch (Exception ex)
                {
                    MessageBoxIgnoreException(new NullReferenceException(), ex, "Fatal Error");
                    return;
                }

                if (server)
                {
                    var chosenGuild = await client.GetGuildAsync(ulong.Parse(item2S.Substring(item2S.LastIndexOf("(") + 1, item2S.Length - item2S.LastIndexOf("(") - 2)));
                    IEnumerable<DiscordChannel> channels = from value in await chosenGuild.GetChannelsAsync()
                                                           select value;

                    ServerChannelList.Items.Clear();
                    for (int i = 0; i < channels.ToArray().Length; i++)
                        ServerChannelList.Items.Add($"{channels.ToArray()[i].Name} ({channels.ToArray()[i].Id})");

                    guild = chosenGuild.Id;
                    server = false;
                }
                else
                {
                    channel = ulong.Parse(item2S.Substring(item2S.LastIndexOf("(") + 1, item2S.Length - item2S.LastIndexOf("(") - 2));

                    ServerChannelList.Visible = false;

                    Output_Chat.Visible = true;
                    Input_Chat.Visible = true;
                    Change_Channel.Visible = true;
                    ViewImageButton.Visible = true;
                    Add_Image.Visible = true;

                    server = true;
                }
            };
            #endregion

            return 0;
        }

        #region DiscordEvents
        private async Task<int> OnMessage(MessageCreateEventArgs e)
        {
            var message = e.Message;

            if (message.Channel.Id != channel)
                return 1;

            if (message.Attachments.Count == 0)
                Output_Chat.AppendText(message.Author.Username + "#" + message.Author.Discriminator + ": " + message.Content + "\r\n");
            else if (message.Attachments.Count == 1)
                Output_Chat.AppendText(message.Author.Username + "#" + message.Author.Discriminator + ": " + message.Content + "(IMAGE ATTACHED)\r\n");
            else
                Output_Chat.AppendText(message.Author.Username + "#" + message.Author.Discriminator + ": " + message.Content + "(MULTIPLE IMAGES ATTACHED)\r\n");

            if (!Directory.Exists(messages))
            {
                Directory.CreateDirectory(messages);
                File.WriteAllText(messages + "CHOOSE WHICH MESSAGE YOU WOULD LIKE TO REACT TO.txt", "");

                Directory.CreateDirectory(messages + "images");
            }

            char[] fileNameCheck = new char[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '\n', '\r' };
            string[] replacement = new string[] { "[back slash]", "[forward slash]", "[colon]", "[asterisk]", "[question mark]", "''", "[less than]", "[greater than]", "[vertical line]", " ", " " };

            string newMessage = message.Content;

            if (newMessage.IndexOfAny(fileNameCheck) != -1)
                for (int i = 0; i < newMessage.Length; i++)
                    for (int i2 = 0; i2 < fileNameCheck.Length; i2++)
                        if (newMessage[i] == fileNameCheck[i2])
                        {
                            newMessage = newMessage.Remove(i, 1);
                            newMessage = newMessage.Insert(i, replacement[i2]);
                        }

            if (newMessage.Length > 170)
                newMessage = newMessage.Substring(0, 171 - (message.Author.Username + " said ").Length);

            File.WriteAllText(messages + message.Author.Username + " said " + newMessage + ".txt", message.Id.ToString());

            if (message.Attachments.Count == 1 && message.Attachments[0].Width != 0)
                File.WriteAllText(messages + "images/" + message.Author.Username + (String.IsNullOrWhiteSpace(newMessage) ? " at " + message.Timestamp.Hour + " " + message.Timestamp.Minute + ", file name = " + message.Attachments[0].FileName : " said " + newMessage) + ".txt", message.Attachments[0].Url);
            else if (message.Attachments.Count > 1)
                for (int i = 0; i < message.Attachments.Count; i++)
                {
                    if (message.Attachments[i].Width == 0)
                        continue;
                    File.WriteAllText(messages + "images/" + message.Author.Username + " at " + message.Timestamp.Hour + " " + message.Timestamp.Minute + ", file name = " + message.Attachments[i].FileName + ".txt", message.Attachments[i].Url);
                }

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
            // two different functions use this same program starting sequence, this makes it easier to make changes

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
                    MessageBoxIgnoreException(new ArgumentNullException(), ex);
                }
            };
            process.Start();
            process.BeginOutputReadLine();

            Output_Chat.Visible = false;
            Input_Chat.Visible = false;
            Change_Channel.Visible = false;
            ViewImageButton.Visible = false;
            Add_Image.Visible = false;

            ServerChannelList.BringToFront();
            ServerChannelList.Visible = true;
        }

        #region WinFormsEvents
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(messages))
                Directory.Delete(messages, true);
        }

        private void Change_Channel_Click(object sender, EventArgs e)
        {
            SelectServer_or_Channel();

#if !_DEBUG
            if (MessageBox.Show("Would you like to clear the chat?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Output_Chat.Text = "";
#endif

            if (Directory.Exists(messages))
                Directory.Delete(messages, true);
        }

        private void Add_Image_Click(object sender, EventArgs e)
        {
            if (Add_Image.Text.Contains("Remove"))
            {
                file[0] = "";
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
                file[0] = diag.FileName;
                Add_Image.Text = "Remove Image/File";
            }
        }

        private void ReactText_Enter(object sender, EventArgs e)
        {
            if (ReactText.Text == "Type in your reaction here.")
                ReactText.Text = "";
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Output_Chat.Text = "";
        }

        private void ID_TB_Enter(object sender, EventArgs e)
        {
            if (ID_TB.Text == "Type in your message ID here.")
                ID_TB.Text = "";
        }

        private void ViewImageButton_Click(object sender, EventArgs e)
        {
            var box = MessageBox.Show("Would you like to open the image in your browser?\n(Saying no will open it in a separate window)", "Where would you like to open the image?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (box == DialogResult.No)
            {
                ImageOpenForm IOF = new ImageOpenForm();

                IOF.Show();
            }
            else if (box == DialogResult.Yes)
            {
                using OpenFileDialog diag = new OpenFileDialog
                {
                    DefaultExt = "txt",
                    Filter = "(*.txt) | *.txt",
                    InitialDirectory = Application.StartupPath + "\\deps\\messages\\images",
                    Title = "Choose which image you would like to view"
                };
                diag.ShowDialog();

                if (!String.IsNullOrEmpty(diag.FileName))
                    Process.Start(File.ReadAllText(diag.FileName));
            }
        }
        #endregion
    }
}
