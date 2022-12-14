using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using Xamarin.Essentials;
using System.Reflection;

namespace BestBy {
    public class DB {
        private static string DBName = "log.db";
        public static SQLiteConnection conn;
        public static void OpenConnection() {
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, DBName);
            conn = new SQLiteConnection(fname);
            conn.CreateTable<Product>();
        }
    }
}
