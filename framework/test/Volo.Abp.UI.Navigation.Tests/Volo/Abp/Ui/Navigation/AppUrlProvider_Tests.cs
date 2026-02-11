using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.Testing;
using Volo.Abp.UI.Navigation.Urls;
using Xunit;

namespace Volo.Abp.UI.Navigation;

public class AppUrlProvider_Tests : AbpIntegratedTest<AbpUiNavigationTestModule>
{
    private readonly IAppUrlProvider _appUrlProvider;
    private readonly ICurrentTenant _currentTenant;

    private readonly Guid _tenantAId = Guid.NewGuid();

    public AppUrlProvider_Tests()
    {
        _appUrlProvider = ServiceProvider.GetRequiredService<AppUrlProvider>();
        _currentTenant = ServiceProvider.GetRequiredService<ICurrentTenant>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = "https://{{tenantName}}.abp.io";
            options.Applications["MVC"].Urls["PasswordReset"] = "account/reset-password";
            options.RedirectAllowedUrls.AddRange(new List<string>()
            {
                "https://wwww.volosoft.com",
                "https://wwww.aspnetzero.com",
                "https://{{tenantName}}.abp.io",
                "https://{{tenantId}}.abp.io",
                "https://*.demo.myabp.io"
            });

            options.Applications["BLAZOR"].RootUrl = "https://{{tenantId}}.abp.io";
            options.Applications["BLAZOR"].Urls["PasswordReset"] = "account/reset-password";
        });

        services.Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants = new TenantConfiguration[]
            {
                new(_tenantAId, "community")
            };
        });
    }

    [Fact]
    public async Task GetUrlAsync()
    {
        using (_currentTenant.Change(null))
        {
            var url = await _appUrlProvider.GetUrlAsync("MVC");
            url.ShouldBe("https://abp.io");

            url = await _appUrlProvider.GetUrlAsync("MVC", "PasswordReset");
            url.ShouldBe("https://abp.io/account/reset-password");
        }

        using (_currentTenant.Change(Guid.NewGuid(), "community"))
        {
            var url = await _appUrlProvider.GetUrlAsync("MVC");
            url.ShouldBe("https://community.abp.io");

            url = await _appUrlProvider.GetUrlAsync("MVC", "PasswordReset");
            url.ShouldBe("https://community.abp.io/account/reset-password");
        }

        using (_currentTenant.Change(_tenantAId))
        {
            var url = await _appUrlProvider.GetUrlAsync("BLAZOR");
            url.ShouldBe($"https://{_tenantAId}.abp.io");

            url = await _appUrlProvider.GetUrlAsync("BLAZOR", "PasswordReset");
            url.ShouldBe($"https://{_tenantAId}.abp.io/account/reset-password");
        }

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _appUrlProvider.GetUrlAsync("ANGULAR");
        });
    }

    [Fact]
    public async Task GetUrlOrNullAsync()
    {
        (await _appUrlProvider.GetUrlOrNullAsync("ANGULAR")).ShouldBeNull();
    }

    [Fact]
    public async Task IsRedirectAllowedUrlAsync()
    {
        (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://wwww.volosoft.com")).ShouldBeTrue();
        (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://wwww.demo.myabp.io")).ShouldBeTrue();
        (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://demo.myabp.io")).ShouldBeTrue();
        (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://api.demo.myabp.io")).ShouldBeTrue();
        (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://test.api.demo.myabp.io")).ShouldBeTrue();
        (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://volosoft.com/demo.myabp.io")).ShouldBeFalse();
        (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://wwww.myabp.io")).ShouldBeFalse();

        using (_currentTenant.Change(null))
        {
            (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://abp.io")).ShouldBeTrue();
        }

        using (_currentTenant.Change(_tenantAId, "community"))
        {
            (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://community.abp.io")).ShouldBeTrue();
            (await _appUrlProvider.IsRedirectAllowedUrlAsync("https://community2.abp.io")).ShouldBeFalse();
        }

        using (_currentTenant.Change(_tenantAId))
        {
            (await _appUrlProvider.IsRedirectAllowedUrlAsync($"https://{_tenantAId}.abp.io")).ShouldBeTrue();
            (await _appUrlProvider.IsRedirectAllowedUrlAsync($"https://{Guid.NewGuid()}.abp.io")).ShouldBeFalse();
        }
    }
}
