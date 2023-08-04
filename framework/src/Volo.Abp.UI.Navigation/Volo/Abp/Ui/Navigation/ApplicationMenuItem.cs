using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.UI.Navigation;

public class ApplicationMenuItem : IHasMenuItems, IHasSimpleStateCheckers<ApplicationMenuItem>
{
    private string _displayName = default!;
    private string? _elementId;

    /// <summary>
    /// Default <see cref="Order"/> value of a menu item.
    /// </summary>
    public const int DefaultOrder = 1000;

    /// <summary>
    /// Unique name of the menu in the application.
    /// </summary>
    [NotNull]
    public string Name { get; }

    /// <summary>
    /// Display name of the menu item.
    /// </summary>
    [NotNull]
    public string DisplayName {
        get { return _displayName; }
        set {
            Check.NotNullOrWhiteSpace(value, nameof(value));
            _displayName = value;
        }
    }

    /// <summary>
    /// The Display order of the menu.
    /// Default value: 1000.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// The URL to navigate when this menu item is selected.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Icon of the menu item if exists.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Returns true if this menu item has no child <see cref="Items"/>.
    /// </summary>
    public bool IsLeaf => Items.IsNullOrEmpty();

    /// <summary>
    /// Target of the menu item. Can be null, "_blank", "_self", "_parent", "_top" or a frame name for web applications.
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Can be used to disable this menu item.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <inheritdoc cref="IHasMenuItems.Items"/>
    [NotNull]
    public ApplicationMenuItemList Items { get; }

    [Obsolete("Use RequirePermissions extension method.")]
    public string? RequiredPermissionName { get; set; }

    public List<ISimpleStateChecker<ApplicationMenuItem>> StateCheckers { get; }

    /// <summary>
    /// Can be used to store a custom object related to this menu item. Optional.
    /// </summary>
    [NotNull]
    public Dictionary<string, object> CustomData { get; } = new();

    /// <summary>
    /// Can be used to render the element with a specific Id for DOM selections.
    /// </summary>
    public string? ElementId {
        get { return _elementId; }
        set {
            _elementId = NormalizeElementId(value);
        }
    }

    /// <summary>
    /// Can be used to render the element with extra CSS classes.
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Can be used to group menu items.
    /// </summary>
    public string? GroupName { get; set; }

    public ApplicationMenuItem(
        [NotNull] string name,
        [NotNull] string displayName,
        string? url = null,
        string? icon = null,
        int order = DefaultOrder,
        string? target = null,
        string? elementId = null,
        string? cssClass = null,
        string? groupName = null,
        string? requiredPermissionName = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

        Name = name;
        DisplayName = displayName;
        Url = url;
        Icon = icon;
        Order = order;
        Target = target;
        ElementId = elementId ?? GetDefaultElementId();
        CssClass = cssClass;
        GroupName = groupName;
        RequiredPermissionName = requiredPermissionName;
        StateCheckers = new List<ISimpleStateChecker<ApplicationMenuItem>>();
        Items = new ApplicationMenuItemList();
    }

    /// <summary>
    /// Adds a <see cref="ApplicationMenuItem"/> to <see cref="Items"/>.
    /// </summary>
    /// <param name="menuItem"><see cref="ApplicationMenuItem"/> to be added</param>
    /// <returns>This <see cref="ApplicationMenuItem"/> object</returns>
    public ApplicationMenuItem AddItem([NotNull] ApplicationMenuItem menuItem)
    {
        Items.Add(menuItem);
        return this;
    }

    /// <summary>
    /// Adds a custom data item to <see cref="CustomData"/> with given key &amp; value.
    /// </summary>
    /// <returns>This <see cref="ApplicationMenuItem"/> itself.</returns>
    public ApplicationMenuItem WithCustomData(string key, object value)
    {
        CustomData[key] = value;
        return this;
    }

    private string GetDefaultElementId()
    {
        return "MenuItem_" + Name;
    }
    
    private string? NormalizeElementId(string? elementId)
    {
        return elementId?.Replace(".", "_");
    }

    public override string ToString()
    {
        return $"[ApplicationMenuItem] Name = {Name}";
    }
}
