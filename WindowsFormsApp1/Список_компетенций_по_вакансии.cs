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
    public partial class Список_компетенций_по_вакансии : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        string direct_of_study;
        SqlDataAdapter adapter;
        string sql;
        DataSet ds = new DataSet();

        public Список_компетенций_по_вакансии()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            direct_of_study = Соответствие_студента_выбранной_вакансии.dir_of_study;
        }

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
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                connection.Close();
            }
        }

        private void Список_компетенций_по_вакансии_Load(object sender, EventArgs e)
        {
            textBox1.Text = direct_of_study;
            textBox1.ReadOnly = true;
            textBox1.Font = new System.Drawing.Font("ArialBlack", 9, FontStyle.Regular);

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            sql = @"SELECT Б.Название_компетенции_студента, Б.Описание_компетенции 
                                    FROM Направления_обучения А, Компетенции_студентов Б
                                    WHERE А.Код_направления_обучения = Б.Код_направления_обучения
                                    AND А.Название_направления = '" + direct_of_study + "'";
            print(sql);

        }
    }
}
