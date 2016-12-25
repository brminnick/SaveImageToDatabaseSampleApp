using System;
using System.IO;
using System.Diagnostics;

using SQLite;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public class DownloadedImageModel
	{
		[PrimaryKey]
		public string ImageUrl { get; set;}

		public string DownloadedImageAsBase64String { get; set; }

		public ImageSource DownloadedImageAsImageStreamFromBase64String
		{
			get
			{
				try
				{
					if (DownloadedImageAsBase64String == null)
					{
						return null;
					}

					var imageString = DownloadedImageAsBase64String;
					var imageByteArray = Convert.FromBase64String(imageString);

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
}
