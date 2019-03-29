using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WERToGoogle
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\sgold\Downloads\EnrolledPlayers-example.xml";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(@event));
            System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
            @event @event = (@event)xmlSerializer.Deserialize(reader);
            foreach (var player in @event.teams)
            {
                string playerName = player.name;
                long playerDciNumber = player.players.player.dciNumber;
                Console.WriteLine(String.Format("{0,-50} {1,-20}",playerName,playerDciNumber));
            }
        }
    }
}
