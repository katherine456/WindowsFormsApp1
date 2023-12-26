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
    public partial class Главное_меню_студента : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        public static string log_stud;
        public static string passwd_stud;
        public static string accessi = "0";
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
                connection.Close();
            }
        }

        public Главное_меню_студента()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Главное_меню_студента_Load(object sender, EventArgs e)
        {
            log_stud = Вход_в_систему.loginUser;
            passwd_stud = Вход_в_систему.passwordUser;
            accessi = "ST";
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

        private void данныеОСтудентахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = "SELECT * FROM Студент WHERE Логин_студента = '" + log_stud + "' AND Пароль_студента = '" + passwd_stud + "'";
            print(sql);
        }

        private void списокДисциплинToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = "SELECT А.Название_дисциплины FROM Дисциплины А, Направления_обучения Б, Направления_Дисциплины Г, Студент В " +
                    @"WHERE Б.Код_направления_обучения = Г.Код_направления_обучения
                      AND В.Код_направления_обучения = Г.Код_направления_обучения
                      AND А.Код_дисциплины = Г.Код_дисциплины
                       AND В.Логин_студента = '" + log_stud + "' " +
                       "AND В.Пароль_студента = '" + passwd_stud + "'";
            print(sql);
        }

        private void данныеОПреподавателяхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"SELECT DISTINCT Д.ФИО_преподавателя, Д.E_mail  FROM Дисциплины А, Направления_обучения Б, Направления_Дисциплины Г, 
                    Студент В, Преподаватель Д, Преподаватель_Дисциплина Е
                    WHERE А.Код_дисциплины = Е.Код_дисциплины
                    AND Д.Код_преподавателя = Е.Код_преподавателя
                    AND Б.Код_направления_обучения = Г.Код_направления_обучения
                    AND А.Код_дисциплины = Г.Код_дисциплины
                    AND В.Код_направления_обучения = Б.Код_направления_обучения";
            print(sql);
        }

        private void данныеОКомпетенцияхСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"SELECT А.Название_компетенции_студента, А.Описание_компетенции FROM Компетенции_студентов А, Направления_обучения Б, Студент В
                    WHERE А.Код_направления_обучения = Б.Код_направления_обучения
                    AND Б.Код_направления_обучения = В.Код_направления_обучения
                    AND В.Логин_студента = '" + log_stud + "' " +
                    "AND В.Пароль_студента = '" + passwd_stud + "'";
            print(sql);
        }

        private void данныеОБаллахСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"SELECT Д.Название_дисциплины, Ж.Название_компетенции_студента, Г.Тип_КОС, А.Количество_набранных_баллов 
                    FROM Баллы_за_учебную_деятельность А, Направления_обучения Б, Студент В, КОС Г, Дисциплины Д, 
                    Направления_Дисциплины Е, Компетенции_студентов Ж, Дисциплины_Компетенции_студентов З
                    WHERE А.Код_студента = В.Код_студента
                    AND А.Код_дисциплины = Д.Код_дисциплины
                    AND А.Код_компетенции_студента = Ж.Код_компетенции_студента
                    AND Б.Код_направления_обучения = В.Код_направления_обучения
                    AND Б.Код_направления_обучения = Е.Код_направления_обучения
                    AND Г.Код_КОСа = А.Код_КОСа
                    AND З.Код_дисциплины = Д.Код_дисциплины
                    AND З.Код_компетенции_студента = Ж.Код_компетенции_студента
                    AND Ж.Код_направления_обучения = Б.Код_направления_обучения
                    AND Д.Код_дисциплины = Е.Код_дисциплины
                    AND В.Логин_студента = '" + log_stud + "' " +
                    "AND В.Пароль_студента = '" + passwd_stud + "'";
            print(sql);
        }

        private void данныеОбУровняхРазвитияКомпетенцийСтудентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"SELECT Ж.Название_компетенции_студента, А.Процент_сформированности_компетенции
                    FROM Направления_обучения Б, Студент В, Компетенции_студентов Ж, Уровень_сформированности_компетенций_студентов А
                    WHERE А.Код_направления_обучения = В.Код_направления_обучения
                    AND В.Код_направления_обучения = Б.Код_направления_обучения
                    AND Ж.Код_направления_обучения = Б.Код_направления_обучения
                    AND Ж.Код_компетенции_студента= А.Код_компетенции_студента
                    AND В.Логин_студента = '" + log_stud + "' " +
                    "AND В.Пароль_студента = '" + passwd_stud + "'";
            print(sql);
        }

        private void проверкаСоответствияТребованиямВакансийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Проверка_соответствия_студента_требованиям_вакансии newForm = new Проверка_соответствия_студента_требованиям_вакансии();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void данныеОКурсахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Курсы newForm = new Курсы();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }
    }
}
