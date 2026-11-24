using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Web;

[DependsOn(typeof(AbpAspNetCoreTestBaseModule), typeof(CmsKitCommonWebModule), typeof(AbpAspNetCoreMvcUiBasicThemeModule))]
public class CmsKitCommonWebTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(builder =>
        {
            builder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(typeof(CmsKitCommonWebModule).Assembly));
            builder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(typeof(AbpAspNetCoreMvcUiBasicThemeModule).Assembly));
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthentication("Test")
            .AddScheme<AuthenticationSchemeOptions, CmsKitTestAuthenticationHandler>("Test", _ => { });

        var theme = Substitute.For<ITheme>();
        var themeManager = Substitute.For<IThemeManager>();
        themeManager.CurrentTheme.Returns(theme);
        context.Services.AddSingleton(themeManager);

        var blogFeatures = Substitute.For<IBlogFeatureAppService>();
        blogFeatures.GetOrDefaultAsync(Guid.Parse("1ef78ddd-58be-4dd0-b919-10053abeaf89"), BlogConsts.PreventXssFeatureName)
            .Returns(new BlogFeatureDto { IsEnabled = true });
        blogFeatures.GetOrDefaultAsync(Guid.Parse("bda7328d-c395-4b40-ab50-5f6199ac034d"), BlogConsts.PreventXssFeatureName)
            .Returns(new BlogFeatureDto { IsEnabled = false });
        context.Services.AddSingleton(blogFeatures);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }
}

public class CmsKitTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public CmsKitTestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("X-Test-User"))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var identity = new ClaimsIdentity(new[] { new Claim(AbpClaimTypes.UserId, "fd4f4895-56a3-46f8-b0d5-d15b385843b7") }, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
    }
}
