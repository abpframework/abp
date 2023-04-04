using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

[Dependency(TryRegister = true)]
public class NullAbpMauiAccessTokenProvider : IAbpMauiAccessTokenProvider
{
    public Task<string> GetAccessTokenAsync()
    {
        return Task.FromResult(null as string);
    }
}