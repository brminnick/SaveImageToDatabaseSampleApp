using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.UITest;

using SaveImageToDatabaseSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SaveImageToDatabaseSampleApp.UITest
{
	public class MainPage : BasePage
	{
		#region Constant Fields
		readonly Query _loadImageButton, _imageUrlEntry, _downloadedImage, _isDownloadingActivityIndicator;
		#endregion

		#region Constructors
		public MainPage(IApp app, Platform platform) : base(app, platform)
		{
			_loadImageButton = x => x.Marked(AutomationIdConstants.LoadImageButton);
			_imageUrlEntry = x => x.Marked(AutomationIdConstants.ImageUrlEntry);
			_downloadedImage = x => x.Marked(AutomationIdConstants.DownloadedImage);
			_isDownloadingActivityIndicator = x => x.Marked(AutomationIdConstants.IsDownloadingActivityIndicator);
		}
		#endregion

		#region Properties
		public string ValidUrl =>
			@"https://www.xamarin.com/content/images/pages/branding/assets/xamarin-logo.png";

		public string InvalidUrl =>
			@"https://www.xamarin.com/abc123";

		public string LoadImageButtonText =>
			GetLoadImageButtonText();

		public bool IsDownloadedImageShown =>
			app.Query(_downloadedImage)?.Length > 0;

		public bool IsErrorPromptVisible =>
			app.Query("Ok")?.Length > 0;
		#endregion

		#region Methods
		public void EnterUrl(string url)
		{
			app.Tap(_imageUrlEntry);
			app.ClearText();
			app.EnterText(url);
			app.DismissKeyboard();
			app.Screenshot($"Entered Test: {url}");
		}

		public void TapLoadImageButton()
		{
			app.Tap(_loadImageButton);
			app.Screenshot("Tapped Load Image Button");
		}

		public async Task WaitForNoIsDownloadingActivityIndicator(int timeoutInSeconds = 60)
		{
			await Task.Delay(1000);
			app.WaitForNoElement(_isDownloadingActivityIndicator,"Is Downloading Activity Indicator Never Disappeared",TimeSpan.FromSeconds(timeoutInSeconds));
			app.Screenshot("Is Downloading Activity Indicator Dissapeared");
		}

		public void TapOkOnErrorPrompt()
		{
			app.Tap("Ok");
			app.Screenshot("Tapped Ok On Error Prompt");
		}

		string GetLoadImageButtonText()
		{
			if(IsAndroid)
				return app.Query(_loadImageButton)?.FirstOrDefault()?.Text;

			return app.Query(_loadImageButton)?.FirstOrDefault()?.Label;
		}
		#endregion
	}
}
