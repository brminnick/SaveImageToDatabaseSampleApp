using System;
using System.IO;

using SQLite;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
    public class DownloadedImageModel
    {
		public ImageSource DownloadedImageAsImageStream => GetImageStream();

        [PrimaryKey]
        public string ImageUrl { get; set; }

        public byte[] DownloadedImageBlob { get; set; }

        ImageSource GetImageStream()
        {
            try
            {
                if (DownloadedImageBlob is null)
                    return null;

                var imageByteArray = DownloadedImageBlob;

                return ImageSource.FromStream(() => new MemoryStream(imageByteArray));
            }
            catch (Exception e)
            {
                AnalyticsServices.Report(e);
                return null;
            }
        }
    }
}
