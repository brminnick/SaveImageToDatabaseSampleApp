using System;
using Xamarin.Essentials;

namespace SaveImageToDatabaseSampleApp
{
    public static class AnalyticsConstants
    {
        #region API Keys
        public static string AppCenterApiKey => GetAppCenterApiKey();
        const string AppCenteriOSApiKey = "a52ca09a-2dd1-4e77-b8d7-bec52e67cbc7";
        const string AppCenterAndroidApiKey = "b5229d82-1c2e-4b3d-9e5e-8bedc9fdff26";
        #endregion

        #region MainPage
        public const string ClearButtonTapped = "Clear Button Tapped";
        public const string DownloadImage = "Download Image";
        public const string ImageDownloadSuccessful = "Image Download Successful";
        public const string ImageDownloadFailed = "Image Download Failed";
        public const string ImageUrl = "Image Url";
        public const string LoadImageFromDatabase = "Load Image From Database";
        #endregion

        static string GetAppCenterApiKey()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
                return AppCenteriOSApiKey;

            if (DeviceInfo.Platform == DevicePlatform.Android)
                return AppCenterAndroidApiKey;

            throw new NotSupportedException();
        }
    }
}
