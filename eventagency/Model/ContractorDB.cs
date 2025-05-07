using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;

namespace eventagency.Model
{
    internal class ContractorDB
    {
        DbConnection connection;

        private ContractorDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Contractor contractor)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Contractor` Values (0, @title, @type, @email, @notes);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("title", contractor.Title));
                cmd.Parameters.Add(new MySqlParameter("type", contractor.Type));
                cmd.Parameters.Add(new MySqlParameter("email", contractor.Email));
                cmd.Parameters.Add(new MySqlParameter("notes", contractor.Notes));
                // можно указать параметр через отдельную переменную
                try
                {
                    // выполняем запрос через ExecuteScalar, получаем id вставленной записи
                    // если нам не нужен id, то в запросе убираем часть select LAST_INSERT_ID(); и выполняем команду через ExecuteNonQuery
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        //MessageBox.Show(id.ToString());
                        // назначаем полученный id обратно в объект для дальнейшей работы
                        contractor.ID = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Contractor> SelectAll()
        {
            List<Contractor> contractors = new List<Contractor>();
            if (connection == null)
                return contractors;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `title`, `type`, `email`, `notes` from `Contractor` ");
                try
                {
                    // выполнение запроса, который возвращает результат-таблицу
                    MySqlDataReader dr = command.ExecuteReader();
                    // в цикле читаем построчно всю таблицу
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        string type = string.Empty;
                        string email = string.Empty;
                        string notes = string.Empty;
                        // проверка на то, что столбец имеет значение
                        if (!dr.IsDBNull(1))
                            title = dr.GetString("Title");
                        if (!dr.IsDBNull(2))
                            type = dr.GetString("Type");
                        if (!dr.IsDBNull(3))
                            email = dr.GetString("Email");
                        if (!dr.IsDBNull(4))
                            notes = dr.GetString("Notes");

                        contractors.Add(new Contractor
                        {
                            ID = id,
                            Title = title,
                            Type = type,
                            Email = email,
                            Notes = notes,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return contractors;
        }

        internal bool Update(Contractor edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Contractor` set `Title`=@title, `Type`=@type, `Email`=@email, `Notes`=@notes where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("type", edit.Type));
                mc.Parameters.Add(new MySqlParameter("email", edit.Email));
                mc.Parameters.Add(new MySqlParameter("notes", edit.Notes));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Client remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Contractor` where `id` = {remove.ID}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static ContractorDB db;
        public static ContractorDB GetDb()
        {
            if (db == null)
                db = new ContractorDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
