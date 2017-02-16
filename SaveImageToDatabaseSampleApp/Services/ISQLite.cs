using SQLite;

namespace SaveImageToDatabaseSampleApp
{
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

