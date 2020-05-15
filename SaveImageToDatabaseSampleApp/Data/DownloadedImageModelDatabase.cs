using System.Threading.Tasks;
using System.Collections.Generic;

namespace SaveImageToDatabaseSampleApp
{
    abstract class DownloadedImageModelDatabase : BaseDatabase
    {
        public static async Task<List<DownloadedImageModel>> GetAllDownloadedImagesAsync()
        {
            var databaseConnection = await GetDatabaseConnection<DownloadedImageModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<DownloadedImageModel>().ToListAsync()).ConfigureAwait(false);
        }

        public static async Task<DownloadedImageModel> GetDownloadedImageAsync(string imageUrl)
        {
            var databaseConnection = await GetDatabaseConnection<DownloadedImageModel>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<DownloadedImageModel>().Where(x => x.ImageUrl.Equals(imageUrl)).FirstOrDefaultAsync()).ConfigureAwait(false);
        }

        public static async Task SaveDownloadedImage(DownloadedImageModel downloadedImage)
        {
            var databaseConnection = await GetDatabaseConnection<DownloadedImageModel>().ConfigureAwait(false);

            await AttemptAndRetry(() => databaseConnection.InsertOrReplaceAsync(downloadedImage)).ConfigureAwait(false);
        }
    }
}

