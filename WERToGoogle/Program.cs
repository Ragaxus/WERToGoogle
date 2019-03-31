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
            List<IList<object>> playerValues = new List<IList<object>>();
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
                    AddPlayersInFileToValuesBody(playerValues, filePath);
                }
            }
            else //inputPath is a file
            {
                AddPlayersInFileToValuesBody(playerValues, inputPath);
            }                    

            string spreadsheetId = ConfigurationManager.AppSettings["spreadsheetId"].ToString();
            string range = ConfigurationManager.AppSettings["range"].ToString();
            GoogleSheet sheet = new GoogleSheet(spreadsheetId,range);
            sheet.WriteValues(playerValues);
        }

        private static void AddPlayersInFileToValuesBody(List<IList<object>> playerValues, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(@event));
            System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
            @event @event = (@event)xmlSerializer.Deserialize(reader);
            foreach (var player in @event.teams)
            {
                List<object> playerValuesEntry = new List<object>();
                string playerName = player.name;
                long playerDciNumber = player.players.player.dciNumber;
                playerValuesEntry.Add(playerDciNumber);
                playerValuesEntry.AddRange(playerName.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                playerValues.Add(playerValuesEntry);
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
