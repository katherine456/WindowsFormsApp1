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
    public partial class Главное_меню_учебного_отдела_не_эксперта : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand insert = new SqlCommand();
        SqlCommand delete = new SqlCommand();
        string sql;
        int cl;
        public static string login_p;
        public static string passwd_p;
        public static string acc = "0";

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

        public Главное_меню_учебного_отдела_не_эксперта()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            button2.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void анализСоответствияКомпетенцийСтудетовТребованиямВакансийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В не относитесь к группе экспертов! Функция не доступна. Свяжитесь с администратором для уточнения данного вопроса по телефону: +7-913-905-82-56", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Главное_меню_учебного_отдела_не_эксперта_Load(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            connect.Open();
            acc = "N";
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        private void данныеОСтудентахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 1;
            button3.Enabled = true;
            button1.Enabled = true;
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
            button3.Enabled = true;
            button1.Enabled = true;
            comboBox1.Items.Clear();
            string[] nums2 = new string[] { "Код_преподавателя", "ФИО_преподавателя", "E_mail", "Номер_телефона",
                "Логин_преподавателя", "Пароль_преподавателя" };
            comboBox1.Items.AddRange(nums2);
            sql = "SELECT * FROM Преподаватель";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void данныеОбУчебномОтделеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 7;
            button3.Enabled = true;
            button1.Enabled = true;
            comboBox1.Items.Clear();
            string[] nums7 = new string[] { "Код_сотрудника", "ФИО_сотрудника", "E-mail", "Логин_сотрудника",
                                "Пароль_сотрудника", "Экспертность" };
            comboBox1.Items.AddRange(nums7);
            sql = "SELECT * FROM Учебный_отдел";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void списокКОСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 4;
            button3.Enabled = true;
            button1.Enabled = true;
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
            button3.Enabled = true;
            button1.Enabled = true;
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
            button3.Enabled = true;
            button1.Enabled = true;
            comboBox1.Items.Clear();
            string[] nums9 = new string[] { "Код_направления_обучения", "Название_направления" };
            comboBox1.Items.AddRange(nums9);
            sql = "SELECT * FROM Направления_обучения";
            print(sql);
            comboBox1.SelectedIndex = 0;
        }

        private void списокКомпетенцийПоВакансиямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 10;
            comboBox1.Items.Clear();
            string[] nums10 = new string[] { "Код_вакансии", "Код_компетенции_вакансии", "Тип_компетенции",
                                            "Описание" };
            comboBox1.Items.AddRange(nums10);
            sql = "SELECT * FROM Полученный_список_компетенций_по_вакансии";
            print(sql);
            button3.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
            comboBox1.SelectedIndex = 0;
        }

        private void приоритетыКомпетенцийПоВакансиямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 11;
            comboBox1.Items.Clear();
            string[] nums11 = new string[] { "Код_вакансии", "Код_компетенции_студента", "Тип_компетенции",
                                            "Название_компетенции_студента",   "Процент_приоритета_компетенции_вакансии"};
            comboBox1.Items.AddRange(nums11);
            sql = "SELECT * FROM Приоритеты_компетенций_по_вакансиям";
            print(sql);
            button3.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
            comboBox1.SelectedIndex = 0;
        }

        private void уровниСформированностиКомпетенцийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 12;
            comboBox1.Items.Clear();
            string[] nums12 = new string[] { "Код_уровня_ВСНБ", "Требуемый_уровень_сформированности_компетенции",
                                                    "Нач_интервал", "Кон_интервал" };
            comboBox1.Items.AddRange(nums12);
            sql = "SELECT * FROM Уровни_сформированности_компетенций";
            print(sql);
            button3.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
            comboBox1.SelectedIndex = 0;
        }

        private void данныеОКомпетенцияхСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cl = 8;
            button3.Enabled = true;
            button1.Enabled = true;
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
            button3.Enabled = true;
            button1.Enabled = true;
            comboBox1.Items.Clear();
            string[] nums5 = new string[] { "Код_записи","Код_студента", "Код_дисциплины", "Код_компетенции_студента", "Код_КОСа", "Код_преподавателя",
                                            "Количество_набранных_баллов", "Дата_записи"};
            comboBox1.Items.AddRange(nums5);
            sql = "SELECT * FROM Баллы_за_учебную_деятельность";
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
            button3.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
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

        private void отчетыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void расчетУровняСформированностиКомпетенцийСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    insert = new SqlCommand(@"DELETE Уровень_сформированности_компетенций_студентов
                                                INSERT INTO Уровень_сформированности_компетенций_студентов(Код_студента, Код_компетенции_студента, 
                                                Код_направления_обучения, Процент_сформированности_компетенции)
                                                SELECT А.Код_студента, Е.Код_компетенции_студента, Б.Код_направления_обучения, (SUM(Д.Количество_набранных_баллов) / SUM(В.Макс_баллы_КОСа))
                                                FROM Студент А, Направления_обучения Б, КОС В, Баллы_за_учебную_деятельность Д, Компетенции_студентов Е
                                                WHERE
                                                Д.Код_студента = А.Код_студента AND
                                                Д.Код_КОСа = В.Код_КОСа AND
                                                Д.Код_компетенции_студента = Е.Код_компетенции_студента AND
                                                А.Код_направления_обучения = Б.Код_направления_обучения AND
                                                Б.Код_направления_обучения = Е.Код_направления_обучения
                                                GROUP BY А.Код_студента, Е.Код_компетенции_студента, Б.Код_направления_обучения", connection);
                    insert.ExecuteNonQuery();
                }
            }
            catch
            {
                MessageBox.Show("ОШИБКА!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show("Уровень освоения компетенций студентами посчитан!", "Успешно!");
            }
        }

        private void button1_Click(object sender, EventArgs e) //Добавить
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                dataGridView1.ReadOnly = false;
                button2.Enabled = true;
                button1.Enabled = false;
                button3.Enabled = false;
                dt = dataGridView1.DataSource as DataTable;
                DataRow row = dt.NewRow(); // добавляем новую строку в DataTable
                dt.Rows.Add(row);
            }
        }

        private void button2_Click(object sender, EventArgs e) //Сохранить
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int vuot = 0;
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        if (dataGridView1.SelectedRows[0].Cells[i].Value.ToString() == "")
                            vuot++;
                    }

                    if (vuot > 0)
                        MessageBox.Show("Пожалуйста, заполните все ячейки в строчке!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {

                        switch (cl)
                        {
                            case 1:
                                insert = new SqlCommand("INSERT INTO Студент(Код_студента, ФИО_студента, Курс, Факультет, Номер_группы, " +
                                    "Логин_студента, Пароль_студента) " +
                                    "VALUES (@Код_студента, @ФИО_студента, @Курс, @Факультет, @Номер_группы, @Логин_студента, @Пароль_студента)", connection);
                                string[] nums1 = new string[] { "@Код_студента", "@ФИО_студента", "@Курс", "@Факультет", "@Номер_группы",
                            "@Логин_студента", "@Пароль_студента"};
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums1[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 2:
                                insert = new SqlCommand("INSERT INTO Преподаватель(Код_преподавателя, ФИО_преподавателя, E_mail, Номер_телефона, " +
                                    "Логин_преподавателя, Пароль_преподавателя) " +
                                    "VALUES (@Код_преподавателя, @ФИО_преподавателя, @E_mail, @Номер_телефона, @Логин_преподавателя, @Пароль_преподавателя)", connection);
                                string[] nums2 = new string[] { "@Код_преподавателя", "@ФИО_преподавателя", "@E_mail",
                                                            "@Номер_телефона", "@Логин_преподавателя", "@Пароль_преподавателя" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums2[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 3:
                                insert = new SqlCommand("INSERT INTO Дисциплины(Код_дисциплины, Курс, Семестр, Название_дисциплины) " +
                                    "VALUES (@Код_дисциплины, @Курс, @Семестр, @Название_дисциплины)", connection);
                                string[] nums3 = new string[] { "@Код_дисциплины", "@Курс", "@Семестр", "@Название_дисциплины" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums3[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 4:
                                insert = new SqlCommand("INSERT INTO КОС(Код_КОСа, Тип_КОС, Макс_баллы_КОСа) " +
                                    "VALUES (@Код_КОСа, @Тип_КОС, @Макс_баллы_КОСа)", connection);
                                string[] nums4 = new string[] { "@Код_КОСа", "@Тип_КОС", "@Макс_баллы_КОСа" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums4[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 5:

                                insert = new SqlCommand("INSERT INTO Баллы_за_учебную_деятельность(Код_записи, Код_студента, Код_дисциплины, Код_компетенции_студента, " +
                                    "Код_КОСа, Код_преподавателя, Количество_набранных_балло, Дата_записи) " +
                                    "VALUES (@Код_записи, @Код_студента, @Код_дисциплины, @Код_компетенции_студента, @Код_КОСа, @Код_преподавателя, @Количество_набранных_баллов, @Дата_записи)", connection);
                                string[] nums5 = new string[] { "@Код_записи", "@Код_студента", "@Код_дисциплины", "@Код_компетенции_студента", "@Код_КОСа", "@Код_преподавателя", "@Количество_набранных_баллов", "@Дата_записи"};
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums5[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 6:
                                insert = new SqlCommand("INSERT INTO Уровень_сформированности_компетенций_студентов (Код_компетенции_студента, " +
                                    "Код_КОСа, Код_преподавателя, Процент_сформированности_компетенции) " +
                                    "VALUES (@Код_компетенции_студента, @Код_КОСа, @Код_преподавателя, @Процент_сформированности_компетенции)", connection);
                                string[] nums6 = new string[] { "@Код_компетенции_студента", "@Код_КОСа", "@Код_преподавателя", "@Процент_сформированности_компетенции" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums6[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 7:
                                insert = new SqlCommand("INSERT INTO Учебный_отдел(Код_сотрудника, ФИО_сотрудника, E-mail, " +
                                    "Логин_сотрудника, Пароль_сотрудника, Экспертность) " +
                                    "VALUES (@Код_сотрудника, @ФИО_сотрудника)", connection);
                                string[] nums7 = new string[] { "@Код_сотрудника", "@ФИО_сотрудника", "@E-mail", "@Логин_сотрудника",
                                "@Пароль_сотрудника", "@Экспертность" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums7[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 8:
                                insert = new SqlCommand("INSERT INTO Компетенции_студентов (Код_компетенции_студента, Код_направления_обучения, " +
                                    "Тип_компетенции, Название_компетенции_студента, Описание_компетенции) " +
                                    "VALUES (@Код_компетенции_студента, @Код_направления_обучения, @Тип_компетенции, @Название_компетенции_студента, @Описание_компетенции)", connection);
                                string[] nums8 = new string[] { "@Код_компетенции_студента", "@Код_направления_обучения", "@Тип_компетенции",
                                "@Название_компетенции_студента", "@Описание_компетенции" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums8[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 9:
                                insert = new SqlCommand("INSERT INTO Направления_обучения (Код_направления_обучения, Название_направления) " +
                                    "VALUES (@Код_направления_обучения, @Название_направления)", connection);
                                string[] nums9 = new string[] { "@Код_направления_обучения", "@Название_направления" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums9[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 10:
                                insert = new SqlCommand("INSERT INTO Полученный_список_компетенций_по_вакансии (Код_вакансии, Код_компетенции_вакансии, " +
                                    "Тип_компетенции, Описание) " +
                                    "VALUES (@Код_вакансии, @Код_компетенции_вакансии, @Тип_компетенции, @Описание)", connection);
                                string[] nums10 = new string[] { "@Код_вакансии", "@Код_компетенции_вакансии", "@Тип_компетенции",
                                                             "@Описание"};
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums10[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 11:
                                insert = new SqlCommand("INSERT INTO Приоритеты_компетенций_по_вакансиям (Код_вакансии, Код_компетенции_студента, " +
                                    "Тип_компетенции, Название_компетенции_студента, Процент_приоритета_компетенции_вакансии) " +
                                    "VALUES (@Код_вакансии, @Код_компетенции_студента, @Тип_компетенции, @Название_компетенции_студента, " +
                                    "@Процент_приоритета_компетенции_вакансии)", connection);
                                string[] nums11 = new string[] { "@Код_вакансии", "@Код_компетенции_студента", "@Тип_компетенции",
                                "@Название_компетенции_студента", "@Процент_приоритета_компетенции_вакансии" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums11[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                            case 12:
                                insert = new SqlCommand("INSERT INTO Уровни_сформированности_компетенций (Код_уровня_ВСНБ, Требуемый_уровень_сформированности_компетенции, " +
                                    "Нач_интервал, Кон_интервал) " +
                                    "VALUES (@Код_уровня_ВСНБ, @Требуемый_уровень_сформированности_компетенции, @Нач_интервал, @Кон_интервал)", connection);
                                string[] nums12 = new string[] { "@Код_уровня_ВСНБ", "@Требуемый_уровень_сформированности_компетенции", "@Нач_интервал",
                                "@Кон_интервал" };
                                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                                {
                                    insert.Parameters.AddWithValue(nums12[i], dataGridView1.SelectedRows[0].Cells[i].Value.ToString());
                                }
                                insert.ExecuteNonQuery();
                                break;
                        }
                    }

                }
            }
            catch
            {
                MessageBox.Show("Вы ошиблись при вводе! Пожалуйста, попробуйте еще раз!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
            }
            finally
            {
                dataGridView1.ReadOnly = true;
                button2.Enabled = false;
                button1.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e) //Удалить
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (dataGridView1.Rows.Count - 1 == 0)
                {
                    MessageBox.Show("В таблице ничего нет!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            switch (cl)
                            {
                                case 1:
                                    delete = new SqlCommand("DELETE FROM Студент WHERE Код_студента = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 2:
                                    delete = new SqlCommand("DELETE FROM Преподаватель WHERE Код_преподавателя = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 3:
                                    delete = new SqlCommand("DELETE FROM Дисциплины WHERE Код_дисциплины = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 4:
                                    delete = new SqlCommand("DELETE FROM КОС WHERE Код_КОСа = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 5:
                                    delete = new SqlCommand("DELETE FROM Баллы_за_учебную_деятельность WHERE Код_записи = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 7:
                                    delete = new SqlCommand("DELETE FROM Учебный_отдел WHERE Код_сотрудника = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 8:
                                    delete = new SqlCommand("DELETE FROM Компетенции_студентов WHERE Код_компетенции_студента = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 9:
                                    delete = new SqlCommand("DELETE FROM Направления_обучения WHERE Код_направления_обучения = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                                case 12:
                                    delete = new SqlCommand("DELETE FROM Уровни_ВСНБ_сформ_комп WHERE Код_уровня_ВСНБ = @Код", connection);
                                    delete.Parameters.AddWithValue("@Код", dataGridView1.SelectedRows[0].Cells[0].Value);
                                    delete.ExecuteNonQuery();
                                    break;
                            }
                            adapter.UpdateCommand = new SqlCommandBuilder(adapter).GetUpdateCommand();
                            adapter.Update(dt);
                        }
                    }
                    catch
                    {
                        dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                    }
                }

            }
        }

        private void button4_Click(object sender, EventArgs e) //Найти
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
                            sql = $"SELECT * FROM Баллы__за_учебную_деятельность WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 6:
                            sql = $"SELECT * FROM Уровень_сформированности_компетенций_студентов WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 7:
                            sql = $"SELECT * FROM Учебный_отдел WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
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
                        case 10:
                            sql = $"SELECT * FROM Полученный_список_компетенций_по_вакансии WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
                            print(sql);
                            break;
                        case 11:
                            sql = $"SELECT * FROM Приоритеты_компетенций_по_вакансии WHERE {comboBox1.SelectedItem} = '{textBox1.Text.ToString()}'";
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

        private void отчетОбУровнеСформированностиПКСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            Отчетность newForm = new Отчетность();
            newForm.ShowDialog();
            Show();
            this.Close();
        }
    }
}
