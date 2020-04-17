using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace BackendAdminApp.Host
{
    public class BrandingProvider : IBrandingProvider, ISingletonDependency
    {
        public string AppName => "Backend Admin App";
        public string LogoUrl => null;
    }
}
