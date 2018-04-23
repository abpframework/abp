namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card
{
    public class AbpCardTitleTagHelper : AbpTagHelper<AbpCardTitleTagHelper, AbpCardTitleTagHelperService>
    {
        public HtmlHeadingType Heading { get; set; } = HtmlHeadingType.H5;

        public AbpCardTitleTagHelper(AbpCardTitleTagHelperService tagHelperService) 
            : base(tagHelperService)
        {
        }
    }
}
