using NUnit.Framework;

using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        #region Constructors
        protected BaseTest(Platform platform) => Platform = platform;
        #endregion

        #region Properties
		protected Platform Platform { get; }
        protected IApp App { get; private set; }
        protected LoadImagePage LoadImagePage { get; private set; }
        #endregion

        #region Methods
        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(Platform);
            LoadImagePage = new LoadImagePage(App);

            App.Screenshot("App Launched");
        }
        #endregion
    }
}

