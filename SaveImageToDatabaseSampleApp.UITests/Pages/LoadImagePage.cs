using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.UITest;

using SaveImageToDatabaseSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using Xamarin.UITest.Android;

namespace SaveImageToDatabaseSampleApp.UITests
{
    public class LoadImagePage : BasePage
    {
        readonly Query _loadImageButton;
        readonly Query _imageUrlEntry;
        readonly Query _downloadedImage;
        readonly Query _isDownloadingActivityIndicator;
        readonly Query _clearImageButton;
        public LoadImagePage(IApp app) : base(app)
        {
            _loadImageButton = x => x.Marked(AutomationIdConstants.LoadImageButton);
            _imageUrlEntry = x => x.Marked(AutomationIdConstants.ImageUrlEntry);
            _downloadedImage = x => x.Marked(AutomationIdConstants.DownloadedImage);
            _isDownloadingActivityIndicator = x => x.Marked(AutomationIdConstants.IsDownloadingActivityIndicator);
            _clearImageButton = x => x.Marked(AutomationIdConstants.ClearImageButton);
        }

        public string ValidUrl =>
            @"https://www.xamarin.com/content/images/pages/branding/assets/xamarin-logo.png";

        public string InvalidUrl => @"https://www.xamarin.com/abc123";

        public string LoadImageButtonText => GetLoadImageButtonText();

        public bool IsDownloadedImageShown => App.Query(_downloadedImage)?.Length > 0;

        public bool IsErrorPromptVisible => App.Query("Ok")?.Length > 0;

        public void EnterUrl(string url)
        {
            EnterText(_imageUrlEntry, url);
            App.DismissKeyboard();
            App.Screenshot($"Entered Test: {url}");
        }

        public void TapKeyboardEnterButton()
        {
            App.ScrollUpTo(_imageUrlEntry);
            App.Tap(_imageUrlEntry);
            App.PressEnter();
            App.DismissKeyboard();
            App.Screenshot("Tapped Keyboard Return Button");
        }

        public void TapLoadImageButton()
        {
            App.ScrollUpTo(_imageUrlEntry);
            App.Tap(_loadImageButton);
            App.Screenshot("Tapped Load Image Button");
        }

        public async Task WaitForNoIsDownloadingActivityIndicator(int timeoutInSeconds = 60)
        {
            await Task.Delay(1000);
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
            App.ScrollDownTo(_clearImageButton);
            App.Tap(_clearImageButton);
            App.Screenshot("Clear Image Button Tapped");
        }

        string GetLoadImageButtonText()
        {
            if (App is AndroidApp)
                return App.Query(_loadImageButton).First().Text;

            return App.Query(_loadImageButton).First().Label;
        }

        void EnterUrlAndTapKeyboardReturnButton(string url)
        {
            EnterText(_imageUrlEntry, url);
            App.Screenshot($"Entered Test: {url}");
            App.PressEnter();
            App.Screenshot($"Pressed Keyboard Return Button");
            App.DismissKeyboard();
        }

        void EnterText(Query query, string url)
        {
            App.ScrollUpTo(query);
            App.ClearText(query);
            App.EnterText(query, url);
        }
    }
}
