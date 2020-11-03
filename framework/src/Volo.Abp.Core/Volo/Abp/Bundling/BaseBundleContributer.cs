using System;
using System.Collections.Generic;

namespace Volo.Abp.Bundling
{
    public class BaseBundleContributer : IBundleContributer
    {
        public virtual void AddScripts(List<BundleDefinition> scriptDefinitions)
        {
            var scripts = GetScripts();

            foreach (var script in scripts)
            {
                scriptDefinitions.AddIfNotContains((item) => item.Source == script, () => new BundleDefinition(script));
            }
        }

        public virtual void AddStyles(List<BundleDefinition> styleDefinitions)
        {
            var styles = GetStyles();

            foreach (var style in styles)
            {
                styleDefinitions.AddIfNotContains((item) => item.Source == style, () => new BundleDefinition(style));
            }
        }

        public virtual string[] GetStyles()
        {
            return Array.Empty<string>();
        }

        public virtual string[] GetScripts()
        {
            return Array.Empty<string>();
        }
    }
}
