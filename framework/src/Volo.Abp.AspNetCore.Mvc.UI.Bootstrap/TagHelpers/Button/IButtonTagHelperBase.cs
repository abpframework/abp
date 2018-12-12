namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public interface IButtonTagHelperBase
    {
        AbpButtonType ButtonType { get; }

        AbpButtonSize Size { get; }

        bool? Block { get;}

        string Text { get; }

        string Icon { get; }

        FontIconType IconType { get; }
    }
}