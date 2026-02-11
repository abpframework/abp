using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.MultiTenancy;

public class FallbackTenantResolveContributor_Tests : MultiTenancyTestBase
{
    private readonly Guid _testTenantId = Guid.NewGuid();
    private readonly string _testTenantName = "acme";
    private readonly string _testTenantNormalizedName = "ACME";

    private readonly AbpTenantResolveOptions _options;
    private readonly ITenantResolver _tenantResolver;

    public FallbackTenantResolveContributor_Tests()
    {
        _options = ServiceProvider.GetRequiredService<IOptions<AbpTenantResolveOptions>>().Value;
        _tenantResolver = ServiceProvider.GetRequiredService<ITenantResolver>();
    }

    protected override void BeforeAddApplication(IServiceCollection services)
    {
        services.Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants = new[]
            {
                new TenantConfiguration(_testTenantId, _testTenantName, _testTenantNormalizedName)
            };
        });

        services.Configure<AbpTenantResolveOptions>(options =>
        {
            options.FallbackTenant = _testTenantName;
        });
    }

    [Fact]
    public async Task Should_Resolve_To_Fallback_Tenant_If_No_Other_Contributor_Succeeds()
    {
        var result = await _tenantResolver.ResolveTenantIdOrNameAsync();

        result.TenantIdOrName.ShouldBe(_testTenantName);
        result.AppliedResolvers.ShouldContain(TenantResolverNames.FallbackTenant);
    }

    [Fact]
    public async Task Should_Not_Override_Resolved_Tenant()
    {
        // Arrange
        var customTenantName = "resolved-tenant";
        _options.TenantResolvers.Insert(0, new TestTenantResolveContributor(customTenantName));

        // Act
        var result = await _tenantResolver.ResolveTenantIdOrNameAsync();

        // Assert
        result.TenantIdOrName.ShouldBe(customTenantName);
        result.AppliedResolvers.First().ShouldBe("Test");
        result.AppliedResolvers.ShouldNotContain(TenantResolverNames.FallbackTenant);
    }

    public class TestTenantResolveContributor : TenantResolveContributorBase
    {
        private readonly string _tenant;

        public TestTenantResolveContributor(string tenant)
        {
            _tenant = tenant;
        }

        public override string Name => "Test";

        public override Task ResolveAsync(ITenantResolveContext context)
        {
            context.TenantIdOrName = _tenant;
            return Task.CompletedTask;
        }
    }
}