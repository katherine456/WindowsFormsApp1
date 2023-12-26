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
    public partial class Добавление_учебных_материалов : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        string _login_p, _passwd_p;
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
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                connection.Close();
            }
        }

        public Добавление_учебных_материалов()
        {
            InitializeComponent();
            _login_p = Главное_меню_преподавателя.login_p;
            _passwd_p = Главное_меню_преподавателя.passwd_p;
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

        private void Добавление_учебных_материалов_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT В.Название_дисциплины
                                            FROM Преподаватель А, Преподаватель_Дисциплина Б, Дисциплины В
	                                        WHERE А.Код_преподавателя = Б.Код_преподавателя
											AND Б.Код_дисциплины = В.Код_дисциплины
											AND А.Логин_преподавателя = '" + _login_p + "'" +
											" AND А.Пароль_преподавателя = '" + _passwd_p + "'";
                sqlCommand2.Connection = connection;
                List<General> comp_of_dir = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_дисциплины"].ToString())
                        };
                        comp_of_dir.Add(coun);
                    }
                }
                comboBox1.DataSource = comp_of_dir;
                connection.Close();

            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT А.Название_типа_учебного_материала
                                            FROM Типы_учебных_материалов А";
                sqlCommand2.Connection = connection;
                List<General> comp_of_dir = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_типа_учебного_материала"].ToString())
                        };
                        comp_of_dir.Add(coun);
                    }
                }
                comboBox2.DataSource = comp_of_dir;
                connection.Close();

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sql = @"SELECT Е.Название_учебного_материала, Ж.Название_типа_учебного_материала, Е.Автор_учебного_материала, Е.Год_выпуска, Е.Ссылка_на_источник
                                            FROM Дисциплины Д, 
											Учебные_материалы_по_улучшению_компетенций Е, Типы_учебных_материалов Ж
                                            WHERE
                                            Е.Код_дисциплины = Д.Код_дисциплины
                                            AND Е.Код_типа_учебного_материала = Ж.Код_типа_учебного_материала
                                            AND Д.Название_дисциплины = '" + comboBox1.Text.ToString() + "'";
            print(sql);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int numericValue;
            bool isNumber = int.TryParse(textBox3.Text.ToString(), out numericValue);

            if (textBox1.Text == "" && textBox1.Text == " " && textBox2.Text == " " && textBox2.Text == "")
            {
                MessageBox.Show("Не заполнены название и/или автор учебного материала!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (textBox3.Text.ToString() == "" || isNumber) 
                { 
                    int id_of_discipline, id_of_type;

                object result;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT А.Код_дисциплины FROM Дисциплины А 
                                        WHERE А.Название_дисциплины = '" + comboBox1.Text.ToString() + "'";
                    result = cmd.ExecuteScalar();
                    id_of_discipline = Convert.ToInt32(result);
                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT А.Код_типа_учебного_материала FROM Типы_учебных_материалов А
                                        WHERE А.Название_типа_учебного_материала = '" + comboBox2.Text.ToString() + "'";
                    result = cmd.ExecuteScalar();
                    id_of_type = Convert.ToInt32(result);
                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"INSERT INTO Учебные_материалы_по_улучшению_компетенций(Код_дисциплины, 
                                        Код_типа_учебного_материала, Автор_учебного_материала, Название_учебного_материала, Год_выпуска, Ссылка_на_источник) 
                                        VALUES(" + id_of_discipline + ", " + id_of_type + ", '" + textBox1.Text.ToString() + "', " +
                                        "'" + textBox2.Text.ToString() + "', '" + textBox3.Text.ToString() + "', '" + textBox4.Text.ToString() + "' )";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                sql = @"SELECT Е.Название_учебного_материала, Ж.Название_типа_учебного_материала, Е.Автор_учебного_материала, Е.Год_выпуска, Е.Ссылка_на_источник
                                            FROM Дисциплины Д, 
											Учебные_материалы_по_улучшению_компетенций Е, Типы_учебных_материалов Ж
                                            WHERE
                                            Е.Код_дисциплины = Д.Код_дисциплины
                                            AND Е.Код_типа_учебного_материала = Ж.Код_типа_учебного_материала
                                            AND Д.Название_дисциплины = '" + comboBox1.Text.ToString() + "'";
                print(sql);
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                
                }
                else if (!isNumber)
                {
                    MessageBox.Show("Год не является целым числом!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void вГлавноеМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Главное_меню_преподавателя newForm = new Главное_меню_преподавателя();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }
    }
}
