namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundlingOptions
    {
        public BundleConfigurationCollection StyleBundles { get; set; }

        public BundleConfigurationCollection ScriptBundles { get; set; }

        //TODO: Add option to enable/disable bundling / minification

        /// <summary>
        /// Default: "__bundles".
        /// </summary>
        public string BundleFolderName { get; } = "__bundles";

        public BundlingOptions()
        {
            StyleBundles = new BundleConfigurationCollection();
            ScriptBundles = new BundleConfigurationCollection();
        }
    }
}