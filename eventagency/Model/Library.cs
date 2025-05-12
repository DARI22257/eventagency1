using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace eventagency.Model
{
    public class Library
    {
        private readonly DbConnection dbConnection;

        public List<Client> SearchClient(string search)
        {
            List<Client> result = new();

            string query = $"SELECT Clients.ID AS 'clientid', FullName, Phone, Email, Notes FROM Clients where FullName like @search or Phone like @search";

            if (dbConnection.OpenConnection())
            {// using уничтожает объект после окончания блока (вызывает Dispose)
                using (var mc = dbConnection.CreateCommand(query))

                {
                    // передача поиска через переменную в запрос
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // создание книги на каждую строку в результате
                            var book = new Client();
                            book.ID = dr.GetInt32("clientid");
                            book.FullName = dr.GetString("FullName");
                            book.Phone = dr.GetString("Phone");
                            book.Email = dr.GetString("Email");
                            book.Notes = dr.GetString("Notes");
                        }
                    }
                }
                dbConnection.CloseConnection();
            }
            return result;

        }

        // синглтон start
        static Library library;
        private Library(DbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public static Library GetTable()
        {
            if (library == null)
                library = new Library(DbConnection.GetDbConnection());
            return library;
        }

        internal Order GetLastOrder(Client? value)
        {
            throw new NotImplementedException();
        }
        // синглтон end
    }
}
