using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Testing;
using Volo.Abp.UI.Navigation.Urls;
using Xunit;

namespace Volo.Abp.UI.Navigation;

public class AppUrlProvider_Tests : AbpIntegratedTest<AbpUiNavigationTestModule>
{
    private readonly IAppUrlProvider _appUrlProvider;
    private readonly ICurrentTenant _currentTenant;

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
                "https://wwww.aspnetzero.com"
            });

            options.Applications["BLAZOR"].RootUrl = "https://{{tenantId}}.abp.io";
            options.Applications["BLAZOR"].Urls["PasswordReset"] = "account/reset-password";
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

        var tenantId = Guid.NewGuid();
        using (_currentTenant.Change(tenantId))
        {
            var url = await _appUrlProvider.GetUrlAsync("BLAZOR");
            url.ShouldBe($"https://{tenantId}.abp.io");

            url = await _appUrlProvider.GetUrlAsync("BLAZOR", "PasswordReset");
            url.ShouldBe($"https://{tenantId}.abp.io/account/reset-password");
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
    public void IsRedirectAllowedUrl()
    {
        _appUrlProvider.IsRedirectAllowedUrl("https://community.abp.io").ShouldBeFalse();
        _appUrlProvider.IsRedirectAllowedUrl("https://wwww.volosoft.com").ShouldBeTrue();
    }
}
