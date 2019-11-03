using System;
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
using System.Windows.Forms;

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
            OpenFileDialog diag = new OpenFileDialog
            {
                CheckFileExists = true,
                InitialDirectory = Application.StartupPath,
                DefaultExt = "Hatsune_Miku.exe",
                Filter = "Hatsune_Miku.exe | Hatsune_Miku.exe",
                Title = "Select the Hatsune_Miku.exe file."
            };
            diag.ShowDialog();

            if (diag.FileName.Length > 0)
            {
                try
                {
                    Process process = new Process
                    {
                        EnableRaisingEvents = true
                    };
                    process.StartInfo = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        FileName = diag.FileName,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    process.ErrorDataReceived += (harvestEvent, errput) => Output_Chat.Text += errput.Data + "\r\n";
                    process.OutputDataReceived += (e_vent, output) => Output_Chat.Text += output.Data + "\r\n";

                    process.Start();
                    process.BeginErrorReadLine();
                    process.BeginOutputReadLine();

                    Output_Chat.Visible = true;
                    browse_button.Visible = false;
                }
                catch
                {
                    var message = MessageBox.Show("Process failed to launch.", "Fatal error.", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

                    if (message == DialogResult.Retry)
                    {
                        Start(sender, e);
                        return;
                    }
                    else if (message == DialogResult.Cancel)
                        return;
                    else throw;
                }
            }
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
    }
}
