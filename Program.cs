using PracaDomowaCS;
using PracaDomowaCS.Managers;
using PracaDomowaCS.Objects;
using System.Data;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        MySQLDatabaseManager mysql = new MySQLDatabaseManager("localhost", "SuSklep", "root", "");
        SQLiteDatabaseManager sqlite = new SQLiteDatabaseManager("SuSklep.db");
        Menu menu = new Menu(mysql, sqlite);

        Thread.Sleep(1000);
        Console.Clear();

        while (true)
        {
            menu.DisplayMenu();

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (menu.ProcessChoice(choice))
                {
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