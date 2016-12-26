using Android.OS;
using Android.App;
using Android.Content.PM;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp.Droid
{
	[Activity(Label = "SaveImageToDatabaseSampleApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			AnalyticsHelpers.Start(AnalyticsConstants.MobileCenterAndroidAPIKey);

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}
	}
}
