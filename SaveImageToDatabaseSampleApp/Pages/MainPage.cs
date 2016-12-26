using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp
{
	public class MainPage : ContentPage
	{
		readonly MainViewModel _viewModel;

		public MainPage()
		{
			_viewModel = new MainViewModel();
			BindingContext = _viewModel;

			var imageUrlLabel = new Label
			{
				Text = "Image Url"
			};

			var imageUrlEntry = new Entry
			{
				AutomationId = AutomationIdConstants.ImageUrlEntry
			};
			imageUrlEntry.SetBinding<MainViewModel>(Entry.TextProperty, vm => vm.ImageUrlEntryText);

			var loadImageButton = new Button
			{
				Margin = new Thickness(0, 20, 0, 0),
				AutomationId = AutomationIdConstants.LoadImageButton
			};
			loadImageButton.SetBinding<MainViewModel>(IsEnabledProperty, vm => vm.IsLoadImageButtonEnabled);
			loadImageButton.SetBinding<MainViewModel>(Button.CommandProperty, vm => vm.LoadImageButtonTapped);
			loadImageButton.SetBinding<MainViewModel>(Button.TextProperty, vm => vm.DownloadImageButtonText);

			var downloadedImage = new Image
			{
				AutomationId = AutomationIdConstants.DownloadedImage
			};
			downloadedImage.SetBinding<MainViewModel>(IsVisibleProperty, vm => vm.IsImageVisible);
			downloadedImage.SetBinding<MainViewModel>(Image.SourceProperty, vm => vm.DownloadedImageSource);

			var isDownloadingActivityIndicator = new ActivityIndicator
			{
				AutomationId = AutomationIdConstants.IsDownloadingActivityIndicator
			};
			isDownloadingActivityIndicator.SetBinding<MainViewModel>(IsVisibleProperty, vm => vm.IsImageDownloading);
			isDownloadingActivityIndicator.SetBinding<MainViewModel>(ActivityIndicator.IsRunningProperty, vm => vm.IsImageDownloading);

			Padding = new Thickness(20);

			Title = "Load Images";

			Content = new StackLayout
			{
				Children ={
					imageUrlLabel,
					imageUrlEntry,
					loadImageButton,
					downloadedImage,
					isDownloadingActivityIndicator
				}
			};
		}

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

		void HandleImageDownloadFailed(object sender, RetrievingDataFailureEventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await DisplayAlert("Error Downloading Image", e.RetrievingDataFailureMessage, "Ok");
			});
		}
		#endregion
	}
}
