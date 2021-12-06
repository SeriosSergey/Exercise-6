using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Exercise_6
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBase DB = new DataBase("localhost", "postgres", "trewq123", "SberDB");
            CreateUsersTable(DB);
            CreateAccountsTable(DB);
            CreateHistoryTable(DB);

            //Заполняем таблицы данными
            try
            {
                
                DB.UpdateData("INSERT INTO \"Users\" VALUES (1,111111, 'Иванов', 'Иван', 'Иванович', 'ivanov@mail.ru')");
                DB.UpdateData("INSERT INTO \"Users\" VALUES (2,222222, 'Петров', 'Петр', 'Петрович', 'petrov@mail.ru')");
                DB.UpdateData("INSERT INTO \"Users\" VALUES (3,333333, 'Сидоров', 'Сидор', 'Сидорович', 'sidorov@mail.ru')");
                DB.UpdateData("INSERT INTO \"Users\" VALUES (4,444444, 'Алексеев', 'Алексей', 'Алексеевич', 'alexeev@mail.ru')");
                DB.UpdateData("INSERT INTO \"Users\" VALUES (5,555555, 'Сергеев', 'Сергей', 'Сергеевич', 'sergeev@mail.ru')");

                DB.UpdateData("INSERT INTO \"Accounts\" VALUES (1,100000, 1, 'Ru')");
                DB.UpdateData("INSERT INTO \"Accounts\" VALUES (2,100, 1, 'USD')");
                DB.UpdateData("INSERT INTO \"Accounts\" VALUES (3,2500, 2, 'USD')");
                DB.UpdateData("INSERT INTO \"Accounts\" VALUES (4,500, 3, 'USD')");
                DB.UpdateData("INSERT INTO \"Accounts\" VALUES (5,10000, 3, 'Ru')");
                DB.UpdateData("INSERT INTO \"Accounts\" VALUES (6,4000, 4, 'Ru')");
                DB.UpdateData("INSERT INTO \"Accounts\" VALUES (7,10, 5, 'Ru')");

                DB.UpdateData("INSERT INTO \"History\" VALUES (1,1, 'in', 100000,'Ru')");
                DB.UpdateData("INSERT INTO \"History\" VALUES (2,2, 'in', 1000,'USD')");
                DB.UpdateData("INSERT INTO \"History\" VALUES (3,2, 'out', 900,'USD')");
                DB.UpdateData("INSERT INTO \"History\" VALUES (4,3, 'in', 2500,'USD')");
                DB.UpdateData("INSERT INTO \"History\" VALUES (5,1, 'in', 500,'USD')");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Вывод данных из таблиц
            ShowUsersTable(DB);
            ShowAccountsTable(DB);
            ShowHistoryTable(DB);

            //Создание нового пользователя
            Console.WriteLine("Введите фамилию: ");
            string first_name = Console.ReadLine();
            Console.WriteLine("Введите имя: ");
            string last_name = Console.ReadLine();
            Console.WriteLine("Введите отчество: ");
            string middle_name = Console.ReadLine();
            Console.WriteLine("Введите телефон: ");
            string phone = Console.ReadLine();
            Console.WriteLine("Введите imale: ");
            string email = Console.ReadLine();
            try
            {
                DB.UpdateData($"INSERT INTO \"Users\" (phone, first_name,midle_name,last_name,email) VALUES ('{phone}', '{first_name}', '{last_name}', '{middle_name}', '{email}')");
                Console.WriteLine("Пользователь успешно создан");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        static void CreateUsersTable(DataBase DB)
        {
            try
            {
                Console.WriteLine("Создание таблицы Users");
                DB.UpdateData("CREATE TABLE IF NOT EXISTS public.\"Users\"" +
                                    "(" +
                                        "Id serial NOT NULL," +
                                        "phone text NOT NULL," +
                                        "first_name text NOT NULL," +
                                        "midle_name text," +
                                        "last_name text NOT NULL," +
                                        "email text," +
                                        "PRIMARY KEY(Id)" +
                                    ");"
                                );
                Console.WriteLine("Таблица Users создана");
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось создать таблицу Users - " + e.Message);
            }
        }

        static void ShowUsersTable(DataBase DB)
        {
            Console.WriteLine("Вывод данных из таблицы Users:");
            var data = DB.RequestData("select * from \"Users\"");
            while (data.Read())
            {
                Console.WriteLine($"{data.GetInt32(0)} {data.GetString(1)} {data.GetString(2)} {data.GetString(3)} {data.GetString(4)} {data.GetString(5)}");
            }
            Console.WriteLine();
        }

        static void CreateAccountsTable(DataBase DB)
        {
            try
            {
                Console.WriteLine("Создание таблицы Accounts");
                DB.UpdateData("CREATE TABLE IF NOT EXISTS public.\"Accounts\""+
                                    "("+
                                        "Id serial NOT NULL,"+
                                        "cash double precision NOT NULL,"+
                                        "user_id bigint NOT NULL," +
                                        "currency text NOT NULL," +
                                        "PRIMARY KEY(Id)," +
                                        "FOREIGN KEY (user_id) REFERENCES \"Users\" (Id) ON DELETE CASCADE" +
                                    ");"
                             );
                Console.WriteLine("Таблица Accounts создана");
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось создать таблицу Accounts - " + e.Message);
            }
        }

        static void ShowAccountsTable(DataBase DB)
        {
            Console.WriteLine("Вывод данных из таблицы Accounts:");
            var data = DB.RequestData("select * from \"Accounts\"");
            while (data.Read())
            {
                Console.WriteLine($"{data.GetInt32(0)} {data.GetDouble(1)} {data.GetInt32(2)} {data.GetString(3)}");
            }
            Console.WriteLine();
        }

        static void CreateHistoryTable(DataBase DB)
        {
            try
            {
                Console.WriteLine("Создание таблицы History");
                DB.UpdateData("CREATE TABLE IF NOT EXISTS public.\"History\"" +
                                    "(" +
                                        "Id serial NOT NULL," +
                                        "account_id integer NOT NULL," +
                                        "type text NOT NULL," +
                                        "value double precision NOT NULL," +
                                        "currency text NOT NULL," +
                                        "PRIMARY KEY(id)," +
                                        "FOREIGN KEY (account_id) REFERENCES \"Accounts\" (Id) ON DELETE CASCADE" +
                                    ");"
                             );
                Console.WriteLine("Таблица History создана");
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось создать таблицу History - " + e.Message);
            }
        }

        static void ShowHistoryTable(DataBase DB)
        {
            Console.WriteLine("Вывод данных из таблицы History:");
            var data = DB.RequestData("select * from \"History\"");
            while (data.Read())
            {
                Console.WriteLine($"{data.GetInt32(0)} {data.GetInt32(1)} {data.GetString(2)} {data.GetDouble(3)} {data.GetString(4)}");
            }
            Console.WriteLine();
        }

        class DataBase
        {
            readonly private string CS;

            public DataBase(string host, string username, string password, string database)
            {
                CS = $"Host = {host}; Port=5432; Username = {username}; Password = {password}; Database = {database}";
            }

            public NpgsqlDataReader RequestData(string request)
            {
                try
                {
                    NpgsqlConnection con = new NpgsqlConnection(CS);
                    con.Open();

                    var cmd = new NpgsqlCommand(request, con);

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    return rdr;
                }
                catch
                {
                    throw;
                }
            }

            public void UpdateData(string request)
            {
                try
                {
                    NpgsqlConnection con = new NpgsqlConnection(CS);
                    con.Open();
                    var cmd = new NpgsqlCommand(request, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
