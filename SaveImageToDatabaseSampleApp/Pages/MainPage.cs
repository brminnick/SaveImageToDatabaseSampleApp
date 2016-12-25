using System;
using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public class MainPage : ContentPage
	{
		readonly MainViewModel _viewModel;

		public MainPage()
		{
			_viewModel = new MainViewModel();
			BindingContext = _viewModel;

			var imageUrlEntry = new Entry();
			imageUrlEntry.SetBinding<MainViewModel>(Entry.TextProperty, vm => vm.ImageUrlEntryText);

			var loadImageButton = new Button();
			loadImageButton.SetBinding<MainViewModel>(IsEnabledProperty, vm => vm.IsLoadImageButtonEnabled);
			loadImageButton.SetBinding<MainViewModel>(Button.CommandProperty, vm => vm.LoadImageButtonTapped);
			loadImageButton.SetBinding<MainViewModel>(Button.TextProperty, vm => vm.DownloadImageButtonText);

			var downloadedImage = new Image();
			downloadedImage.SetBinding<MainViewModel>(IsVisibleProperty, vm => vm.IsImageVisible);
			downloadedImage.SetBinding<MainViewModel>(Image.SourceProperty, vm => vm.DownloadedImageSource);

			var isDownloadingActivityIndicator = new ActivityIndicator();
			isDownloadingActivityIndicator.SetBinding<MainViewModel>(IsVisibleProperty, vm => vm.IsImageDownloading);
			isDownloadingActivityIndicator.SetBinding<MainViewModel>(ActivityIndicator.IsRunningProperty, vm => vm.IsImageDownloading);

			Padding = new Thickness(20);

			Content = new StackLayout
			{
				Children ={
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
