using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using Volo.Abp.AspNetCore.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;
using Volo.CmsKit.Public;
using Volo.CmsKit.Public.Web;

namespace Volo.CmsKit.PublicWebTests;

[DependsOn(typeof(AbpAspNetCoreTestBaseModule), typeof(CmsKitApplicationTestModule),
    typeof(CmsKitPublicHttpApiModule), typeof(CmsKitPublicWebModule))]
public class CmsKitPublicWebTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<ICurrentUser, CurrentUser>();
        context.Services.AddSingleton<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>();
        context.Services.AddAuthentication("Test")
            .AddScheme<AuthenticationSchemeOptions, CmsKitPublicWebTestAuthenticationHandler>("Test", _ => { });
        context.Services.AddSingleton<LookupRecordingCache>();
        context.Services.AddSingleton<Microsoft.Extensions.Caching.Distributed.IDistributedCache>(
            provider => provider.GetRequiredService<LookupRecordingCache>());
        Configure<AbpMvcLibsOptions>(options => options.CheckLibs = false);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints(endpoints => endpoints.MapCmsPageRoute(order: -100));
    }
}

[Route("cms-fallback/{**path}")]
public class FallbackController : ControllerBase
{
    [HttpGet]
    public string Get() => "Fallback";
}

public class CmsKitPublicWebTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public CmsKitPublicWebTestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var data = Context.RequestServices.GetRequiredService<CmsKitTestData>();
        if (Request.Headers["X-Test-User"] != data.User1Id.ToString())
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(AbpClaimTypes.UserId, data.User1Id.ToString()),
            new Claim(AbpClaimTypes.UserName, data.User1UserName)
        }, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
    }
}
