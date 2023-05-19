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

    [NotNull]
    public static ApplicationMenuGroup GetMenuGroup(
        [NotNull] this IHasMenuGroups menuWithGroups,
        string groupName)
    {
        var menuGroup = menuWithGroups.GetMenuGroupOrNull(groupName);
        if (menuGroup == null)
        {
            throw new AbpException($"Could not find a group item with given name: {groupName}");
        }

        return menuGroup;
    }

    [CanBeNull]
    public static ApplicationMenuGroup GetMenuGroupOrNull(
        [NotNull] this IHasMenuGroups menuWithGroups,
        string menuGroupName)
    {
        Check.NotNull(menuWithGroups, nameof(menuWithGroups));

        return menuWithGroups.Groups.FirstOrDefault(group => group.Name == menuGroupName);
    }

    public static bool TryRemoveMenuGroup(
        [NotNull] this IHasMenuGroups menuWithGroups,
        string menuGroupName)
    {
        Check.NotNull(menuWithGroups, nameof(menuWithGroups));

        return menuWithGroups.Groups.RemoveAll(group => group.Name == menuGroupName) > 0;
    }

    [NotNull]
    public static IHasMenuGroups SetMenuGroupOrder(
        [NotNull] this IHasMenuGroups menuWithGroups,
        string menuGroupName,
        int order)
    {
        Check.NotNull(menuWithGroups, nameof(menuWithGroups));

        var menuGroup = menuWithGroups.GetMenuGroupOrNull(menuGroupName);
        if (menuGroup != null)
        {
            menuGroup.Order = order;
        }

        return menuWithGroups;
    }
}
