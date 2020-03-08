using System;
using System.IO;
using System.Windows.Forms;

namespace IOF
{
    public partial class ImageOpenForm : Form
    {
        public ImageOpenForm()
        {
            InitializeComponent();

            OpenFileDialog diag = new OpenFileDialog()
            {
                DefaultExt = "txt",
                Filter = "(*.txt) | *.txt",
                InitialDirectory = Application.StartupPath + "\\deps\\messages\\images",
                Title = "Choose which image you would like to view"
            };
            diag.ShowDialog();

            if (!String.IsNullOrEmpty(diag.FileName))
            {
                Uri newUri;
                Uri.TryCreate(File.ReadAllText(diag.FileName), UriKind.Absolute, out newUri);
                webBrowser1.Url = newUri;
                webBrowser1.Visible = true;

                webBrowser1.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs args) =>
                {
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
        }
    }
}
