namespace Cake.HockeyApp
{
    using System;
    using Core;
    using Core.Annotations;
    using Core.Diagnostics;
    using Core.IO;
    using Internal;

    /// <summary>
    /// <para>Contains functionality related to <see href="http://hockeyapp.net">HockeyApp</see>.</para>
    /// <para>
    /// It allows you to upload an app package to HockeyApp with just one line of code. In order to use the exposed
    /// commands you have to add the following line at top of your build.cake file.
    /// </para>
    /// <code>
    /// #addin Cake.HockeyApp
    /// </code>
    /// </summary>
    /// <example>
    /// <para>Upload an apk to HockeyApp:</para>
    /// <code>
    /// Task("Upload-To-HockeyApp")
    ///     .IsDependentOn("Build-APK")
    ///     .Does(() => UploadToHockeyApp("./output/myApp.apk"));
    /// </code>
    ///
    /// <para>Upload an apk to HockeyApp with result.</para>
    /// <code>
    /// Task("Upload-To-HockeyApp")
    ///     .IsDependentOn("Build-APK")
    ///     .Does(() =>
    /// {
    ///     var result = UploadToHockeyApp("./output/myApp.apk"));
    ///     // Use result.PublicUrl to inform others where they can download the newly uploaded package.
    /// }
    /// </code>
    ///
    /// <para>Upload a Windows package.</para>
    /// <para>
    /// Unfortunately, HockeyApp currently does only support metadata discovering for Android, iOS and macOS packages.
    /// Therefore you have to specify a version AND app id your self. This means that you have to create the app once
    /// before uploading. <see href="http://rink.hockeyapp.net/manage/apps/new">Create new App</see>. Creating a new
    /// version is automatically done by this addin.
    /// </para>
    /// <code>
    /// Task("Upload-To-HockeyApp")
    ///     .IsDependentOn("Build-AppX")
    ///     .Does(() =>
    /// {
    ///     UploadToHockeyApp( "./output/myWindowsApp.appx", new HockeyAppUploadSettings
    ///     {
    ///         AppId = appIdFromHockeyApp,
    ///         Version = "1.0.160901.1",
    ///         ShortVersion = "1.0-beta2",
    ///         Notes = "Uploaded via continuous integration."
    ///     });
    /// }
    /// </code>
    ///
    /// <para>
    /// For all request you make you either have to set your API token from HockeyApp as environment variable: HOCKEYAPP_API_TOKEN
    /// or pass it into the call via <see cref="HockeyAppUploadSettings.AppId" />
    /// </para>
    /// </example>
    [CakeAliasCategory("HockeyApp")]
    public static class HockeyAppAliases
    {
        public const string TokenVariable = "HOCKEYAPP_API_TOKEN";

        [Obsolete]
        public static void UploadToHockeyApp(this ICakeContext context, FilePath file, HockeyAppUploadSettings settings, FilePath symbolFile)
            => UploadToHockeyApp(context, file, symbolFile, settings);

        /// <summary>
        /// Uploads the specified package to HockeyApp. If you don't upload an apk or ipa, it is required to specify the AppId and the
        /// Version property of <see cref="Cake.HockeyApp.HockeyAppUploadSettings" />.
        /// </summary>
        /// <param name="context">The Cake context</param>
        /// <param name="file">The app package.</param>
        /// <param name="settings">The upload settings</param>
        /// <example>
        /// <code>
        /// UploadToHockeyApp( pathToYourPackageFile, new HockeyAppUploadSettings
        /// {
        ///     AppId = appIdFromHockeyApp,
        ///     Version = "1.0.160901.1",
        ///     ShortVersion = "1.0-beta2",
        ///     Notes = "Uploaded via continuous integration."
        /// });
        /// </code>
        /// Do not checkin the HockeyApp API Token into your source control.
        /// Either use HockeyAppUploadSettings.ApiToken or the HOCKEYAPP_API_TOKEN environment variable.
        /// </example>
        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static HockeyAppUploadResult UploadToHockeyApp(this ICakeContext context, FilePath file, HockeyAppUploadSettings settings)
            => UploadToHockeyApp(context, file, null, settings);

        /// <summary>
        /// Uploads the specified package and symbols file to HockeyApp. The version is automatically detected from the package metadata.
        /// This only works with *.ipa for iOS, *.app.zip for OS X, or *.apk files for Android.
        /// </summary>
        /// <param name="context">The Cake context</param>
        /// <param name="file">The app package.</param>
        /// <example>
        /// <code>
        /// UploadToHockeyApp( pathToYourPackageFile );
        /// </code>
        /// You have to set the HOCKEYAPP_API_TOKEN environment variable before this call.
        /// </example>
        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static HockeyAppUploadResult UploadToHockeyApp(this ICakeContext context, FilePath file)
            => UploadToHockeyApp(context, file, null, new HockeyAppUploadSettings());

        /// <summary>
        /// Uploads the specified package and symbols file to HockeyApp. The version is automatically detected from the package metadata.
        /// This only works with *.ipa for iOS, *.app.zip for OS X, or *.apk files for Android.
        /// </summary>
        /// <param name="context">The Cake context</param>
        /// <param name="file">The app package.</param>
        /// <param name="symbolsFile">The symbols for the app package.</param>
        /// <example>
        /// <code>
        /// UploadToHockeyApp( pathToYourPackageFile, pathToSymbolsFile );
        /// </code>
        /// You have to set the HOCKEYAPP_API_TOKEN environment variable before this call.
        /// </example>
        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static HockeyAppUploadResult UploadToHockeyApp(this ICakeContext context, FilePath file, FilePath symbolsFile)
            => UploadToHockeyApp(context, file, symbolsFile, new HockeyAppUploadSettings());

        /// <summary>
        /// Uploads the specified package to HockeyApp. Currently it is required to specify the AppId and the
        /// Version property of <see cref="Cake.HockeyApp.HockeyAppUploadSettings" />
        /// </summary>
        /// <param name="context">The Cake context</param>
        /// <param name="file">The app package.</param>
        /// <param name="symbolsFile">The symbols for the app package.</param>
        /// <param name="settings">The upload settings</param>
        /// <example>
        /// <code>
        /// UploadToHockeyApp( pathToYourPackageFile, pathToSymbolsFile, new HockeyAppUploadSettings
        /// {
        ///     AppId = appIdFromHockeyApp,
        ///     Version = "1.0.160901.1",
        ///     ShortVersion = "1.0-beta2",
        ///     Notes = "Uploaded via continuous integration."
        /// });
        /// </code>
        /// Do not checkin the HockeyApp API Token into your source control.
        /// Either use HockeyAppUploadSettings.ApiToken or the HOCKEYAPP_API_TOKEN environment variable.
        /// </example>
        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static HockeyAppUploadResult UploadToHockeyApp(this ICakeContext context, FilePath file, FilePath symbolsFile, HockeyAppUploadSettings settings)
        {
            settings = settings ?? new HockeyAppUploadSettings();

            var uploader = new HockeyAppClient(context.Log, settings);

            settings.ApiToken = settings.ApiToken ??
                                context.Environment.GetEnvironmentVariable(TokenVariable);

            try
            {
                var upload = uploader.UploadFile(file, symbolsFile, settings);
                upload.Wait();

                return upload.Result;
            }
            catch (Exception e)
            {
                do context.Log.Error(e.Message); while ((e = e.InnerException) != null);

                throw new Exception("Upload to Hockey App failed.");
            }
        }
    }
}