using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Resources
{
    public interface IWebRequestResources
    {
        List<string> TryAdd(List<string> resources);
    }
}
