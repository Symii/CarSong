using PracaDomowaCS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowaCS.Objects
{
    public class Song
    {
        private int _id;
        public string Title { get; set; }
        public string Artist { get; set; }
        public int YearReleased { get; set; }

        public Song(int id, string title, string artist, int yearReleased)
        {
            _id = id;
            Title = title;
            Artist = artist;
            YearReleased = yearReleased;
        }

        public static int GetAvaiableIdMySQL()
        {
            return MySQLDatabaseManager.GetMaxID("Song") + 1;
        }

        public static int GetAvaiableIdSQLite()
        {
            return SQLiteDatabaseManager.GetMaxID("Song") + 1;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetTitle()
        {
            return Title;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public string GetArtist()
        {
            return Artist;
        }

        public void SetArtist(string artist)
        {
            Artist = artist;
        }

        public int GetYearReleased()
        {
            return YearReleased;
        }

        public void SetYearReleased(int yearReleased)
        {
            YearReleased = yearReleased;
        }

        public override string ToString()
        {
            return $"id: {_id}, title: {Title}, artist: {Artist}, year: {YearReleased}";
        }

    }
}
