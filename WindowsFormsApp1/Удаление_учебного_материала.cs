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
    public partial class Удаление_учебного_материала : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        string _login_p, _passwd_p;
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        SqlCommand delete = new SqlCommand();
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

        public class General
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                delete = new SqlCommand(@"DELETE FROM Учебные_материалы_по_улучшению_компетенций WHERE Автор_учебного_материала = @Автор_учебного_материала
                                           AND Название_учебного_материала = @Название_учебного_материала", connection);
                delete.Parameters.AddWithValue("@Автор_учебного_материала", dataGridView1.SelectedRows[0].Cells[3].Value);
                delete.Parameters.AddWithValue("@Название_учебного_материала", dataGridView1.SelectedRows[0].Cells[1].Value);
                delete.ExecuteNonQuery();

                sql = @"SELECT Д.Название_дисциплины, Е.Название_учебного_материала, Ж.Название_типа_учебного_материала, Е.Автор_учебного_материала, Е.Год_выпуска, Е.Ссылка_на_источник
                                            FROM Дисциплины Д, 
											Учебные_материалы_по_улучшению_компетенций Е, Типы_учебных_материалов Ж, 
											Преподаватель А, Преподаватель_Дисциплина Б
                                            WHERE
                                            Е.Код_дисциплины = Д.Код_дисциплины
                                            AND Е.Код_типа_учебного_материала = Ж.Код_типа_учебного_материала
											AND А.Код_преподавателя = Б.Код_преподавателя
											AND Б.Код_дисциплины = Д.Код_дисциплины
											AND А.Логин_преподавателя = '" + _login_p + "'" +
                                            " AND А.Пароль_преподавателя = '" + _passwd_p + "'";
                print(sql);
                connection.Close();
            }
        }

        private void Удаление_учебного_материала_Load(object sender, EventArgs e)
        {
            sql = @"SELECT Д.Название_дисциплины, Е.Название_учебного_материала, Ж.Название_типа_учебного_материала, Е.Автор_учебного_материала, Е.Год_выпуска, Е.Ссылка_на_источник
                                            FROM Дисциплины Д, 
											Учебные_материалы_по_улучшению_компетенций Е, Типы_учебных_материалов Ж, 
											Преподаватель А, Преподаватель_Дисциплина Б
                                            WHERE
                                            Е.Код_дисциплины = Д.Код_дисциплины
                                            AND Е.Код_типа_учебного_материала = Ж.Код_типа_учебного_материала
											AND А.Код_преподавателя = Б.Код_преподавателя
											AND Б.Код_дисциплины = Д.Код_дисциплины
											AND А.Логин_преподавателя = '" + _login_p + "'" +
                                            " AND А.Пароль_преподавателя = '" + _passwd_p + "'";
            print(sql);
            
        }

        private void вГлавноеМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Главное_меню_преподавателя newForm = new Главное_меню_преподавателя();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        public Удаление_учебного_материала()
        {
            InitializeComponent();
            _login_p = Главное_меню_преподавателя.login_p;
            _passwd_p = Главное_меню_преподавателя.passwd_p;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
    }
}
