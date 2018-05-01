namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public static class AbpButtonSizeExtensions
    {
        public static string ToClassName(this AbpButtonSize size)
        {
            switch (size)
            {
                case AbpButtonSize.Small:
                    return "btn-sm";
                case AbpButtonSize.Large:
                    return "btn-lg";
                case AbpButtonSize.Default:
                    return "";
                default:
                    throw new AbpException("Unknown AbpButtonSize: " + size);
            }
        }
    }
}