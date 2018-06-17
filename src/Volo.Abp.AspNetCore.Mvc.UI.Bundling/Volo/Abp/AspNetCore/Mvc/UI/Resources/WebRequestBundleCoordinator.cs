using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Resources
{
    public class WebRequestResources : IWebRequestResources, IScopedDependency
    {
        protected List<string> Resources { get; }

        public WebRequestResources()
        {
            Resources = new List<string>();
        }

        public virtual List<string> TryAdd(IEnumerable<string> resources)
        {
            var newFiles = resources.Except(Resources).ToList();
            Resources.AddRange(newFiles);
            return newFiles;
        }

        public bool TryAdd(string resource)
        {
            return Resources.AddIfNotContains(resource);
        }
    }
}