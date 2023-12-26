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
    public partial class Окно_редактирования_баллов_преподавателя : Form
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
        public static string log_in;
        public static string pass_wd;
        public static int id_of_dir;
        public static string accessed = "0";
        List<ID_of_note> id_of_notice = new List<ID_of_note>();
        int id_of_student, id_of_comp, id_of_cos, id_of_disc, id_of_teacher;
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

        public Окно_редактирования_баллов_преподавателя()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox9.ReadOnly = true;
            textBox5.ReadOnly = true;
            comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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

        private void Окно_редактирования_баллов_преподавателя_Load(object sender, EventArgs e)
        {
            log_in = Главное_меню_преподавателя.login_p;
            pass_wd = Главное_меню_преподавателя.passwd_p;
            accessed = "Te";
            
            //определяем ID преподавателя
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Код_преподавателя FROM Преподаватель WHERE Логин_преподавателя = '" + log_in +
                                    "' AND Пароль_преподавателя = '" + pass_wd + "'";
                id_of_teacher = (int)cmd.ExecuteScalar();
                textBox7.Text = id_of_teacher.ToString();
                textBox7.ReadOnly = true;
                connection.Close();
            }

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            string[] nums5 = new string[] { "Код_записи", "Код_студента", "Код_дисциплины", "Код_компетенции_студента", "Код_КОСа", "Код_преподавателя",
                                            "Количество_набранных_баллов"};
            comboBox1.Items.AddRange(nums5);
            sql = "SELECT * FROM Баллы_за_учебную_деятельность WHERE Код_преподавателя = " + id_of_teacher;
            print(sql);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = "SELECT Код_записи FROM Баллы_за_учебную_деятельность WHERE Код_преподавателя = " + id_of_teacher;
                sqlCommand2.Connection = connection;
                SqlDataReader dr = sqlCommand2.ExecuteReader();
                while (dr.Read())
                {
                    ID_of_note coun = new ID_of_note
                    {
                        id_of_note = (Convert.ToInt32(dr["Код_записи"].ToString()))
                    };
                    id_of_notice.Add(coun);
                }
                connection.Close();

            }

            comboBox1.SelectedIndex = 0;

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

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            id_of_notice.Clear();
            Главное_меню_преподавателя newForm = new Главное_меню_преподавателя();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }


        private void справочникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Справочники newForm = new Справочники();
            newForm.ShowDialog();
            Show();
            this.Close();

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
                textBox8.Text = id_of_student.ToString();
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
                textBox5.Text = id_of_cos.ToString();
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
                max_ball = (int)cmd.ExecuteScalar();
                textBox6.Text = max_ball.ToString();
                textBox6.ReadOnly = true;
                connection.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //определяем айди  компетенции
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT Код_компетенции_студента FROM Компетенции_студентов, Направления_обучения
                                    WHERE 
                                    Название_компетенции_студента = '" + comboBox3.Text + "' AND Название_направления = '" + comboBox7.Text + "'";
                id_of_comp = (int)cmd.ExecuteScalar();
                textBox3.Text = id_of_comp.ToString();
                connection.Close();
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
                textBox9.Text = id_of_disc.ToString();
                connection.Close();
            }

            //определяем компетенции по дисциплине
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT А.Название_компетенции_студента FROM Компетенции_студентов А, Дисциплины Б, Дисциплины_Компетенции_студентов В, 
                                                Направления_обучения Г
                                                WHERE А.Код_компетенции_студента = В.Код_компетенции_студента
                                                 AND  А.Код_направления_обучения = Г.Код_направления_обучения
                                                 AND  Б.Код_дисциплины = В.Код_дисциплины
                                                 AND  Г.Код_направления_обучения = " + id_of_dir + " " +
                                                 "AND В.Код_дисциплины = " + id_of_disc + "";
                sqlCommand2.Connection = connection;

                List<General> gen = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_компетенции_студента"].ToString())
                        };
                        gen.Add(coun);
                    }
                }
                comboBox3.DataSource = gen;
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Список_компетенций_по_дисциплинам newForm = new Список_компетенций_по_дисциплинам();
            newForm.ShowDialog();
            Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                float numericValue;
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                int counter = 0;
                for (int i = 0; i < id_of_notice.Count; i++)
                {
                    if (id_of_notice[i].id_of_note == int.Parse(textBox4.Text))
                    {
                        if (float.TryParse(textBox2.Text.ToString(), out numericValue) && float.Parse(textBox2.Text.ToString()) >= 0
                    && float.Parse(textBox2.Text.ToString()) <= float.Parse(textBox6.Text.ToString()))
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"UPDATE Баллы_за_учебную_деятельность SET Код_студента = " + id_of_student + ", Код_дисциплины = " + id_of_disc + ", Код_компетенции_студента = " 
                                                    + int.Parse(textBox3.Text) + ", Код_КОСа = " + id_of_cos +
                                                    ", Код_преподавателя = " + id_of_teacher + ", Количество_набранных_баллов = " + float.Parse(textBox2.Text) +
                                                    " WHERE Код_записи = " + int.Parse(textBox4.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Оценка изменена!", "Внимание!", MessageBoxButtons.OK);
                            counter += 1;
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Введенная оценка не является числом/отрицательная/превышает максимальное значение!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
                }
                if (counter == 1)
                {
                    sql = "SELECT * FROM Баллы_за_учебную_деятельность WHERE Код_преподавателя = " + id_of_teacher;
                    print(sql);
                }
                else
                {
                    MessageBox.Show("Число не является целым/Вы обратились не к своей записи!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void button4_Click(object sender, EventArgs e) //Найти
        {
            sql = $"SELECT * FROM Баллы_за_учебную_деятельность WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
            print(sql);
        }

        public class ID_of_note
        {
            public int id_of_note;
            public override string ToString()
            {
                return string.Format("{0}", id_of_note);
            }
        }


    }
}
