using JetBrains.Annotations;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.UI.Navigation
{
    public static class HasMenuItemsExtensions
    {
        [CanBeNull]
        public static ApplicationMenuItem FindMenuItem(this IHasMenuItems container, string menuItemName)
        {
            foreach (var menuItem in container.Items)
            {
                if (menuItem.Name == menuItemName)
                {
                    return menuItem;
                }

                var subItem = FindMenuItem(menuItem, menuItemName);
                if (subItem != null)
                {
                    return subItem;
                }
            }

            return null;
        }
    }
}
