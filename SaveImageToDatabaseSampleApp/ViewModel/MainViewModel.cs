using System;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
	public class MainViewModel : BaseViewModel
	{
		#region Constant Fields
		const string _loadImageFromDatabaseButtonText = "Load Image From Database";
		const string _downloadImageFromUrlButtonText = "Download Image From Url";
		const int _downloadImageTimeoutInSeconds = 15;

		readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(_downloadImageTimeoutInSeconds) };
		#endregion

		#region Fields
		bool _isImageDownloading, _isImageVisible, _isLoadImageButtonEnabled;
		string _imageUrlEntryText = @"https://www.xamarin.com/content/images/pages/branding/assets/xamarin-logo.png";
		string _downloadImageButtonText = "Download Image";
		ImageSource _downloadedImageSource;
		ICommand _loadImageButtonTapped;
		List<DownloadedImageModel> _imageDatabaseModelList;
		#endregion

		#region Constructors
		public MainViewModel()
		{
			Task.Run(async () =>
			{
				await RefreshDataAsync();
				await UpdateDownloadButtonText();
			});
		}
		#endregion
		public event EventHandler<RetrievingDataFailureEventArgs> ImageDownloadFailed;
		#region Events

		#endregion

		#region Properties
		public bool IsImageDownloading
		{
			get { return _isImageDownloading; }
			set { SetProperty(ref _isImageDownloading, value); }
		}

		public bool IsImageVisible
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
		#endregion

		#region Events

		#endregion

		#region Methods
		async Task ExecuteLoadImageButtonTappedAsync()
		{
			if (DownloadImageButtonText.Equals(_loadImageFromDatabaseButtonText))
				await LoadImageFromDatabase(ImageUrlEntryText);
			else
				await DownloadImageAsync(ImageUrlEntryText);
		}

		async Task RefreshDataAsync()
		{
			DownloadedImageModelList = await App.Database.GetAllDownloadedImagesAsync();
		}

		async Task UpdateDownloadButtonText()
		{
			await RefreshDataAsync();

			if (IsUrlInDatabase(ImageUrlEntryText))
				DownloadImageButtonText = _loadImageFromDatabaseButtonText;
			else
				DownloadImageButtonText = _downloadImageFromUrlButtonText;

			IsLoadImageButtonEnabled = true;
		}

		bool IsUrlInDatabase(string url)
		{
			foreach (DownloadedImageModel downloadedImageModel in DownloadedImageModelList)
			{
				if (downloadedImageModel.ImageUrl.ToLower().Equals(url))
					return true;
			}

			return false;
		}

		async Task LoadImageFromDatabase(string imageUrl)
		{
			var downloadedImageModel = await App.Database.GetDownloadedImageAsync(imageUrl);

			DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStreamFromBase64String;

			IsImageVisible = true;
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
						IsImageVisible = true;
					}
					else
					{
						OnImageDownloadFailed("Invalid Url");
					}
				}
			}
			catch(Exception e)
			{
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
			ImageDownloadFailed?.Invoke(null, new RetrievingDataFailureEventArgs(failureMessage));
		}
		#endregion
	}
}
