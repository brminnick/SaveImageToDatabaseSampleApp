using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp
{
	public class LoadImagePage : ContentPage
	{
		readonly LoadImageViewModel _viewModel;

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
				AutomationId = AutomationIdConstants.ImageUrlEntry
			};
			imageUrlEntry.SetBinding<LoadImageViewModel>(Entry.TextProperty, vm => vm.ImageUrlEntryText);

			var loadImageButton = new Button
			{
				Margin = new Thickness(0, 20, 0, 0),
				AutomationId = AutomationIdConstants.LoadImageButton
			};
			loadImageButton.SetBinding<LoadImageViewModel>(IsEnabledProperty, vm => vm.IsLoadImageButtonEnabled);
			loadImageButton.SetBinding<LoadImageViewModel>(Button.CommandProperty, vm => vm.LoadImageButtonTapped);
			loadImageButton.SetBinding<LoadImageViewModel>(Button.TextProperty, vm => vm.DownloadImageButtonText);

			var downloadedImage = new Image
			{
				AutomationId = AutomationIdConstants.DownloadedImage
			};
			downloadedImage.SetBinding<LoadImageViewModel>(IsVisibleProperty, vm => vm.AreImageAndClearButtonVisible);
			downloadedImage.SetBinding<LoadImageViewModel>(Image.SourceProperty, vm => vm.DownloadedImageSource);

			var clearImageButton = new Button
			{
				AutomationId = AutomationIdConstants.ClearImageButton,
				Text = "Clear Image From Screen"
			};
			clearImageButton.SetBinding<LoadImageViewModel>(IsVisibleProperty, vm => vm.AreImageAndClearButtonVisible);
			clearImageButton.SetBinding<LoadImageViewModel>(Button.CommandProperty, vm => vm.ClearImageButtonTapped);

			var isDownloadingActivityIndicator = new ActivityIndicator
			{
				AutomationId = AutomationIdConstants.IsDownloadingActivityIndicator
			};
			isDownloadingActivityIndicator.SetBinding<LoadImageViewModel>(IsVisibleProperty, vm => vm.IsImageDownloading);
			isDownloadingActivityIndicator.SetBinding<LoadImageViewModel>(ActivityIndicator.IsRunningProperty, vm => vm.IsImageDownloading);

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
