using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocks
{
    [System.Serializable]
    public enum TileType
    {
        Empty,
        Block,
        Deadly
    }

    public class Block
    {
        public String Name = "";
        public String BlockID = "";
        public TileType Type = TileType.Block;

        public Bitmap Image = null;

        public Block() { }

        public Block(string name, string blockid, TileType type, Bitmap img = null)
        {
            this.Name = name;
            this.BlockID = blockid;
            this.Type = type;
            this.Image = img;
        }
    }
}
