using NUnit.Framework;
using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITests
{
    public class ReplTests : BaseTest
    {
        public ReplTests(Platform platform) : base(platform)
        {
        }

        [Test, Ignore("REPL used for manually navigating the UI")]
        public void Repl() => App.Repl();
    }
}
