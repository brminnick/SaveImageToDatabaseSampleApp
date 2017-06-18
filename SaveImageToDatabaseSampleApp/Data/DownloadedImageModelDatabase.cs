using System.Threading.Tasks;
using System.Collections.Generic;

namespace SaveImageToDatabaseSampleApp
{
    public abstract class DownloadedImageModelDatabase : BaseDatabase
    {
        #region Methods
        public static async Task<List<DownloadedImageModel>> GetAllDownloadedImagesAsync()
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            return await databaseConnection.Table<DownloadedImageModel>().ToListAsync();
        }

        public static async Task<DownloadedImageModel> GetDownloadedImageAsync(string imageUrl)
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            return await databaseConnection.Table<DownloadedImageModel>().Where(x => x.ImageUrl.Equals(imageUrl)).FirstOrDefaultAsync();
        }

        public static async Task SaveDownloadedImage(DownloadedImageModel downloadedImage)
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            await databaseConnection.InsertOrReplaceAsync(downloadedImage);
        }
        #endregion
    }
}

