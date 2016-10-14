# Cake.HockeyApp - An Addin for Cake  

![Cake.HockeyApp](https://raw.githubusercontent.com/cake-contrib/Cake.HockeyApp/develop/Cake.HockeyApp.png) 

![AppVeyor master branch](https://img.shields.io/appveyor/ci/reicheltp/cake-hockeyapp.svg)
![nuget pre release](https://img.shields.io/nuget/vpre/Cake.HockeyApp.svg)


Cake.HockeyApp allows you to upload an app package to HockeyApp with just one line of code. In order to use the exposed
commands you have to add the following line at top of your build.cake file.

```cake
#addin Cake.HockeyApp
```

And upload the app.

```cake
UploadToHockeyApp("./output/myApp.apk");
```
> Don't forget to set your api token from HockeyApp as environment variable: `HOCKEYAPP_API_TOKEN`

----

## More Examples

### Upload an apk / ipa to HockeyApp
```cake
Task("Upload-To-HockeyApp")
    .DependsOn("Build-APK")
    .Does(() => UploadToHockeyApp("./output/myApp.apk"));
```

### Upload an apk / ipa to HockeyApp with result.
```cake
Task("Upload-To-HockeyApp")
    .DependsOn("Build-APK")
    .Does(() =>
{
    var result = UploadToHockeyApp("./output/myApp.apk"));
    // Use result.PublicUrl to inform others where they can download the newly uploaded package.
}
```

### Upload a Windows package.

Unfortunately, HockeyApp currently does only support metadata discovering for Android, iOS and macOS packages.
Therefore you have to specify a version AND app id your self. This means that you have to create the app once
before uploading. [Create new App](http://rink.hockeyapp.net/manage/apps/new). Creating a new
version is automatically done by this addin.

```cake
Task("Upload-To-HockeyApp")
    .DependsOn("Build-AppX")
    .Does(() =>
{
    UploadToHockeyApp( "./output/myWindowsApp.appx", new HockeyAppUploadSettings
    {
        AppId = appIdFromHockeyApp,
        Version = "1.0.160901.1",
        ShortVersion = "1.0-beta2",
        Notes = "Uploaded via continuous integration."
    });
}
```

The available parameters for the upload settings are descripted here: http://support.hockeyapp.net/kb/api/api-versions#upload-version

> **REMEMBER** For all request you make you either have to set your API token from HockeyApp as environment variable: `HOCKEYAPP_API_TOKEN`
> or pass it into the call via <see cref="HockeyAppUploadSettings.AppId" />

----

## Build

To build this package we are using Cake.

On Windows PowerShell run:

```powershell
./build
```

On OSX/Linux run:
```bash
./build.sh
```