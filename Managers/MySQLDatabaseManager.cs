using MySqlConnector;
using PracaDomowaCS.Exceptions;
using PracaDomowaCS.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowaCS.Managers
{
    // Install MySQL Connector/NET
    public class MySQLDatabaseManager
    {
        private readonly string connectionString;
        public MySQLDatabaseManager(string server, string database, string username, string password)
        {
            connectionString = $"Server={server};Database={database};User ID={username};Password={password};";
            CreateTables();
        }

        void CreateTables()
        {
            Console.WriteLine($"Sprawdzanie i tworzenie tabel...");

            // Create Car table if not exists
            string createCarTableQuery = "CREATE TABLE IF NOT EXISTS Car(" +
                "id integer PRIMARY KEY AUTO_INCREMENT," +
                "model varchar(128) NOT NULL," +
                "name varchar(128) NOT NULL," +
                "year int);";
            string createSongTableQuery = "CREATE TABLE IF NOT EXISTS Song(" +
                "id integer PRIMARY KEY AUTO_INCREMENT," +
                "title varchar(128) NOT NULL," +
                "artist varchar(128) NOT NULL," +
                "year int);";
            MySqlConnection conn = GetConnection();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(createCarTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Tabela Car została utworzona lub już istnieje.");
            }
            using (MySqlCommand cmd = new MySqlCommand(createSongTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Tabela Song została utworzona lub już istnieje.");
            }

            conn.Close();
        }

        public void InsertData<T>(T obj)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            Console.WriteLine("Dodawanie danych do bazy...");

            if (obj is Car car)
            {
                string insertQuery = $"INSERT INTO {obj.GetType().Name} (id, model, name, year) VALUES (NULL, @model, @name, @year)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@model", car.Model);
                    cmd.Parameters.AddWithValue("@name", car.Name);
                    cmd.Parameters.AddWithValue("@year", car.Year);

                    cmd.ExecuteNonQuery();
                }
            }
            else if (obj is Song song)
            {
                string insertQuery = $"INSERT INTO {obj.GetType().Name} (id, title, artist, year) VALUES (NULL, @title, @artist, @year)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@title", song.Title);
                    cmd.Parameters.AddWithValue("@artist", song.Artist);
                    cmd.Parameters.AddWithValue("@year", song.YearReleased);

                    cmd.ExecuteNonQuery();
                }
            }

            

            Console.WriteLine("Dane zostały dodane.");
            conn.Close();
        }

        public void ReadData(string tableName)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            Console.WriteLine("Odczytywanie danych z bazy...");

            string selectQuery = $"SELECT * FROM {tableName}";

            using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Kolumna 1: {reader[0]}, Kolumna 2: {reader[1]}, Kolumna 3: {reader[2]}, Kolumna 4: {reader[3]}");
                    }
                }
            }

            Console.WriteLine("Dane zostały odczytane.");
            conn.Close();
        }

        public List<Car> GetCars()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            string query = "SELECT * FROM Car";
            List<Car> cars = new List<Car>();

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string model = reader["model"].ToString();
                        string name = reader["name"].ToString();
                        int year = Convert.ToInt32(reader["year"]);

                        Car car = new Car(id, model, name, year);

                        cars.Add(car);
                    }
                }
            }

            conn.Close();
            return cars;
        }

        public List<Song> GetSongs()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            string query = "SELECT * FROM Song";
            List<Song> songs = new List<Song>();

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string title = reader["title"].ToString();
                        string artist = reader["artist"].ToString();
                        int year = Convert.ToInt32(reader["year"]);

                        Song song = new Song(id, title, artist, year);

                        songs.Add(song);
                    }
                }
            }

            conn.Close();
            return songs;
        }

        public static int GetMaxID(string tableName)
        {
            MySqlConnection conn = new MySqlConnection($"Server=localhost;Database=SuSklep;User ID=root;Password=");
            conn.Open();

            string selectQuery = $"SELECT max(id) FROM {tableName}";

            using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            int result = int.Parse(reader[0].ToString());
                            conn.Close();
                            return result;
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            conn.Close();
                            return 0;
                        }
                    }
                }
            }

            conn.Close();
            //throw new IdNotFoundException();
            return 0;
        }

        public void UpdateData()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            Console.WriteLine("Aktualizowanie danych w bazie...");

            string updateQuery = "UPDATE TwojaTabela SET ColumnName1 = @NewValue WHERE ColumnName2 = @SearchValue";

            using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@NewValue", "NowaWartosc");
                cmd.Parameters.AddWithValue("@SearchValue", "Wartość2");

                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Dane zostały zaktualizowane.");
            conn.Close();
        }

        public void DeleteData(string tableName, int id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            string deleteQuery = $"DELETE FROM {tableName} WHERE id = {id}";

            using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Dane zostały usunięte.");
            conn.Close();
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
