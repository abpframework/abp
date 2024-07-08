using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.UI.Navigation;

public class ApplicationMenuGroup
{
    private string _displayName = default!;
    private string? _elementId;

    /// <summary>
    /// Default <see cref="Order"/> value of a group item.
    /// </summary>
    public const int DefaultOrder = 1000;

    /// <summary>
    /// Unique name of the group in the application.
    /// </summary>
    [NotNull]
    public string Name { get; }

    /// <summary>
    /// Display name of the group.
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
    /// Can be used to render the element with a specific Id for DOM selections.
    /// </summary>
    public string? ElementId {
        get { return _elementId; }
        set {
            _elementId = NormalizeElementId(value);
        }
    }

    /// <summary>
    /// The Display order of the group.
    /// Default value: 1000.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Icon of the menu item if exists.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Can be used to store a custom object related to this menu item. Optional.
    /// </summary>
    [NotNull]
    public Dictionary<string, object> CustomData { get; } = new();

    public ApplicationMenuGroup(
        [NotNull] string name,
        [NotNull] string displayName,
        string? elementId = null,
        string? icon = null,
        int order = DefaultOrder)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

        Name = name;
        DisplayName = displayName;
        ElementId = elementId ?? GetDefaultElementId();
        Icon = icon;
        Order = order;
    }

    private string GetDefaultElementId()
    {
        return "MenuGroup_" + Name;
    }

    private string? NormalizeElementId(string? elementId)
    {
        return elementId?.Replace(".", "_");
    }

    /// <summary>
    /// Adds a custom data item to <see cref="CustomData"/> with given key &amp; value.
    /// </summary>
    /// <returns>This <see cref="ApplicationMenuGroup"/> itself.</returns>
    public ApplicationMenuGroup WithCustomData(string key, object value)
    {
        CustomData[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[ApplicationMenuGroup] Name = {Name}";
    }
}
