namespace Cake.HockeyApp
{
    /// <summary>
    /// Api parameter that sets the release distribution type.
    /// </summary>
    public enum ReleaseType
    {
        /// <summary>
        /// Alpha release
        /// </summary>
        Alpha = 2,

        /// <summary>
        /// Beta release
        /// </summary>
        Beta = 0,

        /// <summary>
        /// Store release
        /// </summary>
        Store = 1,

        /// <summary>
        /// Enterprise release
        /// </summary>
        Enterprise = 3
    }
}