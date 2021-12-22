using JetBrains.Annotations;
using System.Linq;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.UI.Navigation;

public static class ApplicationMenuExtensions
{
    [NotNull]
    public static ApplicationMenuItem GetAdministration(
        [NotNull] this ApplicationMenu applicationMenu)
    {
        return applicationMenu.GetMenuItem(
            DefaultMenuNames.Application.Main.Administration
        );
    }

    [NotNull]
    public static ApplicationMenuItem GetMenuItem(
        [NotNull] this IHasMenuItems menuWithItems,
        string menuItemName)
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
        [NotNull] this IHasMenuItems menuWithItems,
        string menuItemName)
    {
        Check.NotNull(menuWithItems, nameof(menuWithItems));

        return menuWithItems.Items.FirstOrDefault(mi => mi.Name == menuItemName);
    }

    public static bool TryRemoveMenuItem(
        [NotNull] this IHasMenuItems menuWithItems,
        string menuItemName)
    {
        Check.NotNull(menuWithItems, nameof(menuWithItems));

        return menuWithItems.Items.RemoveAll(item => item.Name == menuItemName) > 0;
    }

    [NotNull]
    public static IHasMenuItems SetSubItemOrder(
        [NotNull] this IHasMenuItems menuWithItems,
        string menuItemName,
        int order)
    {
        Check.NotNull(menuWithItems, nameof(menuWithItems));

        var menuItem = menuWithItems.GetMenuItemOrNull(menuItemName);
        if (menuItem != null)
        {
            menuItem.Order = order;
        }

        return menuWithItems;
    }
}
