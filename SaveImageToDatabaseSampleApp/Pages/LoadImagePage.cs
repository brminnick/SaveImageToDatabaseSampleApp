using Xamarin.Forms;

using EntryCustomReturn.Forms.Plugin.Abstractions;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp
{
    public class LoadImagePage : ContentPage
    {
        #region Constant Fields
        readonly LoadImageViewModel _viewModel;
        #endregion

        #region Constructors
        public LoadImagePage()
        {
            _viewModel = new LoadImageViewModel();
            BindingContext = _viewModel;

            var imageUrlLabel = new Label
            {
                Text = "Image Url"
            };

            var imageUrlEntry = new Entry
            {
                AutomationId = AutomationIdConstants.ImageUrlEntry,
            };
            imageUrlEntry.SetBinding(Entry.TextProperty, nameof(LoadImageViewModel.ImageUrlEntryText));
            imageUrlEntry.SetBinding(CustomReturnEffect.ReturnCommandProperty, nameof(LoadImageViewModel.LoadImageButtonCommand));
            CustomReturnEffect.SetReturnType(imageUrlEntry, ReturnType.Go);

            var loadImageButton = new Button
            {
                Margin = new Thickness(0, 20, 0, 0),
                AutomationId = AutomationIdConstants.LoadImageButton
            };
            loadImageButton.SetBinding(IsEnabledProperty, nameof(LoadImageViewModel.IsLoadImageButtonEnabled));
            loadImageButton.SetBinding(Button.CommandProperty, nameof(LoadImageViewModel.LoadImageButtonCommand));
            loadImageButton.SetBinding(Button.TextProperty, nameof(LoadImageViewModel.DownloadImageButtonText));

            var downloadedImage = new Image
            {
                AutomationId = AutomationIdConstants.DownloadedImage
            };
            downloadedImage.SetBinding(IsVisibleProperty, nameof(LoadImageViewModel.AreImageAndClearButtonVisible));
            downloadedImage.SetBinding(Image.SourceProperty, nameof(LoadImageViewModel.DownloadedImageSource));

            var clearImageButton = new Button
            {
                AutomationId = AutomationIdConstants.ClearImageButton,
                Text = "Clear Image From Screen"
            };
            clearImageButton.SetBinding(IsVisibleProperty, nameof(LoadImageViewModel.AreImageAndClearButtonVisible));
            clearImageButton.SetBinding(Button.CommandProperty, nameof(LoadImageViewModel.ClearImageButtonCommand));

            var isDownloadingActivityIndicator = new ActivityIndicator
            {
                AutomationId = AutomationIdConstants.IsDownloadingActivityIndicator
            };
            isDownloadingActivityIndicator.SetBinding(IsVisibleProperty, nameof(LoadImageViewModel.IsImageDownloading));
            isDownloadingActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(LoadImageViewModel.IsImageDownloading));

            Padding = new Thickness(20);

            Title = "Load Images";

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children ={
                        imageUrlLabel,
                        imageUrlEntry,
                        loadImageButton,
                        downloadedImage,
                        clearImageButton,
                        isDownloadingActivityIndicator
                    }
                }
            };
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.ImageDownloadFailed += HandleImageDownloadFailed;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _viewModel.ImageDownloadFailed -= HandleImageDownloadFailed;
        }

        void HandleImageDownloadFailed(object sender, string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
                   await DisplayAlert("Error Downloading Image", message, "Ok"));
        }
        #endregion
    }
}
