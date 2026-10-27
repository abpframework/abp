using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.StaticProxying;
using Volo.Abp.Http.Client.Web.Conventions;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Http.Client.Web;

[DependsOn(
    typeof(AbpHttpClientWebModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAutofacModule)
)]
public class AbpHttpClientWebTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(typeof(AbpHttpClientWebTestModule).Assembly);

        Configure<AbpHttpClientStaticProxyingOptions>(options =>
        {
            options.BindingFromQueryTypes.Add(typeof(ClientProxyTestQueryInput));
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpHttpClientWebTestModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseRouting();
        app.UseConfiguredEndpoints();
    }
}
