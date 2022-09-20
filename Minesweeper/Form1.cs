using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataGridView dg;
        public int[,] a;
        bool[,] b;
        int c;
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Show();
            label1.Show();
            dg = new DataGridView();
            dg.ColumnCount = 20;
            dg.RowCount = 20;
            for (int i = 0; i < dg.RowCount; i++)
            {
                dg.Rows[i].Height = 20;
            }
            for (int i = 0; i < dg.ColumnCount; i++)
            {
                dg.Columns[i].Width = 20;
            }
            for (int i = 0; i < dg.RowCount; i++)
            {
                for (int j = 0; j < dg.ColumnCount; j++)
                {
                    dg[j, i].Style.BackColor = Color.DarkCyan;
                }
            }
            dg.ColumnHeadersVisible = false;
            dg.RowHeadersVisible = false;
            dg.Height = dg.RowCount * 20 + 3;
            dg.Width = dg.ColumnCount * 20 + 3;
            dg.Top = 20;
            dg.Left = 50;
            DataGridViewCellStyle st = new DataGridViewCellStyle();
            st.SelectionBackColor = Color.Transparent;
            dg.DefaultCellStyle = st;
            dg.AllowUserToDeleteRows = false;
            dg.AllowUserToResizeColumns = false;
            dg.AllowUserToResizeRows = false;
            dg.AllowUserToOrderColumns = false;
            dg.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg.Cursor = Cursors.Hand;
            dg.MultiSelect = false;
            dg.CellMouseClick += dg_CellMouseClick;
            Controls.Add(dg);

            b = new bool[dg.RowCount, dg.ColumnCount];

            a = new int[dg.RowCount, dg.ColumnCount];
            Random r = new Random();
            int mines = Convert.ToInt32(numericUpDown1.Value);
            for (int i = 0; i < mines; i++)
            {
                int z = r.Next(dg.RowCount * dg.ColumnCount);
                int x = z / dg.ColumnCount, y = z % dg.ColumnCount;
                if (a[x, y] == -1)
                    i--;
                else
                {
                    a[x, y] = -1;
                }
            }
            for (int i = 0; i < dg.RowCount; i++)
            {
                for (int j = 0; j < dg.ColumnCount; j++)
                {
                    FillAround(i, j);
                }
            }
            textBox1.Text = numericUpDown1.Value.ToString();
            c = Convert.ToInt32(numericUpDown1.Value);
            Controls.Remove(button1);
            Controls.Remove(numericUpDown1);

        }
        private void FillAround(int i, int j)
        {
            if (a[i, j] == -1) return;
            if (i != 0 && j != 0 && a[i - 1, j - 1] == -1) a[i, j]++;
            if (i != 0 && j != dg.ColumnCount - 1 && a[i - 1, j + 1] == -1) a[i, j]++;
            if (i != dg.RowCount - 1 && j != 0 && a[i + 1, j - 1] == -1) a[i, j]++;
            if (i != dg.RowCount - 1 && j != dg.ColumnCount - 1 && a[i + 1, j + 1] == -1) a[i, j]++;
            if (i != 0 && a[i - 1, j] == -1) a[i, j]++;
            if (i != dg.RowCount - 1 && a[i + 1, j] == -1) a[i, j]++;
            if (j != 0 && a[i, j - 1] == -1) a[i, j]++;
            if (j != dg.ColumnCount - 1 && a[i, j + 1] == -1) a[i, j]++;
        }
        private void dg_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int x = e.RowIndex;
            int y = e.ColumnIndex;
            if (e.Button.Equals(MouseButtons.Left))
            {
                if (dg[y, x].Style.BackColor.Equals(Color.Red)) return;
                if (a[x, y] == 0) { ShowIsland(x, y); return; }
                if (a[x, y] == -1) { ShowMines(); return; }
                ShowCell(x, y); return;
            }
            else
            {
                if (dg[y, x].Style.BackColor.Equals(Color.Red))
                {
                    dg[y, x].Style.BackColor = Color.DarkCyan;
                    textBox1.Text = (Convert.ToInt32(textBox1.Text) + 1).ToString();
                    if (a[x, y] == -1) c++;
                }
                else if (dg[y, x].Style.BackColor.Equals(Color.DarkCyan))
                {
                    dg[y, x].Style.BackColor = Color.Red;
                    textBox1.Text = (Convert.ToInt32(textBox1.Text) - 1).ToString();
                    if (a[x, y] == -1) c--;
                }
                if (c == 0)
                {
                    MessageBox.Show("Congrats! You won!");
                    Application.Exit();
                }
            }
        }
        public void ShowCell(int x, int y)
        {
            dg[y, x].Style.BackColor = Color.Beige;
            if (a[x, y] != 0)
                dg[y, x].Value = a[x, y];
            dg[y, x].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        public void ShowIsland(int i, int j)
        {
            if (b[i,j]) return;
            ShowCell(i, j);
            b[i, j] = true;
            if (a[i, j] == 0)
            {
                if (i != 0 && j != 0) ShowIsland(i - 1, j - 1);
                if (i != 0 && j != dg.ColumnCount - 1) ShowIsland(i - 1, j + 1);
                if (i != dg.RowCount - 1 && j != 0) ShowIsland(i + 1, j - 1);
                if (i != dg.RowCount - 1 && j != dg.ColumnCount - 1) ShowIsland(i + 1, j + 1);
                if (i != 0) ShowIsland(i - 1, j);
                if (i != dg.RowCount - 1) ShowIsland(i + 1, j);
                if (j != 0) ShowIsland(i, j - 1);
                if (j != dg.ColumnCount - 1) ShowIsland(i, j + 1);
            }
        }
        public void ShowMines()
        {
            for (int i = 0; i < dg.RowCount; i++)
            {
                for (int j = 0; j < dg.ColumnCount; j++)
                {
                    if (a[i, j] == -1)
                    {
                        dg[j, i].Style.BackColor = Color.Coral;
                    }
                }
            }
            MessageBox.Show("You Lost");
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Hide();
            label1.Hide();
            this.Text = "MineSweeper";
        }
    }
}
