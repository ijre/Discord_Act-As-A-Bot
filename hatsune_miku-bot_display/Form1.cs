using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace hatsune_miku_bot_display
{
    public partial class Form1 : Form
    {
        readonly string channel = "./deps/channel.txt";
        readonly string guild = "./deps/guild.txt";
        readonly string messages = "./deps/messages/";

        public Form1()
        {
            InitializeComponent();

            if (File.Exists(guild))
                File.Delete(guild);

            if (File.Exists(channel))
                File.Delete(channel);

            if (Directory.Exists(messages))
                Directory.Delete(messages, true);

            if (!Directory.Exists("./deps/"))
            {
#if !_DEBUG
                MessageBox.Show("Could not find deps folder", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
#else
                Directory.CreateDirectory("./deps/");
#endif
            }
        }

        private void Start(object sender, EventArgs e)
        {
            TrueStart();
            Start_Button.Visible = false;
#if _FINAL
            Please_Wait.Visible = true;
            Please_Wait.BringToFront();
#endif

            Send_Button.Visible = true;
            Input_Chat.Visible = true;
            Output_Chat.Visible = true;
            Change_Channel.Visible = true;
            Add_Image.Visible = true;
            React.Visible = true;
        }

        private async Task<int> TrueStart()
        {
            DiscordConfiguration config = new DiscordConfiguration
            {
#if !_DEBUG
                Token = File.ReadAllText("./deps/id.txt")
#else
                Token = File.ReadAllText("../deps/id.txt")
#endif
            };
            DiscordClient client = new DiscordClient(config);

            await client.ConnectAsync();
            await client.InitializeAsync();
            /*DiscordGame gajm = new DiscordGame
            {
                Name = "Minecraft"
            };
            client.UpdateStatusAsync(gajm, UserStatus.Online);*/

            client.Ready += OnReady;
            client.MessageCreated += OnMessage;

            #region WinFormsClientEvents
            Send_Button.Click += async (sender, e) =>
            {
                var guildObj = await client.GetGuildAsync(ulong.Parse(File.ReadAllText(guild)));

                if (!string.IsNullOrWhiteSpace(File_Name.Text))
                {
                    FileStream fstream = new FileStream(File_Name.Text, FileMode.Open);
                    await guildObj.GetChannel(ulong.Parse(File.ReadAllText(channel))).SendFileAsync(fstream, Input_Chat.Text);

                    File_Name.Text = "";
                    Add_Image.Text = "Add Image/File";
                }
                else
                    await client.SendMessageAsync(guildObj.GetChannel(ulong.Parse(File.ReadAllText(channel))), Input_Chat.Text);

                Input_Chat.Text = "";
            };

            string file = "";

            React.Click += (sender, e) =>
            {
                if (!React.Text.Contains("React "))
                {
                    file = "";
                    return;
                }
                OpenFileDialog diag = new OpenFileDialog
                {
                    DefaultExt = "txt",
                    Filter = "(*.txt) | *.txt",
                    RestoreDirectory = true,
                    Title = "Choose which message you would like to react to"
                };
                diag.ShowDialog();
                if (string.IsNullOrEmpty(diag.FileName))
                    return;

                file = diag.FileName;

                ReactText.Visible = true;
                React_Confirm.Visible = true;

                if (React.Text.Contains("React "))
                    React.Text = "Cancel reacting";
                else
                    React.Text = "React to a message";
            };

            React_Confirm.Click += async (sender, e) =>
            {
                DiscordChannel chan = await client.GetChannelAsync(ulong.Parse(File.ReadAllText(channel)));
                DiscordMessage mess = await chan.GetMessageAsync(ulong.Parse(File.ReadAllText(file)));

                mess.CreateReactionAsync(DiscordEmoji.FromName(client, ReactText.Text));

                ReactText.Visible = false;
                React_Confirm.Visible = false;
                React.Text = "React to a message";
            };
            #endregion

            return 0;
        }

        #region ClientEvents
        private async Task<int> OnMessage(MessageCreateEventArgs message)
        {
            if (message.Channel.Id != ulong.Parse(File.ReadAllText(channel)))
                return 1;

#if !_DEBUG
            Output_Chat.Text += message.Author.Username + "#" + message.Author.Discriminator + ": " + message.Message.Content + "\r\n";
#endif

            if (!Directory.Exists(messages))
            {
                Directory.CreateDirectory(messages);
                File.WriteAllText("./deps/messages/CHOOSE WHICH MESSAGE YOU WOULD LIKE TO REACT TO", "");
            }

            char[] fileNameCheck = new char[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|' };
            string[] replacement = new string[] { "[back slash]", "[forward slash]", ";", "[asterisk]", " ", "''", "[less than]", "[greater than]", " " };

            string originalMessage = message.Message.Content.Length > 214 ? message.Message.Content.Substring(0, 214) : message.Message.Content;

            var indexAny = originalMessage.IndexOfAny(fileNameCheck);
            if (indexAny != -1)
                for (int i = 0; i < originalMessage.Length; i++)
                    for (int i2 = 0; i2 < fileNameCheck.Length; i2++)
                        if (originalMessage[i] == fileNameCheck[i2])
                        {
                            originalMessage = originalMessage.Remove(i, 1);
                            originalMessage = originalMessage.Insert(i, replacement[i2]);
                        }
            // for skin
            File.WriteAllText(messages + message.Author.Username + " said " + originalMessage + ".txt".Trim(), message.Message.Id.ToString());

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

            Process process = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "./deps/saknade_vänner.exe"
                }
            };
            process.Start();
            process.WaitForExit();

#if _FINAL
            Please_Wait.Visible = false;
#endif
        }

        #region WinFormsEvents
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(guild))
                File.Delete(guild);

            if (File.Exists(channel))
                File.Delete(channel);

            if (Directory.Exists(messages))
                Directory.Delete(messages, true);

            try
            {
                Process.GetProcessesByName("saknade_vänner")[0].Kill();
            }
            catch
            {
                Application.Exit();
            }
        }

        private void Change_Channel_Click(object sender, EventArgs e)
        {
            SelectServer_or_Channel();

            if (Directory.Exists(messages))
                Directory.Delete(messages, true);
        }

        private void Add_Image_Click(object sender, EventArgs e)
        {
            if (Add_Image.Text.Contains("Remove"))
            {
                File_Name.Text = "";
                Add_Image.Text = "Add Image/File";
                return;
            }

            OpenFileDialog diag = new OpenFileDialog
            {
                RestoreDirectory = true
            };
            diag.ShowDialog();

            if (!string.IsNullOrWhiteSpace(diag.FileName))
            {
                File_Name.Text = diag.FileName;
                Add_Image.Text = "Remove Image/File";
            }
        }

        private void ReactText_Enter(object sender, EventArgs e)
        {
            if (ReactText.Text == "Type in your reaction here.")
                ReactText.Text = "";
        }
        #endregion
    }
}
