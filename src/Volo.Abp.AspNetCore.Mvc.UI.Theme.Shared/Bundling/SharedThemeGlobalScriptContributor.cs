using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.DatatablesNet;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.DatatablesNetBs4;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQuery;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQueryForm;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQueryValidation;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQueryValidationUnobtrusive;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.SweetAlert;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Toastr;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling
{
    [DependsOn(
        typeof(JQueryScriptContributor),
        typeof(BootstrapScriptContributor),
        typeof(JQueryValidationScriptContributor),
        typeof(JQueryValidationUnobtrusiveScriptContributor),
        typeof(JQueryFormScriptContributor),
        typeof(DatatablesNetScriptContributor),
        typeof(DatatablesNetBs4ScriptContributor),
        typeof(SweetalertScriptContributor),
        typeof(ToastrScriptBundleContributor)
    )]
    public class SharedThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                //TODO: Move to their own contributors, but first consider to change abp.js a bit
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