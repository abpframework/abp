namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class AbpBundlingOptions
    {
        public BundleConfigurationCollection StyleBundles { get; set; }

        public BundleConfigurationCollection ScriptBundles { get; set; }

        //TODO: Add option to enable/disable bundling / minification

        /// <summary>
        /// Default: "__bundles".
        /// </summary>
        public string BundleFolderName { get; } = "__bundles";

        /// <summary>
        /// Default: auto.
        /// </summary>
        public BundlingMode Mode { get; set; } = BundlingMode.Auto;

        public AbpBundlingOptions()
        {
            StyleBundles = new BundleConfigurationCollection();
            ScriptBundles = new BundleConfigurationCollection();
        }
    }
}