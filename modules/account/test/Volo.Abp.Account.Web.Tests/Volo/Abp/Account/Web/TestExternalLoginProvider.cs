using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web;

// Authenticates the existing users with a password that is not their local password.
public class TestExternalLoginProvider : IExternalLoginProvider, ITransientDependency
{
    public const string Name = "Test";
    public const string Password = "External!9Aa";

    public Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
    {
        return Task.FromResult(plainPassword == Password);
    }

    public Task<IdentityUser> CreateUserAsync(string userName, string providerName)
    {
        throw new NotSupportedException();
    }

    public Task UpdateUserAsync(IdentityUser user, string providerName)
    {
        return Task.CompletedTask;
    }

    public Task<bool> IsEnabledAsync()
    {
        return Task.FromResult(true);
    }
}
