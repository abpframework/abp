using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.EmbeddedFiles;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account.Web
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    public class AbpAccountWebModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAccountWebModule>();

            services.Configure<EmbeddedFileOptions>(options =>
            {
                options.Sources.Add(
                    new EmbeddedFileSet(
                        "/Areas/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.Account.Web.Areas"
                    )
                );

                options.Sources.Add(
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
