using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Resources
{
    public interface IWebRequestResources
    {
        /// <summary>
        /// Filters given resources and returns a list of resources those are not
        /// added to the page for current web request.
        /// </summary>
        List<string> Filter(IEnumerable<string> resources);

        bool IsAddedBefore(string resource);

        void Add(IEnumerable<string> resources);

        IReadOnlyList<string> GetAll();
    }
}
