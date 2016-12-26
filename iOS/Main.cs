using UIKit;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp.iOS
{
	public class Application
	{
		static void Main(string[] args)
		{
			AnalyticsHelpers.Start(AnalyticsConstants.MobileCenteriOSAPIKey);

			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
