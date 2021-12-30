using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert;

public class AbpAlertTagHelper : AbpTagHelper<AbpAlertTagHelper, AbpAlertTagHelperService>
{
    public AlertType AlertType { get; set; } = AlertType.Default;

    public bool? Dismissible { get; set; }

    public AbpAlertTagHelper(AbpAlertTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
