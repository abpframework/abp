namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert
{
    public class AbpAlertHeaderTagHelper : AbpTagHelper<AbpAlertHeaderTagHelper, AbpAlertHeaderTagHelperService>
    {
        public AbpAlertHeaderSize Size { get; set; } = AbpAlertHeaderSize.Default;

        public AbpAlertHeaderTagHelper(AbpAlertHeaderTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
