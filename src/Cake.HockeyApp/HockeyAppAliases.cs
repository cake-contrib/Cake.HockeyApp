using System;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.HockeyApp.Internal;

namespace Cake.HockeyApp
{
    [CakeAliasCategory("HockeyApp")]
    public static class HockeyAppAliases
    {
        public const string TokenVariable = "HOCKEYAPP_API_TOKEN";

        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static void UploadToHockeyApp(this ICakeContext context, FilePath file, HockeyAppUploadSettings settings)
        {
            var uploader = new HockeyAppClient(context.Log);

            settings.ApiToken = settings.ApiToken ??
                                context.Environment.GetEnvironmentVariable(TokenVariable);

            try
            {
                uploader.UploadFile(file, settings);
            }
            catch (Exception e)
            {
                do context.Log.Error(e.Message); while ((e = e.InnerException) != null);

                throw new Exception("Upload to Hockey App failed.");
            }
        }
    }
}