namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card
{
    public class AbpCardSubtitleTagHelper : AbpTagHelper<AbpCardSubtitleTagHelper, AbpCardSubtitleTagHelperService>
    {
        public HtmlHeadingType Heading { get; set; } = HtmlHeadingType.H6;

        public AbpCardSubtitleTagHelper(AbpCardSubtitleTagHelperService tagHelperService)
            : base(tagHelperService)
        {
        }
    }
}