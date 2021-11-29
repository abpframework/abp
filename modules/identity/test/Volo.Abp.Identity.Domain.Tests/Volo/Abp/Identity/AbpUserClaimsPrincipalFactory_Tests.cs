using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Identity
{
    public class AbpUserClaimsPrincipalFactory_Tests : AbpIdentityDomainTestBase
    {
        private readonly IdentityUserManager _identityUserManager;
        private readonly AbpUserClaimsPrincipalFactory _abpUserClaimsPrincipalFactory;
        private readonly IdentityTestData _testData;

        public AbpUserClaimsPrincipalFactory_Tests()
        {
            _identityUserManager = GetRequiredService<IdentityUserManager>();
            _abpUserClaimsPrincipalFactory = GetRequiredService<AbpUserClaimsPrincipalFactory>();
            _testData = GetRequiredService<IdentityTestData>();
        }

        [Fact]
        public async Task Add_And_Replace_Claims_Test()
        {
            await UsingUowAsync(async () =>
            {
                var user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);
                user.ShouldNotBeNull();

                var claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);

                claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.NameIdentifier && x.Value == user.Id.ToString());
                claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value == user.UserName);

                claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Uri && x.Value =="www.abp.io");

                claimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Email && x.Value == user.Email);
                claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == "replaced@abp.io");
            });
        }

        class TestAbpClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
        {
            //https://github.com/dotnet/aspnetcore/blob/v5.0.0/src/Identity/Extensions.Core/src/UserClaimsPrincipalFactory.cs#L79
            private static string IdentityAuthenticationType => "Identity.Application";

            public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
            {
                var claimsIdentity = context.ClaimsPrincipal.Identities.First(x => x.AuthenticationType == IdentityAuthenticationType);

                claimsIdentity.AddOrReplace(new Claim(ClaimTypes.Uri, "www.abp.io"));
                claimsIdentity.AddOrReplace(new Claim(ClaimTypes.Email, "replaced@abp.io"));

                context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);

                return Task.CompletedTask;
            }
        }

    }
}
