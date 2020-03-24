using System;
using System.Windows.Forms;

public static class PickyCatch
{
    /// <summary>
    /// Ignores the given exception, useful incase you have a catch(Exception) block but don't want a certain exception.
    /// Returns true if the exception was ignored.
    /// </summary>
    /// <param name="ignoredException">
    /// Exception you wish to ignore.
    /// </param>
    /// <param name="ex">
    /// Exception thrown.
    /// </param>
    /// <param name="messageTitle">
    /// If your exception was not ignored, this is the title of the MessageBox.
    /// Defaults to "Error".
    /// </param>
    /// <param name="messageBody">
    /// If your exception was not ignored, this is the body of the MessageBox.
    /// Defaults to the thrown exception's message.
    /// </param>
    public static bool MessageBoxIgnoreException(Exception ignoredException, Exception ex, string messageTitle = "Error", string messageBody = "")
    {
        if (ex.GetType().FullName == ignoredException.GetType().FullName)
            return true;

        MessageBox.Show(String.IsNullOrEmpty(messageBody) ? ex.Message : messageBody, messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
}