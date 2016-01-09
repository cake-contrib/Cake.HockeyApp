using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.HockeyApp.Internal;
using Xunit;

namespace Cake.HockeyApp.Tests
{
    public class HockeyAppTests
    {
        [Fact]
        public void CanCreateNewVersion()
        {
            var client = new HockeyAppClient(new NullLog());

            client.CreateNewVersionAsync(new HockeyAppUploadSettings
            {
                AppId = "4184d30dce316de5443480ad8254985c",
                Version = "0.3.160108.1",
                ShortVersion = "0.3",
                Notify = NotifyOption.DoNotNotify,
                Private = true,
                Notes = "Uploaded via continuous integration."
            });
        }
    }
}
