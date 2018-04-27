using System;
using System.IO;

using SQLite;

using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Droid;

[assembly: Dependency(typeof(SQLite_Android))]
namespace SaveImageToDatabaseSampleApp.Droid
{
    public class SQLite_Android : ISQLite
    {
        #region ISQLite implementation
        public SQLiteAsyncConnection GetConnection()
        {
            var sqliteFilename = BaseDatabase.DatabaseFileName;
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);

            return new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        }
        #endregion
    }
}