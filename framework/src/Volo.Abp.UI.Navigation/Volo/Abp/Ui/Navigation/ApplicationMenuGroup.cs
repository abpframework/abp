using JetBrains.Annotations;

namespace Volo.Abp.UI.Navigation;

public class ApplicationMenuGroup
{
    private string _displayName;

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
    public string ElementId { get; set; }

    /// <summary>
    /// The Display order of the group.
    /// Default value: 1000.
    /// </summary>
    public int Order { get; set; }

    public ApplicationMenuGroup(
        [NotNull] string name,
        [NotNull] string displayName,
        string elementId = null,
        int order = DefaultOrder)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

        Name = name;
        DisplayName = displayName;
        ElementId = elementId;
        Order = order;
    }

    private string GetDefaultElementId()
    {
        return "MenuGroup_" + Name;
    }

    public override string ToString()
    {
        return $"[ApplicationMenuGroup] Name = {Name}";
    }
}
