﻿using Xamarin.Forms;

using EntryCustomReturn.Forms.Plugin.Abstractions;

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

			var imageUrlEntry = new CustomReturnEntry
			{
				AutomationId = AutomationIdConstants.ImageUrlEntry,
				ReturnType = ReturnType.Go
			};
			imageUrlEntry.SetBinding(Entry.TextProperty, nameof(_viewModel.ImageUrlEntryText));
			imageUrlEntry.SetBinding(CustomReturnEntry.ReturnCommandProperty, nameof(_viewModel.LoadImageButtonTapped));

			var loadImageButton = new Button
			{
				Margin = new Thickness(0, 20, 0, 0),
				AutomationId = AutomationIdConstants.LoadImageButton
			};
			loadImageButton.SetBinding(IsEnabledProperty, nameof(_viewModel.IsLoadImageButtonEnabled));
			loadImageButton.SetBinding(Button.CommandProperty, nameof(_viewModel.LoadImageButtonTapped));
			loadImageButton.SetBinding(Button.TextProperty, nameof(_viewModel.DownloadImageButtonText));

			var downloadedImage = new Image
			{
				AutomationId = AutomationIdConstants.DownloadedImage
			};
			downloadedImage.SetBinding(IsVisibleProperty, nameof(_viewModel.AreImageAndClearButtonVisible));
			downloadedImage.SetBinding(Image.SourceProperty, nameof(_viewModel.DownloadedImageSource));

			var clearImageButton = new Button
			{
				AutomationId = AutomationIdConstants.ClearImageButton,
				Text = "Clear Image From Screen"
			};
			clearImageButton.SetBinding(IsVisibleProperty, nameof(_viewModel.AreImageAndClearButtonVisible));
			clearImageButton.SetBinding(Button.CommandProperty, nameof(_viewModel.ClearImageButtonTapped));

			var isDownloadingActivityIndicator = new ActivityIndicator
			{
				AutomationId = AutomationIdConstants.IsDownloadingActivityIndicator
			};
			isDownloadingActivityIndicator.SetBinding(IsVisibleProperty, nameof(_viewModel.IsImageDownloading));
			isDownloadingActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(_viewModel.IsImageDownloading));

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
