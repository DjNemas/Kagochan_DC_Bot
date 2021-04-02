using System;
using System.IO;

namespace Kagochan_DC_Bot.DataManagment
{
    public static class Folder
    {
        public static string logFolder = @"./log/";
        public static string imageFolder = @"./images/";
        public static string configFolder = @"./config/";

        public static void CreateFolder(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ordner konnte nicht erstellt werden.\n" + e);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
