using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITests
{
    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform) => platform switch
        {
            Platform.Android => ConfigureApp.Android.StartApp(),
            Platform.iOS => ConfigureApp.iOS.StartApp(),
            _ => throw new System.NotSupportedException("Platform Not Supported"),
        };
    }
}
