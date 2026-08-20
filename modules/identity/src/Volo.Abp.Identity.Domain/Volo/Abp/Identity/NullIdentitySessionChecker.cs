using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity;

public class NullIdentitySessionChecker : IIdentitySessionChecker, ISingletonDependency
{
    public Task<bool> IsValidAsync(string sessionId)
    {
        return Task.FromResult(true);
    }
}
