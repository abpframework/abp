namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;

public static class AbpModalSizeExtensions
{
    public static string ToClassName(this AbpModalSize size)
    {
        switch (size)
        {
            case AbpModalSize.Small:
                return "modal-sm";
            case AbpModalSize.Large:
                return "modal-lg";
            case AbpModalSize.ExtraLarge:
                return "modal-xl";
            case AbpModalSize.Default:
                return "";
            default:
                throw new AbpException($"Unknown {nameof(AbpModalSize)}: {size}");
        }
    }
}
