using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Mapperly;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Admin.Navigation;
using Volo.Docs.Localization;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsAdminApplicationContractsModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
        )]
    public class DocsAdminWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(DocsResource), typeof(DocsAdminWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(DocsAdminWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMapperlyObjectMapper<DocsAdminWebModule>();
            
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new DocsMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DocsAdminWebModule>();
            });

            Configure<DynamicJavaScriptProxyOptions>(options =>
            {
                options.DisableModule(DocsAdminRemoteServiceConsts.ModuleName);
            });
        }
    }
}
