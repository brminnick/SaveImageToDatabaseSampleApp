using SaveImageToDatabaseSampleApp.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;

namespace SaveImageToDatabaseSampleApp
{
    public class LoadImagePage : ContentPage
    {
        public LoadImagePage()
        {
            var viewModel = new LoadImageViewModel();
            viewModel.ImageDownloadFailed += HandleImageDownloadFailed;

            BindingContext = viewModel;

            Padding = new Thickness(20);

            Title = "Load Images";

            Content = new StackLayout
            {
                Children =
                {
                    new Label { Text = "Image Url" },

                    new Entry { AutomationId = AutomationIdConstants.ImageUrlEntry, ReturnType = ReturnType.Go, ClearButtonVisibility = ClearButtonVisibility.WhileEditing }
                        .Bind(Entry.TextProperty, nameof(LoadImageViewModel.ImageUrlEntryText))
                        .Bind(Entry.ReturnCommandProperty, nameof(LoadImageViewModel.LoadImageButtonCommand)),

                    new Button { AutomationId = AutomationIdConstants.LoadImageButton }.Margins(0, 20, 0, 0)
                        .Bind(IsEnabledProperty, nameof(LoadImageViewModel.IsLoadImageButtonEnabled))
                        .Bind(Button.CommandProperty, nameof(LoadImageViewModel.LoadImageButtonCommand))
                        .Bind(Button.TextProperty, nameof(LoadImageViewModel.DownloadImageButtonText)),

                    new Image { AutomationId = AutomationIdConstants.DownloadedImage }
                        .Bind(IsVisibleProperty, nameof(LoadImageViewModel.AreImageAndClearButtonVisible))
                        .Bind(Image.SourceProperty, nameof(LoadImageViewModel.DownloadedImageSource)),

                    new Button { AutomationId = AutomationIdConstants.ClearImageButton, Text = "Clear Image From Screen" }
                        .Bind(IsVisibleProperty, nameof(LoadImageViewModel.AreImageAndClearButtonVisible))
                        .Bind(Button.CommandProperty, nameof(LoadImageViewModel.ClearImageButtonCommand)),

                    new ActivityIndicator { AutomationId = AutomationIdConstants.IsDownloadingActivityIndicator }
                        .Bind(IsVisibleProperty, nameof(LoadImageViewModel.IsImageDownloading))
                        .Bind(ActivityIndicator.IsRunningProperty, nameof(LoadImageViewModel.IsImageDownloading))
                }
            };
        }

        void HandleImageDownloadFailed(object sender, string message) =>
            MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("Error Downloading Image", message, "Ok"));
    }
}
