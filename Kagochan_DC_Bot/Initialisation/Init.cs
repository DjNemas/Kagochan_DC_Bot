using System;
using Kagochan_DC_Bot.DataManagment;

namespace Kagochan_DC_Bot.Initialisation
{
    public class Init
    {
        private ConfigFile config;
        public Init()
        {
            CreateAllFolder();
            this.config = new ConfigFile();
            this.config.CreateConfigFile();
        }

        private void CreateAllFolder()
        {
            Folder.CreateFolder(Folder.logFolder);
            Folder.CreateFolder(Folder.imageFolder);
            Folder.CreateFolder(Folder.configFolder);
        }
    }
}
