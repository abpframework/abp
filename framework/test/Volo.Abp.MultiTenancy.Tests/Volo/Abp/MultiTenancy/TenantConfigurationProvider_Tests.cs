using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using NSubstitute;
using Shouldly;
using Volo.Abp.MultiTenancy.Localization;
using Xunit;

namespace Volo.Abp.MultiTenancy;

public class TenantConfigurationProvider_Tests
{
    private readonly ITenantResolver _resolver = Substitute.For<ITenantResolver>();
    private readonly ITenantStore _store = Substitute.For<ITenantStore>();
    private readonly ITenantNormalizer _normalizer = Substitute.For<ITenantNormalizer>();
    private readonly TenantConfigurationProvider _provider;

    public TenantConfigurationProvider_Tests()
    {
        _provider = new TenantConfigurationProvider(_resolver, _store, Substitute.For<ITenantResolveResultAccessor>(),
            Substitute.For<IStringLocalizer<AbpMultiTenancyResource>>(), _normalizer);
    }

    [Fact]
    public async Task Should_Not_Normalize_Or_Look_Up_Overlong_Tenant_Names()
    {
        _resolver.ResolveTenantIdOrNameAsync().Returns(new TenantResolveResult { TenantIdOrName = new string('a', TenantConfigurationProvider.MaxTenantNameLength + 1) });

        await Should.ThrowAsync<BusinessException>(() => _provider.GetAsync());

        _normalizer.DidNotReceiveWithAnyArgs().NormalizeName(default);
        await _store.DidNotReceiveWithAnyArgs().FindAsync(default(string)!);
    }

    [Fact]
    public async Task Should_Find_Tenants_By_Name_And_Id()
    {
        var tenant = new TenantConfiguration(Guid.NewGuid(), "acme", "ACME");
        _normalizer.NormalizeName("acme").Returns("ACME");
        _store.FindAsync("ACME").Returns(tenant);
        _store.FindAsync(tenant.Id).Returns(tenant);

        _resolver.ResolveTenantIdOrNameAsync().Returns(new TenantResolveResult { TenantIdOrName = "acme" });
        (await _provider.GetAsync())!.Id.ShouldBe(tenant.Id);

        _resolver.ResolveTenantIdOrNameAsync().Returns(new TenantResolveResult { TenantIdOrName = tenant.Id.ToString() });
        (await _provider.GetAsync())!.Id.ShouldBe(tenant.Id);
    }
}
