using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Xamarin.Forms;

using SaveImageToDatabaseSampleApp.Shared;

namespace SaveImageToDatabaseSampleApp
{
	public class LoadImageViewModel : BaseViewModel
	{
		#region Constant Fields
		const int _downloadImageTimeoutInSeconds = 15;
		#endregion

		#region Fields
		bool _isImageDownloading, _isImageVisible, _isLoadImageButtonEnabled;
		string _imageUrlEntryText = @"https://www.xamarin.com/content/images/pages/branding/assets/xamarin-logo.png";
		string _downloadImageButtonText;
		ImageSource _downloadedImageSource;
		HttpClient _client;
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
		HttpClient Client => _client ?? (_client = CreateHttpClient());

		public ICommand LoadImageButtonTapped => _loadImageButtonTapped ??
			(_loadImageButtonTapped = new Command(async () => await ExecuteLoadImageButtonTappedAsync()));

		public ICommand ClearImageButtonTapped => _clearImageButtonTapped ??
			(_clearImageButtonTapped = new Command(ExecuteClearImageButtonTapped));

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
			DownloadedImageModelList = await DownloadedImageModelDatabase.GetAllDownloadedImagesAsync();
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
			var downloadedImageModelWithMatchingUrl = DownloadedImageModelList.FirstOrDefault(x => x.ImageUrl.ToUpper().Equals(url.ToUpper()));
			var doesDownloadedImageModelWithMatchingUrlExist = downloadedImageModelWithMatchingUrl != null;

			var isBase64StringNull = string.IsNullOrEmpty(downloadedImageModelWithMatchingUrl?.DownloadedImageAsBase64String);

			return doesDownloadedImageModelWithMatchingUrlExist && !isBase64StringNull;
		}

		async Task LoadImageFromDatabaseAsync(string imageUrl)
		{
			AnalyticsHelpers.TrackEvent(AnalyticsConstants.LoadImageFromDatabase, new Dictionary<string, string>
			{
				{ AnalyticsConstants.ImageUrl, imageUrl }
			});

			var downloadedImageModel = await DownloadedImageModelDatabase.GetDownloadedImageAsync(imageUrl);

			DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStreamFromBase64String;

			AreImageAndClearButtonVisible = true;
		}

		async Task DownloadImageAsync(string imageUrl)
		{
			byte[] downloadedImage;

			SetIsImageDownloading(true);

			try
			{
				using (var httpResponse = await Client.GetAsync(imageUrl).ConfigureAwait(false))
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

						await DownloadedImageModelDatabase.SaveDownloadedImage(downloadedImageModel);

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

		HttpClient CreateHttpClient()
		{
			var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip })
			{
				Timeout = TimeSpan.FromSeconds(_downloadImageTimeoutInSeconds)
			};
			client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

			return client;
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
