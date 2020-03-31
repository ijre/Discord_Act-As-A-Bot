using System;
using System.Windows.Forms;
using DSharpPlus;
using DSharpPlus.Entities;

namespace discord_puppet.utils
{
    public class MessageUtils
    {
        public static DiscordMessage GetMessage(DiscordClient client, ulong message, ulong channel)
        {
            try
            {
                return client.GetChannelAsync(channel).Result.GetMessageAsync(message).Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return null;
            }
        }

        public static ulong GetID(string _string)
        {
            return ulong.Parse(_string.Substring(_string.LastIndexOf("[") + 1, _string.Length - _string.LastIndexOf("[") - 2));
        }
    }
}
