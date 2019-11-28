using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        readonly Platform _platform;

        IApp? _app;
        LoadImagePage? _loadImagePage;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp App => _app ?? throw new NullReferenceException();
        protected LoadImagePage LoadImagePage => _loadImagePage ?? throw new NullReferenceException();

        [SetUp]
        public virtual void TestSetup()
        {
            _app = AppInitializer.StartApp(_platform);
            _loadImagePage = new LoadImagePage(App);

            App.Screenshot("App Launched");
        }
    }
}

