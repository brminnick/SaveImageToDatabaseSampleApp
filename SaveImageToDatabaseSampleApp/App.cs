using System;

using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Shared;

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

        protected override void OnStart()
        {
            base.OnStart();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    AnalyticsHelpers.Start(AnalyticsConstants.MobileCenteriOSAPIKey);
                    break;
                case Device.Android:
                    AnalyticsHelpers.Start(AnalyticsConstants.MobileCenterAndroidAPIKey);
                    break;
                default:
                    throw new NotSupportedException("Runtime Platform Unsupported");
            }
        }
        #endregion
    }
}
