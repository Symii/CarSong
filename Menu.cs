using PracaDomowaCS.Enums;
using PracaDomowaCS.Managers;
using PracaDomowaCS.Objects;
using PracaDomowaCS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowaCS
{
    internal class Menu
    {
        private Dictionary<string, string> create_messages = new Dictionary<string, string>();
        private Dictionary<string, string> input = new Dictionary<string, string>();

        private MySQLDatabaseManager mysql;
        private SQLiteDatabaseManager sqlite;

        public Menu(MySQLDatabaseManager mysql, SQLiteDatabaseManager sqlite)
        {
            this.mysql = mysql;
            this.sqlite = sqlite;

            create_messages.Add("0-0", "Jaki rodzaj obiektu chcesz stworzyc?\n1. Car\n2. Song\n3. Wyjście\nWybierz opcję (1-3):");

            create_messages.Add("1-1", "Podaj model:");
            create_messages.Add("1-2", "Podaj nazwe:");
            create_messages.Add("1-3", "Podaj rocznik:");

            create_messages.Add("2-1", "Podaj tytuł:");
            create_messages.Add("2-2", "Podaj artyste:");
            create_messages.Add("2-3", "Podaj rok wydania:");
        }

        public void DisplayMenu()
        {
            Console.WriteLine("1. Stworz obiekt");
            Console.WriteLine("2. Wyswietl obiekty");
            Console.WriteLine("3. Usun obiekt");
            Console.WriteLine("4. Wyjście");
            Console.Write("Wybierz opcję (1-4): ");
        }

        public bool ProcessChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    CreateChoice();
                    return true;
                case 2:
                    DisplayObjectsMenu();
                    return true;
                case 3:
                    Console.WriteLine("Wybrano opcję 3");
                    return true;
                case 4:
                    Console.WriteLine("Wybrano wyjście. Program zakończył działanie.");
                    Environment.Exit(0);
                    return true;
                default:
                    Console.WriteLine("Niepoprawny wybór. Wybierz opcję od 1 do 4.");
                    return false;
            }
        }

        public void ProcessDisplayObjects(TableType tableType)
        {
            bool isMysql;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Z jakiej bazy danych wyswietlic? [MySQL] lub [SQLite]");
                string choice = Console.ReadLine().ToLower();
                if (choice == "mysql")
                {
                    isMysql = true;
                    break;
                }
                else if (choice == "sqlite")
                {
                    isMysql = false;
                    break;
                }
            }

            switch (tableType)
            {
                case TableType.CAR:
                    {
                        if (isMysql)
                        {
                            FormatUtil.PrintMessages(FormatUtil.CarsToString(this.mysql.GetCars()));
                            return;
                        }
                        FormatUtil.PrintMessages(FormatUtil.CarsToString(this.sqlite.GetCars()));
                        break;
                    }
                    
                case TableType.SONG:
                    {
                        if (isMysql)
                        {
                            FormatUtil.PrintMessages(FormatUtil.SongsToString(this.mysql.GetSongs()));
                            return;
                        }
                        FormatUtil.PrintMessages(FormatUtil.SongsToString(this.sqlite.GetSongs()));
                        break;
                    }
                    
                default:
                    break;
            }
        }

        public void DisplayObjectsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Jaka tabele chcesz wyswietlic?\n1. Car\n2. Song\n3. Wyjscie");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ProcessDisplayObjects(TableType.CAR);
                            return;
                        case 2:
                            ProcessDisplayObjects(TableType.SONG);
                            return;
                        case 3:
                            Environment.Exit(0);
                            return;
                        default:
                            Console.WriteLine("Niepoprawny wybór. Wybierz opcję od 1 do 3.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Niepoprawny format. Wprowadź liczbę.");
                }

                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
                Console.Clear();
            }
        }


        public void ProcessCreate(int choice, int max_stage)
        {
            int stage = 1;
            while (stage <= max_stage)
            {
                Console.Clear();
                Console.WriteLine(create_messages[$"{choice}-{stage}"]);
                input.Add($"{choice}-{stage}", Console.ReadLine());
                stage++;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Czy zapisac do bazy danych? [tak/nie]");
                string readLine = Console.ReadLine();

                if (readLine == "tak")
                {
                    bool mysql;

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Wybierz baze danych: [MySQL / SQLite]");
                        readLine = Console.ReadLine();
                        if (readLine.ToLower() == "mysql")
                        {
                            Console.WriteLine("Trwa dodawanie danych do bazy MySQL...");
                            mysql = true;
                            break;
                        }
                        else if (readLine.ToLower() == "sqlite")
                        {
                            Console.WriteLine("Trwa dodawanie danych do bazy SQLite...");
                            mysql = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wybierz: [MySQL] lub [SQLite]");
                        }
                    }


                    Car car;
                    Song song;

                    if (choice == 1)
                    {
                        int id = Car.GetAvaiableIdMySQL();
                        if (mysql == false)
                        {
                            id = Car.GetAvaiableIdMySQL();
                        }
                        
                        string model = input["1-1"];
                        string name = input["1-2"];
                        int year = int.Parse(input["1-3"]);
                        car = new Car(id, model, name, year);
                        if (mysql)
                        {
                            this.mysql.InsertData(car);
                            return;
                        }
                        this.sqlite.InsertData(car);
                    }
                    else
                    {
                        int id = Song.GetAvaiableIdMySQL();
                        if (mysql == false)
                        {
                            id = Song.GetAvaiableIdSQLite();
                        }
                        
                        string title = input["2-1"];
                        string artist = input["2-2"];
                        int year = int.Parse(input["2-3"]);
                        song = new Song(id, title, artist, year);
                        if (mysql)
                        {
                            this.mysql.InsertData(song);
                            return;
                        }
                        this.sqlite.InsertData(song);
                    }

                    
                    return;
                }
                else if (readLine == "nie")
                {
                    Console.WriteLine("Pomyslnie zakonczono dzialanie programu, zmiany nie zostaly wprowadzone.");
                    return;
                }
                else
                {
                    Console.WriteLine("Wybierz \"tak\" lub \"nie\"");
                }

                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
            }



            foreach (var i in input)
            {
                Console.WriteLine($"{i.Key} -> {i.Value}");
            }
        }

        public void CreateChoice()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(create_messages["0-0"]);

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ProcessCreate(1, 3);
                            return;
                        case 2:
                            ProcessCreate(2, 3);
                            return;
                        case 3:
                            Environment.Exit(0);
                            return;
                        default:
                            Console.WriteLine("Niepoprawny wybór. Wybierz opcję od 1 do 3.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Niepoprawny format. Wprowadź liczbę.");
                }

                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
                Console.Clear();
            }
        }


    }
}
