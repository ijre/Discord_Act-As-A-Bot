using System;
using System.Windows.Forms;

namespace discord_puppet
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        private ushort total = 1;

        public void DAdd(string[] data, bool forceFocus = true)
        {
            string buff = "";
            for (int i = 0; i < data.Length; i++)
            {
                buff += $"{i}. {data[i]} ";
            }

            DebugOutput.AppendText($"DebugAdd() called ({total}):\r\n{buff}\r\n");

            if (forceFocus)
                Focus();

            total++;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            DebugOutput.Clear();
            total = 1;
        }
    }
}
