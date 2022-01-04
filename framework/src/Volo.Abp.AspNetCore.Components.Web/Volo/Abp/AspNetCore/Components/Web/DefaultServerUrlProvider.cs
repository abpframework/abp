using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web;

public class DefaultServerUrlProvider : IServerUrlProvider, ISingletonDependency
{
    public Task<string> GetBaseUrlAsync(string remoteServiceName = null)
    {
        return Task.FromResult("/");
    }
}
