using System.Threading.Tasks;

namespace Volo.Abp.Identity.AspNetCore
{
    public abstract class ExternalLoginProviderBase : IExternalLoginProvider
    {
        public abstract Task<bool> TryAuthenticateAsync(string userName, string plainPassword);

        public abstract Task<IdentityUser> CreateUserAsync(string userName);

        public virtual Task UpdateUserAsync(IdentityUser user)
        {
            return Task.CompletedTask;
        }
    }
}
