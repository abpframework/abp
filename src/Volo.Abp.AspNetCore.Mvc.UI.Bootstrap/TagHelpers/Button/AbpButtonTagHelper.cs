namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public class AbpButtonTagHelper : AbpTagHelper<AbpButtonTagHelper, AbpButtonTagHelperService>
    {
        public AbpButtonType ButtonType { get; set; } = AbpButtonType.Default;

        public string BusyText { get; set; }

        public AbpButtonTagHelper(AbpButtonTagHelperService service) 
            : base(service)
        {

        }
    }
}
