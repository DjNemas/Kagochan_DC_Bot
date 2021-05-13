using Discord;
using Discord.WebSocket;
using Kagochan_DC_Bot.Commands;
using Kagochan_DC_Bot.Initialisation;
using Kagochan_DC_Bot.TicTacToe;
using System;
using System.Threading.Tasks;
using static Kagochan_DC_Bot.Debug.Log;

namespace Kagochan_DC_Bot
{
    class Program
    {
        private DiscordSocketClient _client;
        private ConfigFile configFile = new ConfigFile();

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            new Initialisation.Init();

            _client = new DiscordSocketClient();
            new GameLogik(_client);
            new General(_client);

            _client.Log += Log;
            _client.GuildAvailable += GuildAvailable;

            await _client.LoginAsync(TokenType.Bot, configFile.GetToken());
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            logMain(msg.ToString(), LogLevel.Log);
            return Task.CompletedTask;
        }

        private Task GuildAvailable(SocketGuild guild)
        {
            JSONDatabase.GuildSettings setting = new JSONDatabase.GuildSettings();
            setting.AddGuildToList(guild.Id, guild.SystemChannel.Id);
            return Task.CompletedTask;
        }
    }
}
