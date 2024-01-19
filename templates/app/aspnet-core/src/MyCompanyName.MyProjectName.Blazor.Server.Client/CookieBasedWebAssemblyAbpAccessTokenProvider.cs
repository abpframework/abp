using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace MyCompanyName.MyProjectName.Blazor;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IAbpAccessTokenProvider))]
public class CookieBasedWebAssemblyAbpAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    public virtual Task<string?> GetTokenAsync()
    {
        return Task.FromResult<string?>(null);
    }
}
