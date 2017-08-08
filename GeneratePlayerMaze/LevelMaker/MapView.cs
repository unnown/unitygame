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

        public MapView()
        {
            InitializeComponent();
            block.Show();

            for (int i = 0; i < 50; i++)
            {                
                DataGridViewImageColumn ic = new DataGridViewImageColumn();
                ic.HeaderText = "";
                ic.Image = block.getList().blocks.Find(bl => bl.BlockID == "01").Image;
                ic.Name = "";
                ic.Width = 20;

                dataGridView1.Columns.Add(ic);
            }
            dataGridView1.Rows.Add(20);
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
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
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
                            map.Append("0 ");
                        }
                    }
                    map.AppendLine();
                }

                File.WriteAllText($"{sf.FileName}", map.ToString());
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
                        String dataCell = mapdata[x][y];
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
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < dataGridView1.Rows.Count; y++)
            {
                for (int x = 0; x < dataGridView1.Columns.Count; x++)
                {
                    DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1[x, y];
                    cell.Value = null;
                    cell.Tag = "";
                }
            }
        }
    }
}
