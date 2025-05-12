using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;

namespace eventagency.Model
{
    internal class EventContractorDB
    {
        DbConnection connection;

        private EventContractorDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(EventContractor eventcontractor)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `EventContractor` Values (0, @price, @descriptionservice);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("fullName", eventcontractor.Price));
                cmd.Parameters.Add(new MySqlParameter("phone", eventcontractor.DescriptionService));

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
                        eventcontractor.ID = id;
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

        internal List<EventContractor> SelectAll()
        {
            List<EventContractor> eventcontractors = new List<EventContractor>();
            if (connection == null)
                return eventcontractors;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `price`, `descriptionservice` from `EventContractor` ");
                try
                {
                    // выполнение запроса, который возвращает результат-таблицу
                    MySqlDataReader dr = command.ExecuteReader();
                    // в цикле читаем построчно всю таблицу
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int price = 0;
                        string descriptionservice = string.Empty;
                        // проверка на то, что столбец имеет значение
                        if (!dr.IsDBNull(1))
                            price = dr.GetInt32("Price");
                        if (!dr.IsDBNull(2))
                            descriptionservice = dr.GetString("DescriptionService");


                        eventcontractors.Add(new EventContractor
                        {
                            ID = id,
                            Price = price,
                            DescriptionService = descriptionservice,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return eventcontractors;
        }

        internal bool Update(EventContractor edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `EventContractor` set `Price`=@price, `DescriptionService`=@descriptionservice where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("price", edit.Price));
                mc.Parameters.Add(new MySqlParameter("descriptionservice", edit.DescriptionService));


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


        internal bool Remove(EventContractor remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `EventContractor` where `id` = {remove.ID}");
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

        static EventContractorDB db;
        public static EventContractorDB GetDb()
        {
            if (db == null)
                db = new EventContractorDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
