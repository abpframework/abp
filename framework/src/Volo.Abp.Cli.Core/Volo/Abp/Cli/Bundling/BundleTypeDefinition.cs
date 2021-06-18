using System;

namespace Volo.Abp.Cli.Bundling
{
    internal class BundleTypeDefinition
    {
        public int Level { get; set; }

        public Type BundleContributorType { get; set; }
    }
}
