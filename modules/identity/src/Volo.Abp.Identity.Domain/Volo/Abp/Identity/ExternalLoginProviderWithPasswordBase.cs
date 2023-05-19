using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

public abstract class ExternalLoginProviderWithPasswordBase : ExternalLoginProviderBase, IExternalLoginProviderWithPassword
{
    public bool CanObtainUserInfoWithoutPassword { get; }
    
    public ExternalLoginProviderWithPasswordBase(
        IGuidGenerator guidGenerator, 
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IIdentityUserRepository identityUserRepository,
        IOptions<IdentityOptions> identityOptions,
        bool canObtainUserInfoWithoutPassword = false) : 
        base(guidGenerator, 
            currentTenant, 
            userManager,
            identityUserRepository,
            identityOptions)
    {
        CanObtainUserInfoWithoutPassword = canObtainUserInfoWithoutPassword;
    }
    
    public async Task<IdentityUser> CreateUserAsync(string userName, string providerName, string plainPassword)
    {
        if (CanObtainUserInfoWithoutPassword)
        {
            return await CreateUserAsync(userName, providerName);
        }
        
        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(userName, plainPassword);

        return await CreateUserAsync(externalUser, userName, providerName);
    }

    public async Task UpdateUserAsync(IdentityUser user, string providerName, string plainPassword)
    {
        if (CanObtainUserInfoWithoutPassword)
        {
            await UpdateUserAsync(user, providerName);
            return;
        }
        
        await IdentityOptions.SetAsync();
        
        var externalUser = await GetUserInfoAsync(user, plainPassword);

        await UpdateUserAsync(user, externalUser, providerName);
    }

    protected override Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName)
    {
        throw new NotImplementedException($"{nameof(GetUserInfoAsync)} is not implemented default. It should be overriden and implemented by the deriving class!");
    }

    protected abstract Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName, string plainPassword);
    
    protected virtual Task<ExternalLoginUserInfo> GetUserInfoAsync(IdentityUser user, string plainPassword)
    {
        return GetUserInfoAsync(user.UserName, plainPassword);
    }
}