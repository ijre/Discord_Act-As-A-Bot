using DSharpPlus;
using DSharpPlus.Entities;

namespace hatsune_miku.utils
{
    public class MessageUtils
    {
        public static DiscordMessage GetMessage(DiscordClient client, ulong message, ulong channel)
        {
            return client.GetChannelAsync(channel).Result.GetMessageAsync(message).Result;
        }
    }
}
