using Blocks;
using LevelMaker.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelMaker
{
    public partial class Blocks : Form
    {
        public static BlockList blockList = new BlockList();

        public BlockList getList() { return blockList; }

        public Blocks()
        {
            InitializeComponent();
            int x = 0;
            int y = 0;

            foreach (Block block in blockList.blocks) 
            {
                Button btn = new Button();
                btn.Height = 70;
                btn.Width = 70;
                btn.Location = new Point(12 + (y * 75), 12 + (x * 75));
                btn.Visible = true;
                btn.Name = block.Name;
                btn.Image = block.Image;
                btn.Tag = block.BlockID;
                btn.Click += Btn_Click;
                btn.Refresh();

                this.Controls.Add(btn);
                y += 1;
                if (y >= 4)
                {
                    y = 0;
                    x++;
                }
            }

            defaultbtn.Visible = false;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType() == typeof(Button))
                {
                    Button butt = (Button)ctrl;
                    butt.Refresh();
                }
            }
            this.Refresh();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Program.block = btn.Tag.ToString();
            Program.img = btn.Image;
        }

        public static Object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] info = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in info)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(p, pi.GetValue(o, null), null);
                }
            }
            return p;
        }
    }
}
