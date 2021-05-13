using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;

namespace Kagochan_DC_Bot.Commands
{
    public class General
    {
        private DiscordSocketClient _client;

        public General(DiscordSocketClient client)
        {
            this._client = client;
            this._client.MessageReceived += CommandsWithPrefix;
        }
            
        private async Task CommandsWithPrefix(SocketMessage msg)
        {
            SocketUserMessage msgUser = msg as SocketUserMessage;
            if (msgUser == null) return;

            JSONDatabase.GuildSettings guildSetting = new JSONDatabase.GuildSettings();
            SocketGuildChannel guild = msg.Channel as SocketGuildChannel;

            if (msgUser.Channel.Id != guildSetting.GetTTTChannel(guild.Guild.Id)) return;

            int prefixPosition = 0;

            if (!(msgUser.HasCharPrefix(guildSetting.GetPrefix(guild.Guild.Id), ref prefixPosition) ||
            msgUser.HasMentionPrefix(this._client.CurrentUser, ref prefixPosition)) ||
            msgUser.Author.IsBot)
                return;

            await TTTChannel(msg, guildSetting);
            await Prefix(msg, guildSetting);

        }

        private async Task Prefix(SocketMessage msg, JSONDatabase.GuildSettings guildSetting)
        {
            SocketGuildChannel guild = msg.Channel as SocketGuildChannel;
            SocketGuildUser guildUser = msg.Author as SocketGuildUser;

            string[] command = msg.Content.Split(" ");
            if (!(command[0] == guildSetting.GetPrefix(guild.Guild.Id) + "prefix")) return;

            if (guildUser.Guild.OwnerId != msg.Author.Id)
            {
                await msg.Channel.SendMessageAsync(msg.Author.Mention + " Du hast keine Berechtigung für diesen Command.");
                return;
            }

            if (command.Length == 1)
            {
                await msg.Channel.SendMessageAsync(msg.Author.Mention + " Du kannst den Prefix mit `" +
                    guildSetting.GetPrefix(guild.Guild.Id) + "prefix <prefix>` ändern.");
            }
            else if (command.Length == 2)
            {
                if (command[1].Length != 1)
                {
                    await msg.Channel.SendMessageAsync(msg.Author.Mention + " Der Prefix darf nur 1 Zeichen lang sein.");
                }
                else
                {
                    guildSetting.UpdatePrefix(guild.Guild.Id, Convert.ToChar(command[1]));
                    await msg.Channel.SendMessageAsync(msg.Author.Mention + " Der Prefix wurde geändert!");
                }
            }
            else
            {
                await msg.Channel.SendMessageAsync(msg.Author.Mention + " Zu viele Parameter!");
            }
        }

        private async Task TTTChannel(SocketMessage msg, JSONDatabase.GuildSettings guildSetting)
        {
            SocketGuildChannel guild = msg.Channel as SocketGuildChannel;
            SocketGuildUser guildUser = msg.Author as SocketGuildUser;

            string[] command = msg.Content.Split(" ");
            if (!(command[0] == guildSetting.GetPrefix(guild.Guild.Id) + "tttchannel")) return;

            if (guildUser.Guild.OwnerId != msg.Author.Id)
            {
                await msg.Channel.SendMessageAsync(msg.Author.Mention + " Du hast keine Berechtigung für diesen Command.");
                return;
            }

            if (command.Length == 1)
            {
                await msg.Channel.SendMessageAsync(msg.Author.Mention + " Du kannst den TTTChannel mit `"+
                    guildSetting.GetPrefix(guild.Guild.Id) + "tttchannel <channelid>` bestimmen. Nur dort werden die Commands erkannt.");
            }
            else if (command.Length == 2)
            {
                ulong channelID = 0;
                bool channelExist = false;
                try
                {
                    channelID = Convert.ToUInt64(command[1]);
                }
                catch (Exception)
                {
                    await msg.Channel.SendMessageAsync(msg.Author.Mention + " Deine ID war keine Zahl");
                    return;
                }
                foreach(var channel in guild.Guild.Channels)
                {
                    if (channel.Id == channelID)
                    {
                        channelExist = true;
                        break;
                    }
                }
                if (!channelExist)
                {
                    await msg.Channel.SendMessageAsync(msg.Author.Mention + " Der Channel existiert nicht.");
                    return;
                }

                guildSetting.UpdateTTTChannel(guild.Guild.Id, channelID);
                await msg.Channel.SendMessageAsync(msg.Author.Mention + " Der TTTChannel wurde auf " + guild.Guild.GetChannel(channelID).Name + " gesetzt.");
            }
            else
            {
                await msg.Channel.SendMessageAsync(msg.Author.Mention + " Zu viele Parameter!");
            }
        }
    }
}
