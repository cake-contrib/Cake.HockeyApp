using System;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Path = Cake.Core.IO.Path;

namespace Cake.HockeyApp.Internal
{
    internal class HockeyAppClient
    {
        private const string HockeyAppBaseUrl = "https://rink.hockeyapp.net";

        private readonly HockeyAppApiClient _client;
        private readonly ICakeLog _log;

        public HockeyAppClient(ICakeLog log)
        {
            _log = log;
            _client = new HockeyAppApiClient(HockeyAppBaseUrl);
            _log.Verbose("Initialized HockeyApp Api at {0}", HockeyAppBaseUrl);
        }

        public void UploadFile(FilePath file, HockeyAppUploadSettings settings, FilePath symbolFile = null)
        {
            if (string.IsNullOrEmpty(settings.ApiToken))
                throw new ArgumentNullException("settings.ApiToken", $"You have to ether specify an ApiToken or define the {HockeyAppAliases.TokenVariable} environment variable.");

            if (string.IsNullOrEmpty(settings.AppId))
                throw new ArgumentNullException("settings.AppId", "You have to specify an AppId");

            _log.Information("Uploading file to HockeyApp. This can take several minutes....");

            var versionId = CreateNewVersionAsync(settings);

            UploadToVersion(file, settings, versionId, symbolFile);
        }

        internal string CreateNewVersionAsync(HockeyAppUploadSettings settings)
        {
            _log.Verbose($"Creating Version {settings.ShortVersion} ({settings.Version}) for {settings.AppId}.");

            var response = _client.CreateNewVersionAsync(settings.ApiToken, settings.AppId,
                settings.Version, settings.ShortVersion);

            _log.Information( $"Created Version {response.ShortVersion} ({response.Version}) for {response.Title}.");

            return response.Id;
        }

        internal void UploadToVersion(Path file, HockeyAppUploadSettings settings, string versionId, Path symbolFile = null)
        {
            _log.Verbose($"Uploading file {file.FullPath} to {settings.ShortVersion} ({settings.Version}) for {settings.AppId}.");

            var response = _client.UploadFileAsync(settings.ApiToken, settings.AppId, versionId,
                    settings.Notes, ((int?) settings.NoteType).ToString(), ((int?) settings.Status).ToString(),
                    ((int?) settings.Notify).ToString(),
                    settings.Tags == null ? null : string.Join(",", settings.Tags),
                    settings.Teams == null ? null : string.Join(",", settings.Teams),
                    settings.Users == null ? null : string.Join(",", settings.Users),
                    ((int?) settings.Mandatory).ToString(), settings?.CommitSha, 
                    settings?.BuildServerUrl, settings?.RepositoryUrl, file?.FullPath, symbolFile?.FullPath);


            _log.Information( $"Uploaded file to HockeyApp. Title: {response.Title}, Version: {response.ShortVersion} ({response.Version})");
        }
    }
}
