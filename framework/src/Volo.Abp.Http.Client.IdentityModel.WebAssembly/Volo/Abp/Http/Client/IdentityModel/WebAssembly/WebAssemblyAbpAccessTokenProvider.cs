using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly;

[Dependency(ReplaceServices = true)]
public class WebAssemblyAbpAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    protected IAccessTokenProvider? AccessTokenProvider { get; }

    public WebAssemblyAbpAccessTokenProvider(IAccessTokenProvider accessTokenProvider)
    {
        AccessTokenProvider = accessTokenProvider;
    }

    public virtual async Task<string?> GetTokenAsync()
    {
        if (AccessTokenProvider == null)
        {
            return null;
        }

        var result = await AccessTokenProvider.RequestAccessToken();
        if (result.Status != AccessTokenResultStatus.Success)
        {
            return null;
        }

        result.TryGetToken(out var token);
        return token?.Value;
    }
}
