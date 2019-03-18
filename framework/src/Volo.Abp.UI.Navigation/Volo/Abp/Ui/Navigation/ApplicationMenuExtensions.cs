using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Ui.Navigation;

namespace Volo.Abp.UI.Navigation
{
    public static class ApplicationMenuExtensions
    {
        [NotNull]
        public static ApplicationMenuItem GetAdministration(this ApplicationMenu applicationMenu)
        {
            return applicationMenu.GetMenuItem(DefaultMenuNames.Application.Main.Administration);
        }

        [NotNull]
        public static ApplicationMenuItem GetMenuItem(this IHasMenuItems menuWithItems, string menuItemName)
        {
            var menuItem = menuWithItems.GetMenuItemOrNull(menuItemName);
            if (menuItem == null)
            {
                throw new AbpException($"Could not find a menu item with given name: {menuItemName}");
            }

            return menuItem;
        }

        [CanBeNull]
        public static ApplicationMenuItem GetMenuItemOrNull(
            this IHasMenuItems menuWithItems, 
            string menuItemName)
        {
            return menuWithItems.Items.FirstOrDefault(mi => mi.Name == menuItemName);
        }
    }
}