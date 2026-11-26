using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;
using Volo.Docs.Admin.Documents;

namespace Volo.Docs.Admin;

[DependsOn(typeof(AbpAspNetCoreTestBaseModule), typeof(DocsAdminHttpApiModule))]
public class DocsAdminHttpApiTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        context.Services.AddAuthentication("DocsTest")
            .AddCookie("DocsTest", options => options.Cookie.Name = "DocsTests.Auth");
        Configure<AbpAntiForgeryOptions>(options => options.AuthCookieSchemaName = "DocsTest");
        context.Services.AddSingleton(Substitute.For<IDocumentPdfAdminAppService>());
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
