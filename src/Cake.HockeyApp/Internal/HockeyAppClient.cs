namespace Cake.HockeyApp.Internal
{
    using System;
    using System.Threading.Tasks;
    using Core.Diagnostics;
    using Core.IO;

    internal class HockeyAppClient
    {
        private readonly HockeyAppApiClient _client;
        private readonly ICakeLog _log;

        public HockeyAppClient(ICakeLog log, HockeyAppUploadSettings settings)
        {
            _log = log;
            _client = new HockeyAppApiClient(settings.ApiBaseUrl);
            _log.Verbose("Initialized HockeyApp Api at {0}", settings.ApiBaseUrl);
        }

        public async Task<HockeyAppUploadResult> UploadFile(FilePath file, FilePath symbolFile,
            HockeyAppUploadSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ApiToken))
                throw new ArgumentNullException("settings.ApiToken",
                    $"You have to either specify an ApiToken or define the {HockeyAppAliases.TokenVariable} environment variable.");

            _log.Information("Uploading file to HockeyApp. This can take several minutes....");

            if (settings.AppId == null)
            {
                return await Upload(file, symbolFile, settings);
            }

            var versionId = await CreateNewVersionAsync(settings);

            var res = await UploadToVersion(file, symbolFile, settings, versionId);
            res.VersionId = versionId;

            return res;
        }

        public async Task<HockeyAppUploadResult> UploadFileToVersion(FilePath file, FilePath symbolFile,
            HockeyAppUploadSettings settings, string versionId)
        {
            if (string.IsNullOrEmpty(settings.ApiToken))
                throw new ArgumentNullException("settings.ApiToken",
                    $"You have to either specify an ApiToken or define the {HockeyAppAliases.TokenVariable} environment variable.");

            if (string.IsNullOrEmpty(settings.AppId))
            {
                throw new ArgumentNullException("settings.AppId",
                    $"You have to specify an settings.AppId");
            }

            if (string.IsNullOrEmpty(versionId))
            {
                throw new ArgumentNullException("versionId",
                    $"You have to specify an versionId");
            }

            _log.Information("Uploading file to HockeyApp. This can take several minutes....");
            

            return await UploadToVersion(file, symbolFile, settings, versionId);
        }

        internal async Task<string> CreateNewVersionAsync(HockeyAppUploadSettings settings)
        {
            _log.Verbose($"Creating Version {settings.ShortVersion} ({settings.Version}) for {settings.AppId}.");

            var response = await _client.CreateNewVersionAsync(settings.ApiToken, settings.AppId,
                settings.Version, settings.ShortVersion);

            if (!response.Success)
                throw new Exception(response.Message);

            _log.Information($"Created Version {response.ShortVersion} ({response.Version}) for {response.Title}.");

            return response.Id;
        }

        /// <summary>
        /// Uploads file and symbols to an existing version.
        /// </summary>
        internal async Task<HockeyAppUploadResult> UploadToVersion(FilePath file, FilePath symbolFile, HockeyAppUploadSettings settings, string versionId)
        {
            _log.Verbose($"Uploading files: {file?.FullPath}, {symbolFile?.FullPath} to {settings.ShortVersion} ({settings.Version}) for {settings.AppId}.");

            var response = await _client.UploadFileToVersionAsync(settings.ApiToken, settings.AppId, versionId,
                    settings.Notes, ((int?)settings.NoteType).ToString(), ((int?)settings.Status).ToString(),
                    ((int?)settings.Notify).ToString(),
                    settings.Tags == null ? null : string.Join(",", settings.Tags),
                    settings.Teams == null ? null : string.Join(",", settings.Teams),
                    settings.Users == null ? null : string.Join(",", settings.Users),
                    ((int?)settings.Mandatory).ToString(), settings?.CommitSha,
                    settings?.BuildServerUrl, settings?.RepositoryUrl, file?.FullPath, symbolFile?.FullPath);

            if (!response.Success)
                throw new Exception(response.Message);

            _log.Information($"Uploaded file to HockeyApp. Title: {response.Title}, Version: {response.ShortVersion} ({response.Version})");

            return new HockeyAppUploadResult
            {
                Title = response.Title,
                ConfigUrl = response.ConfigUrl,
                PublicUrl = response.PublicUrl,
                DownloadStatus = (DownloadStatus?)response.Status ?? DownloadStatus.NotAllowed
            };
        }

        /// <summary>
        /// Uploads file and symbols to autodetect endpoint.
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
                var isSupportedSymbols = symbolFile.FullPath.EndsWith(".dSYM.zip")
                || symbolFile.FullPath.EndsWith("mapping.txt");

                if (!isSupportedSymbols)
                {
                    throw new ArgumentException(
                        "Symbols file needs to be of the following type: *.dSYM.zip for iOS / OS X, or mapping.txt file for Android.",
                        nameof(symbolFile));
                }
            }

            _log.Verbose($"Uploading file {file.FullPath} to autodiscover endpoint.");

            var response = await _client.UploadFileAsync(
                apiToken: settings.ApiToken,
                filePath: file?.FullPath,
                symbolPath: symbolFile?.FullPath,
                notes: settings.Notes,
                notesType: ((int?)settings.NoteType).ToString(),
                notify: ((int?)settings.Notify).ToString(),
                status: ((int?)settings.Status).ToString(),
                tags: settings.Tags == null ? null : string.Join(",", settings.Tags),
                teams: settings.Teams == null ? null : string.Join(",", settings.Teams),
                users: settings.Users == null ? null : string.Join(",", settings.Users),
                mandatory: ((int?)settings.Mandatory).ToString(),
                releaseType: ((int?)settings.ReleaseType).ToString(),
                @private: (settings.Private).ToString(),
                ownerId: settings.OwnerId,
                commitSha: settings?.CommitSha,
                buildServerUrl: settings?.BuildServerUrl,
                repositoryUrl: settings?.RepositoryUrl);

            if (!response.Success)
                throw new Exception(response.Message);

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