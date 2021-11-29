namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.ListGroup
{
    public class AbpListGroupTagHelper : AbpTagHelper<AbpListGroupTagHelper, AbpListGroupTagHelperService>
    {
        public bool? Flush { get; set; }

        public AbpListGroupTagHelper(AbpListGroupTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
