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
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp1
{
    public partial class Отчетность : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        SqlCommand insert = new SqlCommand();
        public static int id_of_dir;
        string[] words_of_columns_vacancy = new string[] { };
        string[] words_of_columns_students_semestr = new string[] { };
        string sql;
        int cl = 0;
        private BindingSource bs = new BindingSource();

        public class General
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

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

        public Отчетность()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            pictureBox1.Image = Image.FromFile("C:\\Users\\Admin\\Downloads\\WindowsFormsApp1\\Пример_отчета.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;


            //заполняем комбобоксы
            string[] yes_no = new string[2] { "Да", "Нет" };
            for (int i = 0; i < yes_no.Length; i++)
            {
                comboBox1.Items.Add(yes_no[i]);
                comboBox2.Items.Add(yes_no[i]);
                comboBox3.Items.Add(yes_no[i]);
                comboBox4.Items.Add(yes_no[i]);
                comboBox6.Items.Add(yes_no[i]);
                comboBox8.Items.Add(yes_no[i]);
                comboBox9.Items.Add(yes_no[i]);
                comboBox10.Items.Add(yes_no[i]);
                if (i == 1)
                {
                    comboBox1.SelectedItem = ("Нет");
                    comboBox2.SelectedItem = ("Нет");
                    comboBox3.SelectedItem = ("Нет");
                    comboBox4.SelectedItem = ("Нет");
                    comboBox6.SelectedItem = ("Нет");
                    comboBox8.SelectedItem = ("Нет");
                    comboBox9.SelectedItem = ("Нет");
                    comboBox10.SelectedItem = ("Нет");
                }


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




        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Отчетность_Load(object sender, EventArgs e)
        {
          
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

            //определяем вакансии по направлению
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT DISTINCT Название_вакансии FROM Вакансии WHERE Код_направления_обучения = " + id_of_dir;
                sqlCommand2.Connection = connection;

                List<General> gen = new List<General>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        General coun = new General
                        {
                            description = (dr["Название_вакансии"].ToString())
                        };
                        gen.Add(coun);
                    }
                }
                comboBox11.DataSource = gen;
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
        }


        private void get_otchet(string[] yes_no)
        {
            Random rnd = new Random();
            int value = rnd.Next(0, 1000);
            string path = System.IO.Directory.GetCurrentDirectory() + @"\" + "Отчет по компетенциям по группе " + comboBox5.Text + "_" + value + ".xlsx";

            Excel.Application excelapp = new Excel.Application();
            Excel.Workbook workbook = excelapp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            excelapp.SheetsInNewWorkbook = 3;
            worksheet.StandardWidth = 15;

            excelapp.Visible = false;
            // Выделяем диапазон ячеек 
            Excel.Range _excelCells1 = (Excel.Range)worksheet.get_Range("A1", "E1").Cells;
            _excelCells1.WrapText = true;
            // Производим объединение
            worksheet.Cells[1, 5] = @"Федеральное государственное бюджетное образовательное учреждение высшего образования ""Сибирский государственный университет телекоммуникаций и информатики"" (СибГУТИ)";
            _excelCells1.Merge();
            worksheet.get_Range("A1", "E1").EntireRow.AutoFit();
            worksheet.get_Range("A1", "E1").RowHeight = 60;
            worksheet.get_Range("A1", "E1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            _excelCells1 = (Excel.Range)worksheet.get_Range("A2", "D2").Cells;
            _excelCells1.WrapText = true;
            // Производим объединение
            worksheet.Cells[1, 4].Range("A2", "D2").Font.Bold = true;
            worksheet.Cells[1, 4].Range("A2", "D2").Font.Size = 20;
            worksheet.Cells[2, 4] = "Сводная ведомость";
            _excelCells1.Merge();
            worksheet.get_Range("A2", "D2").EntireRow.AutoFit();

            //направление
            _excelCells1 = (Excel.Range)worksheet.get_Range("D3", "I3").Cells;
            _excelCells1.WrapText = true;
            worksheet.Cells[3, 5].Range("D3", "I3").Font.Bold = true;
            worksheet.Cells[3, 5].Range("D3", "I3").Font.Size = 18;
            worksheet.Cells[3, 5] = comboBox7.Text;
            _excelCells1.Merge();
            worksheet.get_Range("D3", "I3").EntireRow.AutoFit();
            worksheet.get_Range("D3", "I3").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            //получение названий компетенций вакансии
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                Array.Resize(ref words_of_columns_vacancy, 0);
                cmd.CommandText = @"SELECT В.Название_компетенции_студента 
                                            FROM Направления_обучения А, Вакансии Б, Компетенции_студентов В
                                            WHERE А.Код_направления_обучения = В.Код_направления_обучения AND 
	                                            Б.Код_направления_обучения = А.Код_направления_обучения AND 
	                                            Б.Название_вакансии = '" + comboBox11.Text + "' " +
                                                "GROUP BY В.Код_компетенции_студента, В.Название_компетенции_студента";
                SqlDataReader reader = cmd.ExecuteReader();
                int k = 0;
                while (reader.Read())
                {
                    Array.Resize(ref words_of_columns_vacancy, words_of_columns_vacancy.Length + 1);
                    words_of_columns_vacancy[k] = reader["Название_компетенции_студента"].ToString();
                    words_of_columns_vacancy[k] = words_of_columns_vacancy[k].Replace(" ", "");
                    k++;
                }
                connection.Close();
            }

            int number_of_cell = 4;

            for (int i = 0; i < yes_no.Length; i++)
            {
                
                if (yes_no[i] == "Да")
                {
                    //вакансия
                    _excelCells1 = (Excel.Range)worksheet.get_Range("D" + (number_of_cell + 3), "I" + (number_of_cell + 3)).Cells;
                    _excelCells1.WrapText = true;
                    worksheet.Cells[(number_of_cell + 3), 5].Range("D" + (number_of_cell + 3), "I" + (number_of_cell + 3)).Font.Bold = true;
                    worksheet.Cells[(number_of_cell + 3), 5].Range("D" + (number_of_cell + 3), "I" + (number_of_cell + 3)).Font.Size = 18;
                    worksheet.Cells[(number_of_cell + 3), 5] = comboBox11.Text;
                    _excelCells1.Merge();
                    worksheet.get_Range("D" + (number_of_cell + 3), "I" + (number_of_cell + 3)).EntireRow.AutoFit();
                    worksheet.get_Range("D" + (number_of_cell + 3), "I" + (number_of_cell + 3)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    worksheet.Cells[number_of_cell, 1] = @"Период контроля"; worksheet.Cells[number_of_cell, 2] = (i + 1) + "-ый семестр";
                    worksheet.Cells[number_of_cell + 1, 1] = @"Группа"; worksheet.Cells[number_of_cell + 1, 2] = comboBox5.Text;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = @"DELETE FROM Temp
                        INSERT INTO Temp   
                       (Название_компетенции_студента, Нач_интервал)  
                        SELECT А.Название_компетенции_студента, Б.Нач_интервал
                        FROM Уровни_сформированности_компетенций А, Уровни_ВСНБ_сформ_комп Б, 
                        Направления_обучения В, Вакансии Г, Студент Д, Компетенции_студентов Е
                        WHERE А.Код_уровня_ВСНБ = Б.Код_уровня_ВСНБ
                        AND В.Код_направления_обучения = Г.Код_направления_обучения
                        AND А.Код_вакансии = Г.Код_вакансии
                        AND Е.Код_направления_обучения = В.Код_направления_обучения
                        AND Е.Название_компетенции_студента = А.Название_компетенции_студента
                        AND Г.Название_вакансии = '" + comboBox11.Text + "' " +
                                "GROUP BY А.Название_компетенции_студента, Б.Нач_интервал, Е.Код_компетенции_студента ";
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }

                    sql = @"SELECT * FROM Temp";
                    print(sql);

                    worksheet.get_Range("A" + (number_of_cell + 5), "Z" + (number_of_cell + 5)).Font.Bold = true;
                    worksheet.get_Range("A" + (number_of_cell + 5), "Z" + (number_of_cell + 5)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    worksheet.get_Range("A" + (number_of_cell + 6), "Z" + (number_of_cell + 6)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    worksheet.get_Range("A" + (number_of_cell + 5), "Z" + (number_of_cell + 5)).Font.Size = 11;

                    int kol = (number_of_cell + 5);
                    for (int l = 1; l < dataGridView1.ColumnCount + 1; l++)
                    {
                        for (int b = 1; b < dataGridView1.RowCount + 1; b++)
                        {
                            worksheet.Rows[kol].Columns[b] = dataGridView1.Rows[b - 1].Cells[l - 1].Value;
                        }
                        kol++;
                    }

                    //направление
                    _excelCells1 = (Excel.Range)worksheet.get_Range("D" + (number_of_cell + 8), "I" + (number_of_cell + 8)).Cells;
                    _excelCells1.WrapText = true;
                    worksheet.Cells[(number_of_cell + 8), 5].Range("D" + (number_of_cell + 8), "I" + (number_of_cell + 8)).Font.Bold = true;
                    worksheet.Cells[(number_of_cell + 8), 5].Range("D" + (number_of_cell + 8), "I" + (number_of_cell + 8)).Font.Size = 18;
                    worksheet.Cells[(number_of_cell + 8), 5] = "Результаты студентов группы " + comboBox5.Text;
                    _excelCells1.Merge();
                    worksheet.get_Range("D" + (number_of_cell + 8), "I" + (number_of_cell + 8)).EntireRow.AutoFit();
                    worksheet.get_Range("D" + (number_of_cell + 8), "I" + (number_of_cell + 8)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = @"DELETE FROM Уровень_сформированности_компетенций_студентов";

                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }

                    //получение уровней сформированности компетенций
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        int ctetchik = 1;
                        int counterin = i + 1;
                        while (counterin != 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"
                                            INSERT INTO Уровень_сформированности_компетенций_студентов(Код_студента, Код_компетенции_студента, 
                                            Код_направления_обучения, Процент_сформированности_компетенции)
                                            SELECT А.Код_студента, Е.Код_компетенции_студента, Б.Код_направления_обучения, (SUM(Д.Количество_набранных_баллов) / SUM(В.Макс_баллы_КОСа))
                                            FROM Студент А, Направления_обучения Б, КОС В, Баллы_за_учебную_деятельность Д, 
                                            Компетенции_студентов Е, Дисциплины Г, Дисциплины_Компетенции_студентов Ж, Направления_Дисциплины З
                                            WHERE
                                            Д.Код_студента = А.Код_студента AND
                                            Д.Код_КОСа = В.Код_КОСа AND
                                            Д.Код_компетенции_студента = Е.Код_компетенции_студента AND
                                            А.Код_направления_обучения = Б.Код_направления_обучения AND
                                            Б.Код_направления_обучения = Е.Код_направления_обучения
                                            AND Г.Код_дисциплины = Ж.Код_дисциплины
                                            AND Ж.Код_компетенции_студента = Е.Код_компетенции_студента
                                            AND З.Код_дисциплины = Г.Код_дисциплины
                                            AND З.Код_направления_обучения = Б.Код_направления_обучения
                                            AND Д.Код_дисциплины = Г.Код_дисциплины
                                            AND Б.Название_направления = '" + comboBox7.Text + "' " +
                                                "AND А.Номер_группы = '" + comboBox5.Text + "' " +
                                                            @"AND Г.Семестр = " + ctetchik;
                            cmd.CommandText += " GROUP BY А.Код_студента, Е.Код_компетенции_студента, Б.Код_направления_обучения";
                            cmd.ExecuteNonQuery();
                            ctetchik++;
                            counterin--;
                            
                        }
                        connection.Close();
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = @"DELETE FROM Temp
                        INSERT INTO Temp   
                       (Название_компетенции_студента, Нач_интервал)  
                        SELECT Б.Название_компетенции_студента, AVG(А.Процент_сформированности_компетенции)
                        FROM Направления_обучения В, Студент Д, Уровень_сформированности_компетенций_студентов А, Компетенции_студентов Б
                        WHERE Д.Код_направления_обучения = В.Код_направления_обучения
                        AND А.Код_направления_обучения = В.Код_направления_обучения
                        AND А.Код_компетенции_студента = Б.Код_компетенции_студента
                        AND Б.Код_направления_обучения = В.Код_направления_обучения
                        AND А.Код_студента = Д.Код_студента
                        AND А.Код_компетенции_студента = Б.Код_компетенции_студента
                        AND В.Название_направления = '" + comboBox7.Text + "' " +
                            @"AND Д.Номер_группы = '" + comboBox5.Text + "' " +
                            "GROUP BY Б.Название_компетенции_студента, Б.Код_компетенции_студента";
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }

                    //получение списка компетенций по семестру
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        Array.Resize(ref words_of_columns_students_semestr, 0);
                        cmd.CommandText = @"SELECT Б.Название_компетенции_студента
                                FROM Направления_обучения В, Студент Д, 
                                Уровень_сформированности_компетенций_студентов А, 
                                Компетенции_студентов Б, Дисциплины Г, Направления_Дисциплины Е, Дисциплины_Компетенции_студентов Ж
                                WHERE Д.Код_направления_обучения = В.Код_направления_обучения
                                AND А.Код_направления_обучения = В.Код_направления_обучения
                                AND А.Код_компетенции_студента = Б.Код_компетенции_студента
                                AND Б.Код_направления_обучения = В.Код_направления_обучения
                                AND Г.Код_дисциплины = Е.Код_дисциплины
                                AND Е.Код_направления_обучения = В.Код_направления_обучения
                                AND Ж.Код_дисциплины = Г.Код_дисциплины
                                AND Ж.Код_компетенции_студента = Б.Код_компетенции_студента
                                AND А.Код_студента = Д.Код_студента
                                AND А.Код_компетенции_студента = Б.Код_компетенции_студента
                                AND В.Название_направления = '" + comboBox7.Text + "' " +
                                    "AND Д.Номер_группы = '" + comboBox5.Text + "' " +
                                    @"AND (Г.Семестр = ";
                        int ctetchik;
                        if (i > 0)
                        {
                            ctetchik = i;
                            for (int w = 0; w <= i; w++)
                            {
                                if (w == i)
                                    cmd.CommandText += "" + (w + 1) + ") ";
                                else
                                    cmd.CommandText += "" + (w + 1) + " or Г.Семестр = ";

                            }
                        }
                        else
                        {
                                cmd.CommandText += "" + i + 1 + ") ";
                        }

                        cmd.CommandText += "GROUP BY Б.Название_компетенции_студента, Б.Код_компетенции_студента";
                        SqlDataReader reader = cmd.ExecuteReader();
                        int t = 0;
                        while (reader.Read())
                        {
                            Array.Resize(ref words_of_columns_students_semestr, words_of_columns_students_semestr.Length + 1);
                            words_of_columns_students_semestr[t] = reader["Название_компетенции_студента"].ToString();
                            words_of_columns_students_semestr[t] = words_of_columns_students_semestr[t].Replace(" ", "");
                            t++;
                        }
                        connection.Close();
                    }

                    if (words_of_columns_students_semestr.Length == 0)
                    {
                        MessageBox.Show("Нет данных по компетенциям данной группы!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    else
                    {

                        //получение формирующихся компетенций за семестр
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = @"DELETE FROM Temp_1
                        INSERT INTO Temp_1   
                       (Название_компетенции_студента, Процент_сформированности_компетенции)  
                        SELECT Б.Название_компетенции_студента, AVG(А.Процент_сформированности_компетенции)
                                FROM Направления_обучения В, Студент Д, 
                                Уровень_сформированности_компетенций_студентов А, 
                                Компетенции_студентов Б, Дисциплины Г, Направления_Дисциплины Е, Дисциплины_Компетенции_студентов Ж
                                WHERE Д.Код_направления_обучения = В.Код_направления_обучения
                                AND А.Код_направления_обучения = В.Код_направления_обучения
                                AND А.Код_компетенции_студента = Б.Код_компетенции_студента
                                AND Б.Код_направления_обучения = В.Код_направления_обучения
                                AND Г.Код_дисциплины = Е.Код_дисциплины
                                AND Е.Код_направления_обучения = В.Код_направления_обучения
                                AND Ж.Код_дисциплины = Г.Код_дисциплины
                                AND Ж.Код_компетенции_студента = Б.Код_компетенции_студента
                                AND А.Код_студента = Д.Код_студента
                                AND А.Код_компетенции_студента = Б.Код_компетенции_студента
                                AND В.Название_направления = '" + comboBox7.Text + "' " +
                                        "AND Д.Номер_группы = '" + comboBox5.Text + "' " +
                                                    @"AND (Г.Семестр = ";
                            int ctetchik;
                            if (i > 0)
                            {
                                ctetchik = i;
                                for (int w = 0; w <= i; w++)
                                {
                                    if (w == i)
                                        cmd.CommandText += "" + (w + 1) + ") ";
                                    else
                                        cmd.CommandText += "" + (w + 1) + " or Г.Семестр = ";

                                }
                            }
                            else
                            {
                                cmd.CommandText += "" + i + 1 + ")";
                            }
                            cmd.CommandText += " GROUP BY Б.Название_компетенции_студента, Б.Код_компетенции_студента";
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }

                        sql = @"SELECT * FROM Temp_1";
                        print(sql);


                        worksheet.get_Range("A" + (number_of_cell + 10), "Z" + (number_of_cell + 10)).Font.Bold = true;
                        worksheet.get_Range("A" + (number_of_cell + 10), "Z" + (number_of_cell + 10)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        worksheet.get_Range("A" + (number_of_cell + 11), "Z" + +(number_of_cell + 11)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        int count_comp_stud_more_comp_vac = 0;

                        kol = (number_of_cell + 10);
                        int n = 1, d = 1, counter = 0, j = 1;
                        while (j < dataGridView1.ColumnCount)
                        {
                            for (int k = 0; k < words_of_columns_vacancy.Length; k++)
                            {
                                if (words_of_columns_students_semestr[counter] == words_of_columns_vacancy[k])
                                {
                                    object result = dataGridView1.Rows[d - 1].Cells[j].Value.ToString();
                                    object result_1 = worksheet.Rows[number_of_cell + 6].Columns[n].Value.ToString();
                                    if (Convert.ToDouble(result) >= Convert.ToDouble(result_1))
                                        count_comp_stud_more_comp_vac += 1;
                                    worksheet.Rows[kol].Columns[n] = words_of_columns_vacancy[k];
                                    worksheet.Rows[kol + 1].Columns[n] = dataGridView1.Rows[d - 1].Cells[j].Value;
                                    d++;
                                    counter++;
                                }
                                else
                                {
                                    worksheet.Rows[kol].Columns[n] = words_of_columns_vacancy[k];
                                    worksheet.Rows[kol + 1].Columns[n] = 0;
                                }
                                n++;
                            }
                            j++;
                        }

                        _excelCells1 = (Excel.Range)worksheet.get_Range("E" + (number_of_cell + 13), "I" + (number_of_cell + 13)).Cells;
                        _excelCells1.Merge();
                        worksheet.get_Range("E" + (number_of_cell + 13), "I" + (number_of_cell + 13)).EntireRow.AutoFit();
                        worksheet.get_Range("E" + (number_of_cell + 13), "I" + (number_of_cell + 13)).RowHeight = 60;
                        worksheet.get_Range("E" + (number_of_cell + 13), "I" + (number_of_cell + 13)).Font.Bold = true;
                        worksheet.get_Range("E" + (number_of_cell + 13), "I" + (number_of_cell + 13)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        worksheet.get_Range("E" + (number_of_cell + 13), "I" + (number_of_cell + 13)).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                        worksheet.Cells[(number_of_cell + 13), 5] = @"Процент компетенций, которые подходят к вакансии: " + ((float)count_comp_stud_more_comp_vac) / words_of_columns_students_semestr.Length;
                    }
                    number_of_cell += 17;
                }               
            }

            if (words_of_columns_students_semestr.Length > 0)
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
                workbook.SaveAs(path);
                if (MessageBox.Show("Отчет готов! Хотите открыть его?", "Ура!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    excelapp.Visible = true;
                }
                else
                {
                    MessageBox.Show("Ищите отчет в C:\\Users\\Admin\\Downloads\\WindowsFormsApp1\\WindowsFormsApp1\\bin\\Debug", "Отчет готов!", MessageBoxButtons.OK);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] yes_no = new string[] { comboBox1.Text,  comboBox2.Text, comboBox3.Text, comboBox4.Text, 
                    comboBox6.Text, comboBox8.Text, comboBox10.Text, comboBox9.Text, };
            get_otchet(yes_no);
            
        }
    }
}
