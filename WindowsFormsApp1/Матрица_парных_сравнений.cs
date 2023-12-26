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
    public partial class Матрица_парных_сравнений : Form
    {
        public static int number_of_vac;
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        SqlDataAdapter adapter;
        DataSet ds = new DataSet();
        DataSet db = new DataSet();
        string sql;
        int number_of_column = 0;
        string[] words_of_columns = new string[] { };
        string selected_competention = "";
        string type_of_comp = "";
        int id_of_comp;
        int[] count_diff_study_comp_of_vac = new int[] { };
        string[] types_of_competit = new string[3] { "ПК", "УК", "ОПК" };
        int[] count_of_comp_for_vac = new int[3];

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

        private void print_datagridview2() // функция отображения таблицы
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                sql = "SELECT В.Название_компетенции_студента, В.Описание_компетенции " +
                                    "FROM Вакансии А, Направления_обучения Б, Компетенции_студентов В " +
                                    "WHERE А.Код_направления_обучения = Б.Код_направления_обучения " +
                                    "AND Б.Код_направления_обучения = В.Код_направления_обучения " +
                                    "AND А.Код_вакансии = " + number_of_vac +
                                    " AND В.Тип_компетенции = '" + comboBox2.Text.ToString() + "'";
                db.Reset(); // обновление dataGridView2
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                db = new DataSet();
                adapter.Fill(db);
                dataGridView2.DataSource = db.Tables[0];
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                connection.Close();
            }
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public class Competentions_of_vacancy
        {
            public string description;
            public override string ToString()
            {
                return string.Format("{0}", description);
            }
        }

        //размер матрицы + отображение количества компетенций по направлению
        public void size_of_matrix()
        {
            //отображения в текстбоксах
            textBox5.Text = count_of_comp_for_vac[0].ToString();
            textBox5.ReadOnly = true;
            textBox6.Text = count_of_comp_for_vac[1].ToString();
            textBox6.ReadOnly = true;
            textBox4.Text = count_of_comp_for_vac[2].ToString();
            textBox4.ReadOnly = true;
            switch (type_of_comp)
            {
                case "ПК":
                    textBox1.Text = count_of_comp_for_vac[0].ToString();
                    break;
                case "УК":
                    textBox1.Text = count_of_comp_for_vac[1].ToString();
                    break;
                case "ОПК":
                    textBox1.Text = count_of_comp_for_vac[2].ToString();
                    break;
            }
            textBox1.ReadOnly = true;
            number_of_column = Int32.Parse(textBox1.Text.ToString());
        }

        //получить названия столбцов для матрицы
        public void receive_names_of_comp()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                Array.Resize(ref words_of_columns, 0);
                cmd.CommandText = @"SELECT В.Название_компетенции_студента 
                                            FROM Направления_обучения А, Вакансии Б, Компетенции_студентов В
                                            WHERE А.Код_направления_обучения = В.Код_направления_обучения AND 
	                                            Б.Код_направления_обучения = А.Код_направления_обучения AND 
	                                            Б.Код_вакансии = " + number_of_vac +
                                                " AND В.Тип_компетенции = '" + comboBox2.Text.ToString() + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                int k = 0;
                while (reader.Read())
                {
                    Array.Resize(ref words_of_columns, words_of_columns.Length + 1);
                    words_of_columns[k] = reader["Название_компетенции_студента"].ToString();
                    words_of_columns[k] = words_of_columns[k].Replace(" ", "");
                    k++;
                }
                connection.Close();
            }
        }

        public Матрица_парных_сравнений()
        {
            InitializeComponent();
            dataGridView1.ReadOnly = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = true;
            print_datagridview2();
            button7.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
        }

        //Начало программы
        private void Матрица_парных_сравнений_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            number_of_vac = Ознакомление_эксперта_с_данными.number_of_vacancy;
            SqlConnection connect = new SqlConnection(connectionString);

            for (int i = 0; i < types_of_competit.Length; i++)
            {
                comboBox2.Items.Add(types_of_competit[i]);
                if (i == 0)
                {
                    comboBox2.SelectedItem = ("ПК");
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT Название_вакансии FROM Вакансии WHERE Код_вакансии = " + number_of_vac;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox2.Text = reader["Название_вакансии"].ToString();
                }
                textBox2.ReadOnly = true;
                reader.Close();
                connection.Close();
            }

            //показ количества ПК, УК, ОПК
            object result;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                for (int j = 0; j < types_of_competit.Length; j++)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT COUNT(В.Тип_компетенции) FROM Направления_обучения А, Вакансии Б, Компетенции_студентов В 
                                        WHERE А.Код_направления_обучения = В.Код_направления_обучения " +
                                        "AND Б.Код_направления_обучения = А.Код_направления_обучения" +
                                        "  AND В.Тип_компетенции = '" + types_of_competit[j] + "'  AND Б.Код_вакансии = " + number_of_vac;
                    result = cmd.ExecuteScalar();
                    count_of_comp_for_vac[j] = Convert.ToInt32(result);
                    //записываем количество компетенций студентов в каждой группе компетенций
                    Array.Resize(ref count_diff_study_comp_of_vac, count_diff_study_comp_of_vac.Length + 1);
                    count_diff_study_comp_of_vac[j] = Convert.ToInt32(result);
                }
                connection.Close();
            }

            size_of_matrix();
            receive_names_of_comp();
        }

        //проверка корректного ввода размерности таблицы и создание в БД данной таблицы
        private void button1_Click(object sender, EventArgs e)
        {
            button7.Enabled = true; button4.Enabled = true;
            button3.Enabled = true;

            SqlConnection connect = new SqlConnection(connectionString);
            selected_competention = comboBox1.Text.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT Код_компетенции_вакансии
                                            FROM Полученный_список_компетенций_по_вакансии
                                            WHERE Описание = '" + selected_competention + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    selected_competention = reader["Код_компетенции_вакансии"].ToString();
                    id_of_comp = Int32.Parse(selected_competention);
                }

                reader.Close();
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"DROP TABLE Temporal_matrix
                                        CREATE TABLE Temporal_matrix(";
                for (int i = 0; i < number_of_column; i++)
                {
                    cmd.CommandText += "[" + words_of_columns[i] + "] FLOAT NULL, ";

                }
                cmd.CommandText += ")";
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            sql = "SELECT * FROM Temporal_matrix";
            print(sql);
        }

        //список промежуточных результатов компетенций
        public class Mass_priorit
        {
            public int id_of_vacancy { get; set; }
            public int id_of_compet { get; set; }
            public string tp_of_compet { get; set; }
            public string name_of_comp { get; set; }
            public float result { get; set; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] words_col;
            Array.Copy(words_of_columns, words_col = new string[words_of_columns.Length], words_of_columns.Length);
            float sum_of_strok, general_sum = 0;
            float[] mass_sum_of_strok = new float[] { };
            float[][] mass_cov = new float[][] { };
            string name = "";
            int c = 0, nulls = 0;

            //если матрица не имеет строчек
            if (dataGridView1.Rows.Count - 1 == 0)
            {
                MessageBox.Show("В матрице ничего нет!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //проверка на пустоты ячеек матрицы и простановку нулей в таблицу
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < words_of_columns.Length; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value.ToString() == "")
                    {
                        c++;
                    }
                    else if (dataGridView1.Rows[i].Cells[j].Value.ToString() == "0")
                    {
                        nulls++;
                    }
                }
            }

            if (c == 0 && nulls == 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (dataGridView1.RowCount == number_of_column + 1)
                    {
                        for (int i = 0; i < number_of_column; i++)
                        {
                            words_col[i] = words_col[i].Insert(0, "@");
                            words_col[i] = words_col[i].Replace("-", "_");
                        }

                        //запрос на добавление данных в таблицу (можно убрать, но пусть будет)
                        SqlCommand insert = new SqlCommand();
                        string query = "TRUNCATE TABLE Temporal_matrix";
                        insert = new SqlCommand(query, connection);
                        connection.Open();
                        insert.ExecuteNonQuery();
                        connection.Close();

                        for (int i = 0; i < words_of_columns.Length; i++)
                        {
                            sum_of_strok = 0;
                            insert = new SqlCommand();
                            query = "INSERT INTO Temporal_matrix([";
                            for (int n = 0; n < words_of_columns.Length; n++)
                            {
                                query += words_of_columns[n] + "], [";
                            }
                            query = query.Remove((query.Length - 3), 3);
                            query += ") VALUES (";
                            for (int m = 0; m < words_col.Length; m++)
                            {
                                query += words_col[m] + ", ";
                            }
                            query = query.Remove((query.Length - 2), 2);
                            query += ")";
                            insert.Parameters.Clear();
                            insert = new SqlCommand(query, connection);
                            connection.Open();
                            sum_of_strok = 0;
                            for (int j = 0; j < words_of_columns.Length; j++)
                            {
                                name = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                insert.Parameters.AddWithValue(words_col[j], float.Parse(name));
                                //сумма значений в строке
                                sum_of_strok += float.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString());
                            }
                            //записываем сумму значений для каждой строки в массив
                            Array.Resize(ref mass_sum_of_strok, mass_sum_of_strok.Length + 1);
                            mass_sum_of_strok[i] = sum_of_strok;
                            sum_of_strok = 0;
                            insert.ExecuteNonQuery();
                            connection.Close();
                        }

                        //общая сумма значений в текущей матрице
                        for (int i = 0; i < mass_sum_of_strok.Length; i++)
                        {
                            general_sum += mass_sum_of_strok[i];
                        }

                        var mass_prior = new List<Mass_priorit>();
                        //Промежуточная таблица приоритетов. Заполнение
                        for (int i = 0; i < mass_sum_of_strok.Length; i++)
                        {
                            mass_prior.Add(new Mass_priorit()
                            {
                                id_of_vacancy = number_of_vac,
                                id_of_compet = id_of_comp,
                                tp_of_compet = type_of_comp,
                                name_of_comp = words_of_columns[i],
                                result = (float)mass_sum_of_strok[i] / general_sum
                            });
                        }

                        general_sum = 0;

                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        //если пользователь исправил данные в таблице, удаляем строки, которые будут заново добавлены в таблицу
                        for (int j = 0; j < mass_prior.Count; j++)
                        {
                            cmd.Connection = connection;
                            cmd.CommandText = @"DELETE FROM Промежуточная_таблица_приоритетов
                                            WHERE Код_вакансии = " + mass_prior[j].id_of_vacancy +
                                                " AND Код_компетенции_вакансии = " + mass_prior[j].id_of_compet +
                                                " AND Тип_компетенции = '" + mass_prior[j].tp_of_compet +
                                                "' AND Название_компетенции_студента = '" + mass_prior[j].name_of_comp + "'";
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();

                        query = @"INSERT INTO Промежуточная_таблица_приоритетов(Код_вакансии, Код_компетенции_вакансии, Тип_компетенции,
                                Название_компетенции_студента, Промежуточное_значение_приоритета) VALUES (" +
                                    "@Код_вакансии, @Код_компетенции_вакансии, @Тип_компетенции, @Название_компетенции_студента, " +
                                    "@Промежуточное_значение_приоритета)";
                        insert = new SqlCommand(query, connection);
                        connection.Open();
                        int[] k = new int[mass_prior.Count];
                        //добавляем промежуточные приоритеты в таблицу
                        for (int j = 0; j < mass_prior.Count; j++)
                        {
                            insert.Parameters.Clear();
                            insert.Parameters.AddWithValue("@Код_вакансии", mass_prior[j].id_of_vacancy);
                            insert.Parameters.AddWithValue("@Код_компетенции_вакансии", mass_prior[j].id_of_compet);
                            insert.Parameters.AddWithValue("@Тип_компетенции", mass_prior[j].tp_of_compet);
                            insert.Parameters.AddWithValue("@Название_компетенции_студента", mass_prior[j].name_of_comp);
                            insert.Parameters.AddWithValue("@Промежуточное_значение_приоритета", mass_prior[j].result);
                            insert.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Количество строк и столбцов вашей матрицы не совпадают. Пожалуйста, очистите лишние строки таблицы!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Матрица содержит пустую(ые) строку(и) или/и '0' в ячейке(ках)! Пожалуйста, введите корректные значения матрицы!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //возможность удаления строки из матрицы, если пользователь ввел лишний строки (матрица должна быть NxN)
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                int delet = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows.RemoveAt(delet);
            }
            catch
            {
                MessageBox.Show("Строчка одна, ее нельзя удалить!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //получить значение типа компетенции
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            type_of_comp = comboBox2.Text.ToString();
            size_of_matrix();
            receive_names_of_comp();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand2 = new SqlCommand();
                sqlCommand2.Connection = connection;
                sqlCommand2.CommandText = @"SELECT Полученный_список_компетенций_по_вакансии.Описание
                                            FROM Полученный_список_компетенций_по_вакансии, Вакансии
	                                        WHERE
		                                        Вакансии.Код_вакансии = Полученный_список_компетенций_по_вакансии.Код_вакансии AND
			                                    Вакансии.Код_вакансии =" + number_of_vac + " AND " +
                                                "Полученный_список_компетенций_по_вакансии.Тип_компетенции = '" + comboBox2.Text.ToString() + "'";
                sqlCommand2.Connection = connection;
                List<Competentions_of_vacancy> comp_of_vacancy = new List<Competentions_of_vacancy>();
                {
                    SqlDataReader dr = sqlCommand2.ExecuteReader();
                    while (dr.Read())
                    {
                        Competentions_of_vacancy coun = new Competentions_of_vacancy
                        {
                            description = (dr["Описание"].ToString())
                        };
                        comp_of_vacancy.Add(coun);
                    }
                }
                comboBox1.DataSource = comp_of_vacancy;
                connection.Close();

            }
            print_datagridview2();
        }

        //показать таблицу глобальных приоритетов
        private void button4_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button7.Enabled = false;
            //проверка, есть ли хотя бы одна матрица парных сравнений для каждого типа компетенции студентов
            int[] checking = new int[types_of_competit.Length];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                int chet = 0;
                for (int j = 0; j < checking.Length; j++)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT COUNT(DISTINCT Код_компетенции_вакансии) 
                                        FROM Промежуточная_таблица_приоритетов 
                                        WHERE Код_вакансии =" + number_of_vac +
                                        " AND Тип_компетенции = '" + types_of_competit[j] +
                                        "' AND Код_вакансии = " + number_of_vac;
                    checking[j] = (int)cmd.ExecuteScalar();
                    if (checking[j] >= 1)
                        chet++;
                }
                connection.Close();
                if (chet != types_of_competit.Length)
                    MessageBox.Show("Для подсчета глобальных приоритетов необходимо заполнить не менее одной матрицы парных сравнений для каждого типа компетенции!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            button7.Enabled = false;
            //очищаем данные для их обновления, если были посчитаны глобальные приоритеты для рассматриваемой акансии
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.Parameters.Clear();
                cmd.CommandText = @"DELETE FROM Приоритеты_компетенций_по_вакансиям
                                            WHERE Код_вакансии = " + number_of_vac;
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            //считаем компетенции вакансии
            int[] count_of_comp_for_vac = new int[types_of_competit.Length];
            selected_competention = comboBox1.Text.ToString();
            object result;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                for (int j = 0; j < types_of_competit.Length; j++)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT COUNT(А.Тип_компетенции) FROM Полученный_список_компетенций_по_вакансии А
                                    WHERE А.Код_вакансии = " + number_of_vac + " AND А.Тип_компетенции = '" + types_of_competit[j] + "'";
                    result = cmd.ExecuteScalar();
                    count_of_comp_for_vac[j] = Convert.ToInt32(result);
                }
                connection.Close();
            }

            //для БД float необходимо представить в следующем виде: 0.00 + считаем вес компетенций 1/(их количество)
            float[] chislo_1 = new float[count_of_comp_for_vac.Length];
            string[] chislo = new string[count_of_comp_for_vac.Length];
            for (int i = 0; i < count_of_comp_for_vac.Length; i++)
            {
                chislo_1[i] = (float)1 / count_of_comp_for_vac[i];
                chislo[i] = chislo_1[i].ToString();
                chislo[i] = chislo[i].Replace(",", ".");
            }

            //сам расчет глобальных компетенций
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                connection.Open();
                cmd.Connection = connection;
                for (int i = 0; i < types_of_competit.Length; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"INSERT INTO Приоритеты_компетенций_по_вакансиям(Код_вакансии, Код_компетенции_студента, 
                                    Тип_компетенции, Название_компетенции_студента, Процент_приоритета_компетенции_вакансии) 
                                    SELECT В.Код_вакансии, Г.Код_компетенции_студента, Б.Тип_компетенции, 
                                    Б.Название_компетенции_студента, " + chislo[i] + "*SUM(Б.Промежуточное_значение_приоритета)" +
                                    " AS Процент_приоритета_компетенции_вакансии " +
                                    "FROM Промежуточная_таблица_приоритетов Б, Вакансии В, Направления_обучения А," +
                                    "Компетенции_студентов Г " +
                                    "WHERE В.Код_вакансии = Б.Код_вакансии " +
                                            "AND Б.Тип_компетенции = '" + types_of_competit[i] + "' " +
                                            "AND Б.Код_вакансии = " + number_of_vac +
                                            " AND А.Код_направления_обучения = Г.Код_направления_обучения" +
                                            " AND В.Код_направления_обучения = А.Код_направления_обучения" +
                                            " AND Б.Название_компетенции_студента = Г.Название_компетенции_студента" +
                                    " GROUP BY В.Код_вакансии, Г.Код_компетенции_студента, Б.Тип_компетенции, Б.Название_компетенции_студента";
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                //sql = "SELECT * FROM Приоритеты_компетенций_по_вакансиям";
                //print(sql);
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                required_competency_levels_vacancy();
            }
        }

        //вернуться назад
        private void вернутьсяНазадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ознакомление_эксперта_с_данными newForm = new Ознакомление_эксперта_с_данными();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //список промежуточных результатов компетенций
        public class Level_of_form_comp
        {
            public int id_of_vacancy { get; set; }
            public string name_of_comp { get; set; }
            public int level { get; set; }
        }

        //определяем необходимые уровни для каждой компетенции и заносим в таблицу
        private void add_level_of_comp(string[] arr, double[] interv)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                var levels_of_comp = new List<Level_of_form_comp>();
                float res = 0;
                int lev = 0;
                for (int j = 0; j < arr.Length; j++)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT DISTINCT В.Процент_приоритета_компетенции_вакансии FROM Вакансии А, 
                                                Направления_обучения Б, 
                                                Приоритеты_компетенций_по_вакансиям В, Компетенции_студентов Г
                                                WHERE А.Код_направления_обучения = Б.Код_направления_обучения
                                                AND В.Код_вакансии = А.Код_вакансии
                                                AND Б.Код_направления_обучения = Г.Код_направления_обучения
                                                AND В.Название_компетенции_студента = '" + arr[j] + "' " +
                                            "AND В.Код_вакансии = " + number_of_vac;
                    res = (float)Convert.ToDouble(cmd.ExecuteScalar());
                    if ((res > interv[0]) && (res <= interv[1]))
                    {
                        lev = 1;
                    }
                    else if ((res > interv[1]) && (res <= interv[2]))
                    {
                        lev = 2;
                    }
                    else if ((res > interv[2]) && (res <= interv[3]))
                    {
                        lev = 3;
                    }
                    else if ((res > interv[3]) && (res <= interv[4]))
                    {
                        lev = 4;
                    }
                    levels_of_comp.Add(new Level_of_form_comp()
                    {
                        id_of_vacancy = number_of_vac,
                        name_of_comp = arr[j],
                        level = lev
                    });
                }

                string query = @"INSERT INTO Уровни_сформированности_компетенций(Код_вакансии, Название_компетенции_студента, 
                                    Код_уровня_ВСНБ) VALUES (" +
                                    "@Код_вакансии, @Название_компетенции_студента, " +
                                    "@Код_уровня_ВСНБ)";
                SqlCommand insert = new SqlCommand();
                insert = new SqlCommand(query, connection);
                for (int j = 0; j < levels_of_comp.Count; j++)
                {
                    insert.Parameters.Clear();
                    insert.Parameters.AddWithValue("@Код_вакансии", levels_of_comp[j].id_of_vacancy);
                    insert.Parameters.AddWithValue("@Название_компетенции_студента", levels_of_comp[j].name_of_comp.ToString());
                    insert.Parameters.AddWithValue("@Код_уровня_ВСНБ", levels_of_comp[j].level.ToString());

                    insert.ExecuteNonQuery();
                }
                connection.Close();
            }

        }

        //определяем уровень сформированности компетенции
        void required_competency_levels_vacancy()
        {
            button7.Enabled = false;
            int[] count = new int[types_of_competit.Length];
            int c = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                for (int i = 0; i < types_of_competit.Length; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = $"SELECT COUNT(Код_вакансии) FROM Приоритеты_компетенций_по_вакансиям " +
                                       $"WHERE " +
                                        "Код_вакансии = " + number_of_vac +
                                        " AND Тип_компетенции = '" + types_of_competit[i] + "'";
                    count[i] = (int)cmd.ExecuteScalar();
                    if (count[i] > 0)
                        c++;
                }
                connection.Close();
            }

            //проверка на наличие нужных данных по всем трем группам компетенций студентов для рассматриваемой вакансии
            if (c == types_of_competit.Length)
            {
                double[] max_glob_prior_in_group = new double[types_of_competit.Length];
                SqlConnection connect = new SqlConnection(connectionString);
                object result;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    for (int j = 0; j < types_of_competit.Length; j++)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"SELECT MAX(Процент_приоритета_компетенции_вакансии) FROM Приоритеты_компетенций_по_вакансиям 
                                    WHERE Код_вакансии = " + number_of_vac + " AND Тип_компетенции = '" + types_of_competit[j] + "'";
                        result = cmd.ExecuteScalar();
                        max_glob_prior_in_group[j] = Convert.ToDouble(result);
                    }
                    connection.Close();
                }

                //шаг
                double[] pass = new double[types_of_competit.Length];
                //массив интервалов
                double[,] intervals = new double[types_of_competit.Length, 5];

                //определяем шаги для определения интервалов
                for (int i = 0; i < max_glob_prior_in_group.Length; i++)
                {
                    pass[i] = (double)max_glob_prior_in_group[i] / 4;
                }

                //получение интервалов для каждого типа компетенции
                double chag;
                for (int i = 0; i < intervals.GetLength(0); i++)
                {
                    chag = pass[i];
                    intervals[i, 0] = 0;
                    for (int j = 1; j < intervals.GetLength(1); j++)
                    {
                        intervals[i, j] = chag;
                        chag += pass[i];
                    }
                }

                string[] names_of_pc = new string[count_of_comp_for_vac[0]];
                string[] names_of_uc = new string[count_of_comp_for_vac[1]];
                string[] names_of_opc = new string[count_of_comp_for_vac[2]];

                int k = 0, v = 0, b = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;

                    //заполняем названия компетенций
                    for (int i = 0; i < types_of_competit.Length; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"SELECT DISTINCT В.Название_компетенции_студента FROM Вакансии А, 
                                                Направления_обучения Б, 
                                                Приоритеты_компетенций_по_вакансиям В, Компетенции_студентов Г
                                                WHERE А.Код_направления_обучения = Б.Код_направления_обучения
                                                AND В.Код_вакансии = А.Код_вакансии
                                                AND Б.Код_направления_обучения = Г.Код_направления_обучения
                                                AND В.Тип_компетенции = '" + types_of_competit[i] + "' " +
                                                "AND В.Код_вакансии = " + number_of_vac;

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            switch (i)
                            {
                                case 0:
                                    names_of_pc[k] = reader["Название_компетенции_студента"].ToString();
                                    names_of_pc[k] = names_of_pc[k].Replace(" ", "");
                                    k++;
                                    break;
                                case 1:
                                    names_of_uc[b] = reader["Название_компетенции_студента"].ToString();
                                    names_of_uc[b] = names_of_uc[b].Replace(" ", "");
                                    b++;
                                    break;
                                case 2:
                                    names_of_opc[v] = reader["Название_компетенции_студента"].ToString();
                                    names_of_opc[v] = names_of_opc[v].Replace(" ", "");
                                    v++;
                                    break;
                            }
                        }
                        k = 0; b = 0; v = 0;
                        reader.Close();
                    }
                    connection.Close();
                }

                //удаляем данные если есть для обновления таблицы
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"DELETE FROM Уровни_сформированности_компетенций
                                            WHERE Код_вакансии = " + number_of_vac;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                double[] intervali_pc = new double[5];
                double[] intervali_uc = new double[5];
                double[] intervali_opc = new double[5];

                //устанавливаем интервалы для каждой группы компетенций
                for (int i = 0; i < intervals.GetLength(0); i++)
                {
                    for (int j = 0; j < intervals.GetLength(1); j++)
                    {
                        if (i == 0)
                            intervali_pc[j] = intervals[i, j];
                        if (i == 1)
                            intervali_uc[j] = intervals[i, j];
                        if (i == 2)
                            intervali_opc[j] = intervals[i, j];
                    }
                }

                //определяем требуемые уровни каждой группы компетенций и записываем в таблицу
                add_level_of_comp(names_of_pc, intervali_pc);
                add_level_of_comp(names_of_uc, intervali_uc);
                add_level_of_comp(names_of_opc, intervali_opc);

                //отображение таблицы
                sql = @"SELECT А.Код_вакансии, А.Название_компетенции_студента, Б.Требуемый_уровень_сформированности_компетенции
                        FROM Уровни_сформированности_компетенций А, Уровни_ВСНБ_сформ_комп Б
                        WHERE Б.Код_уровня_ВСНБ = А.Код_уровня_ВСНБ";
                print(sql);
                MessageBox.Show("Поздравляем Вас с успешным определением требуемого уровня компетенций для вакансии '" + textBox2.Text.ToString() + "'!", "Ура!", MessageBoxButtons.OK);

            }
            else if ((c > 0) && (c < types_of_competit.Length))
            {
                MessageBox.Show("Для вакансии не были посчитаны все глобальные приоритеты из 3-х видов компетенций! Какого-то вида не хватает!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Глобальные приоритеты для вакансии не были посчитаны!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

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
            else if (Главное_меню_учебного_отдела_эксперта.accessin == "Ex")
            {
                Главное_меню_учебного_отдела_эксперта newForm = new Главное_меню_учебного_отдела_эксперта();
                Hide();
                newForm.ShowDialog();
                Show();
                this.Close();
            }
        }
    }
}
