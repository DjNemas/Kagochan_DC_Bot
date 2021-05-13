using Discord.Commands;
using Discord.WebSocket;
using Kagochan_DC_Bot.BitmapDraw;
using Kagochan_DC_Bot.DataManagment;
using Kagochan_DC_Bot.Initialisation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Kagochan_DC_Bot.TicTacToe
{
    class GameLogik
    {
        private DiscordSocketClient client;
        private ConfigFile configFile;
        private bool playfieldStarted = false;
        private List<Player> playerList = new List<Player>();
        private List<Playfield> playFieldList = new List<Playfield>();
        private Bitmap bitmapPlayfield;
        private Bitmap bitmapHeader;
        private Bitmap complete;
        private AppendBitmap appandBM;
        private Graphics graphicHeader;
        private Graphics graphics;
        private Pen penField;
        private Pen penP1;
        private Pen penP2;
        private string bitmapPathHeader = Folder.imageFolder + "header.png";
        private string bitmapPathPlayfield = Folder.imageFolder + "tttplayfield.png";
        private string bitmapPathCompleteImage = Folder.imageFolder + "completeImage.png";
        private TicTacToe.Draw draw;
        private int bitmapWidth = 999;
        private int bitmapHeight = 999;
        private float penThicknes = 15;

        public GameLogik(DiscordSocketClient client)
        {
            this.configFile = new ConfigFile();
            this.client = client;
            this.client.MessageReceived += PlayTTT;
            this.client.MessageReceived += ResetTTT;
            this.client.MessageReceived += Game;
        }

        private void DrawHeader(string nameP1, string nameP2)
        {
            this.bitmapHeader = new Bitmap(999, 150, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                using (this.graphicHeader = Graphics.FromImage(bitmapHeader))
                {
                    this.graphicHeader.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    this.graphicHeader.DrawString("VS", new Font(FontFamily.GenericSansSerif, 60f), Brushes.Orange, 439, 10);

                    float stringP1em = 60f;
                    float stringP2em = 60f;
                    SizeF stringP1 = this.graphicHeader.MeasureString(nameP1, new Font(FontFamily.GenericSansSerif, stringP1em));
                    SizeF stringP2 = this.graphicHeader.MeasureString(nameP2, new Font(FontFamily.GenericSansSerif, stringP2em));
                    while (stringP1.Width > 350)
                    {
                        stringP1em--;
                        if (stringP1em == 40) break;
                        stringP1 = this.graphicHeader.MeasureString(nameP1, new Font(FontFamily.GenericSansSerif, stringP1em));
                    }
                    while (stringP2.Width > 350)
                    {
                        stringP2em--;
                        if (stringP2em == 40) break;
                        stringP2 = this.graphicHeader.MeasureString(nameP2, new Font(FontFamily.GenericSansSerif, stringP2em));
                    }
                    this.graphicHeader.DrawString(nameP1, new Font(FontFamily.GenericSansSerif, stringP1em), Brushes.DeepSkyBlue, new Rectangle(14, 10, 350 - 8, 130 - 8));
                    this.graphicHeader.DrawString(nameP2, new Font(FontFamily.GenericSansSerif, stringP2em), Brushes.BlueViolet, new Rectangle(639, 10, 350 - 8, 130 - 8));
                    if (this.playerList[0].turn == true)
                    {
                        this.graphicHeader.DrawPolygon(new Pen(Brushes.Orange, 5), new Point[] { new Point(430, 25), new Point(375, 55), new Point(430, 85) });
                    }
                    else if (this.playerList[1].turn == true)
                    {
                        this.graphicHeader.DrawPolygon(new Pen(Brushes.Orange, 5), new Point[] { new Point(578, 25), new Point(633, 55), new Point(578, 85) });
                    }

                    try
                    {
                        this.bitmapHeader.Save(bitmapPathHeader);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Fehler 1:\n" + e);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
        }

        private async Task PlayTTT(SocketMessage msg)
        {
            SocketUserMessage msgUser = msg as SocketUserMessage;
            if (msgUser == null) return;

            JSONDatabase.GuildSettings guildSetting = new JSONDatabase.GuildSettings();
            SocketGuildChannel guild = msg.Channel as SocketGuildChannel;

            if (msgUser.Channel.Id != guildSetting.GetTTTChannel(guild.Guild.Id)) return;

            int prefixPosition = 0;

            if (!(msgUser.HasCharPrefix(guildSetting.GetPrefix(guild.Guild.Id), ref prefixPosition) ||
            msgUser.HasMentionPrefix(this.client.CurrentUser, ref prefixPosition)) ||
            msgUser.Author.IsBot)
                return;

            string[] command = msg.Content.Split(" ");
            if (!(command[0] == guildSetting.GetPrefix(guild.Guild.Id) + "playttt")) return;
            if (command[0] == guildSetting.GetPrefix(guild.Guild.Id) + "playttt" && playerList.Count == 2)
            {
                await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Es läuft bereits ein Spiel.");
                return;
            }

            if (this.playerList.Count == 1 && this.playerList[0].playerID == msgUser.Author.Id)
            {
                await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Du bist bereits als Spieler 1 registriert, bitte warte auf einen Gegner.");
                return;
            }

            this.playerList.Add(new Player
            {
                playerID = msgUser.Author.Id,
                playerName = msgUser.Author.Username,
                turn = false
            });

            if (this.playerList.Count == 2 && this.playfieldStarted == false)
            {
                this.playfieldStarted = true;
                await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Du bist Player 2. Los gehts!");

                for (int i = 0; i < 9; i++)
                {
                    this.playFieldList.Add(new Playfield(i));
                }

                this.playerList[0].SetTurn(true);

                this.bitmapPlayfield = new Bitmap(this.bitmapWidth, this.bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                this.graphics = Graphics.FromImage(this.bitmapPlayfield);
                this.penField = new Pen(Color.FromKnownColor(KnownColor.Black), penThicknes);
                this.penP1 = new Pen(Color.FromKnownColor(KnownColor.DeepSkyBlue), penThicknes);
                this.penP2 = new Pen(Color.FromKnownColor(KnownColor.BlueViolet), penThicknes);
                this.draw = new Draw(this.bitmapPlayfield, this.graphics);
                DrawHeader(this.playerList[0].playerName, this.playerList[1].playerName);
                DrawField();
                this.appandBM = new AppendBitmap();
                this.complete = appandBM.AppendBottom(this.bitmapHeader, this.bitmapPlayfield);
                try
                {

                this.complete.Save(this.bitmapPathCompleteImage);
                }
                    catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fehler 5:\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }

            await msgUser.Channel.SendMessageAsync("Du bist dran <@!" + this.playerList[0].playerID + "> Schreib eine Zahl von 1 - 9 in den Chat um dein Feld zu füllen.");
            await msgUser.Channel.SendFileAsync(this.bitmapPathCompleteImage);
            }

            if (playfieldStarted == false)
            {
                await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Du bist Player 1, warte auf einen Mitspieler");
            }
        }

        private async Task ResetTTT(SocketMessage msg)
        {
            SocketUserMessage msgUser = msg as SocketUserMessage;
            if (msgUser == null) return;

            JSONDatabase.GuildSettings guildSetting = new JSONDatabase.GuildSettings();
            SocketGuildChannel guild = msg.Channel as SocketGuildChannel;

            if (msgUser.Channel.Id != guildSetting.GetTTTChannel(guild.Guild.Id)) return;

            int prefixPosition = 0;

            if (!(msgUser.HasCharPrefix(guildSetting.GetPrefix(guild.Guild.Id), ref prefixPosition) ||
            msgUser.HasMentionPrefix(this.client.CurrentUser, ref prefixPosition)) ||
            msgUser.Author.IsBot)
                return;

            string[] command = msg.Content.Split(" ");
            if (!(command[0] == guildSetting.GetPrefix(guild.Guild.Id) + "resetttt")) return;

            this.playfieldStarted = false;
            this.playerList.Clear();
            this.playerList = new List<Player>();
            this.playFieldList.Clear();
            this.playFieldList = new List<Playfield>();

            await msgUser.Channel.SendMessageAsync("TicTacToe resetet.");

        }

        private async Task Game(SocketMessage msg)
        {
            if (playfieldStarted == false) return;
            SocketUserMessage msgUser = msg as SocketUserMessage;
            if (msgUser == null) return;

            JSONDatabase.GuildSettings guildSetting = new JSONDatabase.GuildSettings();
            SocketGuildChannel guild = msg.Channel as SocketGuildChannel;

            if (msgUser.Channel.Id != guildSetting.GetTTTChannel(guild.Guild.Id)) return;

            int prefixPosition = 0;

            if (msgUser.HasMentionPrefix(client.CurrentUser, ref prefixPosition) || msgUser.Author.IsBot) return;

            if (playerList[0].turn == true)
            {
                if (this.playerList[0].playerID != msgUser.Author.Id) return;
                if (CheckNumber(msgUser) == -1) return;
                if (this.playFieldList[CheckNumber(msgUser) - 1].isSet == true)
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Platz schon belegt!");
                    return;
                }
                this.playFieldList[CheckNumber(msgUser) - 1].SetIsSet(true);
                this.playFieldList[CheckNumber(msgUser) - 1].SetPlayer(PlayerOnField.Player1);
                this.playerList[0].SetTurn(false);
                this.playerList[1].SetTurn(true);
                DrawHeader(this.playerList[0].playerName, this.playerList[1].playerName);
                DrawField();
                this.complete = appandBM.AppendBottom(this.bitmapHeader, this.bitmapPlayfield);
                try { 

                this.complete.Save(bitmapPathCompleteImage);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fehler 2:\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                await msgUser.Channel.SendFileAsync(this.bitmapPathCompleteImage);
                if (CheckIfWon())
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Glückwunsch du hast gewonnen!");
                    ResetGameLists();
                    this.playfieldStarted = false;
                }
                if (CheckIfNoOneWon())
                {
                    await msgUser.Channel.SendMessageAsync("Das war ein Unentschieden. Ihr seid einfach zu gut!");
                    ResetGameLists();
                    this.playfieldStarted = false;
                }
            }
            else if (this.playerList[1].turn == true)
            {
                if (this.playerList[1].playerID != msgUser.Author.Id) return;
                if (CheckNumber(msgUser) == -1) return;
                if (this.playFieldList[CheckNumber(msgUser) - 1].isSet == true)
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Platz schon belegt!");
                    return;
                }
                this.playFieldList[CheckNumber(msgUser) - 1].SetIsSet(true);
                this.playFieldList[CheckNumber(msgUser) - 1].SetPlayer(PlayerOnField.Player2);
                this.playerList[0].SetTurn(true);
                this.playerList[1].SetTurn(false);
                DrawHeader(this.playerList[0].playerName, this.playerList[1].playerName);
                DrawField();
                this.complete = appandBM.AppendBottom(this.bitmapHeader, this.bitmapPlayfield);

                try 
                { 
                this.complete.Save(this.bitmapPathCompleteImage);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fehler 3:\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                await msgUser.Channel.SendFileAsync(this.bitmapPathCompleteImage);
                if (CheckIfWon())
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Glückwunsch du hast gewonnen!");
                    ResetGameLists();
                    this.playfieldStarted = false;
                }
                if (CheckIfNoOneWon())
                {
                    await msgUser.Channel.SendMessageAsync("Das war ein Unentschieden. Ihr seid einfach zu gut!");
                    ResetGameLists();
                    this.playfieldStarted = false;
                }
            }
        }

        private void ResetGameLists()
        {
            playFieldList.Clear();
            playerList.Clear();
            playFieldList = new List<Playfield>();
            playerList = new List<Player>();
        }

        private void DrawField()
        {
            this.draw.DrawTicTacToeField(this.penField);
            for (int i = 0; i < 9; i++)
            {
                if (this.playFieldList[i].playerOnField == PlayerOnField.Player1)
                {
                    this.draw.DrawCircle(i + 1, this.penP1);
                }
                else if (this.playFieldList[i].playerOnField == PlayerOnField.Player2)
                {
                    this.draw.DrawCross(i + 1, this.penP2);
                }
            }
            try
            {
            this.bitmapPlayfield.Save(bitmapPathPlayfield);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fehler 4:\n" + e);
                Console.ForegroundColor = ConsoleColor.White;
            }
}

        private int CheckNumber(SocketUserMessage msgUser)
        {
            int number;
            try
            {
                number = Convert.ToInt32(msgUser.Content);
            }
            catch (Exception)
            {
                msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Deine Eingabe war keine Zahl");
                return -1;
            }

            if (number < 1 && number > 9)
            {
                return -1;
            }
            return number;
        }

        private bool CheckIfWon()
        {
            bool won = false;

            // First Row
            if (this.playFieldList[0].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[1].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[2].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[0].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[1].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[2].playerOnField == PlayerOnField.Player2)
                won = true;
            // Second Row
            if (this.playFieldList[3].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[5].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[3].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[5].playerOnField == PlayerOnField.Player2)
                won = true;
            // Third Row
            if (this.playFieldList[6].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[7].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[8].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[6].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[7].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[8].playerOnField == PlayerOnField.Player2)
                won = true;
            // First Column
            if (this.playFieldList[0].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[3].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[6].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[0].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[3].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[6].playerOnField == PlayerOnField.Player2)
                won = true;
            // Second Column
            if (this.playFieldList[1].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[7].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[1].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[7].playerOnField == PlayerOnField.Player2)
                won = true;
            // Third Column
            if (this.playFieldList[2].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[5].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[8].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[2].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[5].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[8].playerOnField == PlayerOnField.Player2)
                won = true;
            // TopLeft to BottomRight
            if (this.playFieldList[0].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[8].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[0].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[8].playerOnField == PlayerOnField.Player2)
                won = true;
            // TopRight to BottomLeft
            if (this.playFieldList[2].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                this.playFieldList[6].playerOnField == PlayerOnField.Player1 ||
                this.playFieldList[2].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                this.playFieldList[6].playerOnField == PlayerOnField.Player2)
                won = true;

            return won;
        }

        private bool CheckIfNoOneWon()
        {
            bool draw = false;
            int count = 0;

            foreach (var item in this.playFieldList)
            {
                if (item.isSet == true)
                    count++;
            }
            if (count == 9)
            {
                draw = true;
            }
            return draw;
        }

        public class Player
        {
            public ulong playerID;
            public string playerName;
            public bool turn;

            public void SetTurn(bool turn)
            {
                this.turn = turn;
            }
        }

        public class Playfield
        {
            public int pos;
            public bool isSet;
            public PlayerOnField playerOnField;

            public Playfield(int pos, bool isSet = false, PlayerOnField playerOnField = PlayerOnField.NoPlayer)
            {
                this.pos = pos;
                this.isSet = isSet;
                this.playerOnField = playerOnField;
            }
            
            public void SetPlayer(PlayerOnField playerOnField)
            {
                this.playerOnField = playerOnField;
            }

            public void SetIsSet(bool isSet)
            {
                this.isSet = isSet;
            }
        }

        public enum PlayerOnField
        {
            NoPlayer,
            Player1,
            Player2
        }

    }
}
