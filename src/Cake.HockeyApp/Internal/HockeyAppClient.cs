using System;
using System.IO;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Refit;
using Path = Cake.Core.IO.Path;

namespace Cake.HockeyApp.Internal
{
    internal class HockeyAppClient
    {
        private const string HockeyAppBaseUrl = "https://rink.hockeyapp.net";

        private readonly IHockeyAppApiClient _client;
        private readonly ICakeLog _log;

        public HockeyAppClient(ICakeLog log)
        {
            _log = log;
            _client = RestService.For<IHockeyAppApiClient>(HockeyAppBaseUrl);
        }

        public async Task UploadFileAsync(FilePath file, HockeyAppUploadSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ApiToken))
                throw new ArgumentNullException("settings.ApiToken", "You have to specify an ApiToken");

            if (string.IsNullOrEmpty(settings.AppId))
                throw new ArgumentNullException("settings.AppId", "You have to specify an AppId");

            var versionId = await CreateNewVersionAsync(settings);

            await UploadToVersionAsync(file, settings, versionId);
        }

        private async Task<string> CreateNewVersionAsync(HockeyAppUploadSettings settings)
        {
            var response = await _client.CreateNewVersionAsync(settings.ApiToken, settings.AppId, 
                settings.Version, settings.ShortVersion);

            _log.Write(Verbosity.Normal, LogLevel.Information, $"Created Version {response.ShortVersion} ({response.Version}) for {response.Title}.");

            return response.Id;
        }

        private async Task UploadToVersionAsync(Path file, HockeyAppUploadSettings settings, string versionId)
        {
            HockeyAppResponse response;
            using (var stream = File.OpenRead(file.FullPath))
            {
                response = await _client.UploadFileAsync(settings.ApiToken, settings.AppId, versionId,
                    stream, settings.Notes, (int?) settings.NoteType, (int?) settings.Status, (int?) settings.Notify,
                    settings.Tags == null ? null : string.Join(",", settings.Tags),
                    settings.Teams == null ? null : string.Join(",", settings.Teams),
                    settings.Users == null ? null : string.Join(",", settings.Users),
                    (int?) settings.Mandatory, settings.CommitSha, settings.BuildServerUrl, settings.RepositoryUrl);
            }

            _log.Write(Verbosity.Normal, LogLevel.Information, $"Uploaded file to HockeyApp. Title: {response.Title}, Version: {response.ShortVersion} ({response.Version})");
        }
    }
}