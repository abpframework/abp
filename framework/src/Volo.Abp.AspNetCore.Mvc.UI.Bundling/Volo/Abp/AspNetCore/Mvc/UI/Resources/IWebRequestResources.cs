using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Resources;

public interface IWebRequestResources
{
    List<BundleFile> TryAdd(List<BundleFile> resources);
}
