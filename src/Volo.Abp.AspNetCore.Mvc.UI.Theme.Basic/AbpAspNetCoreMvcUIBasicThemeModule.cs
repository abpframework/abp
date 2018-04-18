using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
        )]
    public class AbpAspNetCoreMvcUiBasicThemeModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiBasicThemeModule>("Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic");
            });

            services.Configure<BundlingOptions>(options =>
            {
                options.StyleBundles.Add("GlobalStyles", new[]
                {
                    "/libs/font-awesome/css/font-awesome.css",
                    "/libs/bootstrap/css/bootstrap.css",
                    "/libs/datatables.net-bs4/css/dataTables.bootstrap4.css",

                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/datatables/datatables.css"
                });

                //TODO: Handle ticks stuff for all files
                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/libs/jquery/jquery.js",
                    "/libs/bootstrap/js/bootstrap.bundle.js",
                    "/libs/jquery-validation/dist/jquery.validate.js",
                    "/libs/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
                    "/libs/jquery-form/jquery.form.min.js",
                    "/libs/datatables.net/js/jquery.dataTables.js",
                    "/libs/datatables.net-bs4/js/dataTables.bootstrap4.js",

                    "/libs/abp/core/src/abp.js",
                    "/libs/abp/jquery/abp.ajax.js",
                    "/libs/abp/jquery/abp.resource-loader.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/jquery/jquery-extensions.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/bootstrap/abp.modal-manager.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/datatables/datatables-extensions.js",

                    "/Abp/ApplicationConfigurationScript",
                    "/Abp/ServiceProxyScript"
                });
            });

            services.AddAssemblyOf<AbpAspNetCoreMvcUiBasicThemeModule>();
        }
    }
}
