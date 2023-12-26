using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Регистрация_пользователей : Form
    {
        public Регистрация_пользователей()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Регистрация_учебного_отдела newForm = new Регистрация_учебного_отдела();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Регистрация_преподавателя newForm = new Регистрация_преподавателя();
            newForm.ShowDialog();
            Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            Регистрация_учебного_отдела newForm = new Регистрация_учебного_отдела();
            newForm.ShowDialog();
            Show();
            this.Close();
        }
    }
}
