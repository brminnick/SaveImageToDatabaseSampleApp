using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp
{
    public class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new LoadImagePage())
            {
                BarBackgroundColor = Color.FromHex(ColorConstants.NavigationPageBarBackgroundColorHex),
                BarTextColor = Color.FromHex(ColorConstants.NavigationPageBarTextColorHex)
            };
        }

        protected override void OnStart()
        {
            base.OnStart();

			AnalyticsServices.Start();
        }
    }
}
