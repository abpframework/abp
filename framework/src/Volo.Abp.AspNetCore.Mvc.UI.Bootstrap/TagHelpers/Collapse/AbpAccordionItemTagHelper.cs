namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse;

public class AbpAccordionItemTagHelper : AbpTagHelper<AbpAccordionItemTagHelper, AbpAccordionItemTagHelperService>
{
    public string Id { get; set; }

    public string Title { get; set; }

    public bool? Active { get; set; }

    public AbpAccordionItemTagHelper(AbpAccordionItemTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
