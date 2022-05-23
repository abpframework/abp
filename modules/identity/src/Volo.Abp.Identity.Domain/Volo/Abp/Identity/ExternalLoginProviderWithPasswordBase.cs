using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

public abstract class ExternalLoginProviderWithPasswordBase : ExternalLoginProviderBase, IExternalLoginProviderWithPassword
{
    public ExternalLoginProviderWithPasswordBase(
        IGuidGenerator guidGenerator, 
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IIdentityUserRepository identityUserRepository,
        IOptions<IdentityOptions> identityOptions) : 
        base(guidGenerator, 
            currentTenant, 
            userManager,
            identityUserRepository,
            identityOptions)
    {
    }
    
    public async Task<IdentityUser> CreateUserAsync(string userName, string providerName, string plainPassword)
    {
        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(userName, plainPassword);

        return await CreateUserAsync(externalUser, userName, providerName);
    }

    public async Task UpdateUserAsync(IdentityUser user, string providerName, string plainPassword)
    {
        await IdentityOptions.SetAsync();
        
        var externalUser = await GetUserInfoAsync(user, plainPassword);

        await UpdateUserAsync(user, externalUser, providerName);
    }
    
    protected abstract Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName, string plainPassword);
    
    protected virtual Task<ExternalLoginUserInfo> GetUserInfoAsync(IdentityUser user, string plainPassword)
    {
        return GetUserInfoAsync(user.UserName, plainPassword);
    }
}