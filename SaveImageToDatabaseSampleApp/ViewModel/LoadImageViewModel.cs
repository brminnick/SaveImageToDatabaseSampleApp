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
        string _imageUrlEntryText = @"https://blobstoragesampleapp.blob.core.windows.net/photos/Punday";
        string _downloadImageButtonText;
        ImageSource _downloadedImageSource;
        Lazy<HttpClient> _clientHolder = new Lazy<HttpClient>(() => CreateHttpClient(TimeSpan.FromSeconds(10)));
        ICommand _loadImageButtonTapped, _clearImageButtonTapped;
        List<DownloadedImageModel> _imageDatabaseModelList;
        #endregion

        #region Constructors
        public LoadImageViewModel()
        {
            Task.Run(async () =>
            {
                await RefreshDataAsync().ConfigureAwait(false);
                await UpdateDownloadButtonText().ConfigureAwait(false);
            });
        }
        #endregion

        #region Events
        public event EventHandler<string> ImageDownloadFailed;
        #endregion

        #region Properties
        public ICommand LoadImageButtonCommand => _loadImageButtonTapped ??
            (_loadImageButtonTapped = new Command(async () => await ExecuteLoadImageButtonCommand()));

        public ICommand ClearImageButtonTapped => _clearImageButtonTapped ??
            (_clearImageButtonTapped = new Command(ExecuteClearImageButtonTapped));

        public bool IsImageDownloading
        {
            get => _isImageDownloading;
            set => SetProperty(ref _isImageDownloading, value);
        }

        public bool AreImageAndClearButtonVisible
        {
            get => _isImageVisible;
            set => SetProperty(ref _isImageVisible, value);
        }

        public string ImageUrlEntryText
        {
            get => _imageUrlEntryText;
            set => SetProperty(ref _imageUrlEntryText, value, async () => await UpdateDownloadButtonText());
        }

        public ImageSource DownloadedImageSource
        {
            get => _downloadedImageSource;
            set => SetProperty(ref _downloadedImageSource, value);
        }

        public string DownloadImageButtonText
        {
            get => _downloadImageButtonText;
            set => SetProperty(ref _downloadImageButtonText, value);
        }

        public bool IsLoadImageButtonEnabled
        {
            get => _isLoadImageButtonEnabled;
            set => SetProperty(ref _isLoadImageButtonEnabled, value);
        }

        public List<DownloadedImageModel> DownloadedImageModelList
        {
            get => _imageDatabaseModelList;
            set => SetProperty(ref _imageDatabaseModelList, value);
        }

        HttpClient Client => _clientHolder.Value;
        #endregion

        #region Methods
        static HttpClient CreateHttpClient(TimeSpan timeout)
        {
            HttpClient client;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    client = new HttpClient();
                    break;
                default:
                    client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
                    break;

            }
            client.Timeout = timeout;
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            return client;
        }

        async Task ExecuteLoadImageButtonCommand()
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

        async Task UpdateDownloadButtonText()
        {
            await RefreshDataAsync().ConfigureAwait(false);

            if (IsUrInDatabase(ImageUrlEntryText))
                DownloadImageButtonText = LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText;
            else
                DownloadImageButtonText = LoadImageButtonTextConstants.DownloadImageFromUrlButtonText;

            IsLoadImageButtonEnabled = true;
        }

        async Task LoadImageFromDatabaseAsync(string imageUrl)
        {
            AnalyticsHelpers.TrackEvent(AnalyticsConstants.LoadImageFromDatabase, new Dictionary<string, string>
            {
                { AnalyticsConstants.ImageUrl, imageUrl }
            });

            var downloadedImageModel = await DownloadedImageModelDatabase.GetDownloadedImageAsync(imageUrl).ConfigureAwait(false);

            DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStream;

            AreImageAndClearButtonVisible = true;
        }

        async Task DownloadImageAsync(string imageUrl)
        {
            if(!imageUrl.Contains("https"))
            {
                OnImageDownloadFailed("URL must use https");
                return;
            }

            byte[] downloadedImage;

            SetIsImageDownloading(true);

            try
            {
                using (var httpResponse = await Client.GetAsync(imageUrl).ConfigureAwait(false))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        downloadedImage = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                        var downloadedImageModel = new DownloadedImageModel
                        {
                            ImageUrl = imageUrl,
                            DownloadedImageBlob = downloadedImage
                        };

                        await DownloadedImageModelDatabase.SaveDownloadedImage(downloadedImageModel).ConfigureAwait(false);

                        DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStream;
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

        bool IsUrInDatabase(string url) =>
            DownloadedImageModelList.Any(x => x.ImageUrl.ToUpper().Equals(url.ToUpper()));

        async Task RefreshDataAsync() =>
            DownloadedImageModelList = await DownloadedImageModelDatabase.GetAllDownloadedImagesAsync();

        void OnImageDownloadFailed(string failureMessage) =>
            ImageDownloadFailed?.Invoke(this, failureMessage);
        #endregion
    }
}
