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

        public void DebugAdd(string[] data)
        {
            string buff = "";
            for (int i = 0; i < data.Length; i++)
                buff += $"{i}. {data[i]} ";

            textBox1.AppendText($"DebugAdd() called:\r\n{buff}\r\n");

            Focus();
        }
    }
}
