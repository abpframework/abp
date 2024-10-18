using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.TestServices;
using Xunit;

namespace Volo.Abp;

public class PermissionValueProviderManager_Tests: AuthorizationTestBase
{
    private readonly IPermissionValueProviderManager _permissionValueProviderManager;

    public PermissionValueProviderManager_Tests()
    {
        _permissionValueProviderManager = GetRequiredService<IPermissionValueProviderManager>();
    }
    
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.Services.Configure<AbpPermissionOptions>(permissionOptions =>
        {
            permissionOptions.ValueProviders.Add<TestDuplicatePermissionValueProvider>();
        });
    }
    
    [Fact]
    public void Should_Throw_Exception_If_Duplicate_Provider_Name_Detected()
    {
        var exception = Assert.Throws<AbpException>(() =>
        {
            var providers = _permissionValueProviderManager.ValueProviders;
        });
        
        exception.Message.ShouldBe($"Duplicate permission value provider name detected: TestPermissionValueProvider1. Providers:{Environment.NewLine}{typeof(TestDuplicatePermissionValueProvider).FullName}{Environment.NewLine}{typeof(TestPermissionValueProvider1).FullName}");
    }
}

public class TestDuplicatePermissionValueProvider : PermissionValueProvider
{
    public TestDuplicatePermissionValueProvider(IPermissionStore permissionStore) : base(permissionStore)
    {
    }

    public override string Name => "TestPermissionValueProvider1";

    public override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        throw new NotImplementedException();
    }
}