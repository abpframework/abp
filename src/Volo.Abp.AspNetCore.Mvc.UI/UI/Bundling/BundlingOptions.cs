namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundlingOptions
    {
        public BundleConfigurationCollection StyleBundles { get; set; }

        public BundleConfigurationCollection ScriptBundles { get; set; }

        public BundlingOptions()
        {
            StyleBundles = new BundleConfigurationCollection();
            ScriptBundles = new BundleConfigurationCollection();
        }
    }
}