namespace Cake.HockeyApp
{
    /// <summary>
    /// Api parameter option for allowing app downloads.
    /// </summary>
    public enum DownloadStatus
    {
        /// <summary>
        /// Do not allow download or installation
        /// </summary>
        NotAllowed = 1,

        /// <summary>
        /// Allow download or installation
        /// </summary>
        Allowed = 2
    }
}