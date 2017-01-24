using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoC.DataAccess.DbInit
{

    public class DbInit
    {
        private const string TableInit = @";CREATE TABLE Probabilities(Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Probability TEXT , LanguageId INTEGER, WordId INTEGER) ;
                                    CREATE TABLE Words(Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Word TEXT UNIQUE);
                                    CREATE TABLE Languages(Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, LanguageCode TEXT UNIQUE);
                                    CREATE TABLE User(Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Login TEXT UNIQUE NOT NULL, Password TEXT NOT NULL, Token TEXT UNIQUE NOT NULL, Start TEXT NOT NULL, ShelfLife TEXT NOT NULL);
                                    CREATE TABLE UserProfile(Id INTEGER UNIQUE, AverageTime INTEGER, LastRequest TEXT, RequestCounter INTEGER);";

        private static readonly string dbPath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\project.db";
        private readonly static string[] SupportedLangugaesCode = new string[] { "en", "pt", "se", "ru", "bg" };

        public static string ConnectionString
        {
            get
            {
                return "Data Source=" + dbPath + "; Version=3;";
            }
        }

        public static void InitDB()
        {
            var isNew = CheckDB();
            TestConnection();
            InitBaselineData(isNew);
        }

        private static void InitBaselineData(bool isNew)
        {
            if (!isNew)
            {
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = TableInit;
                command.ExecuteNonQuery();
                string newLanguages = "Insert into Languages (LanguageCode) values";
                List<string> parameters = new List<string>();
                foreach (var languageCode in SupportedLangugaesCode)
                {
                    parameters.Add(string.Format("('{0}')", languageCode));
                }
                newLanguages += string.Join(",", parameters);
                command.Parameters.Clear();
                command.CommandText = newLanguages;
                command.ExecuteNonQuery();
            }
        }

        private static void TestConnection()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                connection.Clone();
            }
        }

        private static bool CheckDB()
        {
            bool isNew = false;
            if (!Directory.Exists(Path.GetDirectoryName(dbPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            }
            if (!File.Exists(dbPath))
            {
                isNew = true;
                File.Create(dbPath).Dispose();
            }
            return isNew;
        }
    }
}
