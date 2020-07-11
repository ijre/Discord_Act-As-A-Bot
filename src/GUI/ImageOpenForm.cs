using System;
using System.Windows.Forms;

namespace discord_puppet
{
    public partial class IOF : Form
    {
        public IOF(string url, string name)
        {
            InitializeComponent();

            Uri newUri;
            Uri.TryCreate(url, UriKind.Absolute, out newUri);
            webBrowser1.Url = newUri;
            webBrowser1.Visible = true;

            webBrowser1.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs args) =>
            {
                Text = name;
                Height = webBrowser1.Document.Images[0].ScrollRectangle.Height + 70;
                Width = webBrowser1.Document.Images[0].ScrollRectangle.Width + 70;

                if (int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString()) <= int.Parse(Size.Width.ToString())
                || int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString()) <= int.Parse(Size.Height.ToString()))
                {
                    Height = Screen.PrimaryScreen.Bounds.Height / 2;
                    Width = Screen.PrimaryScreen.Bounds.Width / 2;
                }

                SetDesktopLocation((Screen.PrimaryScreen.Bounds.Width / 2) - (Size.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (Size.Height / 2));
            };
        }

        static bool ignoreNext;

        private void WebBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                if (FormBorderStyle == FormBorderStyle.Sizable && !ignoreNext)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;

                    ignoreNext = true;
                }
                else if (!ignoreNext)
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;

                    ignoreNext = true;
                }
                else
                    ignoreNext = false;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (FormBorderStyle == FormBorderStyle.None)
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;

                    ignoreNext = true;
                }
                else if (!ignoreNext)
                    Hide();
                else
                    ignoreNext = false;
            }
        }
    }
}
