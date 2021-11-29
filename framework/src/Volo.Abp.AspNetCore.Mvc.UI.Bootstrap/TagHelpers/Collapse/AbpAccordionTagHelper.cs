namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpAccordionTagHelper : AbpTagHelper<AbpAccordionTagHelper, AbpAccordionTagHelperService>
    {
        public string Id { get; set; }

        public AbpAccordionTagHelper(AbpAccordionTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
