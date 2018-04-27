using System;
using System.Threading.Tasks;

using SQLite;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public abstract class BaseDatabase
	{
		#region Constant Fields
		public const string DatabaseFileName = "ImageDatabaseModelSQLite.db3";
		static readonly Lazy<SQLiteAsyncConnection> _databaseConnectionHolder = new Lazy<SQLiteAsyncConnection>(() =>
			DependencyService.Get<ISQLite>().GetConnection());
		#endregion

		#region Fields
		static bool _isInitialized;
		#endregion

		#region Properties
		static SQLiteAsyncConnection DatabaseConnection => _databaseConnectionHolder.Value;
		#endregion

		#region Methods
		protected static async ValueTask<SQLiteAsyncConnection> GetDatabaseConnectionAsync()
		{
			if (!_isInitialized)
				await Initialize();

			return DatabaseConnection;
		}

		static async Task Initialize()
		{
			await DatabaseConnection.CreateTableAsync<DownloadedImageModel>().ConfigureAwait(false);
			_isInitialized = true;
		}
		#endregion

	}
}
