using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePlayerMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(String.Format("{0}\\BaseLevels\\Skill1\\config.ini", Directory.GetCurrentDirectory()));

            string useFullScreenStr = data["GeneralConfiguration"]["setUpdate"];
            Console.ReadLine();

        }
    }
}
