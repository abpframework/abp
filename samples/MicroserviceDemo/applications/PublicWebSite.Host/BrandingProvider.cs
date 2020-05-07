using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace PublicWebSite.Host
{
    public class BrandingProvider : DefaultBrandingProvider, ISingletonDependency
    {
        public override string AppName => "Public Web Site";
    }
}
