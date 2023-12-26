using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        string sql;
        private void print(string sql) // функция отображения таблицы
        {
            ds.Reset(); // обновление dataGridView1
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                connection.Close();
            }
        }

        private void компетенцииВакансийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*cl = 1;
            sql = "SELECT * FROM Полученный_список_компетенций_по_вакансии";
            print(sql);*/
            /*dataGridView1.RowCount = 5;
            dataGridView1.ColumnCount = 3;*/

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            connect.Open();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

                /*создание столбцов*/
                //1 столбец, текстовый
                DataGridViewTextBoxColumn column00 = new DataGridViewTextBoxColumn();
                column00.Name = "ПК";
                column00.HeaderText = "Выполнять Строительно-монтажные работы";
                DataGridViewTextBoxColumn column0 = new DataGridViewTextBoxColumn();
                column0.Name = "ПК1";
                column0.HeaderText = "ПК1";
                //2 столбец, текстовый
                DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
                column1.Name = "ПК2";
                column1.HeaderText = "ПК2";
                //3 столбец, изображение
                DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
                column2.Name = "ПК3";
                column2.HeaderText = "ПК3";
                //добавляем столбцы
                DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
                column3.Name = "ПК4";
                column3.HeaderText = "ПК4";
                dataGridView1.Columns.AddRange(column00, column0, column1, column2, column3);

                /*создание ячеек*/
                //ячейки для 1 строки
                DataGridViewCell id0 = new DataGridViewTextBoxCell();
                DataGridViewCell id1 = new DataGridViewTextBoxCell();
                DataGridViewCell id2 = new DataGridViewTextBoxCell();
                DataGridViewCell id3 = new DataGridViewTextBoxCell();
                DataGridViewCell id4 = new DataGridViewTextBoxCell();
                id0.Value = "ПК1";
                id1.Value = "1";
                id2.Value = "1";
                id3.Value = "0.5";
                id4.Value = "0.143";
                DataGridViewRow row0 = new DataGridViewRow();
                row0.Cells.AddRange(id0, id1, id2, id3, id4);
                //добавление строки в таблицу
                dataGridView1.Rows.Add(row0);

                DataGridViewCell id00 = new DataGridViewTextBoxCell();
                DataGridViewCell id10 = new DataGridViewTextBoxCell();
                DataGridViewCell id20 = new DataGridViewTextBoxCell();
                DataGridViewCell id30 = new DataGridViewTextBoxCell();
                DataGridViewCell id40 = new DataGridViewTextBoxCell();
                id00.Value = "ПК2";
                id10.Value = "1";
                id20.Value = "1";
                id30.Value = "0.143";
                id40.Value = "0.125";
                DataGridViewRow row00 = new DataGridViewRow();
                row00.Cells.AddRange(id00, id10, id20, id30, id40);
                //добавление строки в таблицу
                dataGridView1.Rows.Add(row00);

                DataGridViewCell id000 = new DataGridViewTextBoxCell();
                DataGridViewCell id100 = new DataGridViewTextBoxCell();
                DataGridViewCell id200 = new DataGridViewTextBoxCell();
                DataGridViewCell id300 = new DataGridViewTextBoxCell();
                DataGridViewCell id400 = new DataGridViewTextBoxCell();
                id000.Value = "ПК3";
                id100.Value = "2";
                id200.Value = "7";
                id300.Value = "1";
                id400.Value = "1";
                DataGridViewRow row000 = new DataGridViewRow();
                row000.Cells.AddRange(id000, id100, id200, id300, id400);
                //добавление строки в таблицу
                dataGridView1.Rows.Add(row000);

                DataGridViewCell id0000 = new DataGridViewTextBoxCell();
                DataGridViewCell id1000 = new DataGridViewTextBoxCell();
                DataGridViewCell id2000 = new DataGridViewTextBoxCell();
                DataGridViewCell id3000 = new DataGridViewTextBoxCell();
                DataGridViewCell id4000 = new DataGridViewTextBoxCell();
                id0000.Value = "ПК4";
                id1000.Value = "7";
                id2000.Value = "8";
                id3000.Value = "1";
                id4000.Value = "1";
                DataGridViewRow row0000 = new DataGridViewRow();
                row0000.Cells.AddRange(id0000, id1000, id2000, id3000, id4000);
                //добавление строки в таблицу
                dataGridView1.Rows.Add(row0000);

        }

        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            comboBox1.Items.Clear();
            string[] nums1 = new string[] { "Название вакансии" };
            comboBox1.Items.AddRange(nums1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sql = "SELECT * FROM ЦЦ";
            print(sql);

            /*dataGridView1.RowCount = 5;
            dataGridView1.ColumnCount = 3;*/
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sql = "SELECT * FROM Вакансии";
            print(sql);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
