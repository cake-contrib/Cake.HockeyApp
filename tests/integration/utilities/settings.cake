#addin "nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.HockeyApp&prerelease"

public static class HockeyAppSettings
{
    public static HockeyAppUploadSettings Settings { get; private set; }

    public static string ApiToken { get { return Settings.ApiToken; } }
    public static string AppId { get { return Settings.AppId; } }
    public static string Version { get { return Settings.Version; } }
    public static string ShortVersion { get { return Settings.ShortVersion; } }

    public static HockeyAppUploadSettings Initialize(ICakeContext context)
    {
        Settings = new HockeyAppUploadSettings
        {
            ApiToken = context.EnvironmentVariable("HOCKEYAPP_API_TOKEN"),
            Version = "1",
            ShortVersion = "1.0.0.1",
            Status = DownloadStatus.Allowed,
            ReleaseType = ReleaseType.Beta
        };

        return Settings;
    }

    public static void WithSettings(ICakeContext context, Action<HockeyAppUploadSettings> actions)
    {
        if(actions == null)
        {
            throw new ArgumentNullException();
        }

        var settings = Initialize(context);

        actions(settings);

        Settings = settings;
    }
}

HockeyAppSettings.Initialize(Context);