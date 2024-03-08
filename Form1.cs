using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace DbTest
{
    public partial class Form1 : Form
    {
        int IDBook = 0;
        CRUD crud = new CRUD();
        public Form1()
        {

            InitializeComponent();
        }

        private void dataGridViewBooks_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                IDBook = 0;
                if (dataGridViewBooks.SelectedCells.Count > 0)
                {
                    IDBook = int.Parse(dataGridViewBooks.SelectedCells[0].Value.ToString());
                    txtTitle.Text = dataGridViewBooks.SelectedCells[1].Value.ToString();
                    txtDescription.Text = dataGridViewBooks.SelectedCells[2].Value.ToString();
                    btnDelete.Enabled = true;
                }
                else
                {
                    btnDelete.Enabled = false;
                }

            }
            catch { }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IDBook != 0)
            {
                Book book = crud.GetByID(IDBook);
                book.title = txtTitle.Text;
                book.description = txtDescription.Text;

                crud.Update(book);
            }
            else
            {
                Book book = new Book();
                book.title = txtTitle.Text;
                book.description = txtDescription.Text;
                crud.Insert(ref book);
            }


            getBooks();
        }
        public void getBooks()
        {
            DataTable table = new DataTable();
            if (dataGridViewBooks.IsCurrentCellInEditMode)
            {
                dataGridViewBooks.EndEdit();
                dataGridViewBooks.Rows.Clear();
            }
            
            string conString = "server=localhost;uid=root;pwd=12qwas123;database=books;";

            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = conString;
            con.Open();
            string sqlQuery = "select * from bookinfo";
            MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string title = reader["Title"].ToString();
                string description = reader["Description"].ToString();
                table.Rows.Add(reader["ID"], title, description);
            }
            dataGridViewBooks.DataSource = table;
        }

        private void btnNewBook_Click(object sender, EventArgs e)
        {
            IDBook = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IDBook != 0)
            {
                Book book = crud.GetByID(IDBook);
                crud.Delete(book);
                getBooks();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                string conString = "server=localhost;uid=root;pwd=12qwas123;database=books;";

                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = conString;
                con.Open();
                string sqlQuery = "select * from bookinfo";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("Title", typeof(string));
                table.Columns.Add("Description", typeof(string));

                while (reader.Read())
                {
                    table.Rows.Add(reader["ID"], reader["Title"], reader["Description"]);
                }
                dataGridViewBooks.DataSource = table;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
