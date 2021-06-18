using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web
{
    public class DefaultServerUrlProvider : IServerUrlProvider, ISingletonDependency
    {
        public string GetBaseUrl(string remoteServiceName = null)
        {
            return "/";
        }
    }
}