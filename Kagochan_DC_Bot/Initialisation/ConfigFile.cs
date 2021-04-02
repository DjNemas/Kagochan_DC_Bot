using Kagochan_DC_Bot.DataManagment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kagochan_DC_Bot.Initialisation
{
    class ConfigFile
    {
        private string token;
        private char prefix;
        private ulong tttChannel;
        private string configPath = Folder.configFolder + "config.xml";

        public void CreateConfigFile()
        {
            if (File.Exists(configPath)) return;
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Config",
                        new XElement("Token", "Token Here"),
                        new XElement("Prefix", "$"),
                        new XElement("TTTChannel", "0")
                    )
            );
            doc.Save(configPath);
        }

        public string GetToken()
        {
            XDocument doc = XDocument.Load(configPath);
            var tokenQuery = from config in doc.Descendants("Config")
                        select config;

            foreach (var item in tokenQuery)
            {
                this.token = item.Element("Token").Value;
            }

            return this.token;
        }

        public char GetPrefix()
        {
            this.prefix = ' ';
            XDocument doc = XDocument.Load(configPath);
            var prefixQueue = from config in doc.Descendants("Config")
                             select config;

            foreach (var item in prefixQueue)
            {
                if (item.Element("Prefix").Value.Length > 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error] Prefix only can handle 1 letter");
                    Console.ForegroundColor = ConsoleColor.White;
                    Debug.Log.logMain("Prefix only can handle 1 letter", Debug.Log.LogLevel.Error);
                    break;
                }
                this.prefix = Convert.ToChar(item.Element("Prefix").Value);
            }
            if (this.prefix == ' ') return '$';

            return this.prefix;
        }

        public ulong GetTTTChannel()
        {
            XDocument doc = XDocument.Load(configPath);
            var tttChannelQuery = from config in doc.Descendants("Config")
                             select config;

            foreach (var item in tttChannelQuery)
            {
                this.tttChannel = Convert.ToUInt64(item.Element("TTTChannel").Value);
            }

            return this.tttChannel;;
        }
    }
}
