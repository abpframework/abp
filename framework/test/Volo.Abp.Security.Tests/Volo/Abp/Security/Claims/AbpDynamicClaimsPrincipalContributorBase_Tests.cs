using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.Claims;

class TestAbpDynamicClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
{
    public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        Check.NotNull(identity, nameof(identity));

        await AddDynamicClaimsAsync(context, identity, AbpDynamicClaimsPrincipalContributorBase_Tests.DynamicClaims);
    }
}

public class AbpDynamicClaimsPrincipalContributorBase_Tests : AbpIntegratedTest<AbpSecurityTestModule>
{
    private readonly TestAbpDynamicClaimsPrincipalContributor _dynamicClaimsPrincipalContributorBase = new TestAbpDynamicClaimsPrincipalContributor();

    public readonly static List<AbpClaimCacheItem> DynamicClaims = new List<AbpClaimCacheItem>();

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public async Task AddDynamicClaimsAsync()
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.UserName, "test-source-userName"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.Name, "test-source-name"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.SurName, "test-source-surname"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.Role, "test-source-role1"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.Role, "test-source-role2"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.Email, "test-source-email"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.EmailVerified, "test-source-emailVerified"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.PhoneNumber, "test-source-phoneNumber"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(AbpClaimTypes.PhoneNumberVerified, "test-source-phoneNumberVerified"));
        claimsPrincipal.Identities.First().AddClaim(new Claim("my-claim", "test-source-my-claim"));

        DynamicClaims.AddRange(new []
        {
            new AbpClaimCacheItem("preferred_username", "test-preferred_username"),
            new AbpClaimCacheItem(ClaimTypes.GivenName, "test-given_name"),
            new AbpClaimCacheItem("family_name", "test-family_name"),
            new AbpClaimCacheItem("role", "test-role1"),
            new AbpClaimCacheItem("roles", "test-role2"),
            new AbpClaimCacheItem(ClaimTypes.Role, "test-role3"),
            new AbpClaimCacheItem("email", "test-email"),
            new AbpClaimCacheItem(AbpClaimTypes.EmailVerified, "test-email-verified"),
            new AbpClaimCacheItem(AbpClaimTypes.PhoneNumberVerified, null),
        });

        await _dynamicClaimsPrincipalContributorBase.ContributeAsync(new AbpClaimsPrincipalContributorContext(claimsPrincipal, GetRequiredService<IServiceProvider>()));

        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.UserName && c.Value == "test-preferred_username");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.SurName && c.Value == "test-family_name");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.Name && c.Value == "test-given_name");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.Role && c.Value == "test-role1");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.Role && c.Value == "test-role2");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.Role && c.Value == "test-role3");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.Email && c.Value == "test-email");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.EmailVerified && c.Value == "test-email-verified");
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == AbpClaimTypes.PhoneNumber && c.Value == "test-source-phoneNumber");
        claimsPrincipal.Identities.First().Claims.ShouldNotContain(c => c.Type == AbpClaimTypes.PhoneNumberVerified);
        claimsPrincipal.Identities.First().Claims.ShouldContain(c => c.Type == "my-claim" && c.Value == "test-source-my-claim");
    }
}
