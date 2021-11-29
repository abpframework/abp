namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public static class AbpButtonSizeExtensions
    {
        public static string ToClassName(this AbpButtonSize size)
        {
            switch (size)
            {
                case AbpButtonSize.Small:
                case AbpButtonSize.Block_Small:
                    return "btn-sm";
                case AbpButtonSize.Medium:
                case AbpButtonSize.Block_Medium:
                    return "btn-md";
                case AbpButtonSize.Large:
                case AbpButtonSize.Block_Large:
                    return "btn-lg";
                case AbpButtonSize.Block:
                case AbpButtonSize.Default:
                    return "";
                default:
                    throw new AbpException($"Unknown {nameof(AbpButtonSize)}: {size}");
            }
        }
    }
}
