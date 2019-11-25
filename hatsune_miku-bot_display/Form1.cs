﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace hatsune_miku_bot_display
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Start(object sender, EventArgs e)
        {
            TrueStart();
            start_button.Visible = false;
            send_button.Visible = true;
            Input_Chat.Visible = true;
            Output_Chat.Visible = true;
            change_channel.Visible = true;
        }

        private async Task<int> TrueStart()
        {
            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = File.ReadAllText("./id.txt")
            };
            DiscordClient client = new DiscordClient(config);

            await client.ConnectAsync();
            await client.InitializeAsync();

            client.Ready += OnReady;
            this.send_button.Click += async (sender, e) =>
            {
                var guildObj = await client.GetGuildAsync(ulong.Parse(File.ReadAllText("./guild.txt")));

                await client.SendMessageAsync(guildObj.GetChannel(ulong.Parse(File.ReadAllText("./channel.txt"))), Input_Chat.Text);
                Input_Chat.Text = "";
            };
            client.MessageCreated += OnMessage;

            return 0;
        }

        private async Task<int> OnMessage(MessageCreateEventArgs message)
        {
            if (message.Channel.Id != ulong.Parse(File.ReadAllText("./channel.txt")))
                return 1;

            Output_Chat.Text += message.Author.Username + "#" + message.Author.Discriminator + ": " + message.Message.Content + "\r\n";
            return 0;
        }

        private async Task<int> OnReady(ReadyEventArgs e)
        {
#if _DEBUG
            if (File.Exists("./Hatsune_Miku.exe"))
            {
                File.Delete("./Hatsune_Miku.exe");
                File.Copy("../Hatsune_Miku.exe", "./Hatsune_Miku.exe");
            }
            else
                File.Copy("../Hatsune_Miku.exe", "./Hatsune_Miku.exe");
#endif
            Process process = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                UseShellExecute = false,
                FileName = "./Hatsune_Miku.exe"
                }
            };
            process.Start();
            process.WaitForExit();

            return 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Process.GetProcessesByName("Hatsune_Miku")[0].Kill();
            }
            catch
            {
                Application.Exit();
            }
        }

        private void change_channel_Click(object sender, EventArgs e)
        {
            Process process = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                UseShellExecute = false,
                FileName = "./Hatsune_Miku.exe"
                }
            };
            process.Start();
            process.WaitForExit();
        }
    }
}
