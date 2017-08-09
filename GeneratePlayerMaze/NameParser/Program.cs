using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TempParser
{
    class Program
    {
        static Censor censor = null;
        static List<String> correctNames = new List<string>();
        static int correct = 0;
        static int incorrect = 0;
        static int filtered = 0;
        static Dictionary<String, String> paterns = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Parsing bad words!");            

            IList<string> censoredWords = new List<string>();
            string[] files = Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Resources\\");
            foreach (string filename in files)
            {
                String[] badwords = File.ReadAllLines(filename);
                foreach (string word in badwords)
                {
                    if (word.Contains("$  ="))
                    {
                        foreach (Match match in Regex.Matches(word, "\"([^\"]*)\""))
                        {
                            string censorword = match.ToString().Replace("\"", "").ToLower();
                            if (!censorword.Contains(" ")) censoredWords.Add(censorword);
                        }
                    }
                }
            }

            files = Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Urban\\");
            foreach (string filename in files)
            {
                String[] badwords = File.ReadAllLines(filename);
                foreach (string word in badwords)
                {
                    if (!word.Contains(" ")) censoredWords.Add(word);
                }
            }
            censor = new Censor(censoredWords);

            StringBuilder newData = new StringBuilder();
            List<String> result = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\usernames.txt").ToList();
            result.AddRange(File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\mcNames.txt").ToList());

            foreach (string word in censor.CensoredWords)
            {
                string regularExpression = Censor.ToRegexPattern(word);
                if (!paterns.ContainsKey(word)) paterns.Add(word, regularExpression);
            }
            Console.Write("Found ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(paterns.Keys.Count);
            Console.ResetColor();
            Console.WriteLine(" badwords!");

            bool running = true;
            int limitFilter = 0;
            int lastval = -1;
            while (running)
            {
                if (filtered >= limitFilter)
                {
                    limitFilter += 10;
                    for (int i = filtered; i <= limitFilter; i++)
                    {
                        parseName(result[i]);
                    }
                }

                if (lastval != filtered)
                {
                    Console.Write($"Filtered out {filtered} of {result.Count} records (");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{incorrect} incorrect");
                    Console.ResetColor();
                    Console.Write(" - ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{correct} correct");
                    Console.ResetColor();
                    Console.WriteLine(")");

//                    Console.WriteLine($"Filtered out {filtered} of {result.Count} records ({incorrect} incorrect - {correct} correct)");
                    lastval = filtered;
                }
                System.Threading.Thread.Sleep(100);
                if (filtered >= result.Count) running = false;
            }

            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\names.txt", newData.ToString());
        }

        private async static void parseName(string data)
        {
            await Task.Run(() => {
                string name = "";

                if (data.Contains("|"))
                {
                    string[] userdata = data.Split('|');
                    if (userdata.Length >= 20)
                    {
                        name = userdata[19].ToLower().Trim();
                    }
                }
                else
                {
                    name = data;
                }

                StringBuilder sb = new StringBuilder();
                foreach (char c in name)
                {
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    {
                        sb.Append(c);
                    }
                }
                name = sb.ToString();

                if (name.Length >= 5 && name.Length <= 25)
                {
                    string fancyName = name.Trim();
                    foreach (string word in censor.CensoredWords)
                    {
                        if (fancyName.Contains("word")) fancyName = fancyName.Replace(word, "*");
                        else
                        {
                            string regularExpression = "";
                            paterns.TryGetValue(word, out regularExpression);
                            if (!String.IsNullOrEmpty(regularExpression))
                            {
                                fancyName = Regex.Replace(fancyName, regularExpression, Censor.StarCensoredMatch,
                                  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                            }
                        }                        
                    }

                    if (!fancyName.Contains("*"))
                    {
                        lock (correctNames)
                        {
                            correctNames.Add(name);
                        }

                        Interlocked.Increment(ref correct);
                    } else
                    {
                        Interlocked.Increment(ref incorrect);
                    }
                } else
                {
                    Interlocked.Increment(ref incorrect);
                }

                Interlocked.Increment(ref filtered);
                return;
            });
        }
    }
}
