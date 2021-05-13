using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kagochan_DC_Bot.JSONDatabase
{
    public class GuildSettings
    {
        [NonSerialized]
        private static List<GuildSettings> guildList;
        [NonSerialized]
        private static string configDBFile = "GuildSettings.json";

        public ulong _guildID { get; set; }
        public char _prefix { get; set; }
        public ulong _tttChannel { get; set; }

        public GuildSettings()
        {
            if (guildList == null)
            {
                guildList = new List<GuildSettings>();
            }
        }

        public void AddGuildToList(ulong guildID, ulong tttChannelID)
        {
            bool isInDB = false;
            foreach (var guild in guildList)
            {
                if (guild._guildID == guildID)
                {
                    
                    isInDB = true;
                    break;
                }
            }
            if (isInDB)
            {
                return;
            }

            this._guildID = guildID;
            this._prefix = '!';
            this._tttChannel = tttChannelID;
            guildList.Add(this);
            Update();
        }

        public char GetPrefix(ulong guildID)
        {
            char prefix = ' ';
            guildList.ForEach((x) =>
            {
                if (x._guildID == guildID)
                {
                    prefix = x._prefix;
                }
            });
            return prefix;
        }

        public void UpdateTTTChannel(ulong guildID, ulong tttChannel)
        {
            foreach (var guild in guildList)
            {
                if (guild._guildID == guildID)
                {
                    guild._tttChannel = tttChannel;
                }
            }
            Update();
        }

        public void UpdatePrefix(ulong guildID, char prefix)
        {
            foreach (var guild in guildList)
            {
                if (guild._guildID == guildID)
                {
                    guild._prefix = prefix;
                }
            }
            Update();
        }

        public ulong GetTTTChannel(ulong guildID)
        {
            ulong channel = 0;
            guildList.ForEach((x) =>
            {
                if (x._guildID == guildID)
                {
                    channel = x._tttChannel;
                }
            });
            return channel;
        }

        public static void LoadDB()
        {
            if (File.Exists(DataManagment.Folder.configFolder + configDBFile))
            {
                guildList = JsonSerializer.Deserialize<List<GuildSettings>>(File.ReadAllText(DataManagment.Folder.configFolder + configDBFile));
            }
        }

        private void Update()
        {
            string jsonString = JsonSerializer.Serialize(guildList);
            File.WriteAllText(DataManagment.Folder.configFolder + configDBFile, jsonString);
        }
    }
}
