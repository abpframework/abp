namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert
{
    public class AbpAlertTagHelper : AbpTagHelper<AbpAlertTagHelper, AbpAlertTagHelperService>
    {
        public AbpAlertType AlertType { get; set; } = AbpAlertType.Default;

        public bool? Dismissible { get; set; }

        public AbpAlertTagHelper(AbpAlertTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
