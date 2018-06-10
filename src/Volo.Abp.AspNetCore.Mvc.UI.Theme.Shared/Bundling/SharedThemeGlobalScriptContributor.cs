using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Bootstrap;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling
{
    [DependsOn(
        typeof(BootstrapScriptBundleContributor)
    )]
    public class SharedThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            { //TODO: Create seperated contributors!
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
                "/libs/abp/aspnetcore.mvc.ui.theme.shared/toastr/abp-toastr.js"
            });
        }
    }
}