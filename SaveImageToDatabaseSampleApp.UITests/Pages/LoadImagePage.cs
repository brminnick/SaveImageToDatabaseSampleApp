using System;
using System.Linq;
using SaveImageToDatabaseSampleApp.Shared;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SaveImageToDatabaseSampleApp.UITests
{
    public class LoadImagePage : BasePage
    {
        readonly Query _loadImageButton, _imageUrlEntry, _downloadedImage, _isDownloadingActivityIndicator, _clearImageButton;

        public LoadImagePage(IApp app) : base(app)
        {
            _loadImageButton = x => x.Marked(AutomationIdConstants.LoadImageButton);
            _imageUrlEntry = x => x.Marked(AutomationIdConstants.ImageUrlEntry);
            _downloadedImage = x => x.Marked(AutomationIdConstants.DownloadedImage);
            _isDownloadingActivityIndicator = x => x.Marked(AutomationIdConstants.IsDownloadingActivityIndicator);
            _clearImageButton = x => x.Marked(AutomationIdConstants.ClearImageButton);
        }

        public string ValidUrl { get; } = UrlConstants.ImageUrl;

        public string InvalidUrl { get; } = @"https://www.microsoft.com/abc123";

        public string LoadImageButtonText => GetLoadImageButtonText();

        public bool IsDownloadedImageShown => App.Query(_downloadedImage).Any();

        public bool IsErrorPromptVisible => App.Query("Ok").Any();

        public void EnterUrl(string url)
        {
            EnterText(_imageUrlEntry, url);
            App.DismissKeyboard();
            App.Screenshot($"Entered Test: {url}");
        }

        public void TapKeyboardEnterButton()
        {
            App.Tap(_imageUrlEntry);
            App.PressEnter();
            App.DismissKeyboard();
            App.Screenshot("Tapped Keyboard Return Button");
        }

        public void TapLoadImageButton()
        {
            App.Tap(_loadImageButton);
            App.Screenshot("Tapped Load Image Button");
        }

        public void WaitForNoIsDownloadingActivityIndicator(int timeoutInSeconds = 60)
        {
            App.WaitForNoElement(_isDownloadingActivityIndicator, "Is Downloading Activity Indicator Never Disappeared", TimeSpan.FromSeconds(timeoutInSeconds));
            App.Screenshot("Is Downloading Activity Indicator Dissapeared");
        }

        public void TapOkOnErrorPrompt()
        {
            App.Tap("Ok");
            App.Screenshot("Tapped Ok On Error Prompt");
        }

        public void TapClearImageButton()
        {
            App.Tap(_clearImageButton);
            App.Screenshot("Clear Image Button Tapped");
        }

        string GetLoadImageButtonText() => App switch
        {
            AndroidApp androidApp => androidApp.Query(_loadImageButton).First().Text,
            iOSApp iOSApp => iOSApp.Query(_loadImageButton).First().Label,
            _ => throw new NotSupportedException(),
        };

        void EnterText(Query query, string url)
        {
            App.ClearText(query);
            App.EnterText(query, url);
        }
    }
}
