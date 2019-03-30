using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;

namespace WERToGoogle
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<long, string[]> playerDict = new Dictionary<long, string[]>();
            List<IList<object>> playerValues;
            if (args.Length < 1) {
                Console.Write("To use this program, drag and drop the file or folder to convert onto the icon.");
                Console.Read();
                return;
            }
            string inputPath = args[0];
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(inputPath);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                foreach (string filePath in System.IO.Directory.GetFiles(inputPath))
                {
                    AddPlayersInFileToDictionary(playerDict, filePath);
                }
            }
            else //inputPath is a file
            {
                AddPlayersInFileToDictionary(playerDict, inputPath);
            }
                    
            ConvertPlayerDictionaryToTable(playerDict, out playerValues);

            string spreadsheetId = ConfigurationManager.AppSettings["spreadsheetId"].ToString();
            string range = ConfigurationManager.AppSettings["range"].ToString();
            GoogleSheet sheet = new GoogleSheet(spreadsheetId,range);
            sheet.WriteValues(playerValues);
        }

        private static void AddPlayersInFileToDictionary(Dictionary<long, string[]> playerDict, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(@event));
            System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
            @event @event = (@event)xmlSerializer.Deserialize(reader);
            foreach (var player in @event.teams)
            { 
                string playerName = player.name;
                long playerDciNumber = player.players.player.dciNumber;
                if (!playerDict.ContainsKey(playerDciNumber)) playerDict.Add(playerDciNumber, playerName.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }
        }

        private static void ConvertPlayerDictionaryToTable(Dictionary<long, string[]> playerDict, out List<IList<object>> playerValues)
        {
            playerValues = new List<IList<object>>();

            foreach (var player in playerDict)
            {
                List<object> list = new List<object>();
                list.Add(player.Key);
                foreach (var namepart in player.Value) { list.Add(namepart); };
                playerValues.Add(list);
            }
        }
    }
}
