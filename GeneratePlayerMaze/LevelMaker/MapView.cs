using Blocks;
using Routing.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelMaker
{
    public partial class MapView : Form
    {
        Blocks block = new LevelMaker.Blocks();
        BlockList blockList = new BlockList();
        List<Point> route = null;

        Block start = null;
        Block finish = null;
        Block blank = null;
        Bitmap imgRoute = null;
        static short jumpHeight = 2;

        public MapView()
        {
            InitializeComponent();
            block.Show();

            start = blockList.blocks.Find(bl => bl.Name == "start");
            finish = blockList.blocks.Find(bl => bl.Name == "finish");
            blank = blockList.blocks.Find(bl => bl.Name == "blank");
            imgRoute = (Bitmap)Image.FromFile($"{Directory.GetCurrentDirectory()}\\Resources\\route.png");

            for (int i = 0; i < 50; i++)
            {
                DataGridViewImageColumn ic = new DataGridViewImageColumn();
                ic.HeaderText = "";
                ic.Image = blank.Image;
                ic.Name = "";
                ic.Tag = blank.BlockID;
                ic.Width = 35;

                dataGridView1.Columns.Add(ic);
            }
            dataGridView1.RowTemplate.Height = 35;
            dataGridView1.Rows.Add(20);

            this.Width = CountGridWidth(dataGridView1);
            this.Height = CountGridHeight(dataGridView1);
        }

        public static int CountGridWidth(DataGridView dgv)
        {
            int width = 0;
            foreach (DataGridViewColumn column in dgv.Columns) width += column.Width;
            return width += 100;
        }

        public static int CountGridHeight(DataGridView dgv)
        {
            int height = 0;
            foreach (DataGridViewRow row in dgv.Rows) height += row.Height;
            return height += 100;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            updateCells();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            updateCells();
        }

        private void updateCells()
        {
            for (int y = 0; y < dataGridView1.Rows.Count; y++)
            {
                for (int x = 0; x < dataGridView1.Columns.Count; x++)
                {
                    DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1[x, y];
                    if (cell.Selected)
                    {
                        cell.Value = Program.img;
                        cell.Tag = Program.block;
                    }
                    else if (cell.Value == imgRoute)
                    {
                        cell.Value = blank.Image;
                    }
                }
            }
            checkRoute();
        }

        private void checkRoute()
        {
            Point StartLocation = new Point();
            Point EndLocation = new Point();

            Map map = new Map(dataGridView1.Columns.Count, dataGridView1.Rows.Count);
            for (int y = 0; y < dataGridView1.Rows.Count; y++)
            {
                for (int x = 0; x < dataGridView1.Columns.Count; x++)
                {
                    DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1[x, y];
                    if (cell.Tag != null && !String.IsNullOrEmpty(cell.Tag.ToString()))
                    {
                        foreach (Block block in blockList.blocks)
                        {
                            if (block.BlockID == cell.Tag.ToString())
                            {
                                map.SetTile(x, (dataGridView1.Rows.Count - 1) - y, block.Type);
                            }
                        }

                        if (cell.Tag.ToString() == start.BlockID) StartLocation = new Point(x, (dataGridView1.Rows.Count - 1) - y);
                        if (cell.Tag.ToString() == finish.BlockID) EndLocation = new Point(x, (dataGridView1.Rows.Count - 1) - y);
                    }
                    else
                    {
                        map.SetTile(x, (dataGridView1.Rows.Count - 1) - y, blank.Type);
                    }
                }
            }

            if (!StartLocation.IsEmpty && !EndLocation.IsEmpty)
            {
                route = map.mPathFinder.FindPath(StartLocation, EndLocation, jumpHeight);
                if (route == null) this.Text = "Map gen - No route found!";
                else
                {
                    this.Text = "Map gen";

                    route.Reverse();
                    if (route.Count > 2)
                    {
                        route.RemoveAt(0);
                        route.RemoveAt(route.Count - 1);
                    }

                    foreach (Point point in route)
                    {
                        DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1[point.X, (dataGridView1.Rows.Count - 1) - point.Y];
                        cell.Value = imgRoute;
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (route == null)
            {
                MessageBox.Show("No possible route found, saving disabled");
            }
            else
            {
                SaveFileDialog sf = new SaveFileDialog();
                // Feed the dummy name to the save dialog
                sf.FileName = "level";
                sf.InitialDirectory = Directory.GetCurrentDirectory();
                sf.DefaultExt = ".txt";

                if (sf.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder map = new StringBuilder();

                    for (int y = 0; y < dataGridView1.Rows.Count; y++)
                    {
                        for (int x = 0; x < dataGridView1.Columns.Count; x++)
                        {
                            DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1[x, y];
                            if (cell.Tag != null && !String.IsNullOrEmpty(cell.Tag.ToString()))
                            {
                                map.Append($"{cell.Tag.ToString() } ");
                            }
                            else
                            {
                                map.Append($"{blank.BlockID} ");
                            }
                        }
                        map.AppendLine();
                    }

                    File.WriteAllText($"{sf.FileName}", map.ToString());
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.FileName = "level";
            of.InitialDirectory = Directory.GetCurrentDirectory();
            of.DefaultExt = ".txt";

            if (of.ShowDialog() == DialogResult.OK)
            {
                String[] result = File.ReadAllLines(of.FileName);
                String[][] mapdata = new String[result.Length][];

                for (int i = 0; i < result.Length; i++)
                {
                    mapdata[i] = result[i].Split(' ');
                }

                for (int y = 0; y < dataGridView1.Rows.Count; y++)
                {
                    for (int x = 0; x < dataGridView1.Columns.Count; x++)
                    {
                        DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1[x, y];
                        String dataCell = mapdata[y][x];
                        if (!dataCell.Contains("0"))
                        {
                            foreach (Control ctrl in block.Controls)
                            {
                                if (ctrl.GetType() == typeof(Button))
                                {
                                    Button btn = (Button)ctrl;
                                    if (btn.Tag.ToString() == dataCell)
                                    {
                                        cell.Value = btn.Image;
                                        cell.Tag = btn.Tag;
                                    }
                                }
                            }
                        }
                    }
                }
                checkRoute();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < dataGridView1.Rows.Count; y++)
            {
                for (int x = 0; x < dataGridView1.Columns.Count; x++)
                {
                    DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1[x, y];
                    cell.Value = block.getList().blocks.Find(bl => bl.BlockID == "01").Image;
                    cell.Tag = "01";
                }
            }
        }
    }
}
