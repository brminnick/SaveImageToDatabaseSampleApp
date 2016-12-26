[![Build Status](https://www.bitrise.io/app/8e6b6ccd01b546e4.svg?token=Ggk4zsslVPS4-UBcR74NWA&branch=master)](https://www.bitrise.io/app/8e6b6ccd01b546e4)

# Save Image To Database 
This sample app demonstrates how to download an image from a url and save it to a local SQLite database.

The [DownloadImageAsync method](https://github.com/brminnick/SaveImageToDatabaseSampleApp/blob/master/SaveImageToDatabaseSampleApp/ViewModel/MainViewModel.cs#L147) shows how to download the image as a byte array from a URL using HttpClient.

The [database model](https://github.com/brminnick/SaveImageToDatabaseSampleApp/blob/master/SaveImageToDatabaseSampleApp/Model/DownloadedImageModel.cs) stores the image as a Base64 string. When the image is retrieved from the database, it is converted from a Base64 string to a Xamarin.Forms.ImageSource.

This app contains UITests. The UITests follow the recommended practice of Page Object testing. In the views, we've added an `AutomationId` to each control to show how UITest can interact with controls most efficiently, using their AutomationId. The test results from [Xamarin Test Cloud](https://www.xamarin.com/test-cloud) are [viewable here](https://testcloud.xamarin.com/test/imagedatabasesample_64fc94d8-78f5-4172-abe3-626fc5ac9e60/)

![UI Demo](./Demos/UI%20Demo.gif)
