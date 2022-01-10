namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

public class AbpCardBodyTagHelper : AbpTagHelper<AbpCardBodyTagHelper, AbpCardBodyTagHelperService>
{
    public string Title { get; set; }

    public string Subtitle { get; set; }

    public AbpCardBodyTagHelper(AbpCardBodyTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
