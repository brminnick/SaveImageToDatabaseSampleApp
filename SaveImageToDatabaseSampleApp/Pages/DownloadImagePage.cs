using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp
{
	public class DownloadImagePage : ContentPage
	{
		readonly DownloadImageViewModel _viewModel;

		public DownloadImagePage()
		{
			_viewModel = new DownloadImageViewModel();
			BindingContext = _viewModel;

			var imageUrlLabel = new Label
			{
				Text = "Image Url"
			};

			var imageUrlEntry = new Entry
			{
				AutomationId = AutomationIdConstants.ImageUrlEntry
			};
			imageUrlEntry.SetBinding<DownloadImageViewModel>(Entry.TextProperty, vm => vm.ImageUrlEntryText);

			var loadImageButton = new Button
			{
				Margin = new Thickness(0, 20, 0, 0),
				AutomationId = AutomationIdConstants.LoadImageButton
			};
			loadImageButton.SetBinding<DownloadImageViewModel>(IsEnabledProperty, vm => vm.IsLoadImageButtonEnabled);
			loadImageButton.SetBinding<DownloadImageViewModel>(Button.CommandProperty, vm => vm.LoadImageButtonTapped);
			loadImageButton.SetBinding<DownloadImageViewModel>(Button.TextProperty, vm => vm.DownloadImageButtonText);

			var downloadedImage = new Image
			{
				AutomationId = AutomationIdConstants.DownloadedImage
			};
			downloadedImage.SetBinding<DownloadImageViewModel>(IsVisibleProperty, vm => vm.AreImageAndClearButtonVisible);
			downloadedImage.SetBinding<DownloadImageViewModel>(Image.SourceProperty, vm => vm.DownloadedImageSource);

			var clearImageButton = new Button
			{
				AutomationId = AutomationIdConstants.ClearImageButton,
				Text = "Clear Image From Screen"
			};
			clearImageButton.SetBinding<DownloadImageViewModel>(IsVisibleProperty, vm => vm.AreImageAndClearButtonVisible);
			clearImageButton.SetBinding<DownloadImageViewModel>(Button.CommandProperty, vm => vm.ClearImageButtonTapped);

			var isDownloadingActivityIndicator = new ActivityIndicator
			{
				AutomationId = AutomationIdConstants.IsDownloadingActivityIndicator
			};
			isDownloadingActivityIndicator.SetBinding<DownloadImageViewModel>(IsVisibleProperty, vm => vm.IsImageDownloading);
			isDownloadingActivityIndicator.SetBinding<DownloadImageViewModel>(ActivityIndicator.IsRunningProperty, vm => vm.IsImageDownloading);

			Padding = new Thickness(20);

			Title = "Load Images";

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
