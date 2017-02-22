namespace Volo.Abp.AspNetCore.Mvc.Bundling
{
    public class BundlingOptions
    {
        public BundleCollection StyleBundles { get; set; }

        public BundleCollection ScriptBundles { get; set; }

        public BundlingOptions()
        {
            StyleBundles = new BundleCollection();
            ScriptBundles = new BundleCollection();
        }
    }
}