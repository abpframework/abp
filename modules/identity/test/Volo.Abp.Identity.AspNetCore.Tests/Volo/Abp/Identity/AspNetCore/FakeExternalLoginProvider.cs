using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity.AspNetCore;

public class FakeExternalLoginProvider : ExternalLoginProviderBase, ITransientDependency
{
    public const string Name = "Fake";

    public FakeExternalLoginProvider(
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IIdentityUserRepository identityUserRepository,
        IOptions<IdentityOptions> identityOptions)
        : base(
            guidGenerator,
            currentTenant,
            userManager,
            identityUserRepository,
            identityOptions)
    {

    }

    public override Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
    {
        return Task.FromResult(
            userName == "ext_user" && plainPassword == "abc"
        );
    }

    protected override Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName)
    {
        if (userName != "ext_user")
        {
            throw new ArgumentException();
        }

        return Task.FromResult(
            new ExternalLoginUserInfo("ext_user@test.com")
            {
                Name = "Test Name", //optional, if the provider knows it
                    Surname = "Test Surname", //optional, if the provider knows it
                    EmailConfirmed = true, //optional, if the provider knows it
                    TwoFactorEnabled = false, //optional, if the provider knows it
                    PhoneNumber = "123", //optional, if the provider knows it
                    PhoneNumberConfirmed = false, //optional, if the provider knows it
                    ProviderKey = "123" //The id of the user on the provider side
                }
        );
    }
}
