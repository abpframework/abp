using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace AuthServer.Host
{
    public class BrandingProvider : IBrandingProvider, ISingletonDependency
    {
        public string AppName => "Authentication Server";
        public string LogoUrl => null;
    }
}
