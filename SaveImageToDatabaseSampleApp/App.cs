using System;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public class App : Application
	{
		#region Fields
		static DownloadedImageModelDatabase _database;
		#endregion

		#region Constructors
		public App()
		{
			MainPage = new NavigationPage(new LoadImagePage())
			{
                BarBackgroundColor = Color.FromHex("3498db"),
				BarTextColor = Color.White
			};
		}
		#endregion

		#region Properties
		public static DownloadedImageModelDatabase Database =>
		_database ??
		(_database = new DownloadedImageModelDatabase());
		#endregion
	}
}
