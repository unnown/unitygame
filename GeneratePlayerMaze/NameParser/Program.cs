using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TempParser
{
    class Program
    {
        static void Main(string[] args)
        {
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
                            censoredWords.Add(match.ToString().Replace("\"", "").ToLower());
                    }
                }
            }
            Censor censor = new Censor(censoredWords);

            Console.WriteLine("Parsing names {0}:");
            StringBuilder newData = new StringBuilder();
            String[] result = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\usernames.txt");
            int count = 1;
            int correct = 0;
            foreach (string data in result)
            {
                string[] userdata = data.Split('|');
                if (userdata.Length >= 20)
                {
                    string name = userdata[19].ToLower().Trim();
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in name)
                    {
                        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                        {
                            sb.Append(c);
                        }
                    }
                    name = sb.ToString();

                    if (name.Length > 5 && name.Length < 25)
                    {
                        string fancyName = censor.CensorText(name).Trim();
                        foreach (string word in censoredWords)
                        {
                            fancyName = fancyName.Replace(word, "*");
                        }

                        if (!fancyName.Contains("*"))
                        {
                            newData.AppendLine($"{fancyName};");
                            correct++;
                        }
                        Console.WriteLine($"Censored {count} - {result.Length}: {name} to {fancyName}");
                    }
                }                
                count++;
            }

            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\names.txt", newData.ToString());

            Console.WriteLine($"Filtered out {correct} of {result.Length} records");
            Console.ReadLine();
        }
    }
}
