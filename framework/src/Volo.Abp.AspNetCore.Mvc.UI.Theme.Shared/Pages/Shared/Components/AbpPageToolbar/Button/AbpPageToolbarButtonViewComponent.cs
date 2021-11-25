using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpPageToolbar.Button;

public class AbpPageToolbarButtonViewComponent : AbpViewComponent
{
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    public AbpPageToolbarButtonViewComponent(IStringLocalizerFactory stringLocalizerFactory)
    {
        StringLocalizerFactory = stringLocalizerFactory;
    }

    public IViewComponentResult Invoke(
        ILocalizableString text,
        string name,
        string icon,
        string id,
        ILocalizableString busyText,
        FontIconType iconType,
        AbpButtonType type,
        AbpButtonSize size,
        bool disabled)
    {
        Check.NotNull(text, nameof(text));

        return View(
            "~/Pages/Shared/Components/AbpPageToolbar/Button/Default.cshtml",
            new AbpPageToolbarButtonViewModel(
                text.Localize(StringLocalizerFactory),
                name,
                icon,
                id,
                busyText?.Localize(StringLocalizerFactory),
                iconType,
                type,
                size,
                disabled
            )
        );
    }

    public class AbpPageToolbarButtonViewModel
    {
        public string Text { get; }
        public string Name { get; }
        public string Icon { get; }
        public string Id { get; }
        public string BusyText { get; }
        public FontIconType IconType { get; }
        public AbpButtonType Type { get; }
        public AbpButtonSize Size { get; }
        public bool Disabled { get; }

        public AbpPageToolbarButtonViewModel(
            string text,
            string name,
            string icon,
            string id,
            string busyText,
            FontIconType iconType,
            AbpButtonType type,
            AbpButtonSize size,
            bool disabled)
        {
            Text = text;
            Name = name;
            Icon = icon;
            Id = id;
            BusyText = busyText;
            IconType = iconType;
            Type = type;
            Size = size;
            Disabled = disabled;
        }
    }
}
