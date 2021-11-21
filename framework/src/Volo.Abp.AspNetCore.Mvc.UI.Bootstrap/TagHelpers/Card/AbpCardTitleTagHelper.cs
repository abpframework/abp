namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

public class AbpCardTitleTagHelper : AbpTagHelper<AbpCardTitleTagHelper, AbpCardTitleTagHelperService>
{
    public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H5;

    public HtmlHeadingType Heading { get; set; } = DefaultHeading;

    public AbpCardTitleTagHelper(AbpCardTitleTagHelperService tagHelperService)
        : base(tagHelperService)
    {
    }
}
