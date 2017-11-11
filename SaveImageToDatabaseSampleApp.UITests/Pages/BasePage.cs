using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace SaveImageToDatabaseSampleApp.UITests
{
	public class BasePage
	{
		protected BasePage(IApp app)
		{
			App = app;

            IsAndroid = app is AndroidApp;
            IsiOS = app is iOSApp;
		}

        protected IApp App { get; }
        protected bool IsAndroid { get; }
        protected bool IsiOS { get; }
	}
}

