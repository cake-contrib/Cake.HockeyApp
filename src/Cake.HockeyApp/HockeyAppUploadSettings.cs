namespace Cake.HockeyApp
{
    /// <summary>
    /// Contains settings used by <see cref="Cake.HockeyApp.Internal.HockeyAppClient" />
    /// For a detailed information look at the official <see href="http://support.hockeyapp.net/kb/api/api-versions#upload-version">API Documentation</see>
    /// </summary>
    public class HockeyAppUploadSettings
    {
        /// <summary>
        /// Gets or sets the token used for authentication. 
        /// If the token is not set it is loaded from the HOCKEYAPP_API_TOKEN environment variable.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// Gets or sets the application id. This is a required property if you don't upload an apk or ipa.
        /// You can only upload packages to apps you have created before.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the version tag. This is a required property if you don't upload an apk or ipa. 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the short version. This is a userfriendly alias for <see cref="Version"/>
        /// </summary>
        public string ShortVersion { get; set; }

        /// <summary>
        /// Gets or sets release notes for this version. Optional.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the type of notes. Either Plain Text or Markdown. Optional.
        /// </summary>
        public NoteType? NoteType { get; set; }

        /// <summary>
        /// Gets or sets the notify option declaring who has to be notified. Optional.
        /// </summary>
        public NotifyOption? Notify { get; set; }

        /// <summary>
        /// Gets or sets the download status declaring if this version can be downloaded. Optional.
        /// </summary>
        public DownloadStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets tags belonging to this version. Optional.
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the teams this version is restricted to. Optional.
        /// You need to specify the team ids.
        /// </summary>
        public int[] Teams { get; set; }

        /// <summary>
        /// Gets or sets the users this version is restricted to. Optional.
        /// You need to specify the user ids.
        /// </summary>
        public int[] Users { get; set; }

        /// <summary>
        /// Gets or sets the mandatory option for this version. Optional.
        /// </summary>
        public MandatoryOption? Mandatory { get; set; }

        /// <summary>
        /// Gets or stets the release type for this version. Optional.
        /// Default is Beta.
        /// </summary>
        public ReleaseType? ReleaseType { get; set; }

        /// <summary>
        /// Gets or sets if this is a private version. Optional.
        /// </summary>
        public bool? Private { get; set; }

        /// <summary>
        /// Gets or sets the owner id. Optional.
        /// You need a full access api token for this operation.
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the commit sha this version refers. Optional.
        /// </summary>
        public string CommitSha { get; set; }

        /// <summary>
        /// Gets or sets the build server url this version origins. Optional.
        /// </summary>
        public string BuildServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the repository url this version origins. Optional.
        /// </summary>
        public string RepositoryUrl { get; set; }
    }
}