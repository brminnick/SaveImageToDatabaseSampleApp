using System;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public class App : Application
	{
		static DownloadedImageModelDatabase _database;

		public App()
		{
			MainPage = new NavigationPage(new MainPage());
		}

		public static DownloadedImageModelDatabase Database =>
		_database ??
		(_database = new DownloadedImageModelDatabase());
	}
}
