using PracaDomowaCS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowaCS.Objects
{
    public class Car
    {
        private int _id;
        public string Model { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        public Car(int id, string model, string name, int year)
        {
            _id = id;
            Model = model;
            Name = name;
            Year = year;
        }

        public static int GetAvaiableIdMySQL()
        {
            return MySQLDatabaseManager.GetMaxID("Car") + 1;
        }

        public static int GetAvaiableIdSQLite()
        {
            return SQLiteDatabaseManager.GetMaxID("Car") + 1;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetModel()
        {
            return Model;
        }

        public void SetModel(string model)
        {
            Model = model;
        }

        public string GetName()
        {
            return Name;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public int GetYear()
        {
            return Year;
        }

        public void SetYear(int year)
        {
            Year = year;
        }

        public override string ToString()
        {
            return $"Id: {_id}, Model: {Model}, Name: {Name}, Year: {Year}";
        }

    }
}
