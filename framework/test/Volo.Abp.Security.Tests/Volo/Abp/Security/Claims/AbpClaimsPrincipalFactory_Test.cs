using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.Claims;

public class AbpClaimsPrincipalFactory_Test : AbpIntegratedTest<AbpSecurityTestModule>
{
    private readonly IAbpClaimsPrincipalFactory _abpClaimsPrincipalFactory;
    private static string TestAuthenticationType => "Identity.Application";

    public AbpClaimsPrincipalFactory_Test()
    {
        _abpClaimsPrincipalFactory = GetRequiredService<IAbpClaimsPrincipalFactory>();

    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.Contributors.Add<TestAbpClaimsPrincipalContributor>();
            options.Contributors.Add<Test2AbpClaimsPrincipalContributor>();
            options.Contributors.Add<Test3AbpClaimsPrincipalContributor>();

            options.DynamicContributors.Add<TestAbpClaimsPrincipalContributor>();
            options.DynamicContributors.Add<Test2AbpClaimsPrincipalContributor>();
            options.DynamicContributors.Add<Test3AbpClaimsPrincipalContributor>();
        });

        services.AddTransient<TestAbpClaimsPrincipalContributor>();
        services.AddTransient<Test2AbpClaimsPrincipalContributor>();
        services.AddTransient<Test3AbpClaimsPrincipalContributor>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var claimsPrincipal = await _abpClaimsPrincipalFactory.CreateAsync();
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == "admin2@abp.io");
        claimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Email && x.Value == "admin@abp.io");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Version && x.Value == "2.0");
    }

    [Fact]
    public async Task Create_With_Exists_ClaimsPrincipal()
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(TestAuthenticationType, ClaimTypes.Name, ClaimTypes.Role));
        claimsPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.Name, "123"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.Role, "admin"));

        await _abpClaimsPrincipalFactory.CreateAsync(claimsPrincipal);
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value == "123");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "admin");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == "admin2@abp.io");
        claimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Email && x.Value == "admin@abp.io");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Version && x.Value == "2.0");
    }

    [Fact]
    public async Task DynamicCreateAsync()
    {
        var claimsPrincipal = await _abpClaimsPrincipalFactory.DynamicCreateAsync();
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == "admin2@abp.io");
        claimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Email && x.Value == "admin@abp.io");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Version && x.Value == "2.0");
    }

    [Fact]
    public async Task DynamicCreate_With_Exists_ClaimsPrincipal()
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(TestAuthenticationType, ClaimTypes.Name, ClaimTypes.Role));
        claimsPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.Name, "123"));
        claimsPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.Role, "admin"));

        await _abpClaimsPrincipalFactory.DynamicCreateAsync(claimsPrincipal);
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value == "123");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "admin");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == "admin2@abp.io");
        claimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Email && x.Value == "admin@abp.io");
        claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Version && x.Value == "2.0");
    }

    class TestAbpClaimsPrincipalContributor : IAbpClaimsPrincipalContributor
    {
        public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            var claimsIdentity = context.ClaimsPrincipal.Identities.FirstOrDefault(x => x.AuthenticationType == TestAuthenticationType)
                                 ?? new ClaimsIdentity(TestAuthenticationType);

            claimsIdentity.AddOrReplace(new Claim(ClaimTypes.Email, "admin@abp.io"));

            context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);

            return Task.CompletedTask;
        }
    }

    class Test2AbpClaimsPrincipalContributor : IAbpClaimsPrincipalContributor
    {
        public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            var claimsIdentity = context.ClaimsPrincipal.Identities.FirstOrDefault(x => x.AuthenticationType == TestAuthenticationType)
                                 ?? new ClaimsIdentity(TestAuthenticationType);

            claimsIdentity.AddOrReplace(new Claim(ClaimTypes.Email, "admin2@abp.io"));

            context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);

            return Task.CompletedTask;
        }
    }

    class Test3AbpClaimsPrincipalContributor : IAbpClaimsPrincipalContributor
    {
        public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            var claimsIdentity = context.ClaimsPrincipal.Identities.FirstOrDefault(x => x.AuthenticationType == TestAuthenticationType)
                                 ?? new ClaimsIdentity(TestAuthenticationType);

            claimsIdentity.AddOrReplace(new Claim(ClaimTypes.Version, "2.0"));

            context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);

            return Task.CompletedTask;
        }
    }
}