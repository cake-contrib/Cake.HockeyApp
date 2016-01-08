using System.IO;
using System.Threading.Tasks;
using Refit;

namespace Cake.HockeyApp.Internal
{
    internal interface IHockeyAppApiClient
    {
        [Multipart]
        [Post("/api/2/apps/{appId}/app_versions/new")]
        Task<HockeyAppResponse> CreateNewVersionAsync(
            [Header("X-HockeyAppToken")] string apiToken,
            string appId,
            [AliasAs("bundle_version")] string bundleVersion,
            [AliasAs("bundle_short_version")] string bundleShortVersion);

        [Multipart]
        [Put("/api/2/apps/{appId}/app_versions/{version}")]
        Task<HockeyAppResponse> UploadFileAsync([Header("X-HockeyAppToken")] string apiToken, 
            string appId, string version, 
            [AttachmentName("ipa")] Stream stream, string notes, 
            [AliasAs("notes_type")] int? notesType, int? status, 
            int? notify, string tags, string teams, string users, int? mandatory, 
            [AliasAs("commit_sha")] string commitSha, 
            [AliasAs("build_server_url")] string buildServerUrl, 
            [AliasAs("repository_url")] string repositoryUrl);
    }
}