namespace Cake.HockeyApp.Internal
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    internal class HockeyAppApiClient
    {
        private readonly HttpClient _restClient;

        public HockeyAppApiClient(string baseUrl)
        {
            _restClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<HockeyAppResponse> CreateNewVersionAsync(string apiToken, string appId, string bundleVersion, string bundleShortVersion)
        {
            var request = new MultipartFormDataContent();

            request.Headers.Add("X-HockeyAppToken", apiToken);

            request.Add(new StringContent(bundleVersion), "bundle_version");
            request.Add(new StringContent(bundleShortVersion), "bundle_short_version");

            var httpResponse = await _restClient.PostAsync($"/api/2/apps/{appId}/app_versions/new", request);

            httpResponse.EnsureSuccessStatusCode();
            var response =  await httpResponse.Content.ReadAsStringAsync() ;

            return JsonConvert.DeserializeObject<HockeyAppResponse>(response);
        }

        public async Task<HockeyAppResponse> UploadFileToVersionAsync(string apiToken, string appId, string version, string notes,
            string notesType, string status,
            string notify, string tags, string teams, string users, string mandatory, string commitSha,
            string buildServerUrl,
            string repositoryUrl, string filePath, string symbolPath)
        {
            Stream dsymStream = null, appStream = null;

            var request = new MultipartFormDataContent();

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

            appStream = File.OpenRead(filePath);
            request.Add(new StreamContent(appStream), "ipa");

            if (!string.IsNullOrEmpty(symbolPath))
            {
                dsymStream = File.OpenRead(symbolPath);
                request.Add(new StreamContent(dsymStream), "dsym");
            }

            _restClient.Timeout = TimeSpan.FromHours(1); // larger request timeout

            var httpResponse = await _restClient.PutAsync($"/api/2/apps/{appId}/app_versions/{version}", request);

            dsymStream?.Dispose();
            appStream?.Dispose();

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.Content.ReadAsStringAsync();
           
            return JsonConvert.DeserializeObject<HockeyAppResponse>(response);
        }

        public async Task<HockeyAppResponse> UploadFileAsync(string apiToken, string notes,
            string notesType, string status,
            string notify, string tags, string teams, string users, string mandatory, string commitSha,
            string buildServerUrl, 
            string repositoryUrl, string filePath, string symbolPath, string releaseType, string @private, string ownerId)
        {
            Stream dsymStream = null, appStream = null;

            var request = new MultipartFormDataContent();

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
            request.Add(new StreamContent(appStream), "ipa");

            if (!string.IsNullOrEmpty(symbolPath))
            {
                dsymStream = File.OpenRead(symbolPath);
                request.Add(new StreamContent(dsymStream), "dsym");
            }

            _restClient.Timeout = TimeSpan.FromHours(1); // larger request timeout

            var httpResponse = await _restClient.PostAsync("/api/2/apps/upload", request);

            dsymStream?.Dispose();
            appStream?.Dispose();

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HockeyAppResponse>(response);
        }
    }
}