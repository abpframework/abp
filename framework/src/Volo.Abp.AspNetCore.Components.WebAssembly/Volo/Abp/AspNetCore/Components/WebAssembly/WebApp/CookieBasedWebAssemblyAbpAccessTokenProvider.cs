using System.Threading.Tasks;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

public class CookieBasedWebAssemblyAbpAccessTokenProvider : IAbpAccessTokenProvider
{
    public virtual Task<string?> GetTokenAsync()
    {
        return Task.FromResult<string?>(null);
    }
}
