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
    public partial class Регистрация_студента : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        DataSet db = new DataSet();
        string query;
        SqlCommand insert = new SqlCommand();

        public Регистрация_студента()
        {
            InitializeComponent();
        }

        public class Descr
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

        private void Регистрация_студента_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //ПОКАЗ НАПРАВЛЕНИЙ ОБУЧЕНИЯ В comboBox1
            SqlConnection connect = new SqlConnection(connectionString);
            comboBox1.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT Название_направления FROM Направления_обучения, Вакансии
                                                WHERE Вакансии.Код_направления_обучения = Направления_обучения.Код_направления_обучения";
                sqlCommand2.Connection = connection;
                List<Descr> comp_of_vacancy = new List<Descr>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        Descr coun = new Descr
                        {
                            description = (dr["Название_направления"].ToString())
                        };
                        comp_of_vacancy.Add(coun);
                    }
                }
                comboBox1.DataSource = comp_of_vacancy;
                connection.Close();
            }

            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            for (int i = 0; i < 4; i++)
            {
                comboBox2.Items.Add(i + 1);
                if (i == 0)
                {
                    comboBox2.SelectedItem = (1);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            //Получить случайное число
            int value = rnd.Next(0, 1000000);
            int id_of_dir;

            object result;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                cmd.CommandText = @"SELECT Код_направления_обучения FROM Направления_обучения 
                                    WHERE Название_направления = '" + comboBox1.Text.ToString() + "'";
                result = cmd.ExecuteScalar();
                id_of_dir = Convert.ToInt32(result);
                connection.Close();
            }

            query = @"INSERT INTO Студент(Код_студента, Код_направления_обучения, ФИО_студента, Курс, Факультет, Номер_группы, Логин_студента, Пароль_студента) VALUES (" +
                                    "@Код_студента, @Код_направления_обучения, @ФИО_студента, @Курс, " +
                                    "@Факультет, @Номер_группы, @Логин_студента, @Пароль_студента)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                insert = new SqlCommand(query, connection);
                connection.Open();
                {
                    insert.Parameters.Clear();
                    insert.Parameters.AddWithValue("@Код_студента", value);
                    insert.Parameters.AddWithValue("@Код_направления_обучения", id_of_dir);
                    insert.Parameters.AddWithValue("@ФИО_студента", textBox1.Text.ToString());
                    insert.Parameters.AddWithValue("@Курс", Convert.ToInt32(comboBox2.Text.ToString()));
                    insert.Parameters.AddWithValue("@Факультет", textBox6.Text.ToString());
                    insert.Parameters.AddWithValue("@Номер_группы", textBox5.Text.ToString());
                    insert.Parameters.AddWithValue("@Логин_студента", textBox3.Text.ToString());
                    insert.Parameters.AddWithValue("@Пароль_студента", textBox4.Text.ToString());
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Регистрация прошла успешно!", "Внимание!", MessageBoxButtons.OK);
                }
                connection.Close();
            }
        }
    }
}
