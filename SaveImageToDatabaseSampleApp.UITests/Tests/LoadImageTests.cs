using System.Threading.Tasks;

using NUnit.Framework;

using Xamarin.UITest;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp.UITests
{
	public class LoadImageTests : BaseTest
	{
		public LoadImageTests(Platform platform) : base(platform)
		{
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task DownloadImageFromValidUrl(bool shouldPressKeyboardReturnButton)
		{
			//Arrange

			//Act
			LoadImagePage.EnterUrl(LoadImagePage.ValidUrl);

			//Assert
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, LoadImagePage.LoadImageButtonText);

			//Act
			switch (shouldPressKeyboardReturnButton)
			{
				case true:
					LoadImagePage.TapKeyboardEnterButton();
					break;

				case false:
					LoadImagePage.TapLoadImageButton();
					break;
			}
			await LoadImagePage.WaitForNoIsDownloadingActivityIndicator();

			//Assert
			Assert.IsTrue(LoadImagePage.IsDownloadedImageShown);
			Assert.AreEqual(LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText, LoadImagePage.LoadImageButtonText);

			//Act
			LoadImagePage.TapClearImageButton();

			//Assert
			Assert.IsFalse(LoadImagePage.IsDownloadedImageShown);
			Assert.AreEqual(LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText, LoadImagePage.LoadImageButtonText);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task DownloadImageFromInvalidUrl(bool shouldPressKeyboardReturnButton)
		{
			//Arrange

			//Act
			LoadImagePage.EnterUrl(LoadImagePage.InvalidUrl);

			//Assert
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, LoadImagePage.LoadImageButtonText);

			//Act
			switch (shouldPressKeyboardReturnButton)
			{
				case true:
					LoadImagePage.TapKeyboardEnterButton();
					break;
				
				case false:
					LoadImagePage.TapLoadImageButton();
					break;
			}

			await LoadImagePage.WaitForNoIsDownloadingActivityIndicator();
			LoadImagePage.TapOkOnErrorPrompt();

			//Assert
			Assert.IsFalse(LoadImagePage.IsDownloadedImageShown);
			Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, LoadImagePage.LoadImageButtonText);
		}
	}
}
