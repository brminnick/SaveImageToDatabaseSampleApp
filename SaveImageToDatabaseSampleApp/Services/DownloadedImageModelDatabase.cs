using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SQLite;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public static class DownloadedImageModelDatabase
	{
		#region Constant Fields
		static readonly SQLiteAsyncConnection _database = DependencyService.Get<ISQLite>().GetConnection();
		#endregion

		#region Fields
		static bool _isInitialized;
		#endregion

		#region Methods
		public static async Task<List<DownloadedImageModel>> GetAllDownloadedImagesAsync()
		{
			if (!_isInitialized)
				await InitializeDatabase();
			
			return await _database.Table<DownloadedImageModel>().ToListAsync();
		}

		public static async Task<DownloadedImageModel> GetDownloadedImageAsync(string imageUrl)
		{
			if (!_isInitialized)
				await InitializeDatabase();
			
			return await _database.Table<DownloadedImageModel>().Where(x => x.ImageUrl.Equals(imageUrl)).FirstOrDefaultAsync();
		}

		public static async Task SaveDownloadedImage(DownloadedImageModel downloadedImage)
		{
			if (!_isInitialized)
				await InitializeDatabase();
			
			if (await GetDownloadedImageAsync(downloadedImage.ImageUrl) != null)
			{
				await _database.UpdateAsync(downloadedImage);
			}
			else {
				await _database.InsertAsync(downloadedImage);
			}
		}

		static async Task InitializeDatabase()
		{
			await _database.CreateTableAsync<DownloadedImageModel>();
			_isInitialized = true;
		}
		#endregion
	}
}

