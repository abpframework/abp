using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

[Dependency(ReplaceServices = true)]
public class MauiBlazorAbpAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    public virtual Task<string?> GetTokenAsync()
    {
        return Task.FromResult(null as string);
    }
}
