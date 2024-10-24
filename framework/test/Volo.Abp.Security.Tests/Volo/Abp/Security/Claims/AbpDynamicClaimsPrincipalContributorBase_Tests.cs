using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.Claims;

[DisableConventionalRegistration]
class TestAbpDynamicClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
{
    private readonly List<AbpDynamicClaim> _claims;

    public TestAbpDynamicClaimsPrincipalContributor(List<AbpDynamicClaim> claims)
    {
        _claims = claims;
    }

    public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        Check.NotNull(identity, nameof(identity));

        await AddDynamicClaimsAsync(context, identity, _claims);
    }
}

public class AbpDynamicClaimsPrincipalContributorBase_Tests : AbpIntegratedTest<AbpSecurityTestModule>
{
    private readonly TestAbpDynamicClaimsPrincipalContributor _dynamicClaimsPrincipalContributorBase;

    private readonly AbpDynamicClaimCacheItem _dynamicClaims;

    public AbpDynamicClaimsPrincipalContributorBase_Tests()
    {
        _dynamicClaims = new AbpDynamicClaimCacheItem(new List<AbpDynamicClaim>()
        {
            new AbpDynamicClaim("preferred_username", "test-preferred_username"),
            new AbpDynamicClaim(ClaimTypes.GivenName, "test-given_name"),
            new AbpDynamicClaim("family_name", "test-family_name"),
            new AbpDynamicClaim("role", "test-role1"),
            new AbpDynamicClaim("roles", "test-role2"),
            new AbpDynamicClaim(ClaimTypes.Role, "test-role3"),
            new AbpDynamicClaim("email", "test-email"),
            new AbpDynamicClaim(AbpClaimTypes.EmailVerified, "test-email-verified"),
            new AbpDynamicClaim(AbpClaimTypes.PhoneNumberVerified, null),
        });
        _dynamicClaimsPrincipalContributorBase = new TestAbpDynamicClaimsPrincipalContributor(_dynamicClaims.Claims);
    }

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
