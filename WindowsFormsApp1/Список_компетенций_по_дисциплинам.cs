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
    public partial class Список_компетенций_по_дисциплинам : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sql;
        public static string login;
        public static int id_of_direction;
        public static string passwd;

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

        public Список_компетенций_по_дисциплинам()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public class General
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

        private void Список_компетенций_по_дисциплинам_Load(object sender, EventArgs e)
        {
            login = Главное_меню_преподавателя.login_p;
            passwd = Главное_меню_преподавателя.passwd_p;
            id_of_direction = Главное_меню_преподавателя.id_of_dir;


            //определяем список дисциплин преподавателя по направлению
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.CommandText = @"SELECT Д.Название_дисциплины FROM Направления_обучения А, Направления_Дисциплины Б, Преподаватель В, Преподаватель_Дисциплина Г, Дисциплины Д
                                        WHERE А.Код_направления_обучения = Б.Код_направления_обучения
                                         AND В.Код_преподавателя = Г.Код_преподавателя
                                         AND Г.Код_дисциплины = Д.Код_дисциплины
                                         AND Б.Код_дисциплины = Д.Код_дисциплины
                                         AND В.Пароль_преподавателя = '" + passwd + "' AND В.Логин_преподавателя = '" + login + "' " +
                                         "AND А.Код_направления_обучения = " + id_of_direction + "";
                sqlCommand2.Connection = connection;
                List<General> gen = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_дисциплины"].ToString())
                        };
                        gen.Add(coun);
                    }
                }
                comboBox1.DataSource = gen;
                connection.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //определяем компетенции по дисциплине
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sql = @"SELECT А.Тип_компетенции, А.Название_компетенции_студента, А.Описание_компетенции FROM Компетенции_студентов А, Дисциплины Б, Дисциплины_Компетенции_студентов В, 
                                                Направления_обучения Г
                                                WHERE А.Код_компетенции_студента = В.Код_компетенции_студента
                                                 AND  А.Код_направления_обучения = Г.Код_направления_обучения
                                                 AND  Б.Код_дисциплины = В.Код_дисциплины
                                                 AND  Г.Код_направления_обучения = " + id_of_direction + " " +
                                                 "AND Б.Название_дисциплины = '" + comboBox1.Text + "'";
                print(sql);
                connection.Close();
            }
        }
    }
}
