using System;
using System.ComponentModel;
using System.IO;

using SQLite;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
    public record DownloadedImageModel
    {
		public ImageSource? DownloadedImageAsImageStream => GetImageStream();

        [PrimaryKey]
        public string ImageUrl { get; init; } = string.Empty;

        public byte[]? DownloadedImageBlob { get; init; }

        ImageSource? GetImageStream()
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

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}
