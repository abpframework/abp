using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity.AspNetCore
{
    public class FakeExternalLoginProvider : ExternalLoginProviderBase, ITransientDependency
    {
        public const string Name = "Fake";

        public override Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
        {
            return Task.FromResult(
                userName == "ext_user" && plainPassword == "abc"
            );
        }

        public override Task<IdentityUser> CreateUserAsync(string userName)
        {
            return Task.FromResult(
                new IdentityUser(
                    Guid.NewGuid(),
                    userName,
                    "test@abp.io"
                )
            );
        }
    }
}
