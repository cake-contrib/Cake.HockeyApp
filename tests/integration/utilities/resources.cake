public static class Resources
{
    public static FilePath ApkPath { get; private set; }
    public static FilePath IpaPath { get; private set; }
    public static FilePath ZipPath { get; private set; }

    public static void Initialize(ICakeContext context)
    {
        ApkPath = new FilePath("./resources/Cake.HockeyApp.apk").MakeAbsolute(context.Environment);
        IpaPath = new FilePath("./resources/Cake.HockeyApp.ipa").MakeAbsolute(context.Environment);
        ZipPath = new FilePath("./resources/Cake.HockeyApp.zip").MakeAbsolute(context.Environment);
    }
}

Resources.Initialize(Context);