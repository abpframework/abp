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
            case AbpModalSize.Fullscreen:
                return "modal-fullscreen";
            case AbpModalSize.FullscreenSmDown:
                return "modal-fullscreen-sm-down";
            case AbpModalSize.FullscreenMdDown:
                return "modal-fullscreen-md-down";
            case AbpModalSize.FullscreenLgDown:
                return "modal-fullscreen-lg-down";
            case AbpModalSize.FullscreenXlDown:
                return "modal-fullscreen-xl-down";
            case AbpModalSize.FullscreenXxlDown:
                return "modal-fullscreen-xxl-down";
            default:
                throw new AbpException($"Unknown {nameof(AbpModalSize)}: {size}");
        }
    }
}
