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

		protected LoadImagePage MainPage;

		protected BaseTest(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public virtual void TestSetup()
		{
			app = AppInitializer.StartApp(platform);

			MainPage = new LoadImagePage(app, platform);

			app.Screenshot("App Launched");
		}
	}
}

