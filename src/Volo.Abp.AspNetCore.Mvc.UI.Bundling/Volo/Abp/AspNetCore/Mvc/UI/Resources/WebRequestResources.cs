using System.Collections.Generic;
using System.Collections.Immutable;
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

        public List<string> Filter(IEnumerable<string> resources)
        {
            return resources.Except(Resources).ToList();
        }

        public bool IsAddedBefore(string resource)
        {
            return Resources.Contains(resource);
        }

        public void Add(IEnumerable<string> resources)
        {
            foreach (var resource in resources)
            {
                Resources.AddIfNotContains(resource);
            }
        }

        public IReadOnlyList<string> GetAll()
        {
            return Resources.ToImmutableList();
        }
    }
}