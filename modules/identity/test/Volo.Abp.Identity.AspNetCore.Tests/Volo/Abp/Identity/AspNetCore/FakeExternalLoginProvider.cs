using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity.AspNetCore
{
    public class FakeExternalLoginProvider : ExternalLoginProviderBase, ITransientDependency
    {
        public const string Name = "Fake";

        private readonly ICurrentTenant _currentTenant;
        private readonly IdentityUserManager _userManager;
        private RandomPasswordGenerator _randomPasswordGenerator;

        public FakeExternalLoginProvider(ICurrentTenant currentTenant, IdentityUserManager userManager, RandomPasswordGenerator randomPasswordGenerator)
        {
            _currentTenant = currentTenant;
            _userManager = userManager;
            _randomPasswordGenerator = randomPasswordGenerator;
        }

        public override Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
        {
            return Task.FromResult(
                userName == "ext_user" && plainPassword == "abc"
            );
        }

        public override async Task<IdentityUser> CreateUserAsync(string userName)
        {
            var user = new IdentityUser(
                Guid.NewGuid(),
                userName,
                "test@abp.io",
                tenantId: _currentTenant.Id //Setting TenantId is responsibility of the provider!
            );

            user.SetLoginProvider(Name);
            user.SetEmailConfirmed(true); //Setting this is responsibility of the provider!
            user.SetPhoneNumber("123123", true);

            (await _userManager.CreateAsync(user, await _randomPasswordGenerator.CreateAsync())).CheckErrors();
            (await _userManager.AddDefaultRolesAsync(user)).CheckErrors();

            return user;
        }

        public override async Task UpdateUserAsync(IdentityUser user)
        {
            (await _userManager.SetEmailAsync(user, "test-updated@abp.io")).CheckErrors();
        }
    }
}
