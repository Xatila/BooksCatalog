using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace DbTest
{
    class CRUD
    {
        // Създаваме стринг променлива за настройките към базата данни.
        private string conString = "server=localhost;uid=root;pwd=myPass123;database=books;";
        public void Insert(ref Book book)
        {
            MySqlConnection conn = new MySqlConnection(conString);
            {
                // Отваряме връзката.
                conn.Open();
                
                // Заявка за вмъкване на запис (книга).
                string sqlQuery = "INSERT INTO bookinfo (ID, Title, Description) VALUES (@ID, @Title, @Description)";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                {
                    book.ID = GetNextID();
                    cmd.Parameters.AddWithValue("@ID", book.ID);
                    cmd.Parameters.AddWithValue("@Title", book.title);
                    cmd.Parameters.AddWithValue("@Description", book.description);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Заявка за изтриване дадена книга (запис).
        public void Delete(Book book)
        {
            MySqlConnection conn = new MySqlConnection(conString);
            {
                conn.Open();

                string sqlQuery = "DELETE FROM bookinfo WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                {
                    cmd.Parameters.AddWithValue("@ID", book.ID);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Заявка за редактиране на дадена книга (запис).
        public void Update(Book book)
        {
            MySqlConnection conn = new MySqlConnection(conString);
            {
                conn.Open();

                string sqlQuery = "UPDATE bookinfo SET Title = @Title, Description = @Description WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                {
                    cmd.Parameters.AddWithValue("@ID", book.ID);
                    cmd.Parameters.AddWithValue("@Title", book.title);
                    cmd.Parameters.AddWithValue("@Description", book.description);

                    cmd.ExecuteNonQuery();
                }
            }
        }

// Прочитане на таблицата.
        public Book GetByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection(conString);
            {
                conn.Open();

                string sqlQuery = "SELECT * FROM bookinfo WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        Book book = GetByDataReader(dataReader);
                        return book;
                    }

                }
            }
            return null;
        }

        private Book GetByDataReader(MySqlDataReader dataReader)
        {
            Book book = new Book
            {
                ID = Convert.ToInt32(dataReader["ID"]),
                title = Convert.ToString(dataReader["Title"]),
                description = Convert.ToString(dataReader["Description"])
            };

            return book;
        }
// Валидираме, че няма да се получат грешки в id-тата на книгите след CRUD опреациите.
        public int GetNextID()
        {
            MySqlConnection conn = new MySqlConnection(conString);
            {
                conn.Open();

                string sqlQuery = "SELECT MAX(ID) AS MaxID FROM bookinfo";
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        int maxID = (!DBNull.Value.Equals(dataReader["MaxID"]) ? Convert.ToInt32(dataReader["MaxID"]) : 0);
                        maxID++;
                        return maxID;
                    }
                }
            }

            return 1;
        }
    }
}
