namespace Cake.HockeyApp
{
    /// <summary>
    /// Api parameter option for notifying testers.
    /// </summary>
    public enum NotifyOption
    {
        /// <summary>
        /// Do not notify testers
        /// </summary>
        DoNotNotify = 0,

        /// <summary>
        /// Notify the testers who can install the app
        /// </summary>
        RestrictedTesters = 1,

        /// <summary>
        /// Notify all testers
        /// </summary>
        AllTesters = 2
    }
}