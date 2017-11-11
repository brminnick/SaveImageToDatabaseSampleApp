using NUnit.Framework;

using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        #region Fields
        readonly Platform _platform;

        IApp _app;
        LoadImagePage _mainPage;
        #endregion

        #region Constructors
        protected BaseTest(Platform platform) => _platform = platform;
        #endregion

        #region Properties
        protected IApp App => _app;
        protected Platform Platform => _platform;
        protected LoadImagePage MainPage => _mainPage;
        #endregion

        #region Methods
        [SetUp]
        public virtual void TestSetup()
        {
            _app = AppInitializer.StartApp(Platform);
            _mainPage = new LoadImagePage(App);

            App.Screenshot("App Launched");
        }
        #endregion
    }
}

