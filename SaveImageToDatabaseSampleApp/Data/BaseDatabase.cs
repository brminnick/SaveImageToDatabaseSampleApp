using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SQLite;

using Xamarin.Essentials;

namespace SaveImageToDatabaseSampleApp
{
    abstract class BaseDatabase
    {
        #region Constant Fields
        readonly static string _databasePath = Path.Combine(FileSystem.AppDataDirectory, $"{nameof(SaveImageToDatabaseSampleApp)}.db3");

        static readonly Lazy<SQLiteAsyncConnection> _databaseConnectionHolder = new Lazy<SQLiteAsyncConnection>(() =>
            new SQLiteAsyncConnection(_databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));
        #endregion

        #region Properties
        static SQLiteAsyncConnection DatabaseConnection => _databaseConnectionHolder.Value;
        #endregion

        #region Methods
        protected static async ValueTask<SQLiteAsyncConnection> GetDatabaseConnectionAsync()
        {
            if (!DatabaseConnection.TableMappings.Any())
                await DatabaseConnection.CreateTableAsync<DownloadedImageModel>().ConfigureAwait(false);

            return DatabaseConnection;
        }
        #endregion

    }
}
