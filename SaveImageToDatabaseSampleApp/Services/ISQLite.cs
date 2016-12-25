using SQLite;

namespace SaveImageToDatabaseSampleApp
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}

