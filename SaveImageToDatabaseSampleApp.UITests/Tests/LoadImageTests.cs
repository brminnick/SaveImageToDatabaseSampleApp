using System.Threading.Tasks;
using NUnit.Framework;
using SaveImageToDatabaseSampleApp.Shared;
using Xamarin.UITest;

namespace SaveImageToDatabaseSampleApp.UITests
{
    public class LoadImageTests : BaseTest
    {
        public LoadImageTests(Platform platform) : base(platform)
        {
        }

        [TestCase(false)]
        [TestCase(true)]
        public void DownloadImageFromValidUrl(bool shouldPressKeyboardReturnButton)
        {
            //Arrange

            //Act
            LoadImagePage.EnterUrl(LoadImagePage.ValidUrl);

            //Assert
            Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, LoadImagePage.LoadImageButtonText);

            //Act
            if (shouldPressKeyboardReturnButton)
                LoadImagePage.TapKeyboardEnterButton();
            else
                LoadImagePage.TapLoadImageButton();

            LoadImagePage.WaitForNoIsDownloadingActivityIndicator();

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
        public void DownloadImageFromInvalidUrl(bool shouldPressKeyboardReturnButton)
        {
            //Arrange

            //Act
            LoadImagePage.EnterUrl(LoadImagePage.InvalidUrl);

            //Assert
            Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, LoadImagePage.LoadImageButtonText);

            //Act
            if (shouldPressKeyboardReturnButton)
                LoadImagePage.TapKeyboardEnterButton();
            else
                LoadImagePage.TapLoadImageButton();

            LoadImagePage.WaitForNoIsDownloadingActivityIndicator();
            LoadImagePage.TapOkOnErrorPrompt();

            //Assert
            Assert.IsFalse(LoadImagePage.IsDownloadedImageShown);
            Assert.AreEqual(LoadImageButtonTextConstants.DownloadImageFromUrlButtonText, LoadImagePage.LoadImageButtonText);
        }
    }
}
