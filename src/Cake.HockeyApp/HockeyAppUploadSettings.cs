namespace Cake.HockeyApp
{
    public class HockeyAppUploadSettings
    {
        public string ApiToken { get; set; }

        public string AppId { get; set; }

        public string Version { get; set; }

        public string ShortVersion { get; set; }

        public string Notes { get; set; }

        public NoteType? NoteType { get; set; }

        public NotifyOption? Notify { get; set; }

        public DownloadStatus? Status { get; set; }

        public string[] Tags { get; set; }

        public int[] Teams { get; set; }

        public int[] Users { get; set; }

        public MandatoryOption? Mandatory { get; set; }

        public ReleaseType? ReleaseType { get; set; }

        public bool? Private { get; set; }

        public string OwnerId { get; set; }

        public string CommitSha { get; set; }

        public string BuildServerUrl { get; set; }

        public string RepositoryUrl { get; set; }
    }
}