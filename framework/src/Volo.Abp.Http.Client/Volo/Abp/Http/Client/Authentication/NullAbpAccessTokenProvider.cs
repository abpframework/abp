using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.Authentication;

public class NullAbpAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    public Task<string?> GetTokenAsync()
    {
        return Task.FromResult(null as string);
    }
}
