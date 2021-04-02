using Discord.Commands;
using Discord.WebSocket;
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
        private Bitmap bitmap;
        private Graphics graphics;
        private Pen penField;
        private Pen penP1;
        private Pen penP2;
        private string bitmapPath = Folder.imageFolder + "tttplayfield.png";
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

        private async Task PlayTTT(SocketMessage msg)
        {
            SocketUserMessage msgUser = msg as SocketUserMessage;
            if (msgUser == null) return;

            if (msgUser.Channel.Id != configFile.GetTTTChannel()) return;

            int prefixPosition = 0;

            if (!(msgUser.HasCharPrefix(configFile.GetPrefix(), ref prefixPosition) ||
            msgUser.HasMentionPrefix(client.CurrentUser, ref prefixPosition)) ||
            msgUser.Author.IsBot)
                return;

            string[] command = msg.Content.Split(" ");
            if (!(command[0] == configFile.GetPrefix() + "playttt")) return;
            if (command[0] == configFile.GetPrefix() + "playttt" && playerList.Count == 2)
            {
                await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Es läuft bereits ein Spiel.");
                return;
            }

            if (playerList.Count == 1 && playerList[0].playerID == msgUser.Author.Id)
            {
                await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Du bist bereits als Spieler 1 registriert, bitte warte auf einen Gegner.");
                return;
            }

            playerList.Add(new Player
            {
                playerID = msgUser.Author.Id,
                turn = false
            });

            if (playerList.Count == 2 && playfieldStarted == false)
            {
                playfieldStarted = true;
                await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Du bist Player 2. Los gehts!");

                for (int i = 0; i < 9; i++)
                {
                    playFieldList.Add(new Playfield(i));
                }

                this.bitmap = new Bitmap(bitmapWidth, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                this.graphics = Graphics.FromImage(bitmap);
                this.penField = new Pen(Color.FromKnownColor(KnownColor.Black), penThicknes);
                this.penP1 = new Pen(Color.FromKnownColor(KnownColor.DeepSkyBlue), penThicknes);
                this.penP2 = new Pen(Color.FromKnownColor(KnownColor.BlueViolet), penThicknes);
                draw = new Draw(bitmap, graphics);
                DrawField();
                bitmap.Save(bitmapPath);

                playerList[0].SetTurn(true);

                await msgUser.Channel.SendMessageAsync("Du bist dran Player 1! Schreib eine Zahl von 1 - 9 in den Chat um dein Feld zu füllen.");
                await msgUser.Channel.SendFileAsync(bitmapPath);


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

            if (msgUser.Channel.Id != configFile.GetTTTChannel()) return;

            int prefixPosition = 0;

            if (!(msgUser.HasCharPrefix(configFile.GetPrefix(), ref prefixPosition) ||
            msgUser.HasMentionPrefix(client.CurrentUser, ref prefixPosition)) ||
            msgUser.Author.IsBot)
                return;

            string[] command = msg.Content.Split(" ");
            if (!(command[0] == configFile.GetPrefix() + "resetttt")) return;

            playfieldStarted = false;
            playerList.Clear();
            playerList = new List<Player>();
            playFieldList.Clear();
            playFieldList = new List<Playfield>();

            await msgUser.Channel.SendMessageAsync("TicTacToe resetet.");

        }

        private async Task Game(SocketMessage msg)
        {
            if (playfieldStarted == false) return;
            SocketUserMessage msgUser = msg as SocketUserMessage;
            if (msgUser == null) return;

            if (msgUser.Channel.Id != configFile.GetTTTChannel()) return;

            int prefixPosition = 0;
            if (msgUser.HasMentionPrefix(client.CurrentUser, ref prefixPosition) || msgUser.Author.IsBot) return;

            if (playerList[0].turn == true)
            {
                if (playerList[0].playerID != msgUser.Author.Id) return;
                if (CheckNumber(msgUser) == -1) return;
                if (playFieldList[CheckNumber(msgUser) - 1].isSet == true)
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Platz schon belegt!");
                    return;
                }
                playFieldList[CheckNumber(msgUser) - 1].SetIsSet(true);
                playFieldList[CheckNumber(msgUser) - 1].SetPlayer(PlayerOnField.Player1);
                playerList[0].SetTurn(false);
                playerList[1].SetTurn(true);
                DrawField();
                await msgUser.Channel.SendFileAsync(bitmapPath);
                if (CheckIfWon())
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Glückwunsch du hast gewonnen!");
                    ResetGameLists();
                    playfieldStarted = false;
                }
                if (CheckIfNoOneWon())
                {
                    await msgUser.Channel.SendMessageAsync("Das war ein Unentschieden. Ihr seid einfach zu gut!");
                    ResetGameLists();
                    playfieldStarted = false;
                }
            }
            else if (playerList[1].turn == true)
            {
                if (playerList[1].playerID != msgUser.Author.Id) return;
                if (CheckNumber(msgUser) == -1) return;
                if (playFieldList[CheckNumber(msgUser) - 1].isSet == true)
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Platz schon belegt!");
                    return;
                }
                playFieldList[CheckNumber(msgUser) - 1].SetIsSet(true);
                playFieldList[CheckNumber(msgUser) - 1].SetPlayer(PlayerOnField.Player2);
                playerList[0].SetTurn(true);
                playerList[1].SetTurn(false);
                DrawField();
                await msgUser.Channel.SendFileAsync(bitmapPath);
                if (CheckIfWon())
                {
                    await msgUser.Channel.SendMessageAsync(msgUser.Author.Mention + " Glückwunsch du hast gewonnen!");
                    ResetGameLists();
                    playfieldStarted = false;
                }
                if (CheckIfNoOneWon())
                {
                    await msgUser.Channel.SendMessageAsync("Das war ein Unentschieden. Ihr seid einfach zu gut!");
                    ResetGameLists();
                    playfieldStarted = false;
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
            draw.DrawTicTacToeField(penField);
            for (int i = 0; i < 9; i++)
            {
                if (playFieldList[i].playerOnField == PlayerOnField.Player1)
                {
                    draw.DrawCircle(i + 1, penP1);
                }
                else if (playFieldList[i].playerOnField == PlayerOnField.Player2)
                {
                    draw.DrawCross(i + 1, penP2);
                }
            }
            bitmap.Save(bitmapPath);
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
            if (playFieldList[0].playerOnField == PlayerOnField.Player1 &&
                playFieldList[1].playerOnField == PlayerOnField.Player1 &&
                playFieldList[2].playerOnField == PlayerOnField.Player1 ||
                playFieldList[0].playerOnField == PlayerOnField.Player2 &&
                playFieldList[1].playerOnField == PlayerOnField.Player2 &&
                playFieldList[2].playerOnField == PlayerOnField.Player2)
                won = true;
            // Second Row
            if (playFieldList[3].playerOnField == PlayerOnField.Player1 &&
                playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                playFieldList[5].playerOnField == PlayerOnField.Player1 ||
                playFieldList[3].playerOnField == PlayerOnField.Player2 &&
                playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                playFieldList[5].playerOnField == PlayerOnField.Player2)
                won = true;
            // Third Row
            if (playFieldList[6].playerOnField == PlayerOnField.Player1 &&
                playFieldList[7].playerOnField == PlayerOnField.Player1 &&
                playFieldList[8].playerOnField == PlayerOnField.Player1 ||
                playFieldList[6].playerOnField == PlayerOnField.Player2 &&
                playFieldList[7].playerOnField == PlayerOnField.Player2 &&
                playFieldList[8].playerOnField == PlayerOnField.Player2)
                won = true;
            // First Column
            if (playFieldList[0].playerOnField == PlayerOnField.Player1 &&
                playFieldList[3].playerOnField == PlayerOnField.Player1 &&
                playFieldList[6].playerOnField == PlayerOnField.Player1 ||
                playFieldList[0].playerOnField == PlayerOnField.Player2 &&
                playFieldList[3].playerOnField == PlayerOnField.Player2 &&
                playFieldList[6].playerOnField == PlayerOnField.Player2)
                won = true;
            // Second Column
            if (playFieldList[1].playerOnField == PlayerOnField.Player1 &&
                playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                playFieldList[7].playerOnField == PlayerOnField.Player1 ||
                playFieldList[1].playerOnField == PlayerOnField.Player2 &&
                playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                playFieldList[7].playerOnField == PlayerOnField.Player2)
                won = true;
            // Third Column
            if (playFieldList[2].playerOnField == PlayerOnField.Player1 &&
                playFieldList[5].playerOnField == PlayerOnField.Player1 &&
                playFieldList[8].playerOnField == PlayerOnField.Player1 ||
                playFieldList[2].playerOnField == PlayerOnField.Player2 &&
                playFieldList[5].playerOnField == PlayerOnField.Player2 &&
                playFieldList[8].playerOnField == PlayerOnField.Player2)
                won = true;
            // TopLeft to BottomRight
            if (playFieldList[0].playerOnField == PlayerOnField.Player1 &&
                playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                playFieldList[8].playerOnField == PlayerOnField.Player1 ||
                playFieldList[0].playerOnField == PlayerOnField.Player2 &&
                playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                playFieldList[8].playerOnField == PlayerOnField.Player2)
                won = true;
            // TopRight to BottomLeft
            if (playFieldList[2].playerOnField == PlayerOnField.Player1 &&
                playFieldList[4].playerOnField == PlayerOnField.Player1 &&
                playFieldList[6].playerOnField == PlayerOnField.Player1 ||
                playFieldList[2].playerOnField == PlayerOnField.Player2 &&
                playFieldList[4].playerOnField == PlayerOnField.Player2 &&
                playFieldList[6].playerOnField == PlayerOnField.Player2)
                won = true;

            return won;
        }

        private bool CheckIfNoOneWon()
        {
            bool draw = false;
            int count = 0;

            foreach (var item in playFieldList)
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
