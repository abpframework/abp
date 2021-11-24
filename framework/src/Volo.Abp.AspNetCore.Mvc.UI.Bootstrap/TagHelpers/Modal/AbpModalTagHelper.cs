namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;

public class AbpModalTagHelper : AbpTagHelper<AbpModalTagHelper, AbpModalTagHelperService>
{
    public AbpModalSize Size { get; set; } = AbpModalSize.Default;

    public bool? Centered { get; set; } = false;

    public bool? Scrollable { get; set; } = false;

    public bool? Static { get; set; } = false;

    public AbpModalTagHelper(AbpModalTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
