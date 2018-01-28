using System;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using SaveImageToDatabaseSampleApp.iOS;
using SaveImageToDatabaseSampleApp.Shared;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageCustomRenderer))]
namespace SaveImageToDatabaseSampleApp.iOS
{
    public class NavigationPageCustomRenderer : NavigationRenderer
    {
        UIColor NavigationBarBackgroundColor => Color.FromHex(ColorConstants.NavigationPageBarBackgroundColorHex).ToUIColor();
        UIColor NavigationBarTextColor => Color.FromHex(ColorConstants.NavigationPageBarTextColorHex).ToUIColor();

        public override void ViewDidLoad()
        {
			base.ViewDidLoad();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                NavigationBar.PrefersLargeTitles = true;

                NavigationBar.LargeTitleTextAttributes = new UIStringAttributes
                {
                    ForegroundColor = NavigationBarTextColor
                };
            }

            NavigationBar.TintColor = NavigationBarBackgroundColor;
        }
    }
}
