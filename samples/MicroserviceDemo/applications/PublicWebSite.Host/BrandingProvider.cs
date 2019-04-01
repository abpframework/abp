using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace PublicWebSite.Host
{
    public class BrandingProvider : IBrandingProvider, ISingletonDependency
    {
        public string AppName => "Public Web Site";
        public string LogoUrl => null;
    }
}
