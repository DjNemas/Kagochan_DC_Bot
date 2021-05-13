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
        private string configPath = Folder.configFolder + "config.xml";

        public void CreateConfigFile()
        {
            if (File.Exists(configPath)) return;
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Config",
                        new XElement("Token", "Token Here")
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
    }
}
