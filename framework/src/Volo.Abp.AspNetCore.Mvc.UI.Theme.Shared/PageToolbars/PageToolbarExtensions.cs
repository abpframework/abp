using System;
using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpPageToolbar.Button;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars
{
    public static class PageToolbarExtensions
    {
        public static PageToolbar AddComponent<TComponent>(
            this PageToolbar toolbar,
            object argument = null,
            int order = 0,
            string requiredPolicyName = null)
        {
            return toolbar.AddComponent(
                typeof(TComponent),
                argument,
                order,
                requiredPolicyName
            );
        }

        public static PageToolbar AddComponent(
            this PageToolbar toolbar, 
            Type componentType, 
            object argument = null,
            int order = 0,
            string requiredPolicyName = null)
        {
            toolbar.Contributors.Add(
                new SimplePageToolbarContributor(
                    componentType,
                    argument,
                    order,
                    requiredPolicyName
                )
            );

            return toolbar;
        }

        public static PageToolbar AddButton(
            this PageToolbar toolbar, 
            ILocalizableString text,
            string icon = null,
            string name = null,
            string id = null,
            ILocalizableString busyText = null,
            FontIconType iconType = FontIconType.FontAwesome,
            AbpButtonType type = AbpButtonType.Primary,
            AbpButtonSize size = AbpButtonSize.Small,
            bool disabled = false,
            int order = 0,
            string requiredPolicyName = null)
        {
            if (busyText == null)
            {
                busyText = new LocalizableString(typeof(AbpUiResource), "ProcessingWithThreeDot");
            }

            toolbar.AddComponent<AbpPageToolbarButtonViewComponent>(
                new
                {
                    text,
                    icon,
                    name,
                    id,
                    busyText,
                    iconType,
                    type,
                    size,
                    disabled
                },
                order,
                requiredPolicyName
            );

            return toolbar;
        }
    }
}