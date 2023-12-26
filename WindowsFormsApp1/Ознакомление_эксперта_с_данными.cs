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
    public partial class Ознакомление_эксперта_с_данными : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        string sql;
        string number_of_work = "";
        string[] types_of_competit = new string[3] { "ПК", "УК", "ОПК" };
        public static int[] coun_empty_comp_of_vac = new int[3] {0, 0, 0 };

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

        private void Ознакомление_эксперта_с_данными_Load(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            connect.Open();
        }

        public Ознакомление_эксперта_с_данными()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            comboBox1.Items.Clear();
            string[] nums1 = new string[] { "Код_вакансии" };
            comboBox1.Items.AddRange(nums1);
            if (textBox3.Text.Length == 0)
                button1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sql = @"SELECT Код_вакансии, Название_вакансии FROM Направления_обучения, Вакансии
                    WHERE Вакансии.Код_направления_обучения = Направления_обучения.Код_направления_обучения
                    AND Направления_обучения.Название_направления = '" + comboBox1.SelectedValue.ToString() + "'";
            print(sql);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public static int number_of_vacancy = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            int numericValue;
            if (textBox3.Text == "")
            {
                MessageBox.Show("Вы ничего не ввели!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.TryParse(textBox3.Text.ToString(), out numericValue))
            {
                {
                    int vacan = 0;
                    object result;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = $"SELECT Код_вакансии FROM Вакансии WHERE Код_вакансии = {textBox3.Text.ToString()}";
                        result = cmd.ExecuteScalar();
                        if (result != null)
                            vacan = (int) result;
                        else
                            MessageBox.Show("Данной вакансии нет в списке!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        connection.Close();
                    }

                    //проверка наличия вакансии в списке
                    if (vacan.ToString() == textBox3.Text.ToString())
                    {
                        //получено ли целое число
                        if (int.TryParse(textBox3.Text.ToString(), out numericValue))
                        {
                            sql = $"SELECT * FROM Полученный_список_компетенций_по_вакансии WHERE Код_вакансии = '{textBox3.Text.ToString()}'";
                            print(sql);
                            number_of_work = textBox3.Text.ToString();
                            number_of_vacancy = Int32.Parse(number_of_work);
                            string number_of_col = textBox3.Text.ToString();
                            int number_of_column = Int32.Parse(number_of_col);
                            MessageBox.Show("Число получено!", "Поздравляем!", MessageBoxButtons.OK);

                            int[] count = new int[types_of_competit.Length];
                            int c = 0;
                            //есть ли данные по компетенциям вакансии, пришедшим из другой ИС
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                for (int i = 0; i < types_of_competit.Length; i++)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = $"SELECT COUNT(Тип_компетенции) FROM Полученный_список_компетенций_по_вакансии " +
                                                       $"WHERE Тип_компетенции = '" + types_of_competit[i] + "'" +
                                                        " AND Код_вакансии = " + number_of_vacancy;
                                    count[i] = (int)cmd.ExecuteScalar();
                                    if (count[i] >= 2)
                                    {
                                        c++; //есть 2 и более требований по ПК, УК, ОПК

                                    }
                                    else if (count[i] == 0)
                                    {
                                        coun_empty_comp_of_vac[i]++; //нет по группе компетенций требований
                                    }
                                }
                                connection.Close();
                            }
                            if ((coun_empty_comp_of_vac[0] >= 0 || coun_empty_comp_of_vac[1] >= 0 || coun_empty_comp_of_vac[2] >= 0) && c > 0)
                                button1.Enabled = true;
                            else
                            {
                                MessageBox.Show("Данные по выбранной вакансии пока отсутствуют!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка ввода! Пожалуйста, введите целое число", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                 MessageBox.Show("Ошибка ввода! Пожалуйста, введите целое число", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        public class Study_vacancy
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

        private void Ознакомление_эксперта_с_данными_Load_1(object sender, EventArgs e)
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
                sqlCommand2.CommandText = @"SELECT DISTINCT Название_направления FROM Направления_обучения, Вакансии
                                                WHERE Вакансии.Код_направления_обучения = Направления_обучения.Код_направления_обучения";
                sqlCommand2.Connection = connection;
                List<Study_vacancy> comp_of_vacancy = new List<Study_vacancy>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        Study_vacancy coun = new Study_vacancy
                        {
                            description = (dr["Название_направления"].ToString())
                        };
                        comp_of_vacancy.Add(coun);
                    }
                }
                comboBox1.DataSource = comp_of_vacancy;
                connection.Close();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        //кнопка перехода но форму Матрица парных сравнений
        private void button1_Click(object sender, EventArgs e)
        {

            Матрица_парных_сравнений newForm = new Матрица_парных_сравнений();
            newForm.ShowDialog();
            Show();
            this.Close();

        }

        private void главноеМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Главное_меню_администратора.access == "Ad")
            {
                Главное_меню_администратора newForm = new Главное_меню_администратора();
                Hide();
                newForm.ShowDialog();
                Show();
                this.Close();
            }
            else if (Главное_меню_преподавателя.accessed == "Te")
            {
                Главное_меню_преподавателя newForm = new Главное_меню_преподавателя();
                Hide();
                newForm.ShowDialog();
                Show();
                this.Close();
            }
            else if (Главное_меню_студента.accessi == "ST")
            {
                Главное_меню_студента newForm = new Главное_меню_студента();
                Hide();
                newForm.ShowDialog();
                Show();
                this.Close();
            }
            else if (Главное_меню_учебного_отдела_эксперта.accessin == "Ex")
            {
                Главное_меню_учебного_отдела_эксперта newForm = new Главное_меню_учебного_отдела_эксперта();
                Hide();
                newForm.ShowDialog();
                Show();
                this.Close();
            }
            else if (Главное_меню_учебного_отдела_не_эксперта.acc == "N")
            {
                Главное_меню_учебного_отдела_не_эксперта newForm = new Главное_меню_учебного_отдела_не_эксперта();
                Hide();
                newForm.ShowDialog();
                Show();
                this.Close();
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
