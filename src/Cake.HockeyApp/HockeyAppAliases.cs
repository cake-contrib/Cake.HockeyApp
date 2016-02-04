using System;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.HockeyApp.Internal;

namespace Cake.HockeyApp
{
    /// <summary>
    /// Contains functionality related to <see href="http://hockeyapp.net">HockeyApp</see>
    /// </summary>
    [CakeAliasCategory("HockeyApp")]
    public static class HockeyAppAliases
    {
        public const string TokenVariable = "HOCKEYAPP_API_TOKEN";

        /// <summary>
        /// Uploads the specified package to HockeyApp. Currently it is required to specify the AppId and the
        /// Version property of <see cref="Cake.HockeyApp.HockeyAppUploadSettings" />
        /// </summary>
        /// <param name="context">The app package.</param>
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
        public static void UploadToHockeyApp(this ICakeContext context, FilePath file, HockeyAppUploadSettings settings, FilePath symbolFile = null)
        {
            var uploader = new HockeyAppClient(context.Log);

            settings.ApiToken = settings.ApiToken ??
                                context.Environment.GetEnvironmentVariable(TokenVariable);

            try
            {
                uploader.UploadFile(file, settings, symbolFile);
            }
            catch (Exception e)
            {
                do context.Log.Error(e.Message); while ((e = e.InnerException) != null);

                throw new Exception("Upload to Hockey App failed.");
            }
        }
    }
}
