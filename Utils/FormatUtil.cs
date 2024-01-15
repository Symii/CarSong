using PracaDomowaCS.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowaCS.Utils
{
    public class FormatUtil
    {
        public static List<string> CarsToString(List<Car> list)
        { 
            List<string> result = new List<string>();
            result.Add("id   |   model   |   name   |   year");
            foreach (var item in list)
            {
                result.Add($"{item.GetId()}       {item.GetModel()}       {item.GetName()}       {item.GetYear()}");
            }

            return result;
        }

        public static List<string> SongsToString(List<Song> list)
        {
            List<string> result = new List<string>();
            result.Add("id   |   title   |   artist   |   year_released");
            foreach (var item in list)
            {
                result.Add($"{item.GetId()}       {item.GetTitle()}       {item.GetArtist()}       {item.GetYearReleased()}");
            }

            return result;
        }

        public static void PrintMessages(List<string> list)
        {
            foreach (string s in list)
            {
                Console.WriteLine(s);
            }
        }
    }
}
