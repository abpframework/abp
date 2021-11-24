using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.BootstrapDatepicker;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.DatatablesNetBs5;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryForm;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryValidationUnobtrusive;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Lodash;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Luxon;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.MalihuCustomScrollbar;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Select2;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.SweetAlert2;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Timeago;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Toastr;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;

[DependsOn(
    typeof(JQueryScriptContributor),
    typeof(BootstrapScriptContributor),
    typeof(LodashScriptContributor),
    typeof(JQueryValidationUnobtrusiveScriptContributor),
    typeof(JQueryFormScriptContributor),
    typeof(Select2ScriptContributor),
    typeof(DatatablesNetBs5ScriptContributor),
    typeof(Sweetalert2ScriptContributor),
    typeof(ToastrScriptBundleContributor),
    typeof(MalihuCustomScrollbarPluginScriptBundleContributor),
    typeof(LuxonScriptContributor),
    typeof(TimeagoScriptContributor),
    typeof(BootstrapDatepickerScriptContributor)
    )]
public class SharedThemeGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddRange(new[]
        {
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/ui-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/jquery/jquery-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/jquery-form/jquery-form-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/jquery/widget-manager.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/bootstrap/dom-event-handlers.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/bootstrap/modal-manager.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/datatables/datatables-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/sweetalert2/abp-sweetalert2.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/toastr/abp-toastr.js"
            });
    }
}
