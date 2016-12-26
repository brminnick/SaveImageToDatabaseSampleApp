using NUnit.Framework;

using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITest
{
	public class ReplTests :BaseTest
	{
		public ReplTests(Platform platform): base(platform)
		{
		}

		[Ignore]
		[Test]
		public void Repl()
		{
			app.Repl();
		}
	}
}
