namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

public class AbpButtonGroupTagHelper : AbpTagHelper<AbpButtonGroupTagHelper, AbpButtonGroupTagHelperService>
{
    public AbpButtonGroupDirection Direction { get; set; } = AbpButtonGroupDirection.Horizontal;

    public AbpButtonGroupSize Size { get; set; } = AbpButtonGroupSize.Default;

    public AbpButtonGroupTagHelper(AbpButtonGroupTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
