using System.Threading.Tasks;

using NUnit.Framework;

using Xamarin.UITest;
using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp.UITest
{
	public class MainPageTests : BaseTest
	{
		public MainPageTests(Platform platform) : base(platform)
		{
		}

		[Test]
		public void AppLaunches()
		{

		}

		[Test]
		public async Task DownloadImageFromValidUrl()
		{
			//Arrange

			//Act
			MainPage.EnterUrl(MainPage.ValidUrl);

			//Assert
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, MainPage.LoadImageButtonText);

			//Act
			MainPage.TapLoadImageButton();
			await MainPage.WaitForNoIsDownloadingActivityIndicator();

			//Assert
			Assert.IsTrue(MainPage.IsDownloadedImageShown);
			Assert.AreEqual(LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText, MainPage.LoadImageButtonText);

			//Act
			MainPage.TapClearImageButton();

			//Assert
			Assert.IsFalse(MainPage.IsDownloadedImageShown);
			Assert.AreEqual(LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText, MainPage.LoadImageButtonText);
		}

		[Test]
		public async Task DownloadImageFromInvalidUrl()
		{
			//Arrange

			//Act
			MainPage.EnterUrl(MainPage.InvalidUrl);

			//Assert
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, MainPage.LoadImageButtonText);

			//Act
			MainPage.TapLoadImageButton();
			await MainPage.WaitForNoIsDownloadingActivityIndicator();
			MainPage.TapOkOnErrorPrompt();

			//Assert
			Assert.IsFalse(MainPage.IsDownloadedImageShown);
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, MainPage.LoadImageButtonText);
		}
	}
}
