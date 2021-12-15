using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.ObjectExtending;

public class ObjectExtensionPropertyInfo : IHasNameWithLocalizableDisplayName, IBasicObjectExtensionPropertyInfo
{
    [NotNull]
    public ObjectExtensionInfo ObjectExtension { get; }

    [NotNull]
    public string Name { get; }

    [NotNull]
    public Type Type { get; }

    [NotNull]
    public List<Attribute> Attributes { get; }

    [NotNull]
    public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

    [CanBeNull]
    public ILocalizableString DisplayName { get; set; }

    /// <summary>
    /// Indicates whether to check the other side of the object mapping
    /// if it explicitly defines the property. This property is used in;
    ///
    /// * .MapExtraPropertiesTo() extension method.
    /// * .MapExtraProperties() configuration for the AutoMapper.
    ///
    /// It this is true, these methods check if the mapping object
    /// has defined the property using the <see cref="ObjectExtensionManager"/>.
    ///
    /// Default: null (unspecified, uses the default logic).
    /// </summary>
    public bool? CheckPairDefinitionOnMapping { get; set; }

    [NotNull]
    public Dictionary<object, object> Configuration { get; }

    /// <summary>
    /// Uses as the default value if <see cref="DefaultValueFactory"/> was not set.
    /// </summary>
    [CanBeNull]
    public object DefaultValue { get; set; }

    /// <summary>
    /// Used with the first priority to create the default value for the property.
    /// Uses to the <see cref="DefaultValue"/> if this was not set.
    /// </summary>
    [CanBeNull]
    public Func<object> DefaultValueFactory { get; set; }

    [NotNull]
    public ExtensionPropertyLookupConfiguration Lookup { get; set; }

    public ObjectExtensionPropertyInfo(
        [NotNull] ObjectExtensionInfo objectExtension,
        [NotNull] Type type,
        [NotNull] string name)
    {
        ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
        Type = Check.NotNull(type, nameof(type));
        Name = Check.NotNull(name, nameof(name));

        Configuration = new Dictionary<object, object>();
        Attributes = new List<Attribute>();
        Validators = new List<Action<ObjectExtensionPropertyValidationContext>>();

        Attributes.AddRange(ExtensionPropertyHelper.GetDefaultAttributes(Type));
        DefaultValue = TypeHelper.GetDefaultValue(Type);
        Lookup = new ExtensionPropertyLookupConfiguration();
    }

    public object GetDefaultValue()
    {
        return ExtensionPropertyHelper.GetDefaultValue(Type, DefaultValueFactory, DefaultValue);
    }
}
