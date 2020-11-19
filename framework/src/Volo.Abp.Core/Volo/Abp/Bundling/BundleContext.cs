using System.Collections.Generic;

namespace Volo.Abp.Bundling
{
    public class BundleContext
    {
        public List<BundleDefinition> BundleDefinitions { get; set; }

        public BundleContext()
        {
            BundleDefinitions = new List<BundleDefinition>();
        }

        public void Add(string source, Dictionary<string, string> additionalProperties = null)
        {
            var bundleDefinition = new BundleDefinition
            {
                Source = source,
            };

            if (additionalProperties != null)
            {
                bundleDefinition.AdditionalProperties = additionalProperties;
            }

            BundleDefinitions.AddIfNotContains((item) => item.Source == bundleDefinition.Source, () => bundleDefinition);
        }
    }
}
