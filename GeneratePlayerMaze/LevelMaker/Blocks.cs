using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelMaker
{
    public partial class Blocks : Form
    {
        public Blocks()
        {
            InitializeComponent();
        }

        private void block_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Program.block = btn.Tag.ToString();
            Program.img = btn.Image;
        }
    }
}
