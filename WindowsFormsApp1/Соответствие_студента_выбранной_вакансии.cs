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
    public partial class Соответствие_студента_выбранной_вакансии : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        public static string name_of_vacancy;
        public static string login, password;
        public static string dir_of_study;
        int number_of_comp;
        int id_of_student;


        public Соответствие_студента_выбранной_вакансии()
        {
            InitializeComponent();
            login = Проверка_соответствия_студента_требованиям_вакансии.log;
            password = Проверка_соответствия_студента_требованиям_вакансии.pass;
            name_of_vacancy = Проверка_соответствия_студента_требованиям_вакансии.name_of_vac;
            dir_of_study = Проверка_соответствия_студента_требованиям_вакансии.direction_of_study;
            number_of_comp = Проверка_соответствия_студента_требованиям_вакансии.number_of_comp;
            FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        //список промежуточных результатов компетенций
        public class Comp_requi_stud
        {
            public string name_of_comp { get; set; }
            public float required { get; set; }
            public float result { get; set; }
        }

        private void подсказкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Если не все на графике подписано, просто наведите на интересующий Вас столбец и вы увидете, к какой компетенции он относится и какое его точное значение", "Подсказка!", MessageBoxButtons.OK);
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Проверка_соответствия_студента_требованиям_вакансии newForm = new Проверка_соответствия_студента_требованиям_вакансии();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }


        //выравнивание текста в listBox по центру
        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, listBox1.Items[e.Index].ToString(), e.Font,
                e.Bounds, e.ForeColor, e.BackColor, TextFormatFlags.HorizontalCenter);
        }

        public class Comp_of_direction
        {
            public string names { get; set; }
            public string discription { get; set; }
            public override string ToString()
            {
                return names + " - " + discription;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Список_компетенций_по_вакансии newForm = new Список_компетенций_по_вакансии();
            newForm.ShowDialog();
            Show();
        }

        private void вГлавноеМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Главное_меню_студента newForm = new Главное_меню_студента();
            Hide();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Учебные_материалы_по_компетенциям newForm = new Учебные_материалы_по_компетенциям();
            newForm.ShowDialog();
            Show();
        }

        private void Соответствие_студента_выбранной_вакансии_Load(object sender, EventArgs e)
        {
            //определяем ФИО студента
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(); SqlCommand cmd_1 = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT ФИО_студента FROM Студент WHERE Логин_студента = '" + login +
                                    "' AND Пароль_студента = '" + password + "'";
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

            textBox2.Text = name_of_vacancy;
            textBox2.ReadOnly = true;
            textBox2.Font = new System.Drawing.Font("ArialBlack", 9, FontStyle.Regular);

            // определяем айди студента
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(); SqlCommand cmd_1 = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT Код_студента FROM Студент WHERE Логин_студента = '" + login +
                                    "' AND Пароль_студента = '" + password + "'";
                id_of_student = (int)cmd.ExecuteScalar();
                connection.Close();
            }

            List<Comp_requi_stud> points_of_schedule = new List<Comp_requi_stud>();

            //определяем точки таблицы 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT Ж.Название_компетенции_студента, Е.Нач_интервал, Д.Процент_сформированности_компетенции
                                    FROM Студент А, Направления_обучения Б, Вакансии В, Компетенции_студентов Ж,
                                    Уровни_сформированности_компетенций Г,
                                    Уровень_сформированности_компетенций_студентов Д, Уровни_ВСНБ_сформ_комп Е
                                    WHERE А.Код_студента = Д.Код_студента
										AND А.Код_направления_обучения = Б.Код_направления_обучения
                                        AND В.Код_направления_обучения = Б.Код_направления_обучения
										AND Д.Код_направления_обучения = Б.Код_направления_обучения
                                        AND Д.Код_компетенции_студента = Ж.Код_компетенции_студента
										AND Д.Код_компетенции_студента = Г.Код_компетенции_студента
										AND В.Код_вакансии = Г.Код_вакансии
                                        AND Г.Код_компетенции_студента = Ж.Код_компетенции_студента
                                        AND Е.Код_уровня_ВСНБ = Г.Код_уровня_ВСНБ  
                                        AND А.Код_студента = " + id_of_student + " AND В.Название_вакансии = '" + textBox2.Text + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    points_of_schedule.Add(new Comp_requi_stud()
                    {
                        name_of_comp = reader["Название_компетенции_студента"].ToString(),
                        required = float.Parse(reader["Нач_интервал"].ToString()),
                        result = float.Parse(reader["Процент_сформированности_компетенции"].ToString()),
                    });
                }
                connection.Close();
            }

            //рисуем график
            for (int i = 0; i < number_of_comp; i++)
            {
                points_of_schedule[i].name_of_comp = points_of_schedule[i].name_of_comp.Replace(" ", "");
                chart1.Series[0].Points.AddXY(points_of_schedule[i].name_of_comp, points_of_schedule[i].required);
                chart1.Series[0].ToolTip = "X = #VALX, Y = #VALY";
                chart1.Series[1].Points.AddXY(points_of_schedule[i].name_of_comp, points_of_schedule[i].result);
                chart1.Series[1].ToolTip = "X = #VALX, Y = #VALY";
            }

            int counter_req = 0, counter_res = 0;
            string[] comp = new string[] { };
            int k = 0;
            for (int i = 0; i < number_of_comp; i++)
            {
                if (points_of_schedule[i].required > points_of_schedule[i].result)
                {
                    counter_req++;
                    Array.Resize(ref comp, comp.Length + 1);
                    comp[k] = points_of_schedule[i].name_of_comp;
                    k++;
                }
                else
                    counter_res++;
            }

            float percent_of_col_comp = (float)counter_res / number_of_comp;

            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += ListBox1_DrawItem;

            if (counter_res == number_of_comp)
            {
                listBox1.Items.Add("Поздравляем! " + percent_of_col_comp*100 + " % ваших компетенций соответствуют требованиям вакансии.");
                listBox1.Items.Add("Так держать!");
            }
            else if (counter_req == number_of_comp)
                listBox1.Items.Add("К сожалению, вы полностью не подходите под требования вакансии. Если вас интересует " +
                    "данная вакансия, пожалуйста, приложите все усилия для улучшения Ваших компетенций!");
            else
            {
                string str = "";
                for (int i = 0; i < comp.Length; i++)
                {
                    str += comp[i] + ", ";
                }

                str = str.Remove(str.Length - 2) + ". ";
                listBox1.Font = new System.Drawing.Font("ArialBlack", 9, FontStyle.Regular);
                listBox1.Items.Add("Интересные новости! " + percent_of_col_comp * 100 + " % ваших компетенций соответсвуют требованиям вакансии.");
                listBox1.Items.Add("Вам следует обратить внимание на следующие компетенции: ");
                listBox1.Items.Add(str);
                listBox1.Items.Add("Пожалуйста, приложите все усилия для их улучшения!");
                listBox1.Items.Add("Успехов!");
            }
        }
    }
}

