using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Authorization.TestServices.Resources;
using Xunit;

namespace Volo.Abp.Authorization;

public class ResourcePermissionValueProviderManager_Tests: AuthorizationTestBase
{
    private readonly IResourcePermissionValueProviderManager _resourcePermissionValueProviderManager;

    public ResourcePermissionValueProviderManager_Tests()
    {
        _resourcePermissionValueProviderManager = GetRequiredService<IResourcePermissionValueProviderManager>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.Services.Configure<AbpPermissionOptions>(permissionOptions =>
        {
            permissionOptions.ResourceValueProviders.Add<TestDuplicateResourcePermissionValueProvider>();
        });
    }

    [Fact]
    public void Should_Throw_Exception_If_Duplicate_Provider_Name_Detected()
    {
        var exception = Assert.Throws<AbpException>(() =>
        {
            var providers = _resourcePermissionValueProviderManager.ValueProviders;
        });

        exception.Message.ShouldBe($"Duplicate resource permission value provider name detected: TestResourcePermissionValueProvider1. Providers:{Environment.NewLine}{typeof(TestDuplicateResourcePermissionValueProvider).FullName}{Environment.NewLine}{typeof(TestResourcePermissionValueProvider1).FullName}");
    }
}

public class TestDuplicateResourcePermissionValueProvider : ResourcePermissionValueProvider
{
    public TestDuplicateResourcePermissionValueProvider(IResourcePermissionStore permissionStore) : base(permissionStore)
    {
    }

    public override string Name => "TestResourcePermissionValueProvider1";

    public override Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<MultiplePermissionGrantResult> CheckAsync(ResourcePermissionValuesCheckContext context)
    {
        throw new NotImplementedException();
    }
}
