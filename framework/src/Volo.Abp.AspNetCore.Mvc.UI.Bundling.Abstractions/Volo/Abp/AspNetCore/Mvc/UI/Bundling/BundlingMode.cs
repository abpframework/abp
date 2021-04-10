namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public enum BundlingMode
    {
        /// <summary>
        /// No bundling or minification.
        /// </summary>
        None,

        /// <summary>
        /// Automatically determine the mode.
        /// - Uses <see cref="None"/> for development time.
        /// - Uses <see cref="BundleAndMinify"/> for other environments.
        /// </summary>
        Auto,

        /// <summary>
        /// Bundled but not minified.
        /// </summary>
        Bundle,

        /// <summary>
        /// Bundled and minified.
        /// </summary>
        BundleAndMinify
    }
}