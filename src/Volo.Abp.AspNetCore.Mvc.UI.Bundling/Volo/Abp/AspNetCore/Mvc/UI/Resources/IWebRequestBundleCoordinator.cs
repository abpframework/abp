using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IWebRequestResources
    {
        /// <summary>
        /// Adds resouces to to current web request except the ones added before.
        /// </summary>
        /// <param name="resources">Candidate resources to be added</param>
        /// <returns>Resources actually added</returns>
        List<string> TryAdd(IEnumerable<string> resources);

        bool TryAdd(string resource);
    }
}
