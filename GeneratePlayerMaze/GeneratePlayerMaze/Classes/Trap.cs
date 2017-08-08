using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePlayerMaze.Classes
{
    public class Trap
    {
        public string Type = "d1";
        public bool Moving = false;
        public bool Ground = true;

        public Trap(string type, bool moving, bool ground)
        {
            this.Type = type;
            this.Moving = moving;
            this.Ground = ground;
        }
    }
}
