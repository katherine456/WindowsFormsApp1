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
using System.Drawing.Drawing2D;//add this namespace


namespace WindowsFormsApp1
{
    public partial class Вход_в_систему : Form
    {
        String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
        public static string loginUser;
        public static string passwordUser;

        public Вход_в_систему()
        {
            InitializeComponent();
            InitializeMyControl();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void InitializeMyControl()
        {
            // Set to no text.
            textBox2.Text = "";
            // The password character is an asterisk.
            textBox2.PasswordChar = '*';
            // The control will allow no more than 50 characters.
            textBox2.MaxLength = 50;
            textBox1.MaxLength = 50;
        }

        //стиль кнопки
        private void Form1_Load(object sender, EventArgs e)
        {

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, button1.Width, button1.Height);
            button1.Region = new Region(gp);
            gp.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loginUser = textBox1.Text;
            passwordUser = textBox2.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd_train_div = new SqlCommand();
                SqlCommand cmd_student = new SqlCommand();
                SqlCommand cmd_teacher = new SqlCommand();
                SqlCommand cmd_admin = new SqlCommand();
                cmd_train_div.Connection = connection;
                cmd_student.Connection = connection;
                cmd_teacher.Connection = connection;
                cmd_admin.Connection = connection;
                cmd_train_div.CommandText = @"SELECT COUNT(Пароль_сотрудника) FROM Учебный_отдел 
                                        WHERE Логин_сотрудника = '" + loginUser + "' AND Пароль_сотрудника = '" + passwordUser + "'";
                cmd_student.CommandText = @"SELECT COUNT(Пароль_студента) FROM Студент 
                                           WHERE Логин_студента = '" + loginUser + "' AND Пароль_студента = '" + passwordUser + "'";
                cmd_teacher.CommandText = @"SELECT COUNT(Пароль_преподавателя) FROM Преподаватель 
                                        WHERE Логин_преподавателя = '" + loginUser + "' AND Пароль_преподавателя = '" + passwordUser + "'";
                cmd_admin.CommandText = @"SELECT COUNT(Пароль_администратора) FROM Администратор 
                                        WHERE Логин_администратора = '" + loginUser + "' AND Пароль_администратора = '" + passwordUser + "'";
                int reader_train_div = (int) cmd_train_div.ExecuteScalar();
                int reader_student = (int) cmd_student.ExecuteScalar();
                int reader_teacher = (int) cmd_teacher.ExecuteScalar();
                int reader_admin = (int) cmd_admin.ExecuteScalar();
                if (reader_train_div == 1)
                {
                    SqlCommand cmd_exp = new SqlCommand();
                    cmd_exp.Connection = connection;
                    cmd_exp.CommandText = @"SELECT Экспертность FROM Учебный_отдел 
                                        WHERE Логин_сотрудника = '" + loginUser + "' AND Пароль_сотрудника = '" + passwordUser + "'";
                    int exp = (int)cmd_exp.ExecuteScalar();
                    if (exp == 1)
                    {
                        Главное_меню_учебного_отдела_эксперта newForm = new Главное_меню_учебного_отдела_эксперта();
                        Hide();
                        newForm.ShowDialog();
                        Show();
                        this.Close();
                    }
                    else
                    {
                        Главное_меню_учебного_отдела_не_эксперта newForm = new Главное_меню_учебного_отдела_не_эксперта();
                        Hide();
                        newForm.ShowDialog();
                        Show();
                        this.Close();
                    }
                }
                else if (reader_student == 1)
                {
                    Главное_меню_студента newForm = new Главное_меню_студента();
                    Hide();
                    newForm.ShowDialog();
                    Show();
                    this.Close();
                }
                else if (reader_teacher == 1)
                {
                    Главное_меню_преподавателя newForm = new Главное_меню_преподавателя();
                    Hide();
                    newForm.ShowDialog();
                    Show();
                    this.Close();
                }
                else if (reader_admin == 1)
                {
                    Главное_меню_администратора newForm = new Главное_меню_администратора();
                    Hide();
                    newForm.ShowDialog();
                    Show();
                    this.Close();
                }
                else
                {
                   MessageBox.Show("Совпадения не найдены! ", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        private void Вход_в_систему_Load(object sender, EventArgs e)
        {

        }
    }
}
