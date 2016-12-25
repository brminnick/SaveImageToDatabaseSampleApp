using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SQLite;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public class DownloadedImageModelDatabase
	{
		static readonly object _locker = new object();

		readonly SQLiteConnection _database;

		public DownloadedImageModelDatabase()
		{
			_database = DependencyService.Get<ISQLite>().GetConnection();
			// create the tables
			_database.CreateTable<DownloadedImageModel>();
		}

		public async Task<List<DownloadedImageModel>> GetAllDownloadedImagesAsync()
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Table<DownloadedImageModel>().ToList();
				}
			});
		}

		public async Task<DownloadedImageModel> GetDownloadedImageAsync(string imageUrl)
		{
			return await Task.Run(() =>
			{
				lock (_locker)
				{
					return _database.Table<DownloadedImageModel>().FirstOrDefault(x => x.ImageUrl == imageUrl);
				}
			});
		}

		public async Task SaveDownloadedImage(DownloadedImageModel downloadedImage)
		{
			await Task.Run(async () =>
			{
				if (await GetDownloadedImageAsync(downloadedImage.ImageUrl) != null)
				{
					lock (_locker)
					{
						_database.Update(downloadedImage);
					}
				}
				else {
					lock (_locker)
					{
						_database.Insert(downloadedImage);
					}
				}
			});
		}
	}
}

