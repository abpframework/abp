using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.VirtualFileSystem.Embedded;

namespace Volo.Abp.Account.Web
{
    [DependsOn(typeof(AbpIdentityDomainModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAccountApplicationContractsModule))]
    public class AbpAccountWebModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAccountWebModule>();

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.Add(
                    new EmbeddedFileSet(
                        "/Areas/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.Account.Web.Areas"
                    )
                );

                options.FileSets.Add(
                    new EmbeddedFileSet(
                        "/Pages/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.Account.Web.Pages"
                    )
                );

                options.FileSets.Add(
                    new EmbeddedFileSet(
                        "/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.Account.Web.wwwroot"
                    )
                );
            });
        }
    }
}
