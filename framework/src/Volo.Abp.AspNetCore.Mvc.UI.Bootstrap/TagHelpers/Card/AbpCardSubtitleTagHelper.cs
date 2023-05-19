namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

public class AbpCardSubtitleTagHelper : AbpTagHelper<AbpCardSubtitleTagHelper, AbpCardSubtitleTagHelperService>
{
    public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H6;

    public HtmlHeadingType Heading { get; set; } = DefaultHeading;

    public AbpCardSubtitleTagHelper(AbpCardSubtitleTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
