using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Курсы : Form
    {
		String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
		SqlDataAdapter adapter;
		DataSet ds = new DataSet();
		string sql;
		static int count;
		static string[] _sytes = new string[] { };
		static string[] _name_of_cources = new string[] { };
		static string[] _description = new string[] { };
		static int iter;

		public Курсы()
        {
            InitializeComponent();
			dataGridView1.ReadOnly = false;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.AllowUserToAddRows = true;
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
				dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
				connection.Close();
			}
		}

		private static void ParseHtml(Stream html, int iter)
		{
			var doc = new HtmlAgilityPack.HtmlDocument();
			doc.Load(html);

			if (iter == 1)
			{
				Array.Resize(ref _sytes, 0);
				Array.Resize(ref _name_of_cources, 0);

				HtmlNodeCollection names = doc.DocumentNode.SelectNodes("//div[@class='_eading_heading__TQrRM programCard_title__IyqWA']");
				HtmlNodeCollection sytes = doc.DocumentNode.SelectNodes("//a[@class='programCard_root__aXiIB']/@href");
				count = names.Count;
				for (int i = 0; i < count; i++)
				{
					Array.Resize(ref _sytes, _sytes.Length + 1);
					Array.Resize(ref _name_of_cources, _name_of_cources.Length + 1);
					_sytes[i] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(sytes[i].Attributes["href"].Value));
					_name_of_cources[i] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(names[i].InnerText));
					/*Console.WriteLine("Name: {0}, https://netology.ru{1}", Encoding.UTF8.GetString(Encoding.Default.GetBytes(names[i].InnerText)),
						Encoding.UTF8.GetString(Encoding.Default.GetBytes(sytes[i].Attributes["href"].Value)));*/
				}
			}
			/*else
			{
				HtmlNodeCollection description = doc.DocumentNode.SelectNodes("//div[@class='text_type-c-p2__XMxHx text_weight-500__l4Mb9 styles_title__o990u styles_withIcon__OVGTI']/p");
				Array.Resize(ref _description, _description.Length + 1);
				_description[_description.Length - 1] = "Навыки: ";
				for (int i = 0; i < description.Count; i++)
				{
					_description[_description.Length - 1] += Encoding.UTF8.GetString(Encoding.Default.GetBytes(description[i].InnerText)) + ", ";
				}
				_description[_description.Length - 1] = _description[_description.Length - 1].Substring(0, (_description[_description.Length - 1].Length - 1));
			}*/
		}


		private void Курсы_Load(object sender, EventArgs e)
		{			
			String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";

			iter = 1;
			var url_1 = "https://api.zenrows.com/v1/?apikey=26afd8f0b9ff3eac7919e77a7fa2fa0e8e243c1b&url=https://netology.ru/navigation";
			var request_1 = WebRequest.Create(url_1);
			request_1.Method = "GET";
			var webResponse_1 = request_1.GetResponse();
			var webStream_1 = webResponse_1.GetResponseStream();
			ParseHtml(webStream_1, iter);

			/*iter = 2;
			for (int i = 20; i < 30; i++)
			{
				url_1 = "https://api.zenrows.com/v1/?apikey=26afd8f0b9ff3eac7919e77a7fa2fa0e8e243c1b&url=https://netology.ru" + _sytes[i];
				request_1 = WebRequest.Create(url_1);
				request_1.Method = "GET";
				webResponse_1 = request_1.GetResponse();
				webStream_1 = webResponse_1.GetResponseStream();
				ParseHtml(webStream_1, iter);
			}*/

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = connection;
				cmd.CommandText = @"DELETE FROM Курсы_по_улучшению_уровня_компетенций";
				cmd.ExecuteNonQuery();
				connection.Close();
			}


			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand cmd = new SqlCommand();

				for (int i = 0; i < count; i++)
				{
					cmd.Connection = connection;
					cmd.CommandText = @"INSERT INTO Курсы_по_улучшению_уровня_компетенций (Название_курса, 
                                        Краткое_описание, Сайт) 
                                        VALUES('" + _name_of_cources[i] +
											"', '" + "-" + "', 'https://netology.ru"
											+ _sytes[i] + "' )";
					cmd.ExecuteNonQuery();
				}
				connection.Close();
			}

			//отображение таблицы
			sql = @"SELECT * FROM Курсы_по_улучшению_уровня_компетенций";
			print(sql);
		}

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void вГлавноеМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Главное_меню_студента newForm = new Главное_меню_студента();
			Hide();
			newForm.ShowDialog();
			Show();
			this.Close();
		}

        private void подсказкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
			MessageBox.Show("Представлены курсы с сайтов 'Нетология' и 'Открытое образование'", "Подсказка!", MessageBoxButtons.OK);
		}
    }
}
