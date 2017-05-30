using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public class App : Application
	{
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
	}
}
