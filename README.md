# SaveImageToDatabaseSampleApp
This sample app demonstrates how to download an image from a url and save it to a local SQLite database.

[The DownloadImageAsync method](https://github.com/brminnick/SaveImageToDatabaseSampleApp/blob/master/SaveImageToDatabaseSampleApp/ViewModel/MainViewModel.cs#L144) shows how to download the image as a byte array from a URL using HttpClient.

The [database model](https://github.com/brminnick/SaveImageToDatabaseSampleApp/blob/master/SaveImageToDatabaseSampleApp/Model/DownloadedImageModel.cs) stores the image as a Base64 string. When the image is retrieved from the database, it is converted from a Base64 string to a Xamarin.Forms.ImageSource.
