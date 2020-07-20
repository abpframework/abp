using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit
{
    [Dependency(ReplaceServices = true)]
    public class CmsKitBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "CmsKit";
    }
}
