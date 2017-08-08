using Blocks;
using GeneratePlayerMaze.Classes;
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePlayerMaze
{
    class Program
    {
        static BlockList blockList = new BlockList();
        static Map map;
        static List<Trap> traps = new List<Trap>();
        static Random rand = new Random();

        static String[][] mapdata;
        static int mapHeightY = 0;
        static int mapWidthX = 0;

        static Block start = null;
        static Block finish = null;
        static Point StartLocation = new Point();
        static Point EndLocation = new Point();

        static int mapsmade = 0;
        static int mapsRequested = 1;

        static int skillLvl = 1;
        static int minMoney = 200;
        static int maxMoney = 1000;
        static bool debug = false;

        static List<String[][]> madeMaps = new List<String[][]>();

        static void Main(string[] args)
        {
            Console.WindowWidth = 200;

            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric argument (skill, min-money, max-money)");
                return;
            }
            skillLvl = int.Parse(args[0]);

            if (args.Length >= 3)
            {
                minMoney = int.Parse(args[1]);
                maxMoney = int.Parse(args[2]);
            }

            if (args.Length >= 4)
            {
                mapsRequested = int.Parse(args[3]);
            }

            if (args.Length >= 5)
            {
                debug = bool.Parse(args[4]);
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile($"{Directory.GetCurrentDirectory()}\\BaseLevels\\Skill{skillLvl}\\config.ini");
            int maxSpikes = int.Parse(data["Traps"]["Spikes"]);
            for (int i = 0; i < maxSpikes; i++) traps.Add(new Trap("d1", false, true));

            if (debug) Console.WriteLine("Loading map");
            Loadmap();

            if (debug) Console.WriteLine("Checking route");
            List<Point> route = map.mPathFinder.FindPath(StartLocation, EndLocation, 2);
            if (debug) PrintMap(route);

            bool running = true;
            if (debug) Console.WriteLine("Placing traps");
            while (running)
            {
                if (debug) Console.WriteLine("Reloading map");
                Loadmap();
                route = map.mPathFinder.FindPath(StartLocation, EndLocation, 2);

                if (debug) Console.WriteLine("Adding traps");
                if (route != null && route.Count > 4)
                {
                    foreach (Trap trap in traps)
                    {
                        bool found = false;
                        while (!found)
                        {
                            int randNum = rand.Next(2, route.Count - 2);
                            if (trap.Ground)
                            {
                                if (map.IsGround(route[randNum].X, route[randNum].Y - 1))
                                {
                                    mapdata[(mapHeightY - 1) - route[randNum].Y][route[randNum].X] = trap.Type;
                                    map.SetTile(route[randNum].X, route[randNum].Y, TileType.Deadly);
                                    found = true;
                                }
                            }
                            else
                            {
                                mapdata[(mapHeightY - 1) - route[randNum].Y][route[randNum].X] = trap.Type;
                                map.SetTile(route[randNum].X, route[randNum].Y, TileType.Deadly);
                                found = true;
                            }
                        }
                    }
                } else
                {
                    if (debug) Console.WriteLine("Route to short");
                    route = null;
                }

                // Check if we can still win the map with the added traps
                route = map.mPathFinder.FindPath(StartLocation, EndLocation, 2);
                if (route != null)
                {
                    if (debug) PrintMap(route);
                    SaveMap(route);
                    if (mapsRequested == mapsmade)
                    {
                        Console.WriteLine("Finished writing maps");
                        running = false;
                    }
                }
            }
            Environment.Exit(0);
        }

        public static void PrintMap(List<Point> route)
        {
            for (int y = 0; y < mapHeightY; y++)
            {
                StringBuilder mapstring = new StringBuilder();
                for (int x = 0; x < mapWidthX; x++)
                {
                    if (route != null && route.Any(gp => gp.X == x && gp.Y == (mapHeightY - 1) - y))
                        mapstring.Append("XX ");
                    else
                    {
                        if (mapdata[y][x] == start.BlockID) mapstring.Append("SS ");
                        else if (mapdata[y][x] == finish.BlockID) mapstring.Append("FN ");
                        else mapstring.Append(map.tiles[x, (mapHeightY - 1) - y] == TileType.Block ? "BB " : "OO ");
                    }
                }
                Console.WriteLine(mapstring.ToString());
            }
            Console.WriteLine((route != null) ? "route found" : "no route");
        }

        public static void Loadmap()
        {
            String[] result = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\BaseLevels\\Skill{skillLvl}\\level.txt");
            mapdata = new String[result.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                mapdata[i] = result[i].Split(' ');
            }
            mapHeightY = mapdata.Length;
            mapWidthX = mapdata[0].Length;

            start = blockList.blocks.Find(bl => bl.Name == "start");
            finish = blockList.blocks.Find(bl => bl.Name == "finish");

            map = new Map(mapWidthX, mapHeightY);
            for (int y = 0; y < mapHeightY; y++)
            {
                for (int x = 0; x < mapWidthX; x++)
                {
                    if (mapdata[y][x] == start.BlockID) StartLocation = new Point(x, (mapHeightY - 1) - y);
                    else if (mapdata[y][x] == finish.BlockID) EndLocation = new Point(x, (mapHeightY - 1) - y);

                    foreach (Block blck in blockList.blocks)
                    {
                        if (mapdata[y][x] == blck.BlockID)
                        {
                            map.SetTile(x, (mapHeightY - 1) - y, blck.Type);
                        }
                    }
                }
            }
        }

        public static void SaveMap(List<Point> route)
        {
            StringBuilder mapstring = new StringBuilder();
            for (int y = 0; y < mapHeightY; y++)
            {                
                for (int x = 0; x < mapWidthX; x++)
                {
                    mapstring.Append($"{mapdata[y][x]} ");
                }
                mapstring.AppendLine();
            }

            int mapnum = 1;
            while (Directory.Exists($"{Directory.GetCurrentDirectory()}\\maps\\Skill{skillLvl}\\map{mapnum}\\")) mapnum++;
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\maps\\Skill{skillLvl}\\map{mapnum}\\");
            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\maps\\Skill{skillLvl}\\map{mapnum}\\level.txt", mapstring.ToString());
            mapsmade++;
        }
    }
}
