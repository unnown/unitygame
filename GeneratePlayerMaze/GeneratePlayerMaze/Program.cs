using Blocks;
using GeneratePlayerMaze.Classes;
using IniParser;
using IniParser.Model;
using Routing.Classes;
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
        static int failedAttempts = 0;
        static int maxFailedAttempts = 300;

        static int skillLvl = 1;
        static int minMoney = 200;
        static int maxMoney = 1000;        
        static short jumpHeight = 4;
        static int mintrapDist = 2;
        static bool debug = false;

        static int walkID = 0;

        static List<String[][]> madeMaps = new List<String[][]>();

        static void Main(string[] args)
        {
            Console.WindowWidth = 200;

            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric argument (skill, min-money, max-money, jumpheight)");
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
            mapsRequested = 100000;
            maxFailedAttempts = 1000;

            if (args.Length >= 5)
            {
                jumpHeight = short.Parse(args[4]);
            }

            if (args.Length >= 6)
            {
                mintrapDist = int.Parse(args[5]);
            }            

            if (args.Length >= 7)
            {
                debug = bool.Parse(args[6]);
            }

            if (args.Length >= 8)
            {
                walkID = int.Parse(args[7]);
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile($"{Directory.GetCurrentDirectory()}\\BaseLevels\\Skill{skillLvl}\\config.ini");
            int maxSpikes = int.Parse(data["Traps"]["Spikes"]);
            for (int i = 0; i < maxSpikes; i++) traps.Add(new Trap("d1", false, true));

            Console.WriteLine("Loading existing routes");
            bool loading = true;
            int checkNum = 1;
            while (loading)
            {
                if (File.Exists($"{Directory.GetCurrentDirectory()}\\maps\\Skill{skillLvl}\\map{checkNum}\\level.txt"))
                {
                    Loadmap(walkID);
                    madeMaps.Add(mapdata);
                    checkNum++;
                }
                else
                {
                    loading = false;
                }
            }
            Console.WriteLine($"{checkNum - 1} routes found");

            if (debug) Console.WriteLine("Loading map");
            Loadmap(walkID);

            if (debug) Console.WriteLine("Checking route");
            List<Point> route = map.mPathFinder.FindPath(StartLocation, EndLocation, jumpHeight);
            if (debug) PrintMap(route);
            if (route == null)
            {
                Console.WriteLine("No initial route found!");
                PrintMap(null);
                Console.ReadLine();
                Environment.Exit(-1);
            }

            bool running = true;
            if (walkID > 0)
            {
                route.Reverse();
                int count = 0;
                while (running)
                {
                    Console.WriteLine($"Printing route {count}");
                    List<Point> tempRoute = route.Take(count).ToList();
                    PrintMap(tempRoute);
                    if (count < route.Count)
                    {
                        System.Threading.Thread.Sleep(100);
                        count++;
                    }
                    else
                    {
                        count = 0;
                        System.Threading.Thread.Sleep(1000);
                    }                    
                    Console.Clear();
                }
                Console.ReadLine();
            }
            else
            {
                if (debug) Console.WriteLine("Placing traps");
                while (running)
                {
                    if (debug) Console.WriteLine("Reloading map");
                    Loadmap(walkID);
                    route = map.mPathFinder.FindPath(StartLocation, EndLocation, jumpHeight);

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
                                        double dist = GetDistance(route[randNum - 1].X, route[randNum - 1].Y, route[randNum].X, route[randNum].Y);
                                        if (dist > mintrapDist || dist < -mintrapDist)
                                        {
                                            dist = GetDistance(route[randNum + 1].X, route[randNum + 1].Y, route[randNum].X, route[randNum].Y);
                                            if (dist > mintrapDist || dist < -mintrapDist)
                                            {
                                                mapdata[(mapHeightY - 1) - route[randNum].Y][route[randNum].X] = trap.Type;
                                                map.SetTile(route[randNum].X, route[randNum].Y, TileType.Deadly);
                                                found = true;
                                            }
                                        }
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
                    }
                    else
                    {
                        if (debug) Console.WriteLine("Route to short");
                        route = null;
                    }

                    // Check if we can still win the map with the added traps
                    route = map.mPathFinder.FindPath(StartLocation, EndLocation, jumpHeight);
                    if (route != null)
                    {
                        bool isnew = true;
                        foreach (String[][] madeMapData in madeMaps)
                        {
                            bool foundDiff = false;
                            for (int y = 0; y < mapHeightY; y++)
                            {
                                StringBuilder mapstring = new StringBuilder();
                                for (int x = 0; x < mapWidthX; x++)
                                {
                                    if (madeMapData[y][x] != mapdata[y][x])
                                    {
                                        foundDiff = true;
                                        break;
                                    }
                                }

                                if (foundDiff) break;
                            }

                            if (!foundDiff)
                            {
                                isnew = false;
                                break;
                            }
                        }

                        if (isnew)
                        {
                            if (debug) PrintMap(route);
                            SaveMap(route);
                            madeMaps.Add(mapdata);
                            if (mapsRequested == mapsmade)
                            {
                                Console.WriteLine("Finished writing maps");
                                running = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Created old/existing map");
                            failedAttempts++;
                            if (failedAttempts >= maxFailedAttempts)
                            {
                                Console.WriteLine("To many failed attempts - shutting down");
                                Environment.Exit(-2);
                            }
                        }
                    }
                }
            }
            Environment.Exit(0);
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
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

        public static void Loadmap(int walkID = 0)
        {
            String[] result = null;
            if (walkID != 0) result = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\maps\\Skill{skillLvl}\\map{walkID}\\level.txt");
            else result = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\BaseLevels\\Skill{skillLvl}\\level.txt");
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
