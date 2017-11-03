namespace Cake.HockeyApp
{
    /// <summary>
    /// Result returned after succesfull upload.
    /// </summary>
    public class HockeyAppUploadResult
    {
        /// <summary>
        /// Title of this app package.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Indicates if public download is allowed.
        /// </summary>
        public DownloadStatus DownloadStatus { get; set; }

        /// <summary>
        /// Url to configure this package.
        /// </summary>
        public string ConfigUrl { get; set; }

        /// <summary>
        /// Public url to download this package.
        /// </summary>
        public string PublicUrl { get; set; }

        /// <summary>
        /// Internal HockeyApp version id
        /// </summary>
        public string VersionId { get; set; }
    }
}