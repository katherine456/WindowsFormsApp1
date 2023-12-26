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
    public partial class Учебные_материалы_по_компетенциям : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        string _login, _password;
        public static string _dir_of_study;
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        string sql;


        public Учебные_материалы_по_компетенциям()
        {
            InitializeComponent();
            _login = Соответствие_студента_выбранной_вакансии.login;
            _password = Соответствие_студента_выбранной_вакансии.password;
            _dir_of_study = Соответствие_студента_выбранной_вакансии.dir_of_study;
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

        private void вГлавноеМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Главное_меню_студента newForm = new Главное_меню_студента();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void Учебные_материалы_по_компетенциям_Load(object sender, EventArgs e)
        {
            textBox1.Text = _dir_of_study;
            textBox1.ReadOnly = true;
            textBox1.Font = new System.Drawing.Font("ArialBlack", 9, FontStyle.Regular);
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        public class General
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT DISTINCT Г.Название_компетенции_студента
                                            FROM Направления_обучения А, Направления_Дисциплины Б, Дисциплины Д, 
											Дисциплины_Компетенции_студентов В, Компетенции_студентов Г
	                                        WHERE А.Код_направления_обучения = Б.Код_направления_обучения
											AND Б.Код_дисциплины = Д.Код_дисциплины
											AND В.Код_дисциплины = Д.Код_дисциплины
											AND В.Код_компетенции_студента = Г.Код_компетенции_студента
											AND А.Название_направления =  '" + textBox1.Text.ToString() + "'";
                sqlCommand2.Connection = connection;
                List<General> comp_of_dir = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_компетенции_студента"].ToString())
                        };
                        comp_of_dir.Add(coun);
                    }
                }
                comboBox1.DataSource = comp_of_dir;
                connection.Close();

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT DISTINCT Д.Название_дисциплины
                                            FROM Направления_обучения А, Направления_Дисциплины Б, Дисциплины Д, 
											Дисциплины_Компетенции_студентов В, Компетенции_студентов Г
	                                        WHERE А.Код_направления_обучения = Б.Код_направления_обучения
											AND Б.Код_дисциплины = Д.Код_дисциплины
											AND В.Код_дисциплины = Д.Код_дисциплины
											AND В.Код_компетенции_студента = Г.Код_компетенции_студента
											AND Г.Название_компетенции_студента = '" + comboBox1.Text.ToString() + "'";
                sqlCommand2.Connection = connection;
                List<General> directions = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_дисциплины"].ToString())
                        };
                        directions.Add(coun);
                    }
                }
                comboBox2.DataSource = directions;
                connection.Close();

            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sql = @"SELECT Е.Название_учебного_материала, Ж.Название_типа_учебного_материала, Е.Автор_учебного_материала, Е.Год_выпуска, Е.Ссылка_на_источник
                                            FROM Дисциплины Д, 
											Учебные_материалы_по_улучшению_компетенций Е, Типы_учебных_материалов Ж
                                            WHERE
                                            Е.Код_дисциплины = Д.Код_дисциплины
                                            AND Е.Код_типа_учебного_материала = Ж.Код_типа_учебного_материала
                                            AND Д.Название_дисциплины = '" + comboBox2.Text.ToString() + "'";
            print(sql);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Соответствие_студента_выбранной_вакансии newForm = new Соответствие_студента_выбранной_вакансии();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }
    }
}
