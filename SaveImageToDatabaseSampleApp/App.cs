using SaveImageToDatabaseSampleApp.Shared;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SaveImageToDatabaseSampleApp
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            Device.SetFlags(new[] { "Markup_Experimental" });

            var navigationPage = new Xamarin.Forms.NavigationPage(new LoadImagePage())
            {
                BarBackgroundColor = Color.FromHex(ColorConstants.NavigationPageBarBackgroundColorHex),
                BarTextColor = Color.FromHex(ColorConstants.NavigationPageBarTextColorHex)
            };

            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            base.OnStart();

			AnalyticsServices.Start();
        }
    }
}
