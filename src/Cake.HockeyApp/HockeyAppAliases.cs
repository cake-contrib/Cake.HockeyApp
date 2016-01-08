using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.HockeyApp.Internal;

namespace Cake.HockeyApp
{
    [CakeAliasCategory("HockeyApp")]
    public static class HockeyAppAliases
    {
        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static void UploadToHockeyApp(this ICakeContext context, FilePath file, HockeyAppUploadSettings settings)
        {
            var uploader = new HockeyAppClient(context.Log);

            uploader.UploadFileAsync(file, settings).RunSynchronously();
        }
    }
}