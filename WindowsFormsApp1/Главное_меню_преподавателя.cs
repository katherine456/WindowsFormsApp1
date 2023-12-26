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
    public partial class Главное_меню_преподавателя : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand insert = new SqlCommand();
        SqlCommand delete = new SqlCommand();
        SqlCommand update = new SqlCommand();
        string sql;
        int cl = 0;
        public static string login_p;
        public static string passwd_p;
        public static int id_of_dir;
        public static string accessed = "0";
        int id_of_student, id_of_cos, id_of_disc, id_of_teacher;
        List<ID_of_competitions_of_disc> comp_of_disc = new List<ID_of_competitions_of_disc>();
        int max_ball;

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
                if (cl == 5)
                  dataGridView1.Columns[0].Visible = false;
                connection.Close();
            }
        }

        public Главное_меню_преподавателя()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public class General
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

        private void Главное_меню_преподавателя_Load(object sender, EventArgs e)
        {
            login_p = Вход_в_систему.loginUser;
            passwd_p = Вход_в_систему.passwordUser;
            accessed = "Te";
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            //определяем ID преподавателя
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Код_преподавателя FROM Преподаватель WHERE Логин_преподавателя = '" + login_p +
                                    "' AND Пароль_преподавателя = '" + passwd_p + "'";
                id_of_teacher = (int)cmd.ExecuteScalar();
                textBox7.Text = id_of_teacher.ToString();
                textBox7.ReadOnly = true;
                connection.Close();
            }

            //список направлений обучения
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT Название_направления FROM Направления_обучения";

                List<General> gen = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_направления"].ToString())
                        };
                        gen.Add(coun);
                    }
                }
                comboBox7.DataSource = gen;
                connection.Close();
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void данныеОСтудентахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 1;
            comboBox1.Items.Clear();
            string[] nums1 = new string[] { "Код_студента", "ФИО_студента", "Курс", "Факультет", "Номер_группы",
                "Логин_студента", "Пароль_студента" };
            comboBox1.Items.AddRange(nums1);
            sql = "SELECT * FROM Студент";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void данныеОПреподавателяхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 2;
            comboBox1.Items.Clear();
            string[] nums2 = new string[] { "Код_преподавателя", "ФИО_преподавателя", "E_mail", "Номер_телефона",
                "Логин_преподавателя", "Пароль_преподавателя" };
            comboBox1.Items.AddRange(nums2);
            sql = "SELECT * FROM Преподаватель";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void списокКОСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 4;
            comboBox1.Items.Clear();
            string[] nums4 = new string[] { "Код_КОСа", "Тип_КОС", "Макс_баллы_КОСа" };
            comboBox1.Items.AddRange(nums4);
            sql = "SELECT * FROM КОС";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void списокДисциплинToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 3;
            comboBox1.Items.Clear();
            string[] nums3 = new string[] { "Код_дисциплины", "Курс", "Семестр", "Название_дисциплины" };
            comboBox1.Items.AddRange(nums3);
            sql = "SELECT * FROM Дисциплины";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void списокНаправленийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 9;
            comboBox1.Items.Clear();
            string[] nums9 = new string[] { "Код_направления_обучения", "Название_направления" };
            comboBox1.Items.AddRange(nums9);
            sql = "SELECT * FROM Направления_обучения";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void данныеОКомпетенцияхСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 8;
            comboBox1.Items.Clear();
            string[] nums8 = new string[] { "Код_компетенции_студента", "Код_направления_обучения", "Тип_компетенции",
                                            "Название_компетенции_студента", "Описание_компетенции" };
            comboBox1.Items.AddRange(nums8);
            sql = "SELECT * FROM Компетенции_студентов";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void данныеОБаллахСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 5;
            comboBox1.Items.Clear();
            string[] nums5 = new string[] { "Код_записи", "Код_студента", "Код_дисциплины", "Код_компетенции_студента", "Код_КОСа", "Код_преподавателя",
                                            "Количество_набранных_баллов", "@Дата_записи"};
            comboBox1.Items.AddRange(nums5);
            sql = "SELECT Код_записи, Код_студента, Код_дисциплины, Код_компетенции_студента, Код_КОСа, Код_преподавателя, Количество_набранных_баллов, Дата_записи FROM Баллы_за_учебную_деятельность WHERE Код_преподавателя = " + id_of_teacher;
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void данныеОбУровняхРазвитияКомпетенцийСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 6;
            comboBox1.Items.Clear();
            string[] nums6 = new string[] { "Код_студента", "Код_компетенции_студента", "Код_направления_обучения", "Процент_сформированности_компетенции" };
            comboBox1.Items.AddRange(nums6);
            sql = "SELECT * FROM Уровень_сформированности_компетенций_студентов";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void списокУчебныхМатериаловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 7;
            comboBox1.Items.Clear();
            sql = @"SELECT Д.Название_дисциплины, Е.Название_учебного_материала, Ж.Название_типа_учебного_материала, Е.Автор_учебного_материала, Е.Год_выпуска, Е.Ссылка_на_источник
                                            FROM Дисциплины Д, 
											Учебные_материалы_по_улучшению_компетенций Е, Типы_учебных_материалов Ж, 
											Преподаватель А, Преподаватель_Дисциплина Б
                                            WHERE
                                            Е.Код_дисциплины = Д.Код_дисциплины
                                            AND Е.Код_типа_учебного_материала = Ж.Код_типа_учебного_материала
											AND А.Код_преподавателя = Б.Код_преподавателя
											AND Б.Код_дисциплины = Д.Код_дисциплины
											AND А.Логин_преподавателя = '" + login_p + "'" +
											" AND А.Пароль_преподавателя = '" + passwd_p +"'";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void справочникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Справочники newForm = new Справочники();
            newForm.ShowDialog();
            Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа разработана студенткой группы ИИ-962 Хазанской Е. С. для определения уровня сформированности компетенций студентов" +
                " и анализа соответствия компетенций студента требованиям вакансий");
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Вход_в_систему newForm = new Вход_в_систему();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        public class ID_of_competitions_of_disc
        {
            public int id_of_comp;
            public override string ToString()
            {
                return string.Format("{0}", id_of_comp);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            //определяем айди дисциплины
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT Код_дисциплины FROM Дисциплины
                                    WHERE 
                                    Название_дисциплины = '" + comboBox2.Text + "'";
                id_of_disc = (int)cmd.ExecuteScalar();
                connection.Close();
            }

            //определяем косы по дисциплине
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT Тип_КОС FROM КОС";
                sqlCommand2.Connection = connection;

                List<General> gen = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Тип_КОС"].ToString())
                        };
                        gen.Add(coun);
                    }
                }
                comboBox4.DataSource = gen;
                connection.Close();
            }

            //определяем компетенции по дисциплине
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                comp_of_disc.Clear();
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT А.Код_компетенции_студента FROM Компетенции_студентов А, Дисциплины Б, Дисциплины_Компетенции_студентов В, 
                                                Направления_обучения Г
                                                WHERE А.Код_компетенции_студента = В.Код_компетенции_студента
                                                 AND  А.Код_направления_обучения = Г.Код_направления_обучения
                                                 AND  Б.Код_дисциплины = В.Код_дисциплины
                                                 AND  Г.Код_направления_обучения = " + id_of_dir + " " +
                                                 "AND В.Код_дисциплины = " + id_of_disc + "";
                sqlCommand2.Connection = connection;
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        ID_of_competitions_of_disc coun = new ID_of_competitions_of_disc
                        {
                            id_of_comp = (Convert.ToInt32(dr["Код_компетенции_студента"]))
                        };
                        comp_of_disc.Add(coun);
                    }
                }
                connection.Close();
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            //определяем айди направления
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT Код_направления_обучения FROM Направления_обучения
                                    WHERE 
                                    Название_направления = '" + comboBox7.Text + "'";
                id_of_dir = (int)cmd.ExecuteScalar();
                connection.Close();
            }

            //определяем группы по направлению
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT DISTINCT Номер_группы FROM Студент WHERE Код_направления_обучения = " + id_of_dir;
                sqlCommand2.Connection = connection;

                List<General> gen = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Номер_группы"].ToString().Replace(" ", ""))
                        };
                        gen.Add(coun);
                    }
                }
                comboBox5.DataSource = gen;
                connection.Close();
            }

            //определяем список дисциплин преподавателя по направлению
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT Д.Название_дисциплины FROM Направления_обучения А, Направления_Дисциплины Б, Преподаватель В, Преподаватель_Дисциплина Г, Дисциплины Д
                                        WHERE А.Код_направления_обучения = Б.Код_направления_обучения
                                         AND В.Код_преподавателя = Г.Код_преподавателя
                                         AND Г.Код_дисциплины = Д.Код_дисциплины
                                         AND Б.Код_дисциплины = Д.Код_дисциплины
                                         AND В.Код_преподавателя = " + id_of_teacher + " " +
                                         "AND А.Код_направления_обучения = " + id_of_dir + "";
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
                comboBox2.DataSource = gen;
                connection.Close();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //определяем айди  Коса
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT Код_КОСа FROM КОС
                                    WHERE 
                                    Тип_КОС = '" + comboBox4.Text + "'";
                id_of_cos = (int)cmd.ExecuteScalar();
                connection.Close();
            }

            //определяем макс балл  Коса
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT Макс_баллы_КОСа FROM КОС
                                    WHERE 
                                    Код_КОСа = " + id_of_cos + " ";
                max_ball = (int) cmd.ExecuteScalar();
                textBox6.Text = max_ball.ToString();
                textBox6.ReadOnly = true;
                connection.Close();
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //определяем студентов
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT ФИО_студента FROM Студент WHERE Код_направления_обучения = " + id_of_dir 
                    + " AND Номер_группы = '" + comboBox5.Text + "'";
                sqlCommand2.Connection = connection;

                List<General> gen = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["ФИО_студента"].ToString())
                        };
                        gen.Add(coun);
                    }
                }
                comboBox6.DataSource = gen;
                connection.Close();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //Редактировать таблицу баллов
        {
            Окно_редактирования_баллов_преподавателя newForm = new Окно_редактирования_баллов_преподавателя();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Добавление_учебных_материалов newForm = new Добавление_учебных_материалов();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Удаление_учебного_материала newForm = new Удаление_учебного_материала();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Обновление_учебного_материала newForm = new Обновление_учебного_материала();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) //Добавить оценку
        {
            DateTime dateTime = DateTime.Now;
            string format = "dd.MM.yyyy";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {             
                float numericValue;
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                if (float.TryParse(textBox2.Text.ToString(), out numericValue) && float.Parse(textBox2.Text.ToString()) >= 0 
                    && float.Parse(textBox2.Text.ToString()) <= float.Parse(textBox6.Text.ToString()))
                {
                    for (int i = 0; i < comp_of_disc.Count; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"INSERT INTO Баллы_за_учебную_деятельность (Код_студента, Код_дисциплины, 
                                        Код_компетенции_студента, Код_КОСа, Код_преподавателя, Количество_набранных_баллов, Дата_записи) 
                                        VALUES (" + id_of_student + ", " + id_of_disc + ", " + comp_of_disc[i].id_of_comp + ", " + id_of_cos +
                                            ", " + id_of_teacher + ", " + textBox2.Text + ", + '" + dateTime.ToString(format) + "')"; ;
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Оценка получена!", "Внимание!", MessageBoxButtons.OK);
                    sql = "SELECT Код_записи, Код_студента, Код_дисциплины, Код_компетенции_студента, Код_КОСа, Код_преподавателя, Количество_набранных_баллов, Дата_записи FROM Баллы_за_учебную_деятельность WHERE Код_преподавателя = " + id_of_teacher;
                    print(sql);
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Введенная оценка не является числом/отрицательная/превышает максимальное значение!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //определяем айди студента
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT А.Код_студента FROM Студент А WHERE А.Код_направления_обучения =" + id_of_dir +
                                    " AND ФИО_студента = '" + comboBox6.Text + "'";
                id_of_student = (int)cmd.ExecuteScalar();
                connection.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)//Найти
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    switch (cl)
                    {
                        case 1:
                            sql = $"SELECT * FROM Студент WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 2:
                            sql = $"SELECT * FROM Преподаватель WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 3:
                            sql = $"SELECT * FROM Дисциплины WHERE {comboBox1.SelectedItem} = {textBox1.Text.ToString()}";
                            print(sql);
                            break;
                        case 4:
                            sql = $"SELECT * FROM КОС WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 5:
                            sql = $"SELECT * FROM Баллы_за_учебную_деятельность WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 6:
                            sql = $"SELECT * FROM Уровень_сформированности_компетенций_студентов WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 8:
                            sql = $"SELECT * FROM Компетенции_студентов WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 9:
                            sql = $"SELECT * FROM Направления_обучения WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 12:
                            sql = $"SELECT * FROM Уровни_сформированности_компетенций WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                    }
                }
            }
            catch
            {
                textBox1.Clear();
            }
        }
    }
}
