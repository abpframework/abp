namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public interface IButtonTagHelperBase
    {
        AbpButtonType ButtonType { get; }

        AbpButtonSize Size { get; }

        string Text { get; }

        string Icon { get; }

        bool? Disabled { get; }

        FontIconType IconType { get; }
    }
}