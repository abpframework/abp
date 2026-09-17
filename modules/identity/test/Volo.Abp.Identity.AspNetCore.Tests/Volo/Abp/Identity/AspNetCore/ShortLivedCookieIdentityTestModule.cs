using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.AspNetCore;

[DependsOn(typeof(AbpIdentityAspNetCoreTestModule))]
public class ShortLivedCookieIdentityTestModule : AbpModule
{
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var timeProvider = new TestTimeProvider();
        context.Services.AddSingleton(timeProvider);

        // Short lifetime with sliding expiration on a controllable clock, so a request past the
        // half-life schedules a cookie renewal without real time delays.
        context.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
            options.SlidingExpiration = true;
            options.TimeProvider = timeProvider;
        });
    }
}
