using System.Threading.Tasks;

using NUnit.Framework;

using Xamarin.UITest;
using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp.UITest
{
	public class LoadImageTests : BaseTest
	{
		public LoadImageTests(Platform platform) : base(platform)
		{
		}

		[Test]
		public void AppLaunches()
		{

		}

		[TestCase(false)]
		[TestCase(true)]
		[Test]
		public async Task DownloadImageFromValidUrl(bool shouldPressKeyboardReturnButton)
		{
			//Arrange

			//Act
			MainPage.EnterUrl(MainPage.ValidUrl);

			//Assert
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, MainPage.LoadImageButtonText);

			//Act
			switch (shouldPressKeyboardReturnButton)
			{
				case true:
					MainPage.TapKeyboardEnterButton();
					break;

				case false:
					MainPage.TapLoadImageButton();
					break;
			}
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

		[TestCase(false)]
		[TestCase(true)]
		[Test]
		public async Task DownloadImageFromInvalidUrl(bool shouldPressKeyboardReturnButton)
		{
			//Arrange

			//Act
			MainPage.EnterUrl(MainPage.InvalidUrl);

			//Assert
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, MainPage.LoadImageButtonText);

			//Act
			switch (shouldPressKeyboardReturnButton)
			{
				case true:
					MainPage.TapKeyboardEnterButton();
					break;
				
				case false:
					MainPage.TapLoadImageButton();
					break;
			}

			await MainPage.WaitForNoIsDownloadingActivityIndicator();
			MainPage.TapOkOnErrorPrompt();

			//Assert
			Assert.IsFalse(MainPage.IsDownloadedImageShown);
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, MainPage.LoadImageButtonText);
		}
	}
}
