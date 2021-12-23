namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

public static class HtmlHeadingTypeExtensions
{
    public static string ToHtmlTag(this HtmlHeadingType heading)
    {
        switch (heading)
        {
            case HtmlHeadingType.H1:
                return "h1";
            case HtmlHeadingType.H2:
                return "h2";
            case HtmlHeadingType.H3:
                return "h3";
            case HtmlHeadingType.H4:
                return "h4";
            case HtmlHeadingType.H5:
                return "h5";
            case HtmlHeadingType.H6:
                return "h6";
            default:
                throw new AbpException("Unknown HtmlHeadingType: " + heading);
        }
    }
}
