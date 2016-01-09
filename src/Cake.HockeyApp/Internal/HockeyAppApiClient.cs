using System.IO;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Extensions;

namespace Cake.HockeyApp.Internal
{
    internal class HockeyAppApiClient
    {
        private readonly IRestClient _restClient;

        public HockeyAppApiClient(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public HockeyAppResponse CreateNewVersionAsync(string apiToken, string appId, string bundleVersion, string bundleShortVersion)
        {
            var request = new RestRequest($"/api/2/apps/{appId}/app_versions/new");

            request.AddHeader("X-HockeyAppToken", apiToken);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("bundle_version", bundleVersion);
            request.AddParameter("bundle_short_version", bundleShortVersion);

            var response = _restClient.Post<HockeyAppResponse>(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            return response.Data;
        }

        public HockeyAppResponse UploadFileAsync(string apiToken, string appId, string version, string notes,
            string notesType, string status,
            string notify, string tags, string teams, string users, string mandatory, string commitSha,
            string buildServerUrl,
            string repositoryUrl, string filePath)
        {
            var request = new RestRequest($"/api/2/apps/{appId}/app_versions/{version}");

            request.AddHeader("X-HockeyAppToken", apiToken);

            request.AlwaysMultipartFormData = true;

            if (!string.IsNullOrEmpty(notes)) request.AddParameter("notes", notes);
            if (!string.IsNullOrEmpty(notesType)) request.AddParameter("notes_type", notesType);
            if (!string.IsNullOrEmpty(status)) request.AddParameter("status", status);
            if (!string.IsNullOrEmpty(notify)) request.AddParameter("notify", notify);
            if (!string.IsNullOrEmpty(tags)) request.AddParameter("tags", tags);
            if (!string.IsNullOrEmpty(teams)) request.AddParameter("teams", teams);
            if (!string.IsNullOrEmpty(users)) request.AddParameter("users", users);
            if (!string.IsNullOrEmpty(mandatory)) request.AddParameter("mandatory", mandatory);
            if (!string.IsNullOrEmpty(commitSha)) request.AddParameter("commit_sha", commitSha);
            if (!string.IsNullOrEmpty(buildServerUrl)) request.AddParameter("build_server_url", buildServerUrl);
            if (!string.IsNullOrEmpty(repositoryUrl)) request.AddParameter("repository_url", repositoryUrl);

            request.AddFile("ipa", filePath);
            request.Timeout = 3600000; // larger request timeout

            var response = _restClient.Put<HockeyAppResponse>(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            return response.Data;
        }
    }
}