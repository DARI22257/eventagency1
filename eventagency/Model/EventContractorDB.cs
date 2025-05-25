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
                MySqlCommand cmd = connection.CreateCommand("insert into `EventContractor` Values (0, @price, @descriptionservice, @idClient, @idTask, @idEvents, @idContractor);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("price", eventcontractor.Price));
                cmd.Parameters.Add(new MySqlParameter("descriptionservice", eventcontractor.DescriptionService));
                cmd.Parameters.Add(new MySqlParameter("idClient", eventcontractor.idClient));
                cmd.Parameters.Add(new MySqlParameter("idEvents", eventcontractor.idEvents));
                cmd.Parameters.Add(new MySqlParameter("idTask", eventcontractor.idTask));
                cmd.Parameters.Add(new MySqlParameter("idContractor", eventcontractor.idContractor));

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



        internal List<EventContractor> SelectAll(int clientId)
        {
            List<EventContractor> eventcontractors = new List<EventContractor>();
            if (connection == null)
                return eventcontractors;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT c.ID AS 'clientid', c.FullName, c.Phone, c.Email, c.Notes, ec.descriptionservice, ec.price, c1.id AS 'ClientId', c1.title as 'cTitle', c1.type, c1.email AS 'cemail', c1.notes AS 'cnotes', t.id AS 'TaskId', t.title AS 'ttitle', t.description, t.term, t.assigned, t.status AS 'status1', e.id, e.title AS 'etitle', e.date, e.place, e.budget, e.status FROM Clients c" +
                    " join EventContractor AS ec ON ec.idClient = c.id" +
                    " JOIN Contractor c1 ON ec.idContractor = c1.id" +
                    " JOIN Events e ON ec.idEvents = e.id" +
                    " JOIN Tasks t ON ec.idTask = t.id" +
                    " WHERE c.id = " + clientId + " ORDER BY ec.id DESC LIMIT 1 ");
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
                        int idClient = dr.GetInt32(0);
                        string fullName = string.Empty;
                        string phone = string.Empty;
                        string email = string.Empty;
                        string notes = string.Empty;
                        int idEvents = dr.GetInt32(0);
                        string etitle = string.Empty;
                        DateTime date = DateTime.Now;
                        string place = string.Empty;
                        int budget = 0;
                        string status = string.Empty;
                        int idTask = dr.GetInt32(0);
                        string ttitle = string.Empty;
                        string description = string.Empty;
                        DateTime term = DateTime.Now;
                        string assigned = string.Empty;
                        string status1 = string.Empty;
                        int idContractor = dr.GetInt32(0);
                        string ctitle = string.Empty;
                        string type = string.Empty;
                        string cemail = string.Empty;
                        string cnotes = string.Empty;

                        // проверка на то, что столбец имеет значение
                        if (!dr.IsDBNull(1))
                            price = dr.GetInt32("Price");
                        if (!dr.IsDBNull(2))
                            descriptionservice = dr.GetString("DescriptionService");
                        if (!dr.IsDBNull(3))
                            fullName = dr.GetString("FullName");
                        if (!dr.IsDBNull(4))
                            phone = dr.GetString("Phone");
                        if (!dr.IsDBNull(5))
                            email = dr.GetString("Email");
                        if (!dr.IsDBNull(6))
                            notes = dr.GetString("Notes");
                        if (!dr.IsDBNull(7))
                            etitle = dr.GetString("eTitle");
                        if (!dr.IsDBNull(8))
                            date = dr.GetDateTime("Date");
                        if (!dr.IsDBNull(9))
                            place = dr.GetString("Place");
                        if (!dr.IsDBNull(10))
                            budget = dr.GetInt32("Budget");
                        if (!dr.IsDBNull(11))
                            status = dr.GetString("Status");
                        if (!dr.IsDBNull(12))
                            ttitle = dr.GetString("tTitle");
                        if (!dr.IsDBNull(13))
                            description = dr.GetString("Description");
                        if (!dr.IsDBNull(14))
                            term = dr.GetDateTime("Term");
                        if (!dr.IsDBNull(15))
                            assigned = dr.GetString("Assigned");
                        if (!dr.IsDBNull(16))
                            status1 = dr.GetString("Status1");
                        if (!dr.IsDBNull(17))
                            ctitle = dr.GetString("cTitle");
                        if (!dr.IsDBNull(18))
                            type = dr.GetString("Type");
                        if (!dr.IsDBNull(19))
                            cemail = dr.GetString("cEmail");
                        if (!dr.IsDBNull(20))
                            cnotes = dr.GetString("cNotes");


                        eventcontractors.Add(new EventContractor
                        {
                            ID = id,
                            Price = price,
                            DescriptionService = descriptionservice,
                            Client = new Client
                            {
                                Email = email,
                                FullName = fullName,
                                ID = clientId,
                                Notes = notes,
                                Phone = phone
                            },
                            Event = new Event
                            {
                                ID = id,
                                Title = etitle,
                                Date = date,
                                Place = place,
                                Budget = budget,
                                Status = status
                            },
                            Task = new TaskWork
                            {
                                ID = idTask,
                                Title = ttitle,
                                Description = description,
                                Term = term,
                                Assigned = assigned,
                                Status = status1,

                            },
                            Contractor = new Contractor
                            {
                                ID = idContractor,
                                Title = ctitle,
                                Type = type,
                                Email = cemail,
                                Notes = cnotes,
                            }
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
        public int GetTotalPriceAll()
        {
            int total = 0;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("SELECT SUM(price) FROM EventContractor");
                try
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                        total = Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при подсчёте суммы: " + ex.Message);
                }
                connection.CloseConnection();
            }

            return total;
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
