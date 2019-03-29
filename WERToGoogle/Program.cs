using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WERToGoogle
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<long, string> playerDict = new Dictionary<long, string>();
            List<IList<object>> playerValues;

            string filePath = @"C:\Users\sgold\Downloads\EnrolledPlayers-example.xml";
            AddPlayersInFileToDictionary(playerDict, filePath);
            ConvertPlayerDictionaryToTable(playerDict, out playerValues);

            GoogleSheet sheet = new GoogleSheet();
            sheet.WriteValues(playerValues);
        }

        private static void AddPlayersInFileToDictionary(Dictionary<long, string> playerDict, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(@event));
            System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
            @event @event = (@event)xmlSerializer.Deserialize(reader);
            foreach (var player in @event.teams)
            {
                string playerName = player.name;
                long playerDciNumber = player.players.player.dciNumber;
                if (!playerDict.ContainsKey(playerDciNumber)) playerDict.Add(playerDciNumber, playerName);
            }
        }

        private static void ConvertPlayerDictionaryToTable(Dictionary<long, string> playerDict, out List<IList<object>> playerValues)
        {
            playerValues = new List<IList<object>>();

            foreach (var player in playerDict)
            {
                List<object> list = new List<object>();
                list.Add(player.Key);
                list.Add(player.Value);
                playerValues.Add(list);
            }
        }
    }
}
