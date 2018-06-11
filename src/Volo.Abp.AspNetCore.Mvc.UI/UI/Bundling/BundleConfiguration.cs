using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfiguration
    {
        public string Name { get; }

        public BundleContributorCollection Contributors { get; }

        public List<string> BaseBundles { get; }

        public BundleConfiguration(string name)
        {
            Name = name;

            Contributors = new BundleContributorCollection();
            BaseBundles = new List<string>();
        }
    }
}