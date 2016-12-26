using System;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp
{
	public class LoadImageViewModel : BaseViewModel
	{
		#region Constant Fields
		const int _downloadImageTimeoutInSeconds = 15;

		readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(_downloadImageTimeoutInSeconds) };
		#endregion

		#region Fields
		bool _isImageDownloading, _isImageVisible, _isLoadImageButtonEnabled;
		string _imageUrlEntryText = @"https://www.xamarin.com/content/images/pages/branding/assets/xamarin-logo.png";
		string _downloadImageButtonText;
		ImageSource _downloadedImageSource;
		ICommand _loadImageButtonTapped, _clearImageButtonTapped;
		List<DownloadedImageModel> _imageDatabaseModelList;
		#endregion

		#region Constructors
		public LoadImageViewModel()
		{
			Task.Run(async () =>
			{
				await RefreshDataAsync();
				await UpdateDownloadButtonText();
			});
		}
		#endregion

		#region Events
		public event EventHandler<RetrievingDataFailureEventArgs> ImageDownloadFailed;
		#endregion

		#region Properties
		public bool IsImageDownloading
		{
			get { return _isImageDownloading; }
			set { SetProperty(ref _isImageDownloading, value); }
		}

		public bool AreImageAndClearButtonVisible
		{
			get { return _isImageVisible; }
			set { SetProperty(ref _isImageVisible, value); }
		}

		public string ImageUrlEntryText
		{
			get { return _imageUrlEntryText; }
			set { SetProperty(ref _imageUrlEntryText, value, async () => await UpdateDownloadButtonText()); }
		}

		public ImageSource DownloadedImageSource
		{
			get { return _downloadedImageSource; }
			set { SetProperty(ref _downloadedImageSource, value); }
		}

		public string DownloadImageButtonText
		{
			get { return _downloadImageButtonText; }
			set { SetProperty(ref _downloadImageButtonText, value); }
		}

		public bool IsLoadImageButtonEnabled
		{
			get { return _isLoadImageButtonEnabled; }
			set { SetProperty(ref _isLoadImageButtonEnabled, value); }
		}

		public List<DownloadedImageModel> DownloadedImageModelList
		{
			get { return _imageDatabaseModelList; }
			set { SetProperty(ref _imageDatabaseModelList, value); }
		}

		public ICommand LoadImageButtonTapped =>
		_loadImageButtonTapped ??
		(_loadImageButtonTapped = new Command(async () => await ExecuteLoadImageButtonTappedAsync()));

		public ICommand ClearImageButtonTapped =>
		_clearImageButtonTapped ??
		(_clearImageButtonTapped = new Command(ExecuteClearImageButtonTapped));

		#endregion

		#region Events

		#endregion

		#region Methods
		async Task ExecuteLoadImageButtonTappedAsync()
		{
			if (DownloadImageButtonText.Equals(LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText))
				await LoadImageFromDatabaseAsync(ImageUrlEntryText);
			else
				await DownloadImageAsync(ImageUrlEntryText);
		}

		void ExecuteClearImageButtonTapped(object obj)
		{
			AnalyticsHelpers.TrackEvent(AnalyticsConstants.ClearButtonTapped);

			AreImageAndClearButtonVisible = false;
		}

		async Task RefreshDataAsync()
		{
			DownloadedImageModelList = await App.Database.GetAllDownloadedImagesAsync();
		}

		async Task UpdateDownloadButtonText()
		{
			await RefreshDataAsync();

			if (IsUrlWithNonNullImageInDatabase(ImageUrlEntryText))
				DownloadImageButtonText = LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText;
			else
				DownloadImageButtonText = LoadImageButtonTextConstants.DownloadImageFromUrlButtonText;

			IsLoadImageButtonEnabled = true;
		}

		bool IsUrlWithNonNullImageInDatabase(string url)
		{
			foreach (DownloadedImageModel downloadedImageModel in DownloadedImageModelList)
			{
				var doesUrlMatchExistingUrl = downloadedImageModel.ImageUrl.ToLower().Equals(url.ToLower());
				var isBase64StringNull = string.IsNullOrEmpty(downloadedImageModel.DownloadedImageAsBase64String);

				if (doesUrlMatchExistingUrl && !isBase64StringNull)
					return true;
			}

			return false;
		}

		async Task LoadImageFromDatabaseAsync(string imageUrl)
		{
			AnalyticsHelpers.TrackEvent(AnalyticsConstants.LoadImageFromDatabase, new Dictionary<string, string>
			{
				{ AnalyticsConstants.ImageUrl, imageUrl }
			});

			var downloadedImageModel = await App.Database.GetDownloadedImageAsync(imageUrl);

			DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStreamFromBase64String;

			AreImageAndClearButtonVisible = true;
		}

		async Task DownloadImageAsync(string imageUrl)
		{
			byte[] downloadedImage;

			SetIsImageDownloading(true);

			try
			{
				using (var httpResponse = await _httpClient.GetAsync(imageUrl))
				{
					if (httpResponse.StatusCode == HttpStatusCode.OK)
					{
						downloadedImage = await httpResponse.Content.ReadAsByteArrayAsync();
						var downloadedImageBase64String = Convert.ToBase64String(downloadedImage);

						var downloadedImageModel = new DownloadedImageModel
						{
							ImageUrl = imageUrl,
							DownloadedImageAsBase64String = downloadedImageBase64String
						};

						await App.Database.SaveDownloadedImage(downloadedImageModel);

						DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStreamFromBase64String;
						AreImageAndClearButtonVisible = true;

						AnalyticsHelpers.TrackEvent(AnalyticsConstants.DownloadImage, new Dictionary<string, string>
						{
							{ AnalyticsConstants.ImageDownloadSuccessful, imageUrl }
						});
					}
					else
					{
						AnalyticsHelpers.TrackEvent(AnalyticsConstants.DownloadImage, new Dictionary<string, string>
						{
							{ AnalyticsConstants.ImageDownloadFailed, imageUrl }
						});
						OnImageDownloadFailed("Invalid Url");
					}
				}
			}
			catch (Exception e)
			{
				AnalyticsHelpers.TrackEvent(AnalyticsConstants.DownloadImage, new Dictionary<string, string>
				{
					{ AnalyticsConstants.ImageDownloadFailed, imageUrl }
				});
				OnImageDownloadFailed(e.Message);
			}
			finally
			{
				SetIsImageDownloading(false);
				await UpdateDownloadButtonText();
			}
		}

		void SetIsImageDownloading(bool isImageDownloading)
		{
			IsImageDownloading = isImageDownloading;
			IsLoadImageButtonEnabled = !isImageDownloading;
		}

		void OnImageDownloadFailed(string failureMessage)
		{
			ImageDownloadFailed?.Invoke(this, new RetrievingDataFailureEventArgs(failureMessage));
		}
		#endregion
	}
}
