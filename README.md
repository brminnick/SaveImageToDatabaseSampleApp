# SaveImageToDatabaseSampleApp

|CI Tool                    |Build Status|
|---------------------------|---|
| App Center, iOS           | [![Build status](https://build.appcenter.ms/v0.1/apps/be84ba66-c311-4c33-b3cb-2eb798f2980f/branches/master/badge)](https://appcenter.ms) |
| App Center, Android       | [![Build status](https://build.appcenter.ms/v0.1/apps/556cf4d9-08d2-4e69-8ba9-d2ed68aa6d20/branches/master/badge)](https://appcenter.ms)  |

This sample app demonstrates how to download an image from a url and save it to a local SQLite database.

[`DownloadImageAsync()`](./SaveImageToDatabaseSampleApp/ViewModel/LoadImageViewModel.cs#L162) shows how to download the image as a `byte[]` from a URL using `HttpClient`.

The [database model](https://github.com/brminnick/SaveImageToDatabaseSampleApp/blob/master/SaveImageToDatabaseSampleApp/Model/DownloadedImageModel.cs) stores the image as a `byte[]`. When the image is retrieved from the database, it is converted from a `byte[]` to a Xamarin.Forms.ImageSource.

![UI Demo](https://github.com/brminnick/Videos/blob/master/SaveImageToDatabaseSampleApp/Demo.gif)
