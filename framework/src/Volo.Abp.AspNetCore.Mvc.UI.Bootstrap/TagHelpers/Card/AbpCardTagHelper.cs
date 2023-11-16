namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

public class AbpCardTagHelper : AbpTagHelper<AbpCardTagHelper, AbpCardTagHelperService>
{
    public AbpCardBorderColorType Border { get; set; } = AbpCardBorderColorType.Default;

    public bool AddMarginBottomClass  { get; set; } = true;

    public AbpCardTagHelper(AbpCardTagHelperService tagHelperService)
        : base(tagHelperService)
    {
    }
}
