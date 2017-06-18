using System;
using System.IO;
using System.Diagnostics;

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
                if (DownloadedImageBlob == null)
                    return null;

                var imageByteArray = DownloadedImageBlob;

                return ImageSource.FromStream(() => new MemoryStream(imageByteArray));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
