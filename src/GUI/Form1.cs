using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace hatsune_miku_bot_display
{
    public partial class MainForm : Form
    {
        readonly string channel = "./deps/channel.txt";
        readonly string guild = "./deps/guild.txt";
        readonly string messages = "./deps/messages/";

        public MainForm()
        {
            InitializeComponent();

            if (File.Exists(guild))
                File.Delete(guild);

            if (File.Exists(channel))
                File.Delete(channel);

            if (Directory.Exists(messages))
                Directory.Delete(messages, true);


            if (!Directory.Exists("./deps/"))
#if !_DEBUG
                MessageBox.Show("Could not find deps folder, this folder and its original contents are required for the program to run.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#else
                Directory.CreateDirectory("./deps/");
#endif
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
#if !_DEBUG
            Clear_Button.Visible = true;
#endif
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

            client.Ready += OnReady;
            client.MessageCreated += OnMessage;

            #region WinFormsClientEvents
            Send_Button.Click += async (sender, e) =>
            {
                var guildObj = await client.GetGuildAsync(ulong.Parse(File.ReadAllText(guild)));

                if (!string.IsNullOrWhiteSpace(File_Name.Text))
                {
                    using (FileStream fstream = new FileStream(File_Name.Text, FileMode.Open))
                    {
                        await guildObj.GetChannel(ulong.Parse(File.ReadAllText(channel))).SendFileAsync(fstream, Input_Chat.Text);

                        File_Name.Text = "";
                        Add_Image.Text = "Add Image/File";
                    }
                }
                else
                    await client.SendMessageAsync(guildObj.GetChannel(ulong.Parse(File.ReadAllText(channel))), Input_Chat.Text);

                Input_Chat.Text = "";
            };

            string file = "";

            React.Click += (sender, e) =>
            {
                if (React.Text.Contains("Cancel"))
                {
                    file = "";

                    React.Text = "React to a message";
                    ReactText.Visible = false;
                    React_Confirm.Visible = false;
                    return;
                }
                else if (React.Text.Contains("React to"))
                {
                    React.Text = "Would you like to use an ID to specify which message, or pick one from the recent messages?";
                    ID_B.Visible = true;
                    Recent_Message_B.Visible = true;
                }

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
                        file = Application.StartupPath + "\\deps\\" + "messages";
                    else
                        file = Application.StartupPath + "\\deps\\";

                    using (OpenFileDialog diag = new OpenFileDialog
                    {
                        DefaultExt = "txt",
                        Filter = "(*.txt) | *.txt",
                        InitialDirectory = file,
                        Title = "Choose which message you would like to react to"
                    })
                    {
                        diag.ShowDialog();

                        if (!string.IsNullOrEmpty(diag.FileName))
                        {
                            file = diag.FileName;

                            React.Text = "Cancel reacting";
                            ReactText.Visible = true;
                            React_Confirm.Visible = true;
                            ID_B.Visible = false;
                            Recent_Message_B.Visible = false;
                        }
                    };
                };
            };

            React_Confirm.Click += async (sender, e) =>
            {
                DiscordChannel chan = await client.GetChannelAsync(ulong.Parse(File.ReadAllText(channel)));
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
                    mess = await chan.GetMessageAsync(ulong.Parse(ID_TB.Text));
                else
                    mess = await chan.GetMessageAsync(ulong.Parse(File.ReadAllText(file)));

                mess.CreateReactionAsync(DiscordEmoji.FromName(client, ReactText.Text));

                ReactText.Visible = false;
                React_Confirm.Visible = false;
                React.Text = "React to a message";
                ReactText.Text = "Type in your reaction here.";
                ID_TB.Text = "Type in your message ID here.";
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
            Output_Chat.AppendText(message.Author.Username + "#" + message.Author.Discriminator + ": " + message.Message.Content + "\r\n");
#endif

            if (!Directory.Exists(messages))
            {
                Directory.CreateDirectory(messages);
                File.WriteAllText("./deps/messages/CHOOSE WHICH MESSAGE YOU WOULD LIKE TO REACT TO.txt", "");
            }

            char[] fileNameCheck = new char[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '\n' };
            string[] replacement = new string[] { "[back slash]", "[forward slash]", "[colon]", "[asterisk]", "[question mark]", "''", "[less than]", "[greater than]", "[vertical line]", " " };

            string newMessage = message.Message.Content;

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

            File.WriteAllText(messages + message.Author.Username + " said " + newMessage + ".txt", message.Message.Id.ToString());

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

            using (Process process = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "./deps/saknade_vänner.exe"
                }
            })
            {
                process.Start();
                process.WaitForExit();

#if _FINAL
                Please_Wait.Visible = false;
#endif
            }
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
            catch (IndexOutOfRangeException)
            {
                // this error happens if the program is already closed, which it should be 99% of the time you close the main GUI
                Application.Exit();
            }
            catch
            {
                throw;
            }
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
                File_Name.Text = "";
                Add_Image.Text = "Add Image/File";
                return;
            }

            using (OpenFileDialog diag = new OpenFileDialog
            {
                RestoreDirectory = true
            })
            {
                diag.ShowDialog();

                if (!string.IsNullOrWhiteSpace(diag.FileName))
                {
                    File_Name.Text = diag.FileName;
                    Add_Image.Text = "Remove Image/File";
                }
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
        #endregion

        private void ID_TB_Enter(object sender, EventArgs e)
        {
            if (ID_TB.Text == "Type in your message ID here.")
                ID_TB.Text = "";
        }
    }
}
