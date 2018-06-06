namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfiguration
    {
        public string Name { get; }

        public BundleContributorList Contributors { get; }

        public BundleConfiguration(string name)
        {
            Name = name;
            Contributors = new BundleContributorList();
        }
    }
}