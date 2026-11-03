using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Volo.Abp.OpenIddict.Integration;

public class DisabledTestExternalLoginProvider : IExternalLoginProvider, ITransientDependency
{
    public const string Name = "DisabledTest";

    public Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
    {
        throw new InvalidOperationException("A disabled external login provider should not be used.");
    }

    public Task<IdentityUser> CreateUserAsync(string userName, string providerName)
    {
        throw new InvalidOperationException("A disabled external login provider should not be used.");
    }

    public Task UpdateUserAsync(IdentityUser user, string providerName)
    {
        throw new InvalidOperationException("A disabled external login provider should not be used.");
    }

    public Task<bool> IsEnabledAsync()
    {
        return Task.FromResult(false);
    }
}
