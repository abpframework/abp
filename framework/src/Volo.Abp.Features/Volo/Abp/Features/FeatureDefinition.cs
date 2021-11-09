using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.Features;

public class FeatureDefinition
{
    /// <summary>
    /// Unique name of the feature.
    /// </summary>
    [NotNull]
    public string Name { get; }

    [NotNull]
    public ILocalizableString DisplayName
    {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }
    private ILocalizableString _displayName;

    [CanBeNull]
    public ILocalizableString Description { get; set; }

    /// <summary>
    /// Parent of this feature, if one exists.
    /// If set, this feature can be enabled only if the parent is enabled.
    /// </summary>
    [CanBeNull]
    public FeatureDefinition Parent { get; private set; }

    /// <summary>
    /// List of child features.
    /// </summary>
    public IReadOnlyList<FeatureDefinition> Children => _children.ToImmutableList();
    private readonly List<FeatureDefinition> _children;

    /// <summary>
    /// Default value of the feature.
    /// </summary>
    [CanBeNull]
    public string DefaultValue { get; set; }

    /// <summary>
    /// Can clients see this feature and it's value.
    /// Default: true.
    /// </summary>
    public bool IsVisibleToClients { get; set; }

    /// <summary>
    /// Can host use this feature.
    /// Default: true.
    /// </summary>
    public bool IsAvailableToHost { get; set; }

    /// <summary>
    /// A list of allowed providers to get/set value of this feature.
    /// An empty list indicates that all providers are allowed.
    /// </summary>
    [NotNull]
    public List<string> AllowedProviders { get; }

    /// <summary>
    /// Gets/sets a key-value on the <see cref="Properties"/>.
    /// </summary>
    /// <param name="name">Name of the property</param>
    /// <returns>
    /// Returns the value in the <see cref="Properties"/> dictionary by given <paramref name="name"/>.
    /// Returns null if given <paramref name="name"/> is not present in the <see cref="Properties"/> dictionary.
    /// </returns>
    [CanBeNull]
    public object this[string name]
    {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    /// <summary>
    /// Can be used to get/set custom properties for this feature.
    /// </summary>
    [NotNull]
    public Dictionary<string, object> Properties { get; }

    /// <summary>
    /// Input type.
    /// This can be used to prepare an input for changing this feature's value.
    /// Default: <see cref="ToggleStringValueType"/>.
    /// </summary>
    [CanBeNull]
    public IStringValueType ValueType { get; set; }

    public FeatureDefinition(
        string name,
        string defaultValue = null,
        ILocalizableString displayName = null,
        ILocalizableString description = null,
        IStringValueType valueType = null,
        bool isVisibleToClients = true,
        bool isAvailableToHost = true)
    {
        Name = name;
        DefaultValue = defaultValue;
        DisplayName = displayName ?? new FixedLocalizableString(name);
        Description = description;
        ValueType = valueType;
        IsVisibleToClients = isVisibleToClients;
        IsAvailableToHost = isAvailableToHost;

        Properties = new Dictionary<string, object>();
        AllowedProviders = new List<string>();
        _children = new List<FeatureDefinition>();
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual FeatureDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual FeatureDefinition WithProviders(params string[] providers)
    {
        if (!providers.IsNullOrEmpty())
        {
            AllowedProviders.AddRange(providers);
        }

        return this;
    }

    /// <summary>
    /// Adds a child feature.
    /// </summary>
    /// <returns>Returns a newly created child feature</returns>
    public FeatureDefinition CreateChild(
        string name,
        string defaultValue = null,
        ILocalizableString displayName = null,
        ILocalizableString description = null,
        IStringValueType valueType = null,
        bool isVisibleToClients = true,
        bool isAvailableToHost = true)
    {
        var feature = new FeatureDefinition(
            name,
            defaultValue,
            displayName,
            description,
            valueType,
            isVisibleToClients,
            isAvailableToHost)
        {
            Parent = this
        };

        _children.Add(feature);
        return feature;
    }

    public void RemoveChild(string name)
    {
        var featureToRemove = _children.FirstOrDefault(f => f.Name == name);
        if (featureToRemove == null)
        {
            throw new AbpException($"Could not find a feature named '{name}' in the Children of this feature '{Name}'.");
        }

        featureToRemove.Parent = null;
        _children.Remove(featureToRemove);
    }

    public override string ToString()
    {
        return $"[{nameof(FeatureDefinition)}: {Name}]";
    }
}
