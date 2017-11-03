namespace Cake.HockeyApp.Internal
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    internal class HockeyAppApiClient
    {
        private readonly HttpClient _restClient;

        public HockeyAppApiClient(string baseUrl)
        {
            _restClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _restClient.Timeout = TimeSpan.FromHours(1); // larger request timeout
        }

        public async Task<HockeyAppResponse> CreateNewVersionAsync(string apiToken, string appId, string bundleVersion, string bundleShortVersion)
        {
            var request = new MultipartFormDataContent($"----CakeUpload{Guid.NewGuid().ToString().Replace('-', 'P')}");
            var boundary = request.Headers.ContentType.Parameters.First();
            boundary.Value = boundary.Value.Replace("\"", "");

            request.Headers.Add("X-HockeyAppToken", apiToken);

            request.AddIfNotEmpty("bundle_version",bundleVersion);
            request.AddIfNotEmpty("bundle_short_version", bundleShortVersion);

            var httpResponse = await _restClient.PostAsync($"/api/2/apps/{appId}/app_versions/new", request);

            return await HandleResponse(httpResponse);
        }

        public async Task<HockeyAppResponse> UploadFileToVersionAsync(string apiToken, string appId, string version, string notes,
            string notesType, string status,
            string notify, string tags, string teams, string users, string mandatory, string commitSha,
            string buildServerUrl,
            string repositoryUrl, string filePath, string symbolPath)
        {
            Stream dsymStream = null, appStream = null;

            var request = new MultipartFormDataContent($"----CakeUpload{Guid.NewGuid().ToString().Replace('-', 'P')}");
            var boundary = request.Headers.ContentType.Parameters.First();
            boundary.Value = boundary.Value.Replace("\"", "");

            request.Headers.Add("X-HockeyAppToken", apiToken);

            request.AddIfNotEmpty("notes", notes);
            request.AddIfNotEmpty("notes_type", notesType);
            request.AddIfNotEmpty("notify", notify);
            request.AddIfNotEmpty("status", status);
            request.AddIfNotEmpty("tags", tags);
            request.AddIfNotEmpty("teams", teams);
            request.AddIfNotEmpty("users", users);
            request.AddIfNotEmpty("mandatory", mandatory);
            request.AddIfNotEmpty("commit_sha", commitSha);
            request.AddIfNotEmpty("build_server_url", buildServerUrl);
            request.AddIfNotEmpty("repository_url", repositoryUrl);

            if (!string.IsNullOrEmpty(filePath))
            {
                appStream = File.OpenRead(filePath);
                request.Add(new StreamContent(appStream), "ipa", Path.GetFileName(filePath));
            }

            if (!string.IsNullOrEmpty(symbolPath))
            {
                dsymStream = File.OpenRead(symbolPath);
                request.Add(new StreamContent(dsymStream), "dsym", Path.GetFileName(symbolPath));
            }

            var httpResponse = await _restClient.PutAsync($"/api/2/apps/{appId}/app_versions/{version}", request);

            dsymStream?.Dispose();
            appStream?.Dispose();

            return await HandleResponse(httpResponse);
        }

        public async Task<HockeyAppResponse> UploadFileAsync(string apiToken, string notes,
            string notesType, string status,
            string notify, string tags, string teams, string users, string mandatory, string commitSha,
            string buildServerUrl, 
            string repositoryUrl, string filePath, string symbolPath, string releaseType, string @private, string ownerId)
        {
            Stream dsymStream = null, appStream = null;

            var request = new MultipartFormDataContent($"----CakeUpload{Guid.NewGuid().ToString().Replace('-','P')}");
            var boundary = request.Headers.ContentType.Parameters.First();
            boundary.Value = boundary.Value.Replace("\"", "");

            request.Headers.Add("X-HockeyAppToken", apiToken);

            request.AddIfNotEmpty("notes", notes);
            request.AddIfNotEmpty("notes_type", notesType);
            request.AddIfNotEmpty("notify", notify);
            request.AddIfNotEmpty("status", status);
            request.AddIfNotEmpty("tags", tags);
            request.AddIfNotEmpty("teams", teams);
            request.AddIfNotEmpty("users", users);
            request.AddIfNotEmpty("mandatory", mandatory);
            request.AddIfNotEmpty("release_type", releaseType);
            request.AddIfNotEmpty("private", @private);
            request.AddIfNotEmpty("owner_id", ownerId);
            request.AddIfNotEmpty("commit_sha", commitSha);
            request.AddIfNotEmpty("build_server_url", buildServerUrl);
            request.AddIfNotEmpty("repository_url", repositoryUrl);

            appStream = File.OpenRead(filePath);
            request.AddIfNotEmpty("ipa", Path.GetFileName(filePath), appStream);

            if (!string.IsNullOrEmpty(symbolPath))
            {
                dsymStream = File.OpenRead(symbolPath);
                request.AddIfNotEmpty("dsym", Path.GetFileName(symbolPath), dsymStream);
            }

            var httpResponse = await _restClient.PostAsync("/api/2/apps/upload", request);

            dsymStream?.Dispose();
            appStream?.Dispose();

            return await HandleResponse(httpResponse);
        }

        private static async Task<HockeyAppResponse> HandleResponse(HttpResponseMessage httpResponse)
        {
            var response = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HockeyAppResponse>(response);

            result.Success = httpResponse.IsSuccessStatusCode;
            if (!result.Success)
                result.Message = result.Message ?? $"({httpResponse.StatusCode}) {httpResponse.ReasonPhrase}";

            return result;
        }
    }
}