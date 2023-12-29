using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Data.SqlClient;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;

namespace WindowsFormsApp1
{
    public partial class Курсы : Form
    {
		String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";
		SqlDataAdapter adapter;
		DataSet ds = new DataSet();
		string sql;
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
			int kol = 0;
			if (iter == 0)
			{
				Array.Resize(ref _sytes, 0);
				Array.Resize(ref _name_of_cources, 0);
				HtmlNodeCollection names = doc.DocumentNode.SelectNodes("//div[@class='_eading_heading__TQrRM programCard_title__IyqWA']");
				HtmlNodeCollection sytes = doc.DocumentNode.SelectNodes("//a[@class='programCard_root__aXiIB']/@href");
				for (int i = 0; i < sytes.Count; i++)
				{
					Array.Resize(ref _sytes, _sytes.Length + 1);
					Array.Resize(ref _name_of_cources, _name_of_cources.Length + 1);
					_sytes[i] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(sytes[i].Attributes["href"].Value));
					_name_of_cources[i] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(names[i].InnerText));
				}
			}
			else
			{
				/*HtmlNodeCollection description = doc.DocumentNode.SelectNodes("//div[@class='text_type-c-p1__66vw4 text_weight-500__l4Mb9 programModuleBlock_blockTitle__5qQiv']");*/
				HtmlNodeCollection description = doc.DocumentNode.SelectNodes("//div[@class='text_type-c-p2__XMxHx text_weight-500__l4Mb9 styles_title__o990u styles_withIcon__OVGTI']/p");
				if (description != null)
				{
					if (Encoding.UTF8.GetString(Encoding.Default.GetBytes(description[kol].InnerText)).ToString().Replace(" ", "") != "Отсрочкаотармии")
					{
						for (int i = 0; i < description.Count; i++)
						{
							_description[_description.Length - 1] += Encoding.UTF8.GetString(Encoding.Default.GetBytes(description[i].InnerText)) + ", ";
						}
						_description[_description.Length - 1] = _description[_description.Length - 1].Remove(_description[_description.Length - 1].Length - 2, 2);
					}
					else
						_description[_description.Length - 1] = "Навыки не определены";
					kol++;
				}
				else
				{
					if (description == null)
					{
						/*_sytes[_description.Length - 1] = null;
						_name_of_cources[_description.Length - 1] = null;*/
						_description[_description.Length - 1] = "Навыки не определены";
					}
					kol++;
				}
			}
		}

		private void get_otchet()
		{
			Random rnd = new Random();
			int value = rnd.Next(0, 1000);
			string path = System.IO.Directory.GetCurrentDirectory() + @"\" + "Отчет по курсам " + "_" + value + ".xlsx";

			Excel.Application excelapp = new Excel.Application();
			Excel.Workbook workbook = excelapp.Workbooks.Add();
			Excel.Worksheet worksheet = workbook.ActiveSheet;
			excelapp.SheetsInNewWorkbook = 3;
			worksheet.StandardWidth = 60;
			worksheet.Cells.Font.Size = 10;
			worksheet.Cells.Font.Name = "Times New Roman";
			Excel.Range _excelCells1 = (Excel.Range)worksheet.get_Range("A1", "C1000").Cells;
			worksheet.get_Range("A1", "C1000").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
			worksheet.get_Range("A1", "C1000").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
			_excelCells1.WrapText = true;
			excelapp.Visible = false;

			worksheet.Cells[1, 1] = "Название курса";
			worksheet.Cells[1, 2] = "Описание";
			worksheet.Cells[1, 3] = "Сайт";
			worksheet.get_Range("A1", "C1").Font.Bold = true;

			//получение названий компетенций вакансии
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = connection;
				Array.Resize(ref _sytes, 0);
				Array.Resize(ref _name_of_cources, 0);
				Array.Resize(ref _description, 0);
				cmd.CommandText = @"SELECT Название_курса, Сайт, Краткое_описание FROM Курсы_по_улучшению_уровня_компетенций";
				SqlDataReader reader = cmd.ExecuteReader();
				int k = 0;
				while (reader.Read())
				{
					Array.Resize(ref _name_of_cources, _name_of_cources.Length + 1);
					Array.Resize(ref _sytes, _sytes.Length + 1);
					Array.Resize(ref _description, _description.Length + 1);
					_name_of_cources[k] = reader["Название_курса"].ToString();
					_sytes[k] = reader["Сайт"].ToString();
					_description[k] = reader["Краткое_описание"].ToString();
					k++;
				}
				connection.Close();

				for (int i = 0; i < _description.Length; i++)
				{
					worksheet.Cells[i + 2, 1] = "" + _name_of_cources[i] + "";
					worksheet.Cells[i + 2, 2] = "" + _description[i] + "";
					worksheet.Cells[i + 2, 3] = "" + _sytes[i] + "";
				}
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


			private void Курсы_Load(object sender, EventArgs e)
		{			
			String connectionString = @"Data Source=LAPTOP-9FN9OCFG\SQLEXPRESS;Initial Catalog=Соответствие компетенций требованиям рынка труда;Integrated Security=True";

			iter = 0;
			var url_1 = "https://api.zenrows.com/v1/?apikey=26afd8f0b9ff3eac7919e77a7fa2fa0e8e243c1b&url=https://netology.ru/navigation";
			var request_1 = WebRequest.Create(url_1);
			request_1.Method = "GET";
			var webResponse_1 = request_1.GetResponse();
			var webStream_1 = webResponse_1.GetResponseStream();
			ParseHtml(webStream_1, iter);

			for (int i = 0; i < 15; i++)
			{
				Array.Resize(ref _description, _description.Length + 1);
				iter += 1;
				url_1 = "https://api.zenrows.com/v1/?apikey=26afd8f0b9ff3eac7919e77a7fa2fa0e8e243c1b&url=https://netology.ru" + _sytes[i];
				request_1 = WebRequest.Create(url_1);
				request_1.Method = "GET";
				webResponse_1 = request_1.GetResponse();
				webStream_1 = webResponse_1.GetResponseStream();
				ParseHtml(webStream_1, iter);
			}

			/*_sytes = _sytes.Where(x => x != null).ToArray();
			_name_of_cources = _name_of_cources.Where(x => x != null).ToArray();
			_description = _description.Where(x => x != null).ToArray();*/

			/*for (int i = 0; i < _description.Length; i++)
			{
				Console.WriteLine(i + 1);
				Console.WriteLine("Name: {0}, https://netology.ru{1}", _name_of_cources[i], _sytes[i]);
				Console.WriteLine("Описание: {0}", _description[i]);
				Console.WriteLine("");

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

				for (int i = 0; i < _description.Length; i++)
				{
					cmd.Connection = connection;
					cmd.CommandText = @"INSERT INTO Курсы_по_улучшению_уровня_компетенций (Название_курса, 
                                        Краткое_описание, Сайт) 
                                        VALUES('" + _name_of_cources[i] +
											"', '" + _description[i] + "', 'https://netology.ru"
											+ _sytes[i] + "' )";
					cmd.ExecuteNonQuery();
				}
				connection.Close();
			}

			//отображение таблицы
			sql = @"SELECT * FROM Курсы_по_улучшению_уровня_компетенций";
			print(sql);

			get_otchet();
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
