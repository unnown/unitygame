using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocks
{
    public class BlockList
    {
        public List<Block> blocks = new List<Block>();

        public BlockList()
        {
            StreamReader FileReader = new StreamReader($"{Directory.GetCurrentDirectory()}\\Resources\\Blocks.txt");
            string[] FileContents = FileReader.ReadToEnd().Split('\n');

            foreach (string str in FileContents)
            {
                string[] param = str.Split(';');
                String name = param[0].Replace("\r", "");
                String blockID = param[1].Replace("\r", "");
                TileType type = (TileType)int.Parse(param[2].Replace("\r", ""));
                Bitmap img = (Bitmap)Image.FromFile($"{Directory.GetCurrentDirectory()}\\Resources\\{name}.png");
                blocks.Add(new Blocks.Block(name, blockID, type, img));
            }
        }

    }
}
