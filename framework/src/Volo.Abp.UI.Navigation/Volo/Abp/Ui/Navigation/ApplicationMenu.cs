using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.UI.Navigation;

public class ApplicationMenu : IHasMenuItems
{
    /// <summary>
    /// Unique name of the menu in the application.
    /// </summary>
    [NotNull]
    public string Name { get; }

    /// <summary>
    /// Display name of the menu.
    /// Default value is the <see cref="Name"/>.
    /// </summary>
    [NotNull]
    public string DisplayName {
        get { return _displayName; }
        set {
            Check.NotNullOrWhiteSpace(value, nameof(value));
            _displayName = value;
        }
    }
    private string _displayName;

    /// <inheritdoc cref="IHasMenuItems.Items"/>
    [NotNull]
    public ApplicationMenuItemList Items { get; }

    /// <summary>
    /// Can be used to store a custom object related to this menu.
    /// </summary>
    [NotNull]
    public Dictionary<string, object> CustomData { get; } = new();

    public ApplicationMenu(
        [NotNull] string name,
        string displayName = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Name = name;
        DisplayName = displayName ?? Name;

        Items = new ApplicationMenuItemList();
    }

    /// <summary>
    /// Adds a <see cref="ApplicationMenuItem"/> to <see cref="Items"/>.
    /// </summary>
    /// <param name="menuItem"><see cref="ApplicationMenuItem"/> to be added</param>
    /// <returns>This <see cref="ApplicationMenu"/> object</returns>
    public ApplicationMenu AddItem([NotNull] ApplicationMenuItem menuItem)
    {
        Items.Add(menuItem);
        return this;
    }

    /// <summary>
    /// Adds a custom data item to <see cref="CustomData"/> with given key &amp; value.
    /// </summary>
    /// <returns>This <see cref="ApplicationMenu"/> itself.</returns>
    public ApplicationMenu WithCustomData(string key, object value)
    {
        CustomData[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[ApplicationMenu] Name = {Name}";
    }
}
