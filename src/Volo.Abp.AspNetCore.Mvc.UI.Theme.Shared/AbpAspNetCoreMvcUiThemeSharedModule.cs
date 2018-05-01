using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule)
        )]
    public class AbpAspNetCoreMvcUiThemeSharedModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiThemeSharedModule>("Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared");
            });

            services.Configure<BundlingOptions>(options =>
            {
                options.StyleBundles.Add("GlobalStyles", new[]
                {
                    "/libs/font-awesome/css/font-awesome.css",
                    "/libs/bootstrap/css/bootstrap.css",
                    "/libs/datatables.net-bs4/css/dataTables.bootstrap4.css",
                    "/libs/toastr/toastr.min.css",

                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/datatables/datatables.css"
                });

                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/libs/jquery/jquery.js",
                    "/libs/bootstrap/js/bootstrap.bundle.js",
                    "/libs/jquery-validation/jquery.validate.js",
                    "/libs/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
                    "/libs/jquery-form/jquery.form.min.js",
                    "/libs/datatables.net/js/jquery.dataTables.js",
                    "/libs/datatables.net-bs4/js/dataTables.bootstrap4.js",
                    "/libs/sweetalert/sweetalert.min.js",
                    "/libs/toastr/toastr.min.js",

                    "/libs/abp/core/abp.js",
                    "/libs/abp/jquery/abp.dom.js",
                    "/libs/abp/jquery/abp.ajax.js",
                    "/libs/abp/jquery/abp.resource-loader.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/jquery/jquery-extensions.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/jquery-form/jquery-form-extensions.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/bootstrap/dom-event-handlers.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/bootstrap/modal-manager.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/datatables/datatables-extensions.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/sweetalert/abp-sweetalert.js",
                    "/libs/abp/aspnetcore.mvc.ui.theme.shared/sweetalert/abp-toastr.js"
                });
            });

            services.AddAssemblyOf<AbpAspNetCoreMvcUiThemeSharedModule>();
        }
    }
}
