namespace Cake.HockeyApp.Internal
{
    using System;
    using Core.Diagnostics;
    using Core.IO;
    using System.Threading.Tasks;

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

        public async Task<HockeyAppUploadResult> UploadFile(FilePath file, FilePath symbolFile,
            HockeyAppUploadSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ApiToken))
                throw new ArgumentNullException("settings.ApiToken",
                    $"You have to ether specify an ApiToken or define the {HockeyAppAliases.TokenVariable} environment variable.");

            _log.Information("Uploading file to HockeyApp. This can take several minutes....");

            if (settings.AppId == null)
            {
                return await Upload(file, symbolFile, settings);
            }

            var versionId = await CreateNewVersionAsync(settings);

            return await UploadToVersion(file, symbolFile, settings, versionId);
        }

        internal async Task<string> CreateNewVersionAsync(HockeyAppUploadSettings settings)
        {
            _log.Verbose($"Creating Version {settings.ShortVersion} ({settings.Version}) for {settings.AppId}.");

            var response = await _client.CreateNewVersionAsync(settings.ApiToken, settings.AppId,
                settings.Version, settings.ShortVersion);

            _log.Information( $"Created Version {response.ShortVersion} ({response.Version}) for {response.Title}.");

            return response.Id;
        }

        /// <summary>
        /// Uploads file & symbols to an existing version.
        /// </summary>
        internal async Task<HockeyAppUploadResult> UploadToVersion(FilePath file, FilePath symbolFile, HockeyAppUploadSettings settings, string versionId)
        {
            _log.Verbose($"Uploading file {file.FullPath} to {settings.ShortVersion} ({settings.Version}) for {settings.AppId}.");

            var response = await _client.UploadFileToVersionAsync(settings.ApiToken, settings.AppId, versionId,
                    settings.Notes, ((int?) settings.NoteType).ToString(), ((int?) settings.Status).ToString(),
                    ((int?) settings.Notify).ToString(),
                    settings.Tags == null ? null : string.Join(",", settings.Tags),
                    settings.Teams == null ? null : string.Join(",", settings.Teams),
                    settings.Users == null ? null : string.Join(",", settings.Users),
                    ((int?) settings.Mandatory).ToString(), settings?.CommitSha, 
                    settings?.BuildServerUrl, settings?.RepositoryUrl, file?.FullPath, symbolFile?.FullPath);

            _log.Information( $"Uploaded file to HockeyApp. Title: {response.Title}, Version: {response.ShortVersion} ({response.Version})");

            return new HockeyAppUploadResult
            {
                Title = response.Title,
                ConfigUrl = response.ConfigUrl,
                PublicUrl = response.PublicUrl,
                DownloadStatus = (DownloadStatus?) response.Status ?? DownloadStatus.NotAllowed
            };
        }

        /// <summary>
        /// Uploads file & symbols to autodetect endpoint.
        /// </summary>
        internal async Task<HockeyAppUploadResult> Upload(FilePath file, FilePath symbolFile, HockeyAppUploadSettings settings)
        {
            var isSupported = file.FullPath.EndsWith(".ipa")
                              || file.FullPath.EndsWith(".app.zip")
                              || file.FullPath.EndsWith(".apk");

            if (!isSupported)
            {
                throw new ArgumentException(
                    "File needs to be of the following type: *.ipa for iOS, *.app.zip for OS X, or *.apk file for Android.",
                    nameof(file));
            }

            if (symbolFile != null)
            {
                var isSupportedSymbols = file.FullPath.EndsWith(".dsym.zip")
                                         || file.FullPath.EndsWith("mapping.txt");

                if (!isSupportedSymbols)
                {
                    throw new ArgumentException(
                        "Symbols file needs to be of the following type: *.dsym.zip for iOS / OS X, or mapping.txt file for Android.",
                        nameof(file));
                }
            }

            _log.Verbose($"Uploading file {file.FullPath} to autodiscover endpoint.");

            var response = await _client.UploadFileAsync(
                apiToken: settings.ApiToken,
                filePath: file?.FullPath, 
                symbolPath: symbolFile?.FullPath,
                notes: settings.Notes,
                notesType: ((int?) settings.NoteType).ToString(),
                notify: ((int?) settings.Notify).ToString(),
                status: ((int?) settings.Status).ToString(),
                tags: settings.Tags == null ? null : string.Join(",", settings.Tags),
                teams: settings.Teams == null ? null : string.Join(",", settings.Teams), 
                users: settings.Users == null ? null : string.Join(",", settings.Users),
                mandatory: ((int?) settings.Mandatory).ToString(), 
                releaseType: ((int?)settings.ReleaseType).ToString(),
                @private: (settings.Private).ToString(),
                ownerId: settings.OwnerId,
                commitSha: settings?.CommitSha,
                buildServerUrl: settings?.BuildServerUrl, 
                repositoryUrl: settings?.RepositoryUrl);

            _log.Information($"Uploaded file to HockeyApp. Title: {response.Title}, Version: {response.ShortVersion} ({response.Version})");

            return new HockeyAppUploadResult
            {
                Title = response.Title,
                ConfigUrl = response.ConfigUrl,
                PublicUrl = response.PublicUrl,
                DownloadStatus = (DownloadStatus?)response.Status ?? DownloadStatus.NotAllowed
            };
        }
    }
}
