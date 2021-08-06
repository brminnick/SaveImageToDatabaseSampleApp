[![Xamarin](https://github.com/brminnick/SaveImageToDatabaseSampleApp/actions/workflows/mobile.yml/badge.svg)](https://github.com/brminnick/SaveImageToDatabaseSampleApp/actions/workflows/mobile.yml)

# SaveImageToDatabaseSampleApp

This sample app demonstrates how to download an image from a url and save it to a local SQLite database.

[`DownloadImageAsync()`](./SaveImageToDatabaseSampleApp/ViewModel/LoadImageViewModel.cs#L162) shows how to download the image as a `byte[]` from a URL using `HttpClient`.

The [database model](https://github.com/brminnick/SaveImageToDatabaseSampleApp/blob/master/SaveImageToDatabaseSampleApp/Model/DownloadedImageModel.cs) stores the image as a `byte[]`. When the image is retrieved from the database, it is converted from a `byte[]` to a Xamarin.Forms.ImageSource.

![UI Demo](https://github.com/brminnick/Videos/blob/master/SaveImageToDatabaseSampleApp/Demo.gif)
