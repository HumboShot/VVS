using System;
using SQLite;
using System.IO;
using VVS.Database;
using Xamarin.Forms;
using VVS.Droid.Database;

[assembly: Dependency(typeof(SQLiteDB))]

namespace VVS.Droid.Database
{
    public class SQLiteDB : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "VVSDB.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}