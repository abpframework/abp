using System.Collections.Generic;

namespace Volo.Abp.Bundling
{
    public class BundleContext
    {
        public List<BundleDefinition> BundleDefinitions { get; set; }
        public BundleParameterDictionary Parameters { get; set; }
        
        public BundleContext()
        {
            BundleDefinitions = new List<BundleDefinition>();
            Parameters = new BundleParameterDictionary();
        }

        public void Add(string source, bool excludeFromBundle = false,
            Dictionary<string, string> additionalProperties = null)
        {
            var bundleDefinition = new BundleDefinition
            {
                Source = source,
                ExcludeFromBundle = excludeFromBundle
            };

            if (additionalProperties != null)
            {
                bundleDefinition.AdditionalProperties = additionalProperties;
            }

            BundleDefinitions.AddIfNotContains((item) => item.Source == bundleDefinition.Source,
                () => bundleDefinition);
        }
    }
}