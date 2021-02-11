using System;
using System.Windows.Forms;

namespace discord_puppet.utils
{
    public class ExceptionUtils
    {
        /// <summary>
        /// Ignores a specific exception, to be used if you have a catch(Exception) but don't want a specific exception.
        /// Returns true if the exception was ignored.
        /// </summary>
        /// <param name="exceptionToIgnore">
        /// Exception you wish to ignore.
        /// </param>
        /// <param name="exceptionCaught">
        /// Exception caught.
        /// </param>
        /// <param name="messageBox">
        /// If true, will automatically call MessageBox.Show() if the exception was not ignored.
        /// Defaults to false.
        /// </param>
        /// <param name="title">
        /// MessageBox title.
        /// Defaults to "Error".
        /// </param>
        /// <param name="body">
        /// MessageBox body.
        /// Defaults to the caught exception's message.
        /// </param>
        public static bool IgnoreSpecificException(Exception exceptionToIgnore, Exception exceptionCaught,
            bool messageBox = false, string title = "Error", string body = "")
        {
            if (exceptionCaught.GetType().FullName == exceptionToIgnore.GetType().FullName)
                return true;

            if (messageBox)
            {
                MessageBox.Show(string.IsNullOrEmpty(body) ? exceptionCaught.Message : body, title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
    }
}