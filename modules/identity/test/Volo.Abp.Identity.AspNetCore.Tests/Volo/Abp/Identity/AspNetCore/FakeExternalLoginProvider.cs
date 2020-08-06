using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity.AspNetCore
{
    public class FakeExternalLoginProvider : ExternalLoginProviderBase, ITransientDependency
    {
        public const string Name = "Fake";

        public FakeExternalLoginProvider(
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IdentityUserManager userManager,
            RandomPasswordGenerator randomPasswordGenerator)
            : base(guidGenerator, currentTenant, userManager, randomPasswordGenerator)
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
            return Task.FromResult(
                new ExternalLoginUserInfo("ext_user@test.com")
                {
                    Name = "Test Name",
                    Surname = "Test Surname",
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    PhoneNumber = "123",
                    PhoneNumberConfirmed = false,
                    ProviderKey = "123"
                }
            );
        }
    }
}
