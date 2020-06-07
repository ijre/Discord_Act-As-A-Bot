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

using discord_puppet.utils;

namespace discord_puppet
{
    public partial class MemberListForm : Form
    {
        private readonly DiscordClient client;

        public MemberListForm(DiscordClient mainClient)
        {
            InitializeComponent();
            client = mainClient;
        }

        #region WinForm Events
        private void MemberListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MemberListForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Text = "Member List";
                ShowInTaskbar = true;
            }
            else
            {
                Text = "";
                ShowInTaskbar = false;
            }
        }
        #endregion

        // 32 name max, 37 in total when adding discriminator
    }
}
