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

        public HockeyAppResponse UploadFileAsync(string apiToken, string appId, string version, string notes, string notesType, string status,
            string notify, string tags, string teams, string users, string mandatory, string commitSha, string buildServerUrl,
            string repositoryUrl, string filePath)
        {
            var request = new RestRequest($"/api/2/apps/{appId}/app_versions/{version}");

            request.AddHeader("X-HockeyAppToken", apiToken);

            request.AlwaysMultipartFormData = true;

            if (notes != null) request.AddParameter("notes", notes);
            if (notesType != null) request.AddParameter("notes_type", notesType);
            if (status != null) request.AddParameter("status", status);
            if (notify != null) request.AddParameter("notify", notify);
            if (tags != null) request.AddParameter("tags", tags);
            if (teams != null) request.AddParameter("teams", teams);
            if (users != null) request.AddParameter("users", users);
            if (mandatory != null) request.AddParameter("mandatory", mandatory);
            if (commitSha != null) request.AddParameter("commit_sha", commitSha);
            if (buildServerUrl != null) request.AddParameter("build_server_url", buildServerUrl);
            if (repositoryUrl != null) request.AddParameter("repository_url", repositoryUrl);

            request.AddFile("ipa", filePath);

            var response = _restClient.Put<HockeyAppResponse>(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            return response.Data;
        }
    }
}