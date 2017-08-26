#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.HockeyApp&version=0.6.0"

public static class HockeyAppSettings
{
    public static HockeyAppUploadSettings Settings { get; private set; }

    public static string ApiToken { get { return Settings.ApiToken; } }
    public static string AppId { get { return Settings.AppId; } }
    public static string Version { get { return Settings.Version; } }
    public static string ShortVersion { get { return Settings.ShortVersion; } }

    public static void Initialize(ICakeContext context)
    {
        Settings = new HockeyAppUploadSettings
        {
            ApiToken = context.EnvironmentVariable("HOCKEYAPP_API_TOKEN"),
            Version = "1",
            ShortVersion = "1.0.0.1"
        };

    }

    public static void WithSettings(ICakeContext context, Action<HockeyAppUploadSettings> actions)
    {
        if(actions == null)
        {
            throw new ArgumentNullException();
        }

        var settings = new HockeyAppUploadSettings();

        actions(settings);

        Settings = settings;
    }
}

HockeyAppSettings.Initialize(Context);