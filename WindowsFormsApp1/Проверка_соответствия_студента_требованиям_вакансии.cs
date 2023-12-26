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
    public partial class Проверка_соответствия_студента_требованиям_вакансии : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";

        public static string log, pass;
        public static string direction_of_study;
        public static string name_of_vac;
        public static int number_of_comp;


        public class Names_of_vacancy
        {
            public string names;
            public override string ToString()
            {
                return string.Format("{0}", names);
            }
        }

        private void Проверка_соответствия_студента_требованиям_вакансии_Load(object sender, EventArgs e)
        {

        }

        public Проверка_соответствия_студента_требованиям_вакансии()
        {
            InitializeComponent();
            log = Главное_меню_студента.log_stud;
            pass = Главное_меню_студента.passwd_stud;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            //определяем ФИО студента
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(); 
                cmd.Connection = connection; 
                cmd.CommandText = "SELECT ФИО_студента FROM Студент WHERE Логин_студента = '" + log +
                                    "' AND Пароль_студента = '" + pass + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader["ФИО_студента"].ToString();
                }
                textBox1.ReadOnly = true;
                textBox1.Font = new System.Drawing.Font("ArialBlack", 9, FontStyle.Regular);
                reader.Close();
                connection.Close();
            }


            //определяем направление студента
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(); SqlCommand cmd_1 = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT Название_направления FROM Студент А, Направления_обучения Б WHERE А.Логин_студента = '" + log +
                                    "' AND А.Пароль_студента = '" + pass + "' AND А.Код_направления_обучения = Б.Код_направления_обучения";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    direction_of_study = reader["Название_направления"].ToString();
                    textBox2.Text = reader["Название_направления"].ToString();
                }
                textBox2.ReadOnly = true;
                textBox2.Font = new System.Drawing.Font("ArialBlack", 9, FontStyle.Regular);
                reader.Close();
                connection.Close();
            }

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //определяем список вакансий по направлению  
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                //выводит список тех вакансий, для который в БД хранится информация по требуемому уровню компетенций вакансий
                sqlCommand2.CommandText = @"SELECT DISTINCT А.Название_вакансии FROM Вакансии А, Направления_обучения Б, Уровни_сформированности_компетенций В 
                                            WHERE А.Код_направления_обучения = Б.Код_направления_обучения
                                            AND Б.Название_направления = '" + direction_of_study + "' AND А.Код_вакансии = В.Код_вакансии";
                sqlCommand2.Connection = connection;
                List<Names_of_vacancy> vacancies = new List<Names_of_vacancy>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        Names_of_vacancy coun = new Names_of_vacancy
                        {
                            names = (dr["Название_вакансии"].ToString())
                        };
                        vacancies.Add(coun);
                    }
                }
                comboBox1.DataSource = vacancies;
                connection.Close();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_of_vac = comboBox1.Text.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //определяем количество компетенций студента в вакансии
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT COUNT(В.Код_направления_обучения) 
                                    FROM Компетенции_студентов В, Направления_обучения Б
                                    WHERE Б.Код_направления_обучения = В.Код_направления_обучения
                                    AND Б.Название_направления = '" + direction_of_study + "'";
                number_of_comp = (int) cmd.ExecuteScalar();
                connection.Close();
            }

            int check;
            //проверяем, просчитаны ли компетенции у студента
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(); SqlCommand cmd_1 = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT COUNT(В.Процент_сформированности_компетенции)
                                        FROM Студент А, Направления_обучения Б, Уровень_сформированности_компетенций_студентов В
                                        WHERE А.Логин_студента = '" + log + @"'
                                        AND А.Пароль_студента = '" + pass + @"'
                                        AND А.Код_направления_обучения = Б.Код_направления_обучения
                                        AND В.Код_направления_обучения = А.Код_направления_обучения
                                        AND В.Код_студента = А.Код_студента";
                check = (int)cmd.ExecuteScalar();
                connection.Close();
            }

            if  (comboBox1.Text == "")
            {
                MessageBox.Show("Для Вашего направления пока нельзя проверить соответствие требованиям вакансий! Приносим свои извинения", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (check == number_of_comp)
            {
                Соответствие_студента_выбранной_вакансии newForm = new Соответствие_студента_выбранной_вакансии();
                Hide();
                newForm.ShowDialog();
                Show();
                this.Close();
            }
            else if ((check > 0) && (check <= number_of_comp))
            {
                MessageBox.Show("Не все Ваши компетенции были просчитаны! К сожалению, дальше нельзя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Ни одна из Ваших компетенций еще не сформирована! Пожалуйста, попробуйте позже.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Главное_меню_студента newForm = new Главное_меню_студента();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        } 
    }
}
