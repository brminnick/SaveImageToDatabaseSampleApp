using NUnit.Framework;

using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITest
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public abstract class BaseTest
	{
		protected IApp app;
		protected Platform platform;

		protected MainPage MainPage;

		protected BaseTest(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public virtual void TestSetup()
		{
			app = AppInitializer.StartApp(platform);

			MainPage = new MainPage(app, platform);

			app.Screenshot("App Launched");
		}
	}
}

