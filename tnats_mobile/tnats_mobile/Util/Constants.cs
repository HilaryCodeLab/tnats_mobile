using System;
using System.IO;

namespace tnats_mobile.Util
{
    public class Constants
    {
        public const string DatabaseFilename = "TodoSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        /// <summary>
        /// METHOD THAT RETURNS THE DATABASE PHYSICAL PATH
        /// </summary>
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        public static string RestUrl = "http://tnats-admin-portal.herokuapp.com";
    }
}
