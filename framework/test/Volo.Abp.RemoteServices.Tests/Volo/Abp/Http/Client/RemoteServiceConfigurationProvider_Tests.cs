using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.Http.Client;

public class RemoteServiceConfigurationProvider_Tests : AbpRemoteServicesTestBase
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IRemoteServiceConfigurationProvider _remoteServiceConfigurationProvider;
    private readonly Guid _tenantAId = Guid.NewGuid();
    
    public RemoteServiceConfigurationProvider_Tests()
    {
        _currentTenant =  GetRequiredService<ICurrentTenant>();
        _remoteServiceConfigurationProvider = GetRequiredService<IRemoteServiceConfigurationProvider>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpRemoteServiceOptions>(options =>
        {
            options.RemoteServices[RemoteServiceConfigurationDictionary.DefaultName] = new RemoteServiceConfiguration("https://abp.io");
            options.RemoteServices["Identity"] = new RemoteServiceConfiguration("https://{{tenantName}}.abp.io");
            options.RemoteServices["Permission"] = new RemoteServiceConfiguration("https://{{tenantId}}.abp.io");
            options.RemoteServices["Setting"] = new RemoteServiceConfiguration("https://{0}.abp.io");
        });
        
        services.Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants =
            [
                new TenantConfiguration(_tenantAId, "TenantA")
            ];
        });
    }

    [Fact]
    public async Task GetConfigurationOrDefaultAsync()
    {
        _currentTenant.Id.ShouldBeNull();
        
        var defaultConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(RemoteServiceConfigurationDictionary.DefaultName);
        defaultConfiguration.BaseUrl.ShouldBe("https://abp.io");

        var identityConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync("Identity");
        identityConfiguration.BaseUrl.ShouldBe("https://abp.io");

        var permissionConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync("Permission");
        permissionConfiguration.BaseUrl.ShouldBe("https://abp.io");
        
        var settingConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync("Setting");
        settingConfiguration.BaseUrl.ShouldBe("https://abp.io");

        using (_currentTenant.Change(_tenantAId, "TenantA"))
        { 
            defaultConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(RemoteServiceConfigurationDictionary.DefaultName);
            defaultConfiguration.BaseUrl.ShouldBe("https://abp.io");

            identityConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync("Identity");
            identityConfiguration.BaseUrl.ShouldBe($"https://{_currentTenant.Name}.abp.io");

            permissionConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync("Permission");
            permissionConfiguration.BaseUrl.ShouldBe($"https://{_currentTenant.Id}.abp.io");
            
            settingConfiguration = await _remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync("Setting");
            settingConfiguration.BaseUrl.ShouldBe($"https://{_currentTenant.Name}.abp.io");
        }
    }
}