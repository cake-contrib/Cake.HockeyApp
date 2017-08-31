Task("Create-New-Apk-Version")
    .Does(() =>
    {
        HockeyAppSettings.WithSettings(Context, settings =>
        {
            settings.Version = "1";
            settings.ShortVersion = "1.1.0.1";
            settings.AppId = EnvironmentVariable("HOCKEY_APP_APK_ID");
        });

        Information("Api Token: {0}", string.IsNullOrEmpty(HockeyAppSettings.ApiToken));
        Information("App Id: {0}:", HockeyAppSettings.AppId);
        Information("App Version: {0}:", HockeyAppSettings.Version);
        
        Assert.NotNull(HockeyAppSettings.AppId);

        var result = UploadToHockeyApp(Resources.ApkPath, HockeyAppSettings.Settings);
    });

Task("Upload-Apk")
    .Does(() =>
    {
        HockeyAppSettings.WithSettings(Context, settings =>
        {
            settings.Version = "1.1.0.1";
            settings.AppId = null;
        });
                
        Assert.Null(HockeyAppSettings.AppId);

        Information("Api Token: {0}", string.IsNullOrEmpty(HockeyAppSettings.ApiToken));
        Information("App Version: {0}:", HockeyAppSettings.Version);

        var result = UploadToHockeyApp(Resources.ApkPath, HockeyAppSettings.Settings);
    });

Task("Upload-Apk-With-Upload-Url")
    .Does(() =>
    {
        HockeyAppSettings.WithSettings(Context, settings =>
        {
            settings.ApiBaseUrl = "https://upload.hockeyapp.net";
            settings.AppId = EnvironmentVariable("HOCKEY_APP_APK_ID");
        });
                
        Assert.NotNull(HockeyAppSettings.AppId);

        Information("Api Token: {0}", string.IsNullOrEmpty(HockeyAppSettings.ApiToken));
        Information("App Version: {0}:", HockeyAppSettings.Version);

        var result = UploadToHockeyApp(Resources.ApkPath, HockeyAppSettings.Settings);
    });