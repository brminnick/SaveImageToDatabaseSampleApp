﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using SaveImageToDatabaseSampleApp.Shared;
using Xamarin.Forms;

namespace SaveImageToDatabaseSampleApp
{
    public class LoadImageViewModel : BaseViewModel
    {
        readonly WeakEventManager<string> _imageDownloadFailedEventManager = new();
        readonly Lazy<HttpClient> _clientHolder = new(CreateHttpClient);

        bool _isImageDownloading, _isImageVisible, _isLoadImageButtonEnabled;
        string _imageUrlEntryText = UrlConstants.ImageUrl;
        string _downloadImageButtonText = string.Empty;
        ImageSource? _downloadedImageSource;

        public LoadImageViewModel()
        {
            LoadImageButtonCommand = new AsyncCommand(() => ExecuteLoadImageButtonCommand(DownloadImageButtonText, ImageUrlEntryText));
            InitializeViewModelCommand = new AsyncCommand(ExecuteInitializeViewModelCommand);
            ClearImageButtonCommand = new Command(ExecuteClearImageButtonCommand);
            ExecuteInitializeViewModelCommand().SafeFireAndForget();
        }

        public event EventHandler<string> ImageDownloadFailed
        {
            add => _imageDownloadFailedEventManager.AddEventHandler(value);
            remove => _imageDownloadFailedEventManager.RemoveEventHandler(value);
        }

        public ICommand LoadImageButtonCommand { get; }
        public ICommand InitializeViewModelCommand { get; }
        public ICommand ClearImageButtonCommand { get; }

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
            set => SetProperty(ref _imageUrlEntryText, value, async () => await UpdateDownloadButtonText(ImageUrlEntryText).ConfigureAwait(false));
        }

        public ImageSource? DownloadedImageSource
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

        HttpClient Client => _clientHolder.Value;

        IReadOnlyList<DownloadedImageModel> DownloadedImageModelList { get; set; } = Array.Empty<DownloadedImageModel>();

        static HttpClient CreateHttpClient()
        {
            HttpClient client = Device.RuntimePlatform switch
            {
                Device.iOS => new HttpClient(),
                Device.Android => new HttpClient(),
                _ => new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip })
            };
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            return client;
        }

        Task ExecuteInitializeViewModelCommand() => UpdateDownloadButtonText(ImageUrlEntryText);

        Task ExecuteLoadImageButtonCommand(string downloadImageButtonText, string imageUrlEntryText) => downloadImageButtonText switch
        {
            LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText => LoadImageFromDatabaseAsync(imageUrlEntryText),
            _ => DownloadImage(imageUrlEntryText),
        };

        void ExecuteClearImageButtonCommand()
        {
            AnalyticsServices.Track(AnalyticsConstants.ClearButtonTapped);

            AreImageAndClearButtonVisible = false;
        }

        async Task UpdateDownloadButtonText(string imageUrlEntryText)
        {
            await RefreshDownloadedImageModelList().ConfigureAwait(false);

            if (IsUrInDatabase(imageUrlEntryText))
                DownloadImageButtonText = LoadImageButtonTextConstants.LoadImageFromDatabaseButtonText;
            else
                DownloadImageButtonText = LoadImageButtonTextConstants.DownloadImageFromUrlButtonText;

            IsLoadImageButtonEnabled = true;
        }

        async Task LoadImageFromDatabaseAsync(string imageUrl)
        {
            AnalyticsServices.Track(AnalyticsConstants.LoadImageFromDatabase, new Dictionary<string, string>
            {
                { AnalyticsConstants.ImageUrl, imageUrl }
            });

            var downloadedImageModel = await DownloadedImageModelDatabase.GetDownloadedImageAsync(imageUrl).ConfigureAwait(false);

            DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStream;

            AreImageAndClearButtonVisible = true;
        }

        async Task DownloadImage(string imageUrl)
        {
            if (!imageUrl.Trim().StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                OnImageDownloadFailed("URL must use https");
                return;
            }

            SetIsImageDownloading(true);

            try
            {
                using var httpResponse = await Client.GetAsync(imageUrl).ConfigureAwait(false);

                if (httpResponse.StatusCode is HttpStatusCode.OK)
                {
                    var downloadedImage = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                    var downloadedImageModel = new DownloadedImageModel
                    {
                        ImageUrl = imageUrl,
                        DownloadedImageBlob = downloadedImage
                    };

                    await DownloadedImageModelDatabase.SaveDownloadedImage(downloadedImageModel).ConfigureAwait(false);

                    DownloadedImageSource = downloadedImageModel.DownloadedImageAsImageStream;
                    AreImageAndClearButtonVisible = true;

                    AnalyticsServices.Track(AnalyticsConstants.DownloadImage, AnalyticsConstants.ImageDownloadSuccessful, imageUrl);
                }
                else
                {
                    AnalyticsServices.Track(AnalyticsConstants.DownloadImage, AnalyticsConstants.ImageDownloadFailed, imageUrl);
                    OnImageDownloadFailed("Invalid Url");
                }
            }
            catch (Exception e)
            {
                AnalyticsServices.Track(AnalyticsConstants.DownloadImage, AnalyticsConstants.ImageDownloadFailed, imageUrl);
                OnImageDownloadFailed(e.Message);
            }
            finally
            {
                SetIsImageDownloading(false);
                await UpdateDownloadButtonText(imageUrl).ConfigureAwait(false);
            }
        }

        void SetIsImageDownloading(bool isImageDownloading)
        {
            IsImageDownloading = isImageDownloading;
            IsLoadImageButtonEnabled = !isImageDownloading;
        }

        bool IsUrInDatabase(string url) => DownloadedImageModelList.Any(x => x.ImageUrl.Equals(url, StringComparison.OrdinalIgnoreCase));

        async Task RefreshDownloadedImageModelList() => DownloadedImageModelList = await DownloadedImageModelDatabase.GetAllDownloadedImagesAsync().ConfigureAwait(false);

        void OnImageDownloadFailed(string failureMessage) => _imageDownloadFailedEventManager.RaiseEvent(this, failureMessage, nameof(ImageDownloadFailed));
    }
}
