using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITests
{
    public class BasePage
    {
        protected BasePage(IApp app) => App = app;

        protected IApp App { get; }
    }
}

