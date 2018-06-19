using System.Collections.Generic;
using System.Linq;
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

        public List<string> TryAdd(IEnumerable<string> resources)
        {
            var resourceToBeAdded = resources.Except(Resources).ToList();
            Resources.AddRange(resourceToBeAdded);
            return resourceToBeAdded;
        }
    }
}