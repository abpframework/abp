namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.ListGroup
{
    public class AbpListGroupItemTagHelper : AbpTagHelper<AbpListGroupItemTagHelper, AbpListGroupItemTagHelperService>
    {
        public bool? Active { get; set; }

        public bool? Disabled { get; set; }

        public string Href { get; set; }

        public AbpListItemTagType TagType { get; set; } = AbpListItemTagType.Default;

        public AbpListItemType Type { get; set; } = AbpListItemType.Default;

        public AbpListGroupItemTagHelper(AbpListGroupItemTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
