using MySqlConnector;
using PracaDomowaCS.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PracaDomowaCS.Managers
{
    // Install System.Data.Sqlite
    public class SQLiteDatabaseManager
    {
        private string connectionString;

        public SQLiteDatabaseManager(string dbPath)
        {
            connectionString = $"Data Source={dbPath};Version=3;";

            using (var conn = GetConnection())
            {
                conn.Open();

                // Create databse if not exists
                using (var createDbCommand = new SQLiteCommand($"ATTACH DATABASE '{dbPath}' AS SuSklep", conn))
                {
                    createDbCommand.ExecuteNonQuery();
                }

                // Create Car table if not exists
                string createCarTableQuery = "CREATE TABLE IF NOT EXISTS SuSklep.Car(" +
                    "id integer PRIMARY KEY AUTOINCREMENT," +
                    "model varchar(128) NOT NULL," +
                    "name varchar(128) NOT NULL," +
                    "year int);";
                string createSongTableQuery = "CREATE TABLE IF NOT EXISTS SuSklep.Song(" +
                    "id integer PRIMARY KEY AUTOINCREMENT," +
                    "title varchar(128) NOT NULL," +
                    "artist varchar(128) NOT NULL," +
                    "year int);";
                using (var createTableCommand = new SQLiteCommand(createCarTableQuery, conn))
                {
                    createTableCommand.ExecuteNonQuery();
                    Console.WriteLine($"Tabela Car została utworzona lub już istnieje.");
                }
                using (var createTableCommand = new SQLiteCommand(createSongTableQuery, conn))
                {
                    createTableCommand.ExecuteNonQuery();
                    Console.WriteLine($"Tabela Song została utworzona lub już istnieje.");
                }
                conn.Close();
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
            }

            return dataTable;
        }

        public int ExecuteNonQuery(string query)
        {
            int rowsAffected = 0;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing non-query: {ex.Message}");
            }

            return rowsAffected;
        }

        public void InsertData<T>(T obj)
        {
            Console.WriteLine("Dodawanie danych do bazy...");
            var conn = GetConnection();
            conn.Open();

            if (obj is Car car)
            {  
                string query = "INSERT INTO Car VALUES (NULL,@model,@name,@year)";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@model", car.Model);
                    cmd.Parameters.AddWithValue("@name", car.Name);
                    cmd.Parameters.AddWithValue("@year", car.Year);

                    cmd.ExecuteNonQuery();
                }

            }
            else if (obj is Song song)
            {
                string query = $"INSERT INTO Song VALUES (NULL, @title, @artist, @year)";

                using (var cmd = new SQLiteCommand(query, conn))
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

        public List<Car> GetCars()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Car";
                List<Car> cars = new List<Car>();

                using (var command = new SQLiteCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string model = reader["model"].ToString();
                            string name = reader["name"].ToString();
                            int year = Convert.ToInt32(reader["year"]);

                            Car car = new Car(id,model,name,year);

                            cars.Add(car);
                        }
                    }
                }

                conn.Close();
                return cars;
            }
        }

        public List<Song> GetSongs()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Song";
                List<Song> songs = new List<Song>();

                using (var command = new SQLiteCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
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
        }

        public static int GetMaxID(string tableName)
        {
            using (var conn = new SQLiteConnection("Data Source=SuSklep.db;Version=3;"))
            {
                conn.Open();
                string query = $"SELECT max(id) FROM {tableName}";

                using (var command = new SQLiteCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                return Convert.ToInt32(reader["id"]);
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                return -1;
                            }
  
                        }
                    }
                }

                conn.Close();
                return 0;
            }
        }


        public void DeleteCar(Car car)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Car WHERE id=@id";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", car.GetId());

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void DeleteData(string tableName, int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = $"DELETE FROM {tableName} WHERE id={id}";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }


        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

    }
}
