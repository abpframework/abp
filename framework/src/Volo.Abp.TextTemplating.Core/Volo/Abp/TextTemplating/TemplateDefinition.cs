using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.TextTemplating;

public class TemplateDefinition : IHasNameWithLocalizableDisplayName
{
    public const int MaxNameLength = 128;

    [NotNull]
    public string Name { get; }

    [NotNull]
    public ILocalizableString DisplayName {
        get => _displayName;
        set {
            Check.NotNull(value, nameof(value));
            _displayName = value;
        }
    }
    private ILocalizableString _displayName = default!;

    public bool IsLayout { get; }

    public string? Layout { get; set; }

    public string? LocalizationResourceName { get; set; }

    public bool IsInlineLocalized { get; set; }

    public string? DefaultCultureName { get; }

    public string? RenderEngine { get; set; }

    /// <summary>
    /// Gets/sets a key-value on the <see cref="Properties"/>.
    /// </summary>
    /// <param name="name">Name of the property</param>
    /// <returns>
    /// Returns the value in the <see cref="Properties"/> dictionary by given <paramref name="name"/>.
    /// Returns null if given <paramref name="name"/> is not present in the <see cref="Properties"/> dictionary.
    /// </returns>
    public object? this[string name] {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    /// <summary>
    /// Can be used to get/set custom properties for this feature.
    /// </summary>
    [NotNull]
    public Dictionary<string, object?> Properties { get; }

    public TemplateDefinition(
        [NotNull] string name,
        [NotNull] Type localizationResource,
        ILocalizableString? displayName = null,
        bool isLayout = false,
        string? layout = null,
        string? defaultCultureName = null)
    : this(name, LocalizationResourceNameAttribute.GetName(localizationResource), displayName, isLayout, layout, defaultCultureName)
    {

    }

    public TemplateDefinition(
        [NotNull] string name,
        string? localizationResourceName = null,
        ILocalizableString? displayName = null,
        bool isLayout = false,
        string? layout = null,
        string? defaultCultureName = null)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
        LocalizationResourceName = localizationResourceName;
        DisplayName = displayName ?? new FixedLocalizableString(Name);
        IsLayout = isLayout;
        Layout = layout;
        DefaultCultureName = defaultCultureName;
        Properties = new Dictionary<string, object?>();
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual TemplateDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    /// <summary>
    /// Sets the Render Engine of <see cref="TemplateDefinition"/>.
    /// </summary>
    public virtual TemplateDefinition WithRenderEngine(string renderEngine)
    {
        RenderEngine = renderEngine;
        return this;
    }
}
